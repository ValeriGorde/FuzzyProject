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
                reccomendation = "\"Экструдат имеет более высокую степень желтизны, чем эталонный экструдат, что может указывать на наличие слабой термической деструкции.\n" +
                    "Рекомендуется проверить оборудование и настройки процесса экструзии.\"";
            }
            else if (result >= 0.2 && result <= 0.3)
            {
                reccomendation = "Состояние экструдата подвержено умеренной термодеструкции.\n" +
                    "Возможно, материал не совсем подходит для данного применения или процесс экструзии настроен неправильно.\n" +
                    "Рекомендуется проверить выбранный материал и настройки процесса экструзии, чтобы избежать ухудшения качества продукции.\"";
            }
            else if (result >= 0.3)
            {
                reccomendation = "\"Обратите внимание на состояние экструдата, так как он имеет признаки серьезной термической деструкции, это может привести к повреждению оборудования и ухудшению качества продукции.\n" +
                    "Рекомендуется немедленно остановить процесс экструзии и проверить настройки оборудования.";
            }
            else 
            {
                reccomendation = "Недостаточно информации для получения рекомендации, обратитесь к техническому специалисту.";
            }

            return reccomendation;
        }
    }
}
