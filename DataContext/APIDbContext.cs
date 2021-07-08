using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Happy5ChatTest.Models;

namespace Happy5ChatTest.DataContext
{
    public class APIDbContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages{ get; set; }
        public DbSet<Group> Groups{ get; set; }

        public APIDbContext(DbContextOptions<APIDbContext> options) : base(options)
        { 
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public APIDbContext NewInstance()
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<APIDbContext>();
            dbContextOptionsBuilder.UseSqlServer(this.Database.GetDbConnection().ConnectionString);
            return new APIDbContext(dbContextOptionsBuilder.Options);
        }

    }
}
