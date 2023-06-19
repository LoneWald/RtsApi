using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Database;
using Contracts;

namespace RtsApi.Controllers;
[Authorize]
[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    RtsDbContext _db;
    public AccountController(RtsDbContext db) : base()
    {
        _db = db;
    }
    [Authorize(Roles = "ADMIN, TEACHER")]
    [HttpGet("Get")]
    public async Task<IEnumerable<Account>> GetAll()
    {
        var email = HttpContext.User.Claims.FirstOrDefault(e => e.Type == ClaimTypes.Email)?.Value;
        var user = _db.Accounts.FirstOrDefault(e => e.Email == email);
        Log($"Пользователь {user.Id} запросил даннные обо всех аккаунтах");
        return await _db.Accounts.ToListAsync();
    }

    [Authorize(Roles = "ADMIN, TEACHER")]
    [HttpGet("Get/{id}")]
    public async Task<Account> Get(int id)
    {
        var email = HttpContext.User.Claims.FirstOrDefault(e => e.Type == ClaimTypes.Email)?.Value;
        var user = _db.Accounts.FirstOrDefault(e => e.Email == email);
        Log($"Пользователь {user.Id} запросил даннные об аккаунте с id  = {id}");
        var acc = await _db.Accounts.FirstOrDefaultAsync(e => e.Id == id);
        if (acc == null)
            throw new Exception("Аккаунт не найден");

        return acc;
    }

    [AllowAnonymous]
    [HttpPost("Create")]
    public async Task<Account> Create(AccountCreateInfo data)
    {
        data.Validate();
        var acc = _db.Accounts.FirstOrDefault(e => e.Email == data.Email);
        if (acc is not null)
            throw new Exception("Аккаунт с указанным Email уже существует");
        var account = new Account()
        {
            Email = data.Email,
            Name = data.Name,
            Password = data.Password,
            Role = RolesParser.Parse(data.Role).ToString(),
            IsActive = true
        };
        await _db.Accounts.AddAsync(account);
        await _db.SaveChangesAsync();

        Log($"Зарегесрирован новый аккаунт {account.Email}");
        return account;
    }

    [HttpPut("Update")]
    public async Task<Account> Update(AccountUpdateInfo data)
    {
        data.Validate();
        var edit = await _db.Accounts.FirstOrDefaultAsync(e => e.Id == data.Id);
        if (edit == null)
            throw new Exception("Изменяемый аккаунт не найден");

        if (string.IsNullOrEmpty(data.Email)) edit.Email = data.Email;
        if (string.IsNullOrEmpty(data.Name)) edit.Name = data.Name;
        if (string.IsNullOrEmpty(data.Password)) edit.Password = data.Password;
        if (data.IsActive is not null) edit.IsActive = data.IsActive.Value;
        await _db.SaveChangesAsync();
        Log($"Данные акаунта {edit.Id} были изменены");
        return edit;
    }

    async Task Log(string str)
    {
        var log = new Log()
        {
            Date = DateTime.Now,
            Text = str
        };
        _db.Logs.Add(log);
        await _db.SaveChangesAsync();
    }
}