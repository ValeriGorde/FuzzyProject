using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzyProject.Models
{
    internal class Material
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string Parameters { get; set; }
        public byte[] Image { get; set; }
        public List<Report> Reports { get; set; } = new List<Report>();
    }
}
