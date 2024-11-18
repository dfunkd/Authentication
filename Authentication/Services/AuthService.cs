using Authentication.Core.Helpers;
using Authentication.Data.Repositories;
using Authentication.Models;

namespace Authentication.Services;

public interface IAuthService
{
    Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model, CancellationToken cancellationToken = default);
}

public class AuthService(IConfiguration configuration, IAuthRepository repo) : IAuthService
{
    private readonly AppSettings appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();

    public async Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model, CancellationToken cancellationToken = default)
    {
        User? user = await repo.GetUser(model.Username, model.Password, cancellationToken);

        // return null if user not found
        if (user == null)
            return null;

        // authentication successful so generate jwt token
        var token = await JwtToken.GenerateJwtToken(user, appSettings);

        return new(user, token);
    }
}
