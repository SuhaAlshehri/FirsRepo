using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Users.Models;
using Users.Data;





namespace Users.Data
{
    public class DBusers:IdentityDbContext<User>
    {
        public DBusers(DbContextOptions<DBusers> options) : base(options) { }  
    }
}
