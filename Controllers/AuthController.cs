using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace Controllers;

using System.Text;
using Models;
using DbContexts;
using Other.Passwords;
using Microsoft.AspNetCore.Mvc;

[Route("/")]
public class InitialController : ControllerBase
{

    private DatabaseContext _databaseContext;
    private IConfiguration _configuration;

    public InitialController(DatabaseContext dbContext, IConfiguration configuration)
    {
        _databaseContext = dbContext;
        _configuration = configuration;
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
        _databaseContext.SaveChanges();

        var (tokenHandler, token) = GenerateToken(newUser);
        
        return Ok(tokenHandler.WriteToken(token));
    }

    [HttpPost("login")]
    public ActionResult Login([FromBody] UserLoginApiModel user)
    {
        var foundUser = _databaseContext.Users.OrderBy(e => e.Id)
            .FirstOrDefault(u => u.Login == user.Login && u.Password == user.Password);
        
        if (foundUser is null)
            return Problem("Not registered");

        _databaseContext.SaveChanges();

        var (tokenHandler, token) = GenerateToken(foundUser);
        
        return Ok(tokenHandler.WriteToken(token));
    }

    [HttpGet("users")]
    public ActionResult<List<User>> Users()
    {
        return _databaseContext.Users.ToList();
    }
    
    private Tuple<JwtSecurityTokenHandler, SecurityToken> GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("UserId", user.Id)
            }),
            Audience = _configuration["Jwt:Audience"],
            Issuer = _configuration["Jwt:Issuer"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
        };
        
        var token = tokenHandler.CreateToken(descriptor);
        
        return new Tuple<JwtSecurityTokenHandler, SecurityToken>(tokenHandler, token);
    }
    
}