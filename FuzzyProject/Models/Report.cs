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
    public class Report
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }

        // Внешний ключ
        public int ColorantId { get; set; }
        // Навигационное свойство
        public Colorant Colorants { get; set; }

        // Внешний ключ
        public int MaterialId { get; set; }
        // Навигационное свойство
        public Material Material { get; set; }

        //Навигационное свойство для параметра
        public int ParametersId { get; set; }
        //Внешний ключ параметра
        public Parameter Parameters { get; set; }

    }
}
