using RandomDataGenerator.FieldOptions;
using RandomDataGenerator.Randomizers;

namespace BoilerPlate.Core.Extensions;

public static class RandomExtensions
{
    public static decimal NextDecimal(this Random random, decimal min, decimal max, int precision)
    {
        if (max <= min)
            throw new ArgumentException("Max must be greater than min");

        if (precision > 27)
            throw new ArgumentException("Valid precision of a decimal is 0-27");

        var doubleGenerator = new RandomizerNumber<double>(new FieldOptionsDouble
        {
            Min = (double) min,
            Max = (double) max
        });

        var randomDoubleStr = doubleGenerator.Generate().ToString()!.Split('.');
        var wholePartStr = randomDoubleStr.First();
        var fractionPartStr = randomDoubleStr.Last();
        while (fractionPartStr.Length < precision)
        {
            fractionPartStr += random.Next(0, 10);
        }

        var decimalStr = wholePartStr + '.' + fractionPartStr[..precision];

        return decimal.Parse(decimalStr);
    }

    public static decimal NextWinNumber(this Random random) =>
        random.NextDecimal(min: 1, max: 101, precision: 17);
}