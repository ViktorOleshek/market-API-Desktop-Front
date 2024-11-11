using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abstraction.IRepositories;
using Abstraction.IServices;
using Abstraction.Models;
using AutoMapper;
using Business.Validation;

namespace Business.Services
{
    public class ProductService : AbstractService<ProductModel>, IProductService
    {
        public ProductService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public async Task AddCategoryAsync(ProductCategoryModel categoryModel)
        {
            Validation(categoryModel);
            await this.UnitOfWork.ProductCategoryRepository.AddAsync(categoryModel);
            await this.UnitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<ProductCategoryModel>> GetAllProductCategoriesAsync()
        {
            return await this.UnitOfWork.ProductCategoryRepository.GetAllAsync();
        }

        public async Task<IEnumerable<ProductModel>> GetByFilterAsync(FilterSearchModel filterSearch)
        {
            var product = await this.UnitOfWork.ProductRepository.GetAllWithDetailsAsync();
            var filterProduct = product.Where(p =>
                (filterSearch.MinPrice == null || p.Price >= filterSearch.MinPrice) &&
                (filterSearch.MaxPrice == null || p.Price <= filterSearch.MaxPrice) &&
                (filterSearch.CategoryId == null || p.ProductCategoryId == filterSearch.CategoryId));
            return filterProduct;
        }

        public async Task RemoveCategoryAsync(int categoryId)
        {
            await this.UnitOfWork.ProductCategoryRepository.DeleteByIdAsync(categoryId);
            await this.UnitOfWork.SaveAsync();
        }

        public async Task UpdateCategoryAsync(ProductCategoryModel categoryModel)
        {
            Validation(categoryModel);
            this.UnitOfWork.ProductCategoryRepository.Update(categoryModel);
            await this.UnitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<ProductModel>> GetAllAsync()
        {
            return await this.UnitOfWork.ProductRepository.GetAllWithDetailsAsync();
        }

        public async Task<ProductModel> GetByIdAsync(int id)
        {
            return await this.UnitOfWork.ProductRepository.GetByIdWithDetailsAsync(id);
        }

        protected static void Validation(ProductCategoryModel model)
        {
            if (model == null
                || string.IsNullOrWhiteSpace(model.CategoryName))
            {
                throw new MarketException();
            }
        }

        protected override IProductRepository GetRepository()
        {
            return this.UnitOfWork.ProductRepository;
        }

        protected override void Validation(ProductModel model)
        {
            if (model == null
                || string.IsNullOrWhiteSpace(model.ProductName)
                || model.Price < 0)
            {
                throw new MarketException();
            }
        }
    }
}
