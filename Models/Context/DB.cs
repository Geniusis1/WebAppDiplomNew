using DiplomWeb_Service.Models;
using Microsoft.EntityFrameworkCore;

namespace WebAppDiplomNew.Models.Context
{
    public class DB : DbContext
    {
        DbSet<User> Users {  get; set; }
        DbSet<Inform> Informs { get; set; }
        DbSet<Group> Groups { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=WebApp_diplom;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
    }
}
