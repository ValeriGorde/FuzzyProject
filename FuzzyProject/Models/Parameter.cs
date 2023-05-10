using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzyProject.Models
{
    public class Parameter
    {
        [Key]
        public int Id { get; set; }
        public double _L { get; set; }
        public double _A { get; set; }  
        public double _B { get; set; }
        public List<Material>? Materials { get; set; } = new List<Material>();
        public List<ReferenceParam>? ReferenceParams { get; set; } = new List<ReferenceParam>();
        public List<Report>? Reports { get; set; } = new List<Report>();
    }
}
