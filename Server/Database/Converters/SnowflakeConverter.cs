using EchoLib.Core.Snowflake;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Server.Database.Converters;

public class SnowflakeConverter() : ValueConverter<Snowflake, ulong>(v => v.Value, v => new Snowflake(v));