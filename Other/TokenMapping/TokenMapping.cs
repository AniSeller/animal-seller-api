namespace animal_seller_api.Other.TokenMapping;

public class TokenMapping
{
    public int Id { get; set; }
    public string Token { get; set; }
    public string UserId { get; set; }

    public TokenMapping(string token, string userId)
    {
        Token = token;
        UserId = userId;
    }
}