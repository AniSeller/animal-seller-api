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

    private UserDbContext context;

    public InitialController(UserDbContext dbContext)
    {
        context = dbContext;
    }

    [HttpPost("register")]
    public ActionResult Register([FromBody] UserRegisterApiModel user)
    {
        
        if (context.Users.OrderBy(e => e.Id).LastOrDefault(u => u.Name == user.Name && u.Password == user.Password) != null)
            return Problem("User already exists!");
        
        if (!PasswordChecker.Check(user.Password)) 
            return Problem("Incorrect password!");

        var newUser = new User(user.Name, user.Login, user.Password, new Random().NextInt64().ToString());
        context.Users.Add(newUser);
        context.UserTokens.Add(GenerateTokenMapping(newUser));
        context.SaveChanges();

        return Ok();
    }

    [HttpPost("login")]
    public ActionResult Login([FromBody] UserLoginApiModel user)
    {
        var foundUser = context.Users.OrderBy(e => e.Id)
            .FirstOrDefault(u => u.Login == user.Login && u.Password == user.Password);
        
        if (foundUser is null)
            return Problem("Invalid login / password");

        var tokenAttached = GenerateTokenMapping(foundUser);
        var oldTokenMapping = context.UserTokens.FirstOrDefault(oldTMap => oldTMap.UserId == foundUser.Id);

        if (!(oldTokenMapping is null))
        {
            context.UserTokens.Remove(oldTokenMapping);
            Console.WriteLine($"Found old mapping: {oldTokenMapping.UserId}, {oldTokenMapping.Token}");
        }
        
        context.UserTokens.Add(tokenAttached);
        context.SaveChanges();
        

        return Ok(new UserTokenPair(foundUser, tokenAttached.Token));
    }

    [HttpGet("users")]
    public ActionResult<List<User>> Users()
    {
        return context.Users.ToList();
    }

    [HttpGet("tokens")]
    public ActionResult<List<UserIdTokenPair>> UserTokens()
    {
        return context.UserTokens.ToList();
    }

    private UserIdTokenPair GenerateTokenMapping(User user)
    {
        var token = new StringBuilder();
        for (int i = 0; i < 16; i++) 
            token.Append((char)new Random().Next(97, 123));

        return new UserIdTokenPair(user.Id, token.ToString());
    }
    
}