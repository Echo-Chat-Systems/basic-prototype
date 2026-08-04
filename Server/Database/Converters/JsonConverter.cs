using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace Server.Database.Converters;

public class JsonConverter<T>() : ValueConverter<T, string>(t => JsonConvert.SerializeObject(t), t => JsonConvert.DeserializeObject<T>(t)!)
{
}