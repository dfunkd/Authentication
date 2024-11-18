using Authentication.Data.Contexts;
using Authentication.Models;
using Authentication.Models.Requests;
using Dapper;
using System.Data;

namespace Authentication.Data.Repositories;

public interface IUserRepository
{
    Task<User?> AddUser(AddUserRequest user, CancellationToken cancellationToken = default);
    Task<bool> DeleteUser(Guid userId, CancellationToken cancellationToken = default);
    Task<List<User>> GetAllUsers(CancellationToken cancellationToken = default);
    Task<User?> GetUserById(Guid userId, CancellationToken cancellationToken = default);
    Task<User?> UpdateUser(UpdateUserRequest user, CancellationToken cancellationToken = default);
}

public class UserRepository(IInventoryContext context) : IUserRepository
{
    public async Task<User?> AddUser(AddUserRequest user, CancellationToken cancellationToken = default)
    {
        User? ret = null;
        User? res = null;

        const string iSql = @"
INSERT INTO dbo.[User] (CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, FirstName, LastName, Username, Email, Password, EncryptedPassword)
OUTPUT INSERTED.UserId, INSERTED.CreatedBy, INSERTED.CreatedDate, INSERTED.ModifiedBy, INSERTED.ModifiedDate, INSERTED.FirstName, INSERTED.LastName,
    INSERTED.Username, INSERTED.LastLogin, INSERTED.Email, INSERTED.Password, INSERTED.EncryptedPassword
VALUES (@by, @now, @by, @now, @firstName, @lastName, @userName, @email, @password, @encryptedPassword)
";

        var iParms = new
        {
            DateTime.Now,
            By = "System",
            user.FirstName,
            user.LastName,
            user.Username,
            user.Email,
            user.Password,
            user.EncryptedPassword
        };

        using IDbConnection conn = context.Connection;
        if (conn.State != ConnectionState.Open)
            conn.Open();
        using IDbTransaction trx = conn.BeginTransaction();

        try
        {
            CommandDefinition iCmd = new(iSql, iParms, trx, 150, cancellationToken: cancellationToken);

            res = await conn.QuerySingleAsync<User>(iCmd);

            trx.Commit();
        }
        catch (Exception ex)
        {
            trx.Rollback();
            throw;
        }
        finally
        {
            conn.Close();
        }

        if (res != null)
            ret = res;

        return ret;
    }

    public async Task<bool> DeleteUser(Guid userId, CancellationToken cancellationToken = default)
    {
        int res = -1;

        const string dSql = @"
DELETE dbo.[User]
WHERE UserId = @userId
";

        var parms = new { userId };

        using IDbConnection conn = context.Connection;
        if (conn.State != ConnectionState.Open)
            conn.Open();
        using IDbTransaction trx = conn.BeginTransaction();

        try
        {
            CommandDefinition dCmd = new(dSql, parms, trx, 150, cancellationToken: cancellationToken);

            res = await conn.ExecuteAsync(dCmd);

            trx.Commit();
        }
        catch (Exception ex)
        {
            trx.Rollback();
            throw;
        }
        finally
        {
            conn.Close();
        }

        return res > 0;
    }

    public async Task<List<User>> GetAllUsers(CancellationToken cancellationToken = default)
    {
        List<User> ret = [];

        const string sSql = @"
SELECT Userid, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, FirstName, LastName, Username, Email, Password, EncryptedPassword
FROM dbo.[User]
";

        using IDbConnection conn = context.Connection;

        CommandDefinition iCmd = new(sSql, null, null, 150, cancellationToken: cancellationToken);

        IEnumerable<User> res = await conn.QueryAsync<User>(iCmd);

        if (res is not null)
            ret.AddRange(res);

        return ret;
    }

    public async Task<User?> GetUserById(Guid userId, CancellationToken cancellationToken = default)
    {
        User? ret = null;

        const string sSql = @"
SELECT Userid, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, FirstName, LastName, Username, Email, Password, EncryptedPassword
FROM dbo.[User]
WHERE UserId = @userId
";

        var parms = new { userId };

        using IDbConnection conn = context.Connection;

        CommandDefinition sCmd = new(sSql, parms, null, 150, cancellationToken: cancellationToken);

        User? res = await conn.QueryFirstOrDefaultAsync<User>(sCmd);

        if (res is not null)
            ret = res;

        return ret;
    }

    public async Task<User?> UpdateUser(UpdateUserRequest user, CancellationToken cancellationToken = default)
    {
        User? ret = null;
        int res = 0;

        const string iSql = @"
UPDATE dbo.[User]
SET ModifiedBy = @by
    , ModifiedDate = @now
    , FirstName = @firstName
    , LastName = @lastName
    , LastLogin = @lastLogin
    , Username = @userName
    , Email = @email
    , [Password] = @password
    , EncryptedPassword = @encryptedPassword
WHERE UserId = @userId
";

        var iParms = new
        {
            DateTime.Now,
            By = "System",
            user.FirstName,
            user.LastName,
            user.LastLogin,
            user.Username,
            user.Email,
            user.Password,
            user.EncryptedPassword,
            user.UserId
        };

        using IDbConnection conn = context.Connection;
        if (conn.State != ConnectionState.Open)
            conn.Open();
        using IDbTransaction trx = conn.BeginTransaction();

        try
        {
            CommandDefinition iCmd = new(iSql, iParms, trx, 150, cancellationToken: cancellationToken);

            res = await conn.ExecuteAsync(iCmd);

            trx.Commit();
        }
        catch (Exception ex)
        {
            trx.Rollback();
            throw;
        }
        finally
        {
            conn.Close();
        }

        if (res > 0)
            ret = await GetUserById(user.UserId, cancellationToken);

        return ret;
    }
}
