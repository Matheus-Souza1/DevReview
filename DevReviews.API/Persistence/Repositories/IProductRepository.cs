using System.Collections.Generic;
using System.Threading.Tasks;
using DevReviews.API.Entities;

namespace DevReviews.API.Persistence.Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetDetailsByIdAsync(int id);
        Task<Product> GetByIdAsync(int id);
        Task <List<Product>> GetAllAsync();
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task<ProductReview> GetReviewByIdAsync(int id);
        Task AddReviewAsync(ProductReview productReview);
    }
}