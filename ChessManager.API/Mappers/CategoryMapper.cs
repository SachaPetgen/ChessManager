using ChessManager.Domain.Models;
using ChessManager.DTO.Category;

namespace ChessManager.Mappers;

public static class CategoryMapper
{
    
    
    public static CategoryViewListDTO ToCategoryViewListDTO(this Category category)
    {
        return new CategoryViewListDTO
        {
            Name = category.Name,
            AgeMax = category.AgeMax,
            AgeMin = category.AgeMin
        };
    }
    
    public static Category ToCategory(this CategoryViewListDTO category)
    {
        return new Category()
        {
            Name = category.Name,
            AgeMax = category.AgeMax,
            AgeMin = category.AgeMin
        };
    }
}