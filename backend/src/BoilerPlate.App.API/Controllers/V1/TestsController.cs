using Asp.Versioning;
using BoilerPlate.App.API.Attributes;
using BoilerPlate.Core.Exceptions.Factory;
using BoilerPlate.Core.Utils;
using BoilerPlate.Data.Abstractions.Enums;
using BoilerPlate.Data.DTO.Common.Responses;
using BoilerPlate.Data.DTO.System.Tests.Requests;
using BoilerPlate.Services.Mail.ImapService;
using BoilerPlate.Services.Mail.Message;
using BoilerPlate.Services.Mail.SmtpService;
using BoilerPlate.Services.Telegram.BotTestService;
using Coravel.Invocable;
using Coravel.Queuing.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BoilerPlate.App.API.Controllers.V1;

/// <summary>
/// Test API methods
/// </summary>
[ApiVersion(1)]
[ApiExplorerSettings(GroupName = "v1")]
[Route("api/v{version:apiVersion}/tests")]
public class TestsController : BaseApiController
{
    private readonly IQueue _queue;
    private readonly IMediator _mediator;
    private readonly IMailSmtpService _mailSmtpService;
    private readonly IMailImapService _mailImapService;

    /// <inheritdoc />
    public TestsController(
        IMediator mediator,
        ILogger<TestsController> logger,
        IExceptionFactory exceptionFactory,
        IQueue queue,
        [FromKeyedServices("mail.smtp.logger")] IMailSmtpService mailSmtpService,
        [FromKeyedServices("mail.imap.test")] IMailImapService mailImapService)
        : base(mediator, logger, exceptionFactory)
    {
        _mediator = mediator;
        _queue = queue;
        _mailSmtpService = mailSmtpService;
        _mailImapService = mailImapService;
    }

    /// <summary> Log out user </summary>
    [HttpPost("enqueue-job")]
    [MinimumRoleAuthorize(UserRole.Admin)]
    public ActionResult<IdDto> EnqueueJob([FromBody] EnqueueJobDto request)
    {
        var jobType = AssemblyUtils.GetType(request.JobName);
        if (jobType == null)
        {
            throw new ArgumentException("Job is not found", nameof(request.JobName));
        }

        var payloadType = jobType.GetInterface(typeof(IInvocableWithPayload<>).Name)
            ?.GenericTypeArguments.FirstOrDefault();

        Guid? jobId;

        if (payloadType != null)
        {
            var payload = request.Payload?.ToObject(payloadType);
            if (payload == null)
            {
                throw new ArgumentException("Job requires a valid payload", nameof(request.Payload));
            }

            jobId = _queue.GetType().GetMethod("QueueInvocableWithPayload")
                ?.MakeGenericMethod(jobType, payloadType)
                .Invoke(_queue, new[] { payload }) as Guid?;
        }
        else
        {
            jobId = _queue.GetType().GetMethod("QueueInvocable")
                ?.MakeGenericMethod(jobType)
                .Invoke(_queue, Array.Empty<object>()) as Guid?;
        }

        if (jobId == null)
        {
            throw new Exception("Failed to enqueue a job");
        }

        return new IdDto { Id = jobId.Value };
    }

    /// <summary> Publish test Kafka message </summary>
    [HttpPost("kafka-produce")]
    [MinimumRoleAuthorize(UserRole.Admin)]
    public async Task<ActionResult> KafkaProduceMessage([FromBody] KafkaProduceMessageDto request,
        CancellationToken ct)
    {
        await _mediator.Send(request, ct);
        return Ok();
    }

    // /// <summary> Telegram bot send test message </summary>
    // [HttpPost("telegram-send-message")]
    // [MinimumRoleAuthorize(UserRole.Admin)]
    // public async Task<ActionResult> TelegramSendMessage(string chatId, string message, CancellationToken ct)
    // {
    //     await _telegramBotTestService.SendMessage(chatId, message, ct);
    //     return Ok();
    // }

    /// <summary> Send email </summary>
    [HttpPost("send-email")]
    [MinimumRoleAuthorize(UserRole.Admin)]
    public async Task<ActionResult> SendEmail(string from, string to, string subject, string message,
        CancellationToken ct)
    {
        var mailMessage = new MailMessage { From = [from], To = [to], Subject = subject, Body = message };
        var response = await _mailSmtpService.Send(mailMessage, ct);
        var id = await _mailImapService.AddMessage("Sent Messages", mailMessage, ct);

        return Ok(new { SmtpResponse = response, ImapMessageId = id });
    }

    /// <summary> Get email messages </summary>
    [HttpGet("email-messages")]
    [MinimumRoleAuthorize(UserRole.Admin)]
    public async Task<ActionResult> GetEmailMessages(string path, int? skip, int? take, CancellationToken ct)
    {
        var messages = await _mailImapService.GetMessages(path, skip, take, ct);
        return Ok(messages);
    }
}