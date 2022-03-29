using PasswordManager.Requests;
using PasswordManager.Responses;

namespace PasswordManager.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest request);
        RegisterResponse Register(RegisterRequest request);
    }

    public class UserService : IUserService
    {
        private readonly PasswordManagerContext _repository;

        public UserService(PasswordManagerContext repository)
        {
            _repository = repository;
        }


        public AuthenticateResponse Authenticate(AuthenticateRequest request)
        {
            throw new NotImplementedException();
        }


        public RegisterResponse Register(RegisterRequest request)
        {
            throw new NotImplementedException();
        }
    }
}