using Bank.Auth.DbAccess;
using Bank.Auth.Features.Auth.Models;

namespace Bank.Auth.Features.Auth.Services
{

    public interface IUserService
    {
        Task<RegisterResultModel> Register(string username, string password);
    }

    public class UserService : IUserService
    {
        private AppDbContext _db;
        public UserService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<RegisterResultModel> Register(string username, string password)
        {
            RegisterResultModel rrm = new RegisterResultModel();
            await Task.FromResult(rrm);
            return rrm;
        }
    }
}
