namespace Controllers;

using System.Text;
using animal_seller_api.Other.TokenMapping;
using Models;
using DbContexts;
using Other.Passwords;
using Microsoft.AspNetCore.Mvc;

[Route("/")]
public class InitialController : ControllerBase
{

    private DatabaseContext _databaseContext;

    public InitialController(DatabaseContext dbDatabaseContext)
    {
        _databaseContext = dbDatabaseContext;
    }

    [HttpPost("register")]
    public ActionResult Register([FromBody] UserRegisterApiModel user)
    {
        
        if (_databaseContext.Users.OrderBy(e => e.Id).LastOrDefault(u => u.Name == user.Name && u.Password == user.Password) != null)
            return Problem("User already exists!");
        
        if (!PasswordChecker.Check(user.Password)) 
            return Problem("Incorrect password!");

        var newUser = new User(user.Name, user.Login, user.Password, new Random().NextInt64().ToString());
        _databaseContext.Users.Add(newUser);

        var tokenPair = GenerateTokenMapping(newUser);
        _databaseContext.UserTokens.Add(tokenPair);
        _databaseContext.SaveChanges();

        return Ok(tokenPair.Token);
    }

    [HttpPost("login")]
    public ActionResult Login([FromBody] UserLoginApiModel user)
    {
        var foundUser = _databaseContext.Users.OrderBy(e => e.Id)
            .FirstOrDefault(u => u.Login == user.Login && u.Password == user.Password);
        
        if (foundUser is null)
            return Problem("Invalid login / password");

        var tokenMapping = _databaseContext.UserTokens.FirstOrDefault(oldTMap => oldTMap.UserId == foundUser.Id);
        if (tokenMapping is null)
            return Problem("Not registered");
        
        tokenMapping.Token = GenerateTokenMapping(foundUser).Token;
        _databaseContext.SaveChanges();

        return Ok(new UserTokenPair(foundUser, tokenMapping.Token));
    }

    [HttpGet("users")]
    public ActionResult<List<User>> Users()
    {
        return _databaseContext.Users.ToList();
    }

    [HttpGet("tokens")]
    public ActionResult<List<UserIdTokenPair>> UserTokens()
    {
        return _databaseContext.UserTokens.ToList();
    }

    private UserIdTokenPair GenerateTokenMapping(User user)
    {
        var token = new StringBuilder();
        for (int i = 0; i < 16; i++) 
            token.Append((char)new Random().Next(97, 123));

        return new UserIdTokenPair(user.Id, token.ToString());
    }
    
}