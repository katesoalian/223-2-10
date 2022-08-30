namespace Core.Models;

public class Student : IModel
{
    public string Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? GroupId { get; set; }

    public virtual Group? Group { get; set; }
    
    public virtual ICollection<Student_Point> StudentPoints { get; set; }
}