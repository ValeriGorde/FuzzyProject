using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FuzzyProject.Models
{
    public class Colorant
    {
        [Key]
        public int Id { get; set; } 
        public string Name { get; set; }
        public List<Material> Materials { get; set; } = new List<Material>();
        public List<Report> Reports { get; set; } = new List<Report>();
        public List<ReferenceParam> ReferenceParams { get; set; } = new List<ReferenceParam>();
    }
}
