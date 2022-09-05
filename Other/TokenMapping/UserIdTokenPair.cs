namespace animal_seller_api.Other.TokenMapping;

public class UserIdTokenPair
{
    public int Id { get; set; }
    public string Token { get; set; }
    public string UserId { get; set; }

    public UserIdTokenPair(string userId, string token)
    {
        Token = token;
        UserId = userId;
    }
}