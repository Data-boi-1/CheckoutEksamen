using Eksamen2;
using System;
using System.Collections.Generic;
using System.Threading;

// Delegate type for price calculation methods
public delegate void PriceCalculationDelegate(Dictionary<char, Item> itemLookup);

class MainProgram
{
    private static Dictionary<char, Item> scannedItems = new Dictionary<char, Item>();
    private static PriceCalculationDelegate cheapCalculator;
    private static PriceCalculationDelegate expensiveCalculator;

    public static void Main(string[] args)
    {
        // Initialize the price calculators
        cheapCalculator = PriceCalculator.CheapPriceCalculator;
        expensiveCalculator = PriceCalculator.ExpensivePriceCalculator;

        // Simulate item scanning
        Console.WriteLine("Enter item codes (a-z), 'Q' to stop:");
        while (true)
        {
            char input = Console.ReadKey().KeyChar;
            Console.WriteLine();
            if (input == 'Q')
            {
                // Invoke expensive price calculator only when exiting
                expensiveCalculator(scannedItems);
                break;
            }

            // Translate code to item (### IMPORTANT multi-packs implemented here ###)
            Item item;
            switch (input)
            {
                case 'a':
                    item = new Item { Code = input, Group = 1, Price = 100.0m, Quantity = 1 };
                    break;
                case 'b':
                    item = new Item { Code = input, Group = 2, Price = 2.0m, Quantity = 1 };
                    break;
                case 'c':
                    item = new Item { Code = input, Group = 2, Price = 3.0m, Quantity = 1 };
                    break;
                case 'r': // multi-pack
                    item = new Item { Code = 'f', Group = 1, Price = 10.0m, Quantity = 6 };
                    break;
                case 'f': // part of multi-pack
                    item = new Item { Code = input, Group = 1, Price = 10.0m, Quantity = 1 };
                    break;
                case 'p': // deposit TODO ask about the functionality of this?
                    item = new Item { Code = input, Group = 0, Price = -1.0m, Quantity = 1 };
                    break;
                default:
                    // todo fix item code to value conversion
                    item = new Item { Code = input, Group = input % 9, Price = 'a' - input + 1, Quantity = 1 };
                    break;
            }

            // Add item to dictionary
            if (scannedItems.ContainsKey(item.Code))
            {
                scannedItems[item.Code].Quantity += item.Quantity;
                Console.WriteLine($"'{item.Code}' = {scannedItems[item.Code].Quantity}");
            }
            else
            {
                scannedItems.Add(item.Code, item);
            }

            // Invoke cheap price calculator for each new item
            cheapCalculator(scannedItems);

            Thread.Sleep(500); // 500ms delay
        }
    }
}