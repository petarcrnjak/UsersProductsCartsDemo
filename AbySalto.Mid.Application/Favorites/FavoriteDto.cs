namespace AbySalto.Mid.Application.Favorites;

public sealed record FavoriteDto(
    int Id,
    int UserId,
    int ProductId,
    string Title,
    decimal Price,
    DateTime AddedAt
);