using System.Diagnostics;
using System.Collections;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Authorization;
using PasswordManager.Entities;
using PasswordManager.Requests;

namespace PasswordManager.Services;

public interface IPasswordService
{
    Task<bool> Add(AddPasswordRequest password);
    Task<IEnumerable<Password>> GetAll();
    Task<Password?> GetById(string id);
    Task<bool> Remove(Guid passwordId);
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

    public async Task<bool> Add(AddPasswordRequest request)
    {
        var userId = GetUserId();

        Password password = new()
        {
            Application = request.application,
            Value = request.value,
            UserId = userId
        };

        await _repository.Passwords.AddAsync(password);
        var changes = await _repository.SaveChangesAsync();
        return changes > 0;
    }

    public async Task<IEnumerable<Password>> GetAll()
    {
        var userId = GetUserId();
        var passwords = await _repository.Passwords
            .Where(e => e.UserId == userId)
            .ToListAsync();

        return passwords;
    }

    public async Task<Password?> GetById(string id)
    {
        var userId = GetUserId();

        var password = await _repository.Passwords.FindAsync(new Guid(id));
        if (password?.UserId == userId)
            return password;
        
        return password;
    }

    public async Task<bool> Remove(Guid passwordId)
    {
        var userId = GetUserId();
        var passwordToRemove = await _repository.Passwords.FindAsync(passwordId);

        if (passwordToRemove is null)
            return false;    

        if (userId == passwordToRemove?.UserId)
        {
            _repository.Passwords.Remove(passwordToRemove);

            var changes = await _repository.SaveChangesAsync();

            return changes > 0;
        }

        return false;
    }

    private string GetUserId() => _claimsPrincipal.Claims.First(e => e.Type == "id").Value;
}