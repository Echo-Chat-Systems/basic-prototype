using EchoLib.Crypto.Signing;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Server.Database.Converters;

public class PublicSigningKeyConverter() : ValueConverter<PublicSigningKey, string>(v => v.ToString(), v => new PublicSigningKey(v));