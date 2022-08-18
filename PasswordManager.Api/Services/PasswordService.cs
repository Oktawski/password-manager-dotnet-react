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

    public async Task<bool> AddAsync(AddPasswordRequest request)
    {
        var userId = GetUserId();

        Password password = new()
        {
            Application = request.application,
            Login = request.login,
            Value = request.value,
            UserId = userId
        };

        await _repository.Passwords.AddAsync(password);
        var changes = await _repository.SaveChangesAsync();
        return changes > 0;
    }

    public async Task<IEnumerable<Password>> GetAllAsync()
    {
        var userId = GetUserId();
        var passwords = await _repository.Passwords
            .Where(e => e.UserId == userId)
            .ToListAsync();

        return passwords;
    }

    public async Task<Password?> GetByIdAsync(string id)
    {
        var userId = GetUserId();

        var password = await _repository.Passwords.FindAsync(new Guid(id));
        if (password?.UserId == userId)
            return password;
        
        return password;
    }

    public async Task<bool> EditByIdAsync(EditPasswordRequest request)
    {
        var userId = GetUserId();

        var password = await FindPasswordById(request.id);
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
        var userId = GetUserId();

        var passwordToRemove = await FindPasswordById(passwordId);
        if (passwordToRemove is null)
            return false;    

        if (userId != passwordToRemove?.UserId)
            return false;
        
        _repository.Passwords.Remove(passwordToRemove);
        
        var changes = await _repository.SaveChangesAsync();
        return changes > 0;
    }

    private string GetUserId() => _claimsPrincipal.Claims.First(e => e.Type == "id").Value;

    private async Task<Password?> FindPasswordById(string id) => 
        await _repository.Passwords.FindAsync(new Guid(id));
}