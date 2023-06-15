using FLS;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace FuzzyProject.FuzzyLogic
{
    internal class SetVariable
    {
        public double SetCharachteristics(double[] coors, string material)
        {
            #region Other Rules
            //LinguisticVariable coorl = new LinguisticVariable("Coorl");
            //var black = coorl.MembershipFunctions.AddTrapezoid("Чёрный", 0, 0, 10, 40);
            //var gray = coorl.MembershipFunctions.AddTrapezoid("Серый", 10, 40, 60, 90);
            //var white = coorl.MembershipFunctions.AddTrapezoid("Белый", 60, 90, 100, 100);

            //LinguisticVariable coora = new LinguisticVariable("Coora");
            //var green = coora.MembershipFunctions.AddTrapezoid("Зелёный", -127, -127, -80, -25);
            //var grayA = coora.MembershipFunctions.AddTrapezoid("Серый", -80, -25, 25, 80);
            //var red = coora.MembershipFunctions.AddTrapezoid("Красный", 25, 80, 128, 128);

            //LinguisticVariable coorb = new LinguisticVariable("Coorl");
            //var blue = coorb.MembershipFunctions.AddTrapezoid("Синий", -127, -127, -80, -25);
            //var grayB = coorb.MembershipFunctions.AddTrapezoid("Серый", -80, -25, 25, 80);
            //var yellow = coorb.MembershipFunctions.AddTrapezoid("Жёлтый", 25, 80, 128, 128);
            #endregion

            #region Deltas

            double[] deltas = new double[3];

            if (material == "ПВХ")
            {

            }
            else if (material == "голубой")
            {
                if (coors[0] < 70)
                {
                    deltas[0] = coors[0] / 70;
                }
                else
                    deltas[0] = 70 / coors[0];
                if (coors[1] < 2)
                {
                    deltas[1] = coors[1] / 2;
                }
                else
                    deltas[1] = 2 / coors[1];
                if (coors[2] < -34)
                {
                    deltas[2] = Math.Abs(coors[2] / -34);
                }
                else
                    deltas[2] = Math.Abs(-34 / coors[2]);
            }
            else if (material == "белый")
            {
                if (coors[0] < 86)
                {
                    deltas[0] = coors[0] / 86;
                }
                else
                    deltas[0] = 86 / coors[0];
                if (coors[1] < 0)
                {
                    deltas[1] = (coors[1] / 1) - 1;
                }
                else deltas[1] = (coors[1] / 1) - 1;
                if (coors[2] < 0)
                {
                    deltas[2] = (coors[2] / 1) - 1;
                }
                else deltas[2] = (coors[2] / 1) - 1;
            }
            #endregion

            #region Linguistic
            LinguisticVariable coorl = new LinguisticVariable("Coorl");
            var veryBadL = coorl.MembershipFunctions.AddTriangle("Плохо", -0.3, 0, 0.3);
            var badL = coorl.MembershipFunctions.AddTriangle("Серый", 0.05, 0.3, 0.6);
            var normalL = coorl.MembershipFunctions.AddTriangle("Белый", 0.3, 0.6, 0.99);
            var perfectL = coorl.MembershipFunctions.AddTriangle("Белый", 0.6, 1, 1.3);

            LinguisticVariable coora = new LinguisticVariable("Coora");
            var veryBadA = coora.MembershipFunctions.AddTriangle("Плохо", -0.3, 0, 0.3);
            var badA = coora.MembershipFunctions.AddTriangle("Серый", 0.05, 0.3, 0.6);
            var normalA = coora.MembershipFunctions.AddTriangle("Белый", 0.3, 0.6, 0.99);
            var perfectA = coora.MembershipFunctions.AddTriangle("Белый", 0.6, 1, 1.3);

            LinguisticVariable coorb = new LinguisticVariable("Coorb");
            var veryBadB = coorb.MembershipFunctions.AddTriangle("Плохо", -0.3, 0, 0.3);
            var badB = coorb.MembershipFunctions.AddTriangle("Серый", 0.05, 0.3, 0.6);
            var normalB = coorb.MembershipFunctions.AddTriangle("Белый", 0.3, 0.6, 0.99);
            var perfectB = coorb.MembershipFunctions.AddTriangle("Белый", 0.6, 1, 1.3);

            LinguisticVariable conclusion = new LinguisticVariable("Conclusion");
            var perfect = conclusion.MembershipFunctions.AddTriangle("Плохо", -0.3, 0, 0.3);
            var good = conclusion.MembershipFunctions.AddTriangle("Серый", 0.05, 0.3, 0.6);
            var bad = conclusion.MembershipFunctions.AddTriangle("Белый", 0.3, 0.6, 0.99);
            var veryBad = conclusion.MembershipFunctions.AddTriangle("Белый", 0.6, 1, 1.3);

            IFuzzyEngine fuzzyEngine = new FuzzyEngine(new CoGDefuzzification());
            fuzzyEngine.Rules.If(coorl.Is(perfectL).And(coora.Is(perfectA)).And(coorb.Is(perfectB))).Then(conclusion.Is(perfect));
            fuzzyEngine.Rules.If(coorl.Is(veryBadL).And(coora.Is(veryBadA)).And(coorb.Is(veryBadB))).Then(conclusion.Is(veryBad));
            fuzzyEngine.Rules.If(coorl.Is(normalL).And(coora.Is(normalA)).And(coorb.Is(normalB))).Then(conclusion.Is(good));
            fuzzyEngine.Rules.If(coorl.Is(badL).And(coora.Is(badA)).And(coorb.Is(badB))).Then(conclusion.Is(bad));

            fuzzyEngine.Rules.If(coorl.Is(veryBadL).And(coora.Is(perfectA)).And(coorb.Is(perfectB))).Then(conclusion.Is(bad));
            fuzzyEngine.Rules.If(coorl.Is(perfectL).And(coora.Is(veryBadA)).And(coorb.Is(perfectB))).Then(conclusion.Is(bad));
            fuzzyEngine.Rules.If(coorl.Is(perfectL).And(coora.Is(perfectA)).And(coorb.Is(veryBadB))).Then(conclusion.Is(bad));

            fuzzyEngine.Rules.If(coorl.Is(veryBadL).And(coora.Is(veryBadA)).And(coorb.Is(perfectB))).Then(conclusion.Is(veryBad));
            fuzzyEngine.Rules.If(coorl.Is(veryBadL).And(coora.Is(perfectA)).And(coorb.Is(veryBadB))).Then(conclusion.Is(veryBad));
            fuzzyEngine.Rules.If(coorl.Is(perfectL).And(coora.Is(veryBadA)).And(coorb.Is(veryBadB))).Then(conclusion.Is(veryBad));

            fuzzyEngine.Rules.If(coorl.Is(badL).And(coora.Is(perfectA)).And(coorb.Is(perfectB))).Then(conclusion.Is(good));
            fuzzyEngine.Rules.If(coorl.Is(perfectL).And(coora.Is(badL)).And(coorb.Is(perfectB))).Then(conclusion.Is(good));
            fuzzyEngine.Rules.If(coorl.Is(perfectL).And(coora.Is(perfectA)).And(coorb.Is(badB))).Then(conclusion.Is(good));

            fuzzyEngine.Rules.If(coorl.Is(normalL)).Then(conclusion.Is(good));
            fuzzyEngine.Rules.If(coora.Is(normalL)).Then(conclusion.Is(good));
            fuzzyEngine.Rules.If(coorb.Is(normalL)).Then(conclusion.Is(good));

            double result = fuzzyEngine.Defuzzify(new { coorl = deltas[0], coora = deltas[1], coorb = deltas[2] });
            #endregion


            double[] refCoors = new double[] { 86, 0, 0 };

            double deltaE;
            deltaE = Math.Sqrt(Math.Pow(coors[0] - refCoors[0], 2) + Math.Pow(coors[1] - refCoors[1], 2) + Math.Pow(coors[2] - refCoors[2], 2)) / 100;

            return deltaE;
        }

        public string Delta(double[] coors, string material)
        {
            double LStarE = 0;
            double aStarE = 0;
            double bStarE = 0;

            if (material == "Поливинилхлорид")
            {
                LStarE = 85.0;
                aStarE = 2.0;
                bStarE = -15.0;
            }
            else if (material == "Полиэтилен")
            {
                LStarE = 92.0;
                aStarE = -2.0;
                bStarE = 3.0;
            }
            else if (material == "Полипропилен")
            {
                LStarE = 96.0;
                aStarE = -0.1;
                bStarE = -2.0;
            }
            else if (material == "Поликарбонат")
            {
                LStarE = 82.0;
                aStarE = -0.5;
                bStarE = -10.0;
            }

            return EstimateThermalDestruction(coors, LStarE, aStarE, bStarE);
        }


        public string EstimateThermalDestruction(double[] coors, double lStandard, double aStandard, double bStandard)
        {
            // определяем нечёткие множества для каждой компоненты L, a и b с погрешностями
            double lLow = GetMembershipValue(coors[0], lStandard - 10, lStandard - 5, lStandard);
            double lMedium = GetMembershipValue(coors[0], lStandard - 5, lStandard, lStandard + 5);
            double lHigh = GetMembershipValue(coors[0], lStandard, lStandard + 5, lStandard + 10);

            double aLow = GetMembershipValue(coors[1], aStandard - 10, aStandard - 5, aStandard);
            double aMedium = GetMembershipValue(coors[1], aStandard - 2.5, aStandard, aStandard + 2.5);
            double aHigh = GetMembershipValue(coors[1], aStandard, aStandard + 5, aStandard + 10);

            double bLow = GetMembershipValue(coors[1], bStandard - 10, bStandard - 5, bStandard);
            double bMedium = GetMembershipValue(coors[1], bStandard - 2.5, bStandard, bStandard + 2.5);
            double bHigh = GetMembershipValue(coors[1], bStandard, bStandard + 5, bStandard + 10);

            // определяем степени перекрытия между нечёткими множествами
            double overlapNoDamage = new[] { lLow, aLow, bLow }.Min();
            double overlapLowDamage = new[] { lMedium, aMedium, bMedium }.Min();
            double overlapModerateDamage = new[] { lHigh, aHigh, bHigh }.Min();
            double minOverlap = Math.Min(overlapNoDamage, Math.Min(overlapLowDamage, overlapModerateDamage));
            double overlapSevereDamage = minOverlap == 0.0 ? 0.0 : 1.0 - minOverlap;

            // определяем степень термической деструкции
            if (overlapSevereDamage > 0)
            {
                return "Сильная деструкция";
            }
            else if (overlapModerateDamage > 0.5)
            {
                return "Умеренная деструкция";
            }
            else if (overlapLowDamage > 0.5)
            {
                return "Слабая деструкция";
            }
            else
            {
                return "Отсутствие деструкции";
            }
        }

        // функция для вычисления степени принадлежности значения x к нечётному множеству с треугольной функцией принадлежности и погрешностью
        public double GetMembershipValue(double x, double a, double b, double c)
        {
            if (x <= a  || x >= c )
            {
                return 0.0;
            }
            else if (x >= b)
            {
                return (c - x) / (c - b);
            }
            else if (x <= a)
            {
                return (x - a) / (b - a);
            }
            else
            {
                return 1.0 - (x - a ) / 2;
            }
        }


        public string GetDeltas(double[] coors, double[] refCoors, string material) 
        {

            var deltaE = Math.Sqrt((Math.Pow((coors[0] - refCoors[0]), 2) + (Math.Pow((coors[1] - refCoors[1]), 2) + (Math.Pow((coors[2] - refCoors[2]), 2)))))/100;

            // определяем степень термической деструкции
            if (deltaE >= 0 && deltaE <= 0.13)
            {
                return "Действуй не требуется. Продукт соответствует установленным требованиям.";
            }
            else if (deltaE > 0.13 && deltaE <= 0.20)
            {
                return "Экструдат имеет отклонение от эталонного продукта, что может указывать на наличие слабой термической деструкции.\n" +
                    "Рекомендуется проверить оборудование и настройки процесса экструзии.";
            }
            else if (deltaE > 0.20 && deltaE <= 0.25)
            {
                return "Состояние экструдата подвержено умеренной термодеструкции.\n" +
                    "Возможно, материал не совсем подходит для данного применения или процесс экструзии настроен неправильно.\n" +
                    "Рекомендуется проверить выбранный материал и настройки процесса экструзии, чтобы избежать ухудшения качества продукции.";
            }
            else
            {
                return "Обратите внимание на состояние экструдата, так как он имеет признаки серьезной термической деструкции, это может привести к повреждению оборудования и ухудшению качества продукции.\n" +
                    "Рекомендуется немедленно остановить процесс экструзии и проверить настройки оборудования.";
            }

        }
    }
}
