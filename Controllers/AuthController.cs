namespace Controllers;

using Models;
using DbContexts;
using Other.Passwords;
using Microsoft.AspNetCore.Mvc;

[Route("/")]
public class InitialController : ControllerBase
{
    [HttpPost("register")]
    public ActionResult Register(UserRegisterApiModel user)
    {
        var userDb = new UserDbContext();
        
        if (userDb.Users.Last(u => u.Name == user.Name && u.Password == user.Password) != null)
            return Problem("User already exists!");
        
        if (!PasswordChecker.Check(user.Password)) 
            return Problem("Incorrect password!");
        
        userDb.Add(new User(user.Name, user.Password, new Random().NextInt64().ToString()));
        
        userDb.SaveChanges();

        return Ok();
    }

    [HttpGet("bigGreet")]
    public ActionResult<string> BigGreetingResult()
    {
        return "Hello, my dear friend!";
    }
    
}