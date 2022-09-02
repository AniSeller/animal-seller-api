namespace DbContexts;

using Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class UserDbContext : DbContext
{
    public DbSet<User> Users;
    private string DbPath { get; set; }
    
    public UserDbContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        DbPath = Environment.GetFolderPath(folder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={DbPath}");
    }
}