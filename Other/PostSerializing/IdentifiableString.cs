namespace animal_seller_api.Other.PostSerializing;

public class IdentifiableString
{
    public int Id { get; set; }
    public string Content { get; set; }

    public IdentifiableString(string content)
    {
        Content = content;
    }
}