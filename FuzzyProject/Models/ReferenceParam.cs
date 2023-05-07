using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzyProject.Models
{
    public class ReferenceParam
    {
        [Key]
        public int Id { get; set; }
        public byte[] Image { get; set; }

        //Навигационное свойство для параметра
        public int ParametersId { get; set; }
        //Внешний ключ параметра
        public Parameter Parameters { get; set; }

        //Навигационное свойство для красителя
        public int ColorantId { get; set; }
        //Внешний ключ красителя
        public Colorant Colorant { get; set; }
    }
}
