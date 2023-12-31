using BoilerPlate.App.API.Attributes;
using BoilerPlate.Core.Exceptions.Factory;
using BoilerPlate.Core.Utils;
using BoilerPlate.Data.Abstractions.Enums;
using BoilerPlate.Data.DTO.Common.Responses;
using BoilerPlate.Data.DTO.System.Tests.Requests;
using Coravel.Invocable;
using Coravel.Queuing.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BoilerPlate.App.API.Controllers;

/// <summary>
/// Test API methods
/// </summary>
[Route("api/tests")]
public class TestsController : BaseApiController
{
    private readonly IQueue _queue;
    private readonly IMediator _mediator;

    /// <inheritdoc />
    public TestsController(IMediator mediator, ILogger<TestsController> logger, IExceptionFactory exceptionFactory,
        IQueue queue) : base(mediator, logger, exceptionFactory)
    {
        _mediator = mediator;
        _queue = queue;
    }

    /// <summary> Log out user </summary>
    [HttpPost("enqueue-job")]
    [MinimumRoleAuthorize(UserRole.Admin)]
    public ActionResult<IdDto> EnqueueJobAsync([FromBody] EnqueueJobDto request)
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
    public async Task<ActionResult> KafkaProduceMessageAsync([FromBody] KafkaProduceMessageDto request,
        CancellationToken ct)
    {
        await _mediator.Send(request, ct);
        return Ok();
    }
}