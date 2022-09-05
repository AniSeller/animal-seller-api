using Models;

namespace animal_seller_api.Other.TokenMapping;

public class UserTokenPair
{
    public int Id { get; set; }
    public string Token { get; set; }
    public User User { get; set; }

    public UserTokenPair(User userId, string token)
    {
        Token = token;
        User = userId;
    }
}