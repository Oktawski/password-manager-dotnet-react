using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Entities;
using PasswordManager.Requests;
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
    public async Task<IEnumerable<Password>> GetAll() => await _service.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<Password?> GetById(string id) => await _service.GetByIdAsync(id);

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddPasswordRequest request) 
    {
        var added = await _service.AddAsync(request);

        return added 
            ? Ok("Password added") 
            : BadRequest("Something went wrong");
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Remove(string id)
    {
        var removed = await _service.RemoveAsync(id);

        return removed 
            ? Ok("Password removed") 
            : BadRequest("Password does not exist or something went wrong");
    }
}