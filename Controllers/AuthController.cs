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
            return Problem("Not registered");

        var tokenAttached = GenerateTokenMapping(foundUser);
        var oldTokenMapping = context.UserTokens.FirstOrDefault(oldTMap => oldTMap.UserId == tokenAttached.UserId);

        if (!(oldTokenMapping is null))
            context.Remove(oldTokenMapping);
        
        context.UserTokens.Add(tokenAttached);
        context.SaveChanges();
        

        return Ok(new UserTokenPair(foundUser, tokenAttached.Token));
    }

    [HttpGet("bigGreet")]
    public ActionResult<List<User>> BigGreetingResult()
    {
        return context.Users.ToList();
    }

    private TokenMapping GenerateTokenMapping(User user)
    {
        var token = new StringBuilder();
        for (int i = 0; i < 16; i++) 
            token.Append((char)new Random().Next(97, 123));

        return new TokenMapping(token.ToString(), user.Id);
    }
    
}