using System.Collections;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Authorization;
using PasswordManager.Entities;

namespace PasswordManager.Services;

public interface IPasswordService
{
    Task<bool> Add(Password password);
    Task<IEnumerable> GetAll();
}

public class PasswordService : IPasswordService
{
    private readonly ApplicationDbContext _repository;
    private readonly ClaimsPrincipal _claimsPrincipal;

    public PasswordService(ApplicationDbContext repository,
        ClaimsPrincipal claimsPrincipal)
    {
        _repository = repository;
        _claimsPrincipal = claimsPrincipal;
    }

    public async Task<bool> Add(Password password)
    {
        var userId = GetUserId();

        password.UserId = userId;

        await _repository.Passwords.AddAsync(password);
        var changes = await _repository.SaveChangesAsync();
        return changes > 0;
    }

    public async Task<IEnumerable> GetAll()
    {
        var userId = GetUserId();
        var passwords = await _repository.Passwords.Where(e => e.UserId == userId).ToListAsync();

        return passwords;
    }

    private string GetUserId() => _claimsPrincipal.Claims.First(e => e.Type == "Id").Value;
}