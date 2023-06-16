using Contracts;
using Database;
using Microsoft.AspNetCore.Mvc;
using RtsApi;
using RtsApi.Extensions;
using RtsApi.Models;
using System.Security.Claims;

namespace LemonApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthorizationController : ControllerBase
{
    readonly RtsDbContext _db;
    readonly JWTConfig _jwtConfig;
    public AuthorizationController(RtsDbContext db, JWTConfig jwtConfig) : base()
    {
        _db = db;
        _jwtConfig = jwtConfig;
    }

    [HttpPost("Login")]
    public async Task<LoginResponse> Login(LoginModel login)
    {
        login.Validate();
        var acc = _db.Accounts.FirstOrDefault(e => e.Email == login.Email && e.Password == login.Password);
        if (acc is null)
            throw new Exception("Неврные логин или пароль");
        var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, acc.Name),
                new Claim(ClaimTypes.Email, acc.Email),
                new Claim(ClaimTypes.Role, (RolesParser.Parse(acc.Role)).ToString())
            };

        return new LoginResponse { Account = acc, Token = JWTExtansion.GetToken(claims, _jwtConfig) };
    }
}
