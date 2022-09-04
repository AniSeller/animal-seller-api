using Models;

namespace animal_seller_api.Other.TokenMapping;

public class UserTokenPair
{
    public User User { get; set; }
    public string Token { get; set; }

    public UserTokenPair(User usr, string token)
    {
        User = usr;
        Token = token;
    }
}