using System.Runtime.Serialization;
using Newtonsoft.Json;
using BoilerPlate.Core.Serialization;

namespace BoilerPlate.Tests.Integration.Extensions;

internal static class HttpResponseMessageExtensions
{
    public static async Task<TDto> ToDtoAsync<TDto>(this HttpResponseMessage response)
    {
        var item = await response.Content.ReadAsStringAsync();
        var dto = JsonConvert.DeserializeObject<TDto>(item, SerializationSettings.Default);
        if (dto == null)
        {
            throw new SerializationException("Http response deserialization error");
        }

        return dto;
    }
}