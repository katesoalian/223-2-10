namespace Core.Models;

public class Group : IModel
{
    public string Id { get; set; } = null!;
    public string? Name { get; set; }
    public byte? Course { get; set; }
    public string? ProfessorId { get; set; }

    public virtual Professor? Professor { get; set; }
    public virtual ICollection<Student> Students { get; set; }
}