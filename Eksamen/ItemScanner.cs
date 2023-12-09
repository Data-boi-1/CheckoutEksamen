using Eksamen2;
using System;
using System.Collections.Generic;

public class ItemScanner
{
    private Dictionary<char, Item> scannedItems = new Dictionary<char, Item>();
    public event PriceCalculationDelegate ItemScanned;

    public void StartScanning(PriceCalculationDelegate cheapCalculator, PriceCalculationDelegate expensiveCalculator)
    {
        ItemScanned += cheapCalculator;

        Console.WriteLine("Enter item codes (a-z), 'Q' to stop:");
        while (true)
        {
            char input = Console.ReadKey().KeyChar;
            Console.WriteLine();
            if (input == 'Q')
            {
                // Switch to expensive calculator before final calculation
                ItemScanned -= cheapCalculator;
                ItemScanned += expensiveCalculator;

                ItemScanned?.Invoke(scannedItems);
                break;
            }

            Item item = TranslateCodeToItem(input);

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

            // Raise the event after adding the item
            ItemScanned?.Invoke(scannedItems);

            Thread.Sleep(500); // 500ms delay
        }
    }

    private Item TranslateCodeToItem(char input)
    {
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
            case 'p': // deposit
                item = new Item { Code = input, Group = 0, Price = -1.0m, Quantity = 1 };
                break;
            default:
                // Use input to calculate the rest of the items
                item = new Item { Code = input, Group = (input - 'a') % 9 + 1, Price = 'z' - input + 1, Quantity = 1 };
                break;
        }

        return item;
    }
}