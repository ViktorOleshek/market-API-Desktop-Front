using System.Collections.Generic;
using System.Threading.Tasks;
using Abstraction.Models;

namespace Abstraction.IServices
{
    public interface IProductService : ICrud<ProductModel>
    {
        Task<IEnumerable<ProductModel>> GetByFilterAsync(FilterSearchModel filterSearch);

        Task<IEnumerable<ProductCategoryModel>> GetAllProductCategoriesAsync();

        Task AddCategoryAsync(ProductCategoryModel categoryModel);

        Task UpdateCategoryAsync(ProductCategoryModel categoryModel);

        Task RemoveCategoryAsync(int categoryId);
    }
}
