using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Models;
using PasswordManager.Dtos;
using PasswordManager.Services;

namespace PasswordManager.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PasswordController : ControllerBase
{
    private readonly IPasswordService _service;

    public PasswordController(IPasswordService service)
    {
        _service = service;
    }

    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<Password>>> GetAll() 
    {
        var passwords = await _service.GetAllAsync();
        return passwords.Count() > 0
            ? Ok(passwords)
            : NotFound(passwords);
    }

    [HttpGet("{id}")]
    public async Task<Password?> GetById(string id) => await _service.GetByIdAsync(new Guid(id));

    [HttpPost]
    public async Task<IActionResult> Add(PasswordCreateDto createDto) 
    {
        var added = await _service.AddAsync(createDto);

        return added 
            ? Ok("Password added") 
            : BadRequest("Something went wrong");
    }

    [HttpPost("edit")]
    public async Task<IActionResult> Edit(PasswordEditDto editDto)
    {
        var edited = await _service.EditByIdAsync(editDto);

        return edited 
            ? Ok("Password upadted")
            : BadRequest("Something went wrong");
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Remove(string id)
    {
        var removed = await _service.RemoveAsync(new Guid(id));

        return removed 
            ? Ok("Password removed") 
            : BadRequest("Password does not exist or something went wrong");
    }
}
