namespace Eksamen2;

public class PriceCalculator
{
    public static void CheapPriceCalculator(Dictionary<char, Item> itemLookup)
    {
        // convert to list of items
        var items = itemLookup.Values.ToList();

        // apply promotions
        var itemsWithPromotion = ApplyPromotionalPricing(items);

        // calculate total price
        decimal totalPrice = itemsWithPromotion.Sum(tuple => tuple.Price);

        // print total price
        Console.WriteLine($"Total price: {totalPrice}");
    }

    public static void ExpensivePriceCalculator(Dictionary<char, Item> itemLookup)
    {
        // Convert to list of items
        var items = itemLookup.Values.ToList();

        // Apply promotions
        List<(Item item, decimal PromotionalPrice)> itemsWithPromotion = ApplyPromotionalPricing(items);

        // Create a dictionary from the list with promotional prices for easy access
        var promoPriceDict = itemsWithPromotion.ToDictionary(promo => promo.item.Code, promo => promo.PromotionalPrice);

        // Group by item group
        var groupedItems = items
            .GroupBy(item => item.Group)
            .OrderBy(g => g.Key);

        // Output the results
        Console.WriteLine("--------------------------------------------------");
        decimal totalPrice = 0;
        foreach (var group in groupedItems)
        {
            Console.WriteLine($"Group: {group.Key}");
            decimal groupTotalPrice = 0;

            foreach (var item in group)
            {
                decimal price = promoPriceDict.ContainsKey(item.Code) ? promoPriceDict[item.Code] : item.Price;
                decimal totalPriceForItem = price * item.Quantity;
                groupTotalPrice += totalPriceForItem;

                // Formatting individual item output
                Console.WriteLine($"Item '{item.Code}', Qty: {item.Quantity}, Price: {totalPriceForItem:F2} kr");
            }

            // Formatting group total output
            Console.WriteLine($"\nGroup Total - Items: {group.Sum(i => i.Quantity)}, Price: {groupTotalPrice:F2} kr");
            Console.WriteLine("--------------------------------------------------");
            totalPrice += groupTotalPrice;
        }

        // Formatting grand total output
        Console.WriteLine($"Grand Total: {totalPrice:F2} kr");
    }

    private static List<(Item, decimal Price)> ApplyPromotionalPricing(List<Item> items)
    {
        // tuples of item and price
        var itemsWithPromotion = new List<(Item, decimal Price)>();

        foreach (var item in items)
        {
            decimal price = item.Price * item.Quantity; // Default regular pricing

            // Apply Special Price Promotions
            if (SpecialPricePromotions.TryGetValue(item.Code, out var specialPricePromotion))
            {
                if (item.Quantity >= specialPricePromotion.QuantityForPromotion)
                {
                    // full sets of items that qualify for promotion
                    int fullSets = item.Quantity / specialPricePromotion.QuantityForPromotion;
                    // remaining items that do not qualify for promotion
                    int remainingItems = item.Quantity % specialPricePromotion.QuantityForPromotion;

                    price = (fullSets * specialPricePromotion.PromotionalPrice) + (remainingItems * item.Price);
                }
            }

            // Apply Quantity Discount Promotions
            if (QuantityDiscountPromotions.TryGetValue(item.Code, out var quantityDiscountPromotion))
            {
                if (item.Quantity >= quantityDiscountPromotion.QuantityBought)
                {
                    // full sets of items that qualify for promotion
                    int fullSets = item.Quantity / quantityDiscountPromotion.QuantityBought;
                    // remaining items that do not qualify for promotion
                    int remainingItems = item.Quantity % quantityDiscountPromotion.QuantityBought;

                    price = (fullSets * quantityDiscountPromotion.QuantityCharged * item.Price) +
                            (remainingItems * item.Price);
                }
            }

            itemsWithPromotion.Add((item, price));
        }

        return itemsWithPromotion;
    }

    // ### promotions dictionary ###

    // buy number of items for price
    private static Dictionary<char, (int QuantityForPromotion, decimal PromotionalPrice)> SpecialPricePromotions =
        new Dictionary<char, (int, decimal)>
        {
            { 'a', (2, 80m) }, // Buy 2 for 80 kr
        };

    // buy x number of items for y number of items
    private static Dictionary<char, (int QuantityBought, decimal QuantityCharged)> QuantityDiscountPromotions =
        new Dictionary<char, (int, decimal)>
        {
            { 'b', (3, 2) }, // Buy 3 for 2
        };
}