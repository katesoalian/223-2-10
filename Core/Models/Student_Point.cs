namespace Core.Models;

public class Student_Point : IModel
{
    
    public string Id { get; set; } = null!;
    public string? StudentId { get; set; } = null!;
    public string PointId { get; set; } = null!;

    public virtual Points Points { get; set; } = null!;
    public virtual Student Student { get; set; } = null!;
}