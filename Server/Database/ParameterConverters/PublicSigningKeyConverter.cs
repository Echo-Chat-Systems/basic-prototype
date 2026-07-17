using System.Data;
using Dapper;
using EchoLib.Crypto.Signing;

namespace Server.Database.ParameterConverters;

public class PublicSigningKeyConverter : SqlMapper.TypeHandler<PublicSigningKey>
{
    public override void SetValue(IDbDataParameter parameter, PublicSigningKey? value)
    {
        parameter.DbType = DbType.String;
        parameter.Value = value?.ToString();
    }

    public override PublicSigningKey? Parse(object value)
    {
        return value == DBNull.Value ? null : new  PublicSigningKey((string)value);
    }
}