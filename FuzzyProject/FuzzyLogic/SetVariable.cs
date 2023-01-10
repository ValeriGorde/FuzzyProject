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
        public double SetCharachteristics(double newCoorl, double newCoora, double newCoorb)
        {
            LinguisticVariable coorl = new LinguisticVariable("Coorl");
            var black = coorl.MembershipFunctions.AddTrapezoid("Чёрный", 0, 0, 10, 40);
            var gray = coorl.MembershipFunctions.AddTrapezoid("Серый", 10, 40, 60, 90);
            var white = coorl.MembershipFunctions.AddTrapezoid("Белый", 60, 90, 100, 100);

            LinguisticVariable coora = new LinguisticVariable("Coora");
            var green = coora.MembershipFunctions.AddTrapezoid("Зелёный", -127, -127, -80, -25);
            var grayA = coora.MembershipFunctions.AddTrapezoid("Серый", -80, -25, 25, 80);
            var red = coora.MembershipFunctions.AddTrapezoid("Красный", 25, 80, 128, 128);

            LinguisticVariable coorb = new LinguisticVariable("Coorl");
            var blue = coorb.MembershipFunctions.AddTrapezoid("Синий", -127, -127, -80, -25);
            var grayB = coorb.MembershipFunctions.AddTrapezoid("Серый", -80, -25, 25, 80);
            var yellow = coorb.MembershipFunctions.AddTrapezoid("Жёлтый", 25, 80, 128, 128);

            LinguisticVariable conclusion = new LinguisticVariable("Conclusion");
            var good = conclusion.MembershipFunctions.AddTrapezoid("Хорошо", 0, 0, 10, 40);
            var normal = conclusion.MembershipFunctions.AddTriangle("Нормально", 15, 50, 85);
            var bad = conclusion.MembershipFunctions.AddTrapezoid("Плохо", 61, 90, 100, 100);


            //общие правила
            IFuzzyEngine fuzzyEngine = new FuzzyEngine(new CoGDefuzzification());
            fuzzyEngine.Rules.If(coorl.Is(white).And(coora.Is(grayA)).And(coorb.Is(grayB))).Then(conclusion.Is(good));
            fuzzyEngine.Rules.If(coorl.Is(gray).And(coora.Is(grayA)).And(coorb.Is(grayB))).Then(conclusion.Is(good));
            fuzzyEngine.Rules.If(coorl.Is(black)).Then(conclusion.Is(bad));

            fuzzyEngine.Rules.If(coora.Is(green).Or(coora.Is(red))).Then(conclusion.Is(bad));
            fuzzyEngine.Rules.If(coorb.Is(blue).Or(coorb.Is(yellow))).Then(conclusion.Is(bad));

            fuzzyEngine.Rules.If(coorl.Is(gray).And(coora.Is(grayA)).And(coorb.Is(grayB))).Then(conclusion.Is(good));

            fuzzyEngine.Rules.If(coorl.Is(white)).Then(conclusion.Is(good));
            fuzzyEngine.Rules.If(coora.Is(grayA)).Then(conclusion.Is(good));
            fuzzyEngine.Rules.If(coorb.Is(grayB)).Then(conclusion.Is(good));

            double result = fuzzyEngine.Defuzzify(new { coorl = newCoorl, coora = newCoora, coorb = newCoorb });

            return result;
        }
    }
}
