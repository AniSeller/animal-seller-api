using System.Diagnostics;
using animal_seller_api.ApiModels.Post;
using animal_seller_api.Other.PostSerializing;

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
    private DatabaseContext context;

    public PostsController(DatabaseContext c) => context = c;
    
    [HttpPost("createPost")]
    public ActionResult CreatePost([FromBody] PostApiModel post)
    {
        var tokenMapping = context.UserTokens.FirstOrDefault(tokenUser => tokenUser.Token == post.UserToken);
        if (tokenMapping == null)
            return Problem("Invalid access token!");

        var user = context.Users.FirstOrDefault(u => u.Id == tokenMapping.UserId);
        if (user == null)
            return Problem("Invalid access token!");

        var postModel = Unpack(post, user);

        if (user.PostIds == "[]")
            user.PostIds = "[" + '"' + $"{postModel.Id}" + '"' + "]";
        
        else
        {
            user.PostIds = user.PostIds.Substring(0, user.PostIds.Length - 1);
            user.PostIds += ", " + '"' + postModel.Id + '"' + "]";
        }
        
        context.Posts.Add(postModel);
        context.SaveChanges();
        
        return Ok();
    }

    [HttpGet("posts@all")]
    public ActionResult GetAllPosts()
    {
        return Ok(context.Posts.ToList());
    }

    public Post Unpack(PostApiModel post, User user)
    {
        return new Post
        {
            Id = new Random().NextInt64().ToString(),
            UserId = user.Id,
            Title = post.Title,
            TextContent = post.TextContent,
            Images = post.Images
        };
    }
}