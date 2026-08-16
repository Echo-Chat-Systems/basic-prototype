using EchoLib.Crypto.Encryption;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Server.Database.Converters;

public class PublicEncryptionKeyConverter() : ValueConverter<PublicEncryptionKey, string>(v => v.ToString(), v => new PublicEncryptionKey(v));