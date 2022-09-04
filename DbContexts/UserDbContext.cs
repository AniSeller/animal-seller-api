using animal_seller_api.Other.TokenMapping;

namespace DbContexts;

using Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class UserDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<TokenMapping> UserTokens { get; set; }
    private string DbPath { get; set; }
    
    public UserDbContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        DbPath = Path.Join(Environment.GetFolderPath(folder), "userDatabase.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={DbPath}");
    }
}