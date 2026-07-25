using System.Data;
using Dapper;
using EchoLib.Crypto.Encryption;
using EchoLib.Crypto.Signing;

namespace Server.Database.ParameterConverters;

public class PublicEncryptionKeyConverter : SqlMapper.TypeHandler<PublicEncryptionKey>
{
    public override void SetValue(IDbDataParameter parameter, PublicEncryptionKey? value)
    {
        parameter.DbType = DbType.String;
        parameter.Value = value?.ToString();
    }

    public override PublicEncryptionKey? Parse(object value)
    {
        return value == DBNull.Value ? null : new  PublicEncryptionKey((string)value);
    }
}