using Contracts;
using System.Data;

namespace RtsApi;

public class SaveInfo : IValidatable
{
    public int GameId{ get; set; }
    public string Name{ get; set; }
    public double Level { get; set; }
    public double Money { get; set; }
    public double Bits { get; set; }
    public Dictionary<string, PointInfo> MapInfo { get; set; }
    public string? State{ get; set; }

    public bool IsValid
    {
        get { try { this.Validate(); return true; } catch { return false; } }
    }
    public void Validate()
    {
        if (GameId <= 0) throw new Exception("Неверный Id игры");
        foreach (var point in MapInfo) point.Value.Validate();
    }
}
