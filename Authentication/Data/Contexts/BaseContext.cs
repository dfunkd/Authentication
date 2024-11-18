using Microsoft.Data.SqlClient;
using System.Data;

namespace Authentication.Data.Contexts;

public abstract class BaseContext(string? connectionString) : IContext
{
    public string? ConnectionString { get; } = connectionString;
    public virtual IDbConnection Connection => new SqlConnection(ConnectionString);
}
