using FuzzyProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FuzzyProject.FuzzyLogic
{
    internal class ReccomendsForOperator
    {
        
        public string GiveRecommend(double result) 
        {
            string reccomendation = "";

            if (result < 0.1)
            {
                reccomendation = "Действуй не требуется. Продукт соответствует установленным требованиям.";
            }
            else if (result >= 0.1 && result <= 0.2)
            {
                reccomendation = "Качество продукции отличается от эталонного образца, велика вероятность перегрева.";
            }
            else if (result >= 0.2)
            {
                reccomendation = "Качество продукции значительно отличается от эаталонного образца, велика вероятность сильного перегрева.";
            }
            else
            {
                reccomendation = "Недостаточно информации для получения рекомендации, обратитесь к техническому специалисту.";
            }

            return reccomendation;
        }
    }
}
