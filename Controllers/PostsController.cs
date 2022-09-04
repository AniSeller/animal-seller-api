using animal_seller_api.ApiModels.Post;

namespace Controllers;
using System.Text;
using animal_seller_api.Other.TokenMapping;
using Models;
using DbContexts;
using Other.Passwords;
using Microsoft.AspNetCore.Mvc;

[Route("/")]
public class PostsController : ControllerBase
{
    private UserDbContext context;

    public PostsController(UserDbContext c) => context = c;
    
    [HttpPost("createPost")]
    public ActionResult CreatePost([FromBody] PostApiModel post)
    {
        var tokenMapping = context.UserTokens.FirstOrDefault(tokenUser => tokenUser.Token == post.UserToken);
        if (tokenMapping is null)
            return Problem("Invalid token!");

        var user = context.Users.FirstOrDefault(u => u.Id == tokenMapping.UserId);
        if (user is null)
            return Problem("Invalid user!");

        user.Posts.Add(Unpack(post));
        context.Users.Remove(context.Users.FirstOrDefault(u => u.Id == tokenMapping.UserId));
        context.Users.Add(user);
        
        return Ok();
    }

    public Post Unpack(PostApiModel post)
    {
        return new Post
        {
            Id = new Random().NextInt64().ToString(),
            Title = post.Title,
            TextContent = post.TextContent,
            Images = post.Images
        };
    }
}