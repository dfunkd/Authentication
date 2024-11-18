using Authentication.Data.Contexts;
using Authentication.Models;
using Dapper;
using System.Data;

namespace Authentication.Data.Repositories;

public interface IAuthRepository
{
    Task<User?> GetUser(string username, string password, CancellationToken cancellationToken = default);
}

public class AuthRepository(IInventoryContext context) : IAuthRepository
{
    public async Task<User?> GetUser(string username, string password, CancellationToken cancellationToken = default)
    {
        User? ret = null;

        const string sSql = @"
SELECT Userid, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, FirstName, LastName, Username, Email, Password, EncryptedPassword
FROM dbo.[User]
WHERE UserName = @userName
	AND [Password] = @password
";

        var parms = new { username, password };

        using IDbConnection conn = context.Connection;

        CommandDefinition sCmd = new(sSql, parms, null, 150, cancellationToken: cancellationToken);

        User? res = await conn.QueryFirstOrDefaultAsync<User>(sCmd);

        if (res is not null)
            ret = res;

        return ret;
    }
}
