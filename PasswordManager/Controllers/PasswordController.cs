using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
}