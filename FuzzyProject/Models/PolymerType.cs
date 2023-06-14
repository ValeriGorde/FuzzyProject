using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzyProject.Models
{
    public class PolymerType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Material> Materials { get; set; } = new List<Material>();
    }
}
