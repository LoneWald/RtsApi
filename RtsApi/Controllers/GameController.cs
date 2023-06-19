using Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace RtsApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class GameController : ControllerBase
{
    RtsDbContext _db;
    readonly JsonSerializerOptions _serializerOptions;
    public GameController(RtsDbContext db)
    {
        _db = db;
        _serializerOptions = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = false,
        };
    }

    [HttpGet("Get")]
    public async Task<IEnumerable<Game>> GetAll()
    {
        var acc = await GetAccount();
        Log($"Пользователь {acc.Id} запросил даннные обо всех играх");
        return await _db.Games.Include(e => e.Saves).Where(e => e.Account.Email == acc.Email).ToListAsync();
    }

    [HttpGet("Get/{id}")]
    public async Task<Game> Get(int id)
    {
        var acc = await GetAccount();
        Log($"Пользователь {acc.Id} запросил даннные об игре с id = {id}");
        var game = await _db.Games.Include(e => e.Saves).FirstOrDefaultAsync(e => e.Account.Email == acc.Email && e.Id == id);
        if (game == null) throw new Exception("Игра не найдена");
        return game;
    }

    [Authorize(Roles = "ADMIN, STUDENT")]
    [HttpPost("Create")]
    public async Task<Game> Create()
    {
        var acc = await GetAccount();
        var game = new Game()
        {
            Account = acc,
            CreateDate = DateTime.Now,
            State = "PLAY",
            Saves = new List<Save>(),
        };
        var initSave = new Save()
        {
            Date = game.CreateDate,
            Level = 1,
            Money = 1000,
            Order = 0,
            Bits = 0,
            MapСhanges = ""
        };
        var map = new Dictionary<string, PointInfo>();
        var random = new Random();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                var point = new PointInfo()
                {
                    Resources = random.Next(0, 50),
                    Building = null
                };
                map.Add($"{i},{j}:0", point);
            }
        }
        initSave.MapСhanges = JsonSerializer.Serialize(map, _serializerOptions);
        game.Saves.Add(initSave);
        _db.Games.Add(game);
        await _db.SaveChangesAsync();
        Log($"Пользователем {acc.Id} создана новая игра {game.Id}");
        return game;
    }

    [Authorize(Roles = "ADMIN, STUDENT")]
    [HttpPost("Save")]
    public async Task<Save> Save(SaveInfo saveInfo)
    {
        saveInfo.Validate();
        var game = await _db.Games.Include(e => e.Saves).FirstOrDefaultAsync(e => e.Id == saveInfo.GameId);
        if (game == null) throw new Exception("Игра не найдена");
        var save = new Save()
        {
            Date = DateTime.Now,
            Name = saveInfo.Name,
            Level = saveInfo.Level,
            Money = saveInfo.Money,
            Order = game.Saves.Count,
            Bits = saveInfo.Bits,
            MapСhanges = JsonSerializer.Serialize(saveInfo.MapInfo, _serializerOptions)
        };

        game.Saves.Add(save);
        game.State = string.IsNullOrEmpty(saveInfo.State) ? game.State : saveInfo.State;
        await _db.SaveChangesAsync();
        Log($"Пользователь {(await GetAccount()).Id} сохранил игру {game.Id}");
        return save;
    }
    [Authorize(Roles = "ADMIN, STUDENT")]

    [HttpGet("Load")]
    public async Task<Save> Load(int id)
    {
        var save = await _db.Saves.FirstOrDefaultAsync(e => e.Id == id);
        if (save == null) throw new Exception("Сохранение не найдено");
        Log($"Пользователь {(await GetAccount()).Id} загрузил сохранение {id}");
        return save;
    }

    async Task<Account> GetAccount()
    {
        var email = HttpContext.User.Claims.FirstOrDefault(e => e.Type == ClaimTypes.Email)?.Value;
        if (email == null) throw new Exception("Невалидный токен");
        var acc = _db.Accounts.FirstOrDefault(e => e.Email == email);
        if (acc == null) throw new Exception("Аккааунт не найден");
        return acc;
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
