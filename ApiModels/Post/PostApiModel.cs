namespace animal_seller_api.ApiModels.Post;

public record PostApiModel
{
    public string Id { get; set; }
    public string Title { get; set; } 
    public string TextContent { get; set; }
    public string Images { get; set; }
    
    public string UserToken { get; set; }
}