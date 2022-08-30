namespace Core.Models;

public class Points : IModel
{
    public string Id { get; set; }
    public string? Subject { get; set; }
    public byte? Point { get; set; }

    public virtual ICollection<Student_Point> StudentPoints { get; set; }
}