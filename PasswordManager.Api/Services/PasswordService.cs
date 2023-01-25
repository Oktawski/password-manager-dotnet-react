using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Entities;
using PasswordManager.Requests;

namespace PasswordManager.Services;

public interface IPasswordService
{
    Task<bool> AddAsync(AddPasswordRequest password);
    Task<IEnumerable<Password>> GetAllAsync();
    Task<Password?> GetByIdAsync(string id);
    Task<bool> EditByIdAsync(EditPasswordRequest editPasswordRequest);
    Task<bool> RemoveAsync(string passwordId);
}

public class PasswordService : IPasswordService
{
    private readonly ApplicationDbContext _repository;
    private readonly ClaimsPrincipal _claimsPrincipal;

    public PasswordService(ApplicationDbContext repository, ClaimsPrincipal claimsPrincipal)
    {
        _repository = repository;
        _claimsPrincipal = claimsPrincipal;
    }


    private string UserId { get => _claimsPrincipal.Claims.First(e => e.Type == "id").Value; }


    public async Task<bool> AddAsync(AddPasswordRequest request)
    {
        Password password = new()
        {
            Application = request.application,
            Login = request.login,
            Value = request.value,
            UserId = UserId 
        };

        await _repository.Passwords.AddAsync(password);
        var changes = await _repository.SaveChangesAsync();
        return changes > 0;
    }

    public async Task<IEnumerable<Password>> GetAllAsync()
    {
        var passwords = await _repository.Passwords
            .Where(e => e.UserId == UserId)
            .ToListAsync();

        return passwords;
    }

    public async Task<Password?> GetByIdAsync(string id)
    {
        var password = await _repository.Passwords.FindAsync(new Guid(id));
        if (password?.UserId == UserId)
            return password;
        
        return password;
    }

    public async Task<bool> EditByIdAsync(EditPasswordRequest request)
    {
        var password = await GetByIdAsync(request.id);
        if (password is null)
            return false;

        password.Application = request.application;
        password.Login = request.login;
        password.Value = request.value;

        var changes = await _repository.SaveChangesAsync();
        return changes > 0;
    }

    public async Task<bool> RemoveAsync(string passwordId)
    {
        var passwordToRemove = await GetByIdAsync(passwordId);
        if (passwordToRemove is null)
            return false;    

        if (UserId != passwordToRemove?.UserId)
            return false;
        
        _repository.Passwords.Remove(passwordToRemove);
        
        var changes = await _repository.SaveChangesAsync();
        return changes > 0;
    }
}
