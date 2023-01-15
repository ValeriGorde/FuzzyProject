using FLS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzyProject.FuzzyLogic
{
    internal class SetVariable
    {
        public double SetCharachteristics(double[] coors, string color, double eps)
        {
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


            #region Deltas

            double[] deltas = new double[3];

            if (color == "оранжевый")
            {
                if (coors[0] < 80)
                {
                    deltas[0] = coors[0] / 80;
                }
                else
                    deltas[0] = 80 / coors[0];
                if (coors[1] < 10)
                {
                    deltas[1] = coors[1] / 10;
                }
                else
                    deltas[1] = 10 / coors[1];
                if (coors[2] < 20)
                {
                    deltas[2] = coors[2] / 20;
                }
                else
                    deltas[2] = 20 / coors[2];
            }
            else if (color == "голубой")
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
            else if (color == "белый")
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

            for (int i = 0; i < deltas.Length; i++) 
            {
                deltas[i] += eps;
            }
            //else if (color == "зелёный")
            //{
            //    if (coors[1] < 80)
            //    {
            //        deltas[1] = coors[1] / 80;
            //    }
            //    else
            //        deltas[1] = 80 / coors[1];
            //    if (coors[2] < 80)
            //    {
            //        deltas[2] = coors[2] / 80;
            //    }
            //    else
            //        deltas[2] = 80 / coors[2];
            //    if (coors[3] < 80)
            //    {
            //        deltas[3] = coors[3] / 80;
            //    }
            //    else
            //        deltas[3] = 80 / coors[3];
            //}
            #endregion


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

            return result;
        }
    }
}
