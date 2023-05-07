using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FuzzyProject.Models
{
    public class Material
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }

        //Навигационное свойство для параметра
        public int ParametersId { get; set; }
        //Внешний ключ параметра
        public Parameter Parameters { get; set; }  

        //Навигационное свойство для красителя
        public int ColorantId { get; set; }
        //Внешний ключ красителя
        public Colorant Colorants { get; set; }

        public List<Report> Reports { get; set; } = new List<Report>();
    }
}
