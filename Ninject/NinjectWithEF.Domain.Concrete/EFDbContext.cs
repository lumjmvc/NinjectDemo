using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NinjectWithEF.Domain.Models;

namespace NinjectWithEF.Domain.Concrete
{
    public class EFDbContext : DbContext
    {
        // context class constructor
        public EFDbContext() : base("EFDbContext") { }


        // Entity Sets - property for each table in the database that we want to work with
        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // *****************************************************************
            // By default, database table names will be pluralized
            // The following will disable pluralizing table names
            // *****************************************************************
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            
        }
    }
}
