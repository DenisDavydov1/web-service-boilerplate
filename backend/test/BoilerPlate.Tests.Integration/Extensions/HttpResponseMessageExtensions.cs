using System.Runtime.Serialization;
using Newtonsoft.Json;
using BoilerPlate.Core.Serialization;
using BoilerPlate.Data.DTO.Base;
using Newtonsoft.Json.Linq;

namespace BoilerPlate.Tests.Integration.Extensions;

internal static class HttpResponseMessageExtensions
{
    public static async Task<TDto> ToDtoAsync<TDto>(this HttpResponseMessage response)
        where TDto : BaseDto
    {
        var item = await response.Content.ReadAsStringAsync();
        var dto = JsonConvert.DeserializeObject<TDto>(item, SerializationSettings.Default);
        if (dto == null)
        {
            throw new SerializationException("Http response deserialization error");
        }

        return dto;
    }

    public static async Task<string?> GetErrorCode(this HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        var jObject = JObject.Parse(content);
        return jObject["extensions"]?["code"]?.ToObject<string>();
    }
}