namespace AbySalto.Mid.Application.Carts;

public static class CartValidator
{
    public static void ValidateQuantity(AddToCartCommand command)
    {
        if (command.Quantity <= 0)
            throw new ArgumentException("Quantity must be greater than 0", nameof(command.Quantity));
    }
}
