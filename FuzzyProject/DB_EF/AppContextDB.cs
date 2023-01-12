using FuzzyProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
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
        public AppContextDB()
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = FuzzyProjectDB.db");
        }
    }
}
