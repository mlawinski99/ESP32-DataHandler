using System.ComponentModel.DataAnnotations;

namespace ESP32SensorSystem.SaveToDb.Entities;

public abstract class Entity
{
    [Key]
    public long Id { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}