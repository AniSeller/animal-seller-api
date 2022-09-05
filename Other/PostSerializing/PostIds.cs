using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace animal_seller_api.Other.PostSerializing;

public class PostIds
{
    public int Id { get; set; }
    public List<int> Ids { get; set; } = new List<int>();

    public void Add(int id)
    {
        Ids.Add(id);
    }
}