using System;
namespace Other.Passwords;

public static class PasswordChecker
{
    public static bool Check(string password) => password.Count() >= 6;
}