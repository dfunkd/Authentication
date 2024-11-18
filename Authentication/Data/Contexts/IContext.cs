using System.Data;

namespace Authentication.Data.Contexts;

public interface IContext
{
    string? ConnectionString { get; }
    IDbConnection Connection { get; }
}
