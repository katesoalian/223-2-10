namespace Core.Models;

public class Professor : IModel
{
    public string Id { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Subject { get; set; }

    public virtual ICollection<Group> Groups { get; set; }
}