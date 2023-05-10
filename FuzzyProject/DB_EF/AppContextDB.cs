using FuzzyProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzyProject.DB_EF
{
    internal class AppContextDB: DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<ReferenceParam> ReferencesParams { get; set; }
        public DbSet<Colorant> Colorants { get; set; }
        public DbSet<Parameter> Parameters { get; set; }
        public AppContextDB()
        {
            //Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = FuzzyProjectDB.db");
        }
    }
}
