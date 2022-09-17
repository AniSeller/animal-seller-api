using animal_seller_api.ApiModels.Post;
using Microsoft.AspNetCore.Authorization;
using Models;
using DbContexts;
using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[Route("/")]
public class PostsController : ControllerBase
{
    private DatabaseContext context;

    public PostsController(DatabaseContext c) => context = c;
    
    [HttpPost("createPost")]
    [Authorize]
    public ActionResult CreatePost([FromBody] PostApiModel post)
    {
        // var tokenMapping = context.UserTokens.FirstOrDefault(tokenUser => tokenUser.Token == post.UserToken);
        // if (tokenMapping == null)
        //     return Problem("Invalid access token!");
        
        var jwtId = User.FindFirst("UserId").Value;
        
        var user = context.Users.FirstOrDefault(u => u.Id == jwtId);
        if (user == null)
            return Problem("Invalid token!");

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