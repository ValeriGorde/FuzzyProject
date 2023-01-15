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
            else if (result >= 0.14 && result <= 0.4)
            {
                reccomendation = "Велика вероятность недоплавов или наличия деструктивных элементов. Стоит проверить дозаторы сырья.";
            }
            else if (result >= 0.4 && result <= 0.6)
            {
                reccomendation = "Качество продукции отличается от эаталонного образца, стоит изменить температуру.";
            }
            else if (result > 0.6) 
            {
                reccomendation = "Качество продукции значительно отличается от эталлоного образца, необходимо провести осмотр оборудования.";
            }
            else
            {
                reccomendation = "Недостаточно информации для получения рекомендации, обратитесь к техническому специалисту.";
            }

            return reccomendation;
        }
    }
}
