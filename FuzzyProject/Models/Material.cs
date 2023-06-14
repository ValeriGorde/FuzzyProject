using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FuzzyProject.Models
{
    public class Material
    {
        [Key]
        public int Id { get; set; }
        public byte[] Image { get; set; }

        [DefaultValue(true)]
        public bool IsReference { get; set; }

        //Навигационное свойство для параметра
        public int ParametersId { get; set; }
        //Внешний ключ параметра
        public Parameter Parameters { get; set; }  

        //Навигационное свойство для красителя
        public int ColorantId { get; set; }
        //Внешний ключ красителя
        public Colorant Colorants { get; set; }

        //Навигационное свойство для полимера
        public int PolymerTypeId { get; set; }
        //Внешний ключ красителя
        public PolymerType PolymerTypes { get; set; }

        //public List<Report> Reports { get; set; } = new List<Report>();
    }
}
