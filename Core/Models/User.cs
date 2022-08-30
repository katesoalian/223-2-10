namespace Core.Models;

public class User : IModel
{
    public string Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
}