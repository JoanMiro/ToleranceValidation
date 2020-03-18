using System;
// ReSharper disable All

namespace ToleranceValidation
{
    using System.Linq;

    class Program
    {
        static void Main(string[] args)
        {
            decimal a1 = 2;
            decimal a2 = 12;
            decimal b1 = 435;
            decimal b2 = 488;
            decimal aTolerance = 1;
            decimal bTolerance = 3;

            var results = new[]
            {
              new FieldValidator<decimal>(a1, a2, aTolerance, "Excess Mileage")
                  .AddRule(
                      (firstValue, secondValue, tolerance) => firstValue + tolerance >= secondValue,
                      "Excess mileage fee calculated outside of tolerance.").GetResult(),
              new FieldValidator<decimal>(b1, b2, bTolerance, "Monthly Fee")
                  .AddRule(
                      (firstValue, secondValue, tolerance) => firstValue + tolerance >= secondValue,
                      "Monthly payment calculated outside of tolerance.").GetResult(),
            }
            .Where(result => result.HasErrors)
            .ToArray();

            if (results.Any())
                Console.WriteLine($"{Environment.NewLine}{string.Join(Environment.NewLine, results.Select(r => r.Message()))}{Environment.NewLine}");
            else
                Console.WriteLine($"{Environment.NewLine}All good!{Environment.NewLine}");
        }
    }
}
