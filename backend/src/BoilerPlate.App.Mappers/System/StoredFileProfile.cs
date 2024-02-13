using AutoMapper;
using BoilerPlate.Data.Domain.Entities.System;
using BoilerPlate.Data.DTO.Common.Responses;
using BoilerPlate.Data.DTO.System.StoredFiles.Requests;
using BoilerPlate.Data.DTO.System.StoredFiles.Responses;

namespace BoilerPlate.App.Mappers.System;

public class StoredFileProfile : Profile
{
    public StoredFileProfile()
    {
        CreateMap<StoredFile, StoredFileDto>();
        CreateMap<StoredFile, IdDto>();
        CreateMap<UpdateStoredFileDto, StoredFile>();
    }
}