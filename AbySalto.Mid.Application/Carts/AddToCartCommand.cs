namespace AbySalto.Mid.Application.Carts;

public sealed record AddToCartCommand(
    int ProductId,
    int Quantity
);
