using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzyProject.Models
{
    internal class ReferenceParam
    {
        [Key]
        public int Id { get; set; }
        public double CoordinateL { get; set; }
        public double CoordinateA { get; set; }
        public double CoordinateB { get; set; }
        public string Color { get; set; }
        public byte[] Image { get; set; }
    }
}
