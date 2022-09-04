using Microsoft.VisualBasic.CompilerServices;

namespace Models;
using Microsoft.AspNetCore;
using Other.Passwords;

public class User
{
    public string Name { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string Id { get; set; }

    public List<Post> Posts { get; set; } = new List<Post>();

    public User(string name, string login, string password, string id)
    {
        Name = name;
        Password = password;
        Login = login;

        Id = id;
    }
}