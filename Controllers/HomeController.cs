namespace Controllers;

using System.Text;
using Models;
using DbContexts;
using Other.Passwords;
using Microsoft.AspNetCore.Mvc;

[Route("/")]
public class HomeController : ControllerBase
{
    [HttpGet]
    public IActionResult DafaultPage()
    {
        return Ok("Hello!");
    }
}