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

            if (result < 35)
            {
                reccomendation = "Всё отлично!"; 
                MessageBox.Show(reccomendation, "Рекомендация");
            }
            else if (result >= 35 && result <= 75)
            {
                reccomendation = "Стоит проверить оборудование";
                MessageBox.Show(reccomendation, "Рекомендация");
            }
            else if (result > 75)
            {
                reccomendation = "Всё плохо!";
                MessageBox.Show(reccomendation, "Рекомендация");
            }
            else
            {
                reccomendation = "Недостаточно информации";
                MessageBox.Show(reccomendation, "Рекомендация");
            }

            return reccomendation;
        }
    }
}
