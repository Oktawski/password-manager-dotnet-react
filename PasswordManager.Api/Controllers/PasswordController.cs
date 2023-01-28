using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Models;
using PasswordManager.Requests;
using PasswordManager.Data;
using PasswordManager.Dtos;

namespace PasswordManager.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PasswordController : ControllerBase
{
    private readonly IPasswordRepo _repository;

    public PasswordController(IPasswordRepo repository)
    {
        _repository = repository;
    }

    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<Password>>> GetAll() 
    {
        var passwords = await _repository.GetAllAsync();
        return passwords.Count() > 0
            ? Ok(passwords)
            : NotFound(passwords);
    }

    [HttpGet("{id}")]
    public async Task<Password?> GetById(string id) => await _repository.GetByIdAsync(new Guid(id));

    [HttpPost]
    public async Task<IActionResult> Add(PasswordCreateDto createDto) 
    {
        var added = await _repository.AddAsync(createDto);

        return added 
            ? Ok("Password added") 
            : BadRequest("Something went wrong");
    }

    [HttpPost("edit")]
    public async Task<IActionResult> Edit(PasswordEditDto editDto)
    {
        var edited = await _repository.EditByIdAsync(editDto);

        return edited 
            ? Ok("Password upadted")
            : BadRequest("Something went wrong");
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Remove(string id)
    {
        var removed = await _repository.RemoveAsync(new Guid(id));

        return removed 
            ? Ok("Password removed") 
            : BadRequest("Password does not exist or something went wrong");
    }
}