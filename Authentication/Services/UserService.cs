using Authentication.Data.Repositories;
using Authentication.Models;
using Authentication.Models.Requests;

namespace Authentication.Services;

public interface IUserService
{
    Task<User?> AddUser(AddUserRequest user, CancellationToken cancellationToken = default);
    Task<bool> DeleteUser(Guid userId, CancellationToken cancellationToken = default);
    Task<List<User>> GetAllUsers(CancellationToken cancellationToken = default);
    Task<User?> GetUserById(Guid userId, CancellationToken cancellationToken = default);
    Task<User?> UpdateUser(UpdateUserRequest user, CancellationToken cancellationToken = default);
}

public class UserService(IConfiguration configuration, IUserRepository repo) : IUserService
{
    public async Task<User?> AddUser(AddUserRequest user, CancellationToken cancellationToken = default)
        => await repo.AddUser(user, cancellationToken);

    public async Task<bool> DeleteUser(Guid userId, CancellationToken cancellationToken = default)
        => await repo.DeleteUser(userId, cancellationToken);

    public async Task<List<User>> GetAllUsers(CancellationToken cancellationToken = default)
        => await repo.GetAllUsers(cancellationToken);

    public async Task<User?> GetUserById(Guid userId, CancellationToken cancellationToken = default)
        => await repo.GetUserById(userId, cancellationToken);

    public async Task<User?> UpdateUser(UpdateUserRequest user, CancellationToken cancellationToken = default)
        => await repo.UpdateUser(user, cancellationToken);
}
