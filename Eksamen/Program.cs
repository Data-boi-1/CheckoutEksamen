using Eksamen2;
using System;

public delegate void PriceCalculationDelegate(Dictionary<char, Item> itemLookup);

class MainProgram
{
    private static PriceCalculationDelegate cheapCalculator;
    private static PriceCalculationDelegate expensiveCalculator;

    public static void Main(string[] args)
    {
        // Initialize the price calculators
        cheapCalculator = PriceCalculator.CheapPriceCalculator;
        expensiveCalculator = PriceCalculator.ExpensivePriceCalculator;

        var scanner = new ItemScanner();

        // Start the item scanning process with both calculators
        scanner.StartScanning(cheapCalculator, expensiveCalculator);
    }
}