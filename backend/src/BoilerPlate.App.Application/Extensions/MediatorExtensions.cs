using MediatR;
using Microsoft.Extensions.DependencyInjection;
using BoilerPlate.App.Application.RequestHandlers.Common;
using BoilerPlate.Core.Utils;
using BoilerPlate.Data.Domain.Entities.System;
using BoilerPlate.Data.DTO.Common.Requests;
using BoilerPlate.Data.DTO.System.StoredFiles.Responses;
using BoilerPlate.Data.DTO.System.Users.Responses;

namespace BoilerPlate.App.Application.Extensions;

public static class MediatorExtensions
{
    public static void AddMediator(this IServiceCollection services)
    {
        services.AddMediatR(_ => _.RegisterServicesFromAssemblies(AssemblyUtils.BoilerPlateAssemblies));

        // Get by ID
        services.AddTransient<IRequestHandler<GetByIdRequest<User, UserDto>, UserDto>, GetByIdHandler<User, UserDto>>();

        // Get all
        services.AddTransient<
            IRequestHandler<GetAllRequest<User, UserDto>, IEnumerable<UserDto>>,
            GetAllHandler<User, UserDto>>();
        services.AddTransient<
            IRequestHandler<GetAllRequest<StoredFile, StoredFileDto>, IEnumerable<StoredFileDto>>,
            GetAllHandler<StoredFile, StoredFileDto>>();
    }
}