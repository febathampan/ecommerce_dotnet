namespace test1app.Models;

using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }

    public string Password { get; set; }

    public User() { }
}
