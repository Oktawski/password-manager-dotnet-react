using System.Security.Claims;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Dtos;
using PasswordManager.Models;

namespace PasswordManager.Services;

public interface IPasswordService
{
    Task<bool> AddAsync(PasswordCreateDto createDto);
    Task<IEnumerable<PasswordReadDto>> GetAllAsync();
    Task<Password?> GetByIdAsync(Guid id);
    Task<bool> EditByIdAsync(PasswordEditDto editDto);
    Task<bool> RemoveAsync(Guid passwordId);
}

public class PasswordService : ClaimsService, IPasswordService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public PasswordService(
        ApplicationDbContext context, 
        ClaimsPrincipal claimsPrincipal,
        IMapper mapper)
        : base (claimsPrincipal)
    {
        _context = context;
        _mapper = mapper;
    }


    public async Task<bool> AddAsync(PasswordCreateDto createDto)
    {
        var password = _mapper.Map<PasswordCreateDto, Password>(createDto);

        password.UserId = UserId;

        await _context.Passwords.AddAsync(password);
        var changes = await _context.SaveChangesAsync();
        return changes > 0;
    }

    public async Task<IEnumerable<PasswordReadDto>> GetAllAsync()
    {
        var passwords = await _context.Passwords
            .Where(e => e.UserId == UserId)
            .ToListAsync();

        return _mapper.Map<List<PasswordReadDto>>(passwords);
    }

    public async Task<Password?> GetByIdAsync(Guid id)
    {
        var password = await _context.Passwords.FindAsync(id);
        if (password?.UserId == UserId)
            return password;
        
        return password;
    }

    public async Task<bool> EditByIdAsync(PasswordEditDto editDto)
    {
        var password = await GetByIdAsync(editDto.Id);
        if (password is null)
            return false;


        password.Application = editDto.Application;
        password.Login = editDto.Login;
        password.Value = editDto.Value;

        var changes = await _context.SaveChangesAsync();
        return changes > 0;
    }

    public async Task<bool> RemoveAsync(Guid passwordId)
    {
        var passwordToRemove = await GetByIdAsync(passwordId);
        if (passwordToRemove is null)
            return false;    

        if (UserId != passwordToRemove?.UserId)
            return false;
        
        _context.Passwords.Remove(passwordToRemove);
        
        var changes = await _context.SaveChangesAsync();
        return changes > 0;
    }
}
