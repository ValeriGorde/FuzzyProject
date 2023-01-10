using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace FuzzyProject.Models
{
    internal class Report
    {
        [Key]
        public int Id { get; set; }
        public string Date { get; set; }
        public string Login { get; set; }
        public string Reccomendation { get; set; }

        // Внешний ключ
        public int AccountId { get; set; }
        // Навигационное свойство
        public Account Account { get; set; }

        // Внешний ключ
        public int MaterialId { get; set; }
        // Навигационное свойство
        public Material Material { get; set; }
    }
}
