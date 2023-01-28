using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Models;
using PasswordManager.Requests;
using PasswordManager.Data;

namespace PasswordManager.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PasswordController : ControllerBase
{
    private readonly IPasswordRepo _service;

    public PasswordController(IPasswordRepo service)
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
    public async Task<Password?> GetById(string id) => await _service.GetByIdAsync(id);

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddPasswordRequest request) 
    {
        var added = await _service.AddAsync(request);

        return added 
            ? Ok("Password added") 
            : BadRequest("Something went wrong");
    }

    [HttpPost("edit")]
    public async Task<IActionResult> Edit([FromBody] EditPasswordRequest request)
    {
        var edited = await _service.EditByIdAsync(request);

        return edited 
            ? Ok("Password upadted")
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