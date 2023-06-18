using Contracts;
using System.Data;
using System.Text.Json.Serialization;

namespace RtsApi;

public class PointInfo : IValidatable
{
    public BuildingInfo? Building { get; set; }
    public double Resources { get; set; }
    [JsonIgnore]
    public bool IsValid
    {
        get { try { this.Validate(); return true; } catch { return false; } }
    }

    public void Validate()
    {
        
    }
}
