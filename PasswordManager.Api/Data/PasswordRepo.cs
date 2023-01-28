using System.Security.Claims;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Dtos;
using PasswordManager.Models;

namespace PasswordManager.Data;

public interface IPasswordRepo
{
    Task<bool> AddAsync(PasswordCreateDto createDto);
    Task<IEnumerable<PasswordReadDto>> GetAllAsync();
    Task<Password?> GetByIdAsync(Guid id);
    Task<bool> EditByIdAsync(PasswordEditDto editDto);
    Task<bool> RemoveAsync(Guid passwordId);
}

public class PasswordRepo : IPasswordRepo
{
    private readonly ApplicationDbContext _repository;
    private readonly ClaimsPrincipal _claimsPrincipal;
    private readonly IMapper _mapper;

    public PasswordRepo(
        ApplicationDbContext repository, 
        ClaimsPrincipal claimsPrincipal,
        IMapper mapper)
    {
        _repository = repository;
        _claimsPrincipal = claimsPrincipal;
        _mapper = mapper;
    }


    private string UserId { get => _claimsPrincipal.Claims.First(e => e.Type == "id").Value; }


    public void Test()
    {
        var buba = _repository.Passwords.Where(e => e.Id == new Guid())
        .ToList();
    }


    public async Task<bool> AddAsync(PasswordCreateDto createDto)
    {
        var password = _mapper.Map<PasswordCreateDto, Password>(createDto);

        password.UserId = UserId;

        await _repository.Passwords.AddAsync(password);
        var changes = await _repository.SaveChangesAsync();
        return changes > 0;
    }

    public async Task<IEnumerable<PasswordReadDto>> GetAllAsync()
    {
        var passwords = await _repository.Passwords
            .Where(e => e.UserId == UserId)
            .ToListAsync();

        return _mapper.Map<List<PasswordReadDto>>(passwords);
    }

    public async Task<Password?> GetByIdAsync(Guid id)
    {
        var password = await _repository.Passwords.FindAsync(id);
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

        var changes = await _repository.SaveChangesAsync();
        return changes > 0;
    }

    public async Task<bool> RemoveAsync(Guid passwordId)
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
