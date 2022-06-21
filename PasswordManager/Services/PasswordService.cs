using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Authorization;
using PasswordManager.Entities;

namespace PasswordManager.Services;

public interface IPasswordService
{
    Task<bool> Add(Password password);
}

public class PasswordService : IPasswordService
{
    private readonly ApplicationDbContext _repository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ClaimsPrincipal _claimsPrincipal;

    public PasswordService(ApplicationDbContext repository,
        UserManager<ApplicationUser> userManager,
        ClaimsPrincipal claimsPrincipal)
    {
        _repository = repository;
        _userManager = userManager;
        _claimsPrincipal = claimsPrincipal;
    }

    public async Task<bool> Add(Password password)
    {
        var userId = _claimsPrincipal.Claims.First(e => e.Type == "Id").Value;

        password.UserId = userId;

        await _repository.Passwords.AddAsync(password);
        var changes = await _repository.SaveChangesAsync();
        return changes > 0;
    }
}