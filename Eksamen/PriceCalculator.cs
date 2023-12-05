namespace Eksamen2;

public class PriceCalculator
{
    public static void CheapPriceCalculator(Dictionary<char, Item> items)
    {
        // convert to list of items
        var list = items.Values.ToList();
        
        // apply promotions
        var itemsWithPromotion = ApplyPromotionalPricing(list);
        
        // calculate total price
        decimal totalPrice = itemsWithPromotion.Sum(tuple => tuple.Price);
        
        // print total price
        Console.WriteLine($"Total price: {totalPrice}");
    }

    public static void ExpensivePriceCalculator(Dictionary<char, Item> items)
    {
        // var groupedItems = items.GroupBy(item => item.Group)
        //     .Select(group => new { Group = group.Key, TotalQuantity = group.Count() })
        //     .OrderBy(g => g.Group);
        //
        // foreach (var group in groupedItems)
        // {
        //     Console.WriteLine($"Group: {group.Group}, Quantity: {group.TotalQuantity}");
        // }
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
                    
                    price = (fullSets * quantityDiscountPromotion.QuantityCharged * item.Price) + (remainingItems * item.Price);
                }
            }

            itemsWithPromotion.Add((item, price));
        }

        return itemsWithPromotion;
    }
    
    // ### promotions tables ###
    
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