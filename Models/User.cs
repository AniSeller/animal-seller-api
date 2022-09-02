namespace Models;
using Other.Passwords;

public class User
{
    public string Name { get; set; }
    public string Password { get; set; }

    public readonly string id;

    public User(string name, string password, string uid)
    {
        Name = name;
        Password = password;

        id = uid;
    }
}