using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services
{
    public class ReceiptService : AbstractService<ReceiptModel, Receipt>, IReceiptService
    {
        public ReceiptService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        public async Task AddProductAsync(int productId, int receiptId, int quantity)
        {
            var receipt = await this.UnitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId) ?? throw new MarketException();

            ReceiptDetail receiptDetail = receipt.ReceiptDetails?.FirstOrDefault(rd => rd.ProductId == productId);

            if (receiptDetail == null)
            {
                var product = await this.UnitOfWork.ProductRepository.GetByIdAsync(productId)
                    ?? throw new MarketException($"Product with id {productId} not found.");

                var discountUnitPrice = Math.Round(product.Price * (100 - receipt.Customer.DiscountValue) / 100m, 2);
                var newDetail = new ReceiptDetail
                {
                    ReceiptId = receiptId,
                    ProductId = productId,
                    DiscountUnitPrice = discountUnitPrice,
                    UnitPrice = product.Price,
                    Quantity = quantity,
                };

                await this.UnitOfWork.ReceiptDetailRepository.AddAsync(newDetail);
            }
            else
            {
                receiptDetail.Quantity += quantity;
            }

            await this.UnitOfWork.SaveAsync();
        }

        public async Task CheckOutAsync(int receiptId)
        {
            var receipt = await this.UnitOfWork.ReceiptRepository.GetByIdAsync(receiptId)
                ?? throw new MarketException($"Receipt with id {receiptId} not found.");

            receipt.IsCheckedOut = true;
            await this.UnitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<ReceiptDetailModel>> GetReceiptDetailsAsync(int receiptId)
        {
            var receipt = await this.UnitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);
            return receipt != null
                ? this.Mapper.Map<IEnumerable<ReceiptDetailModel>>(receipt.ReceiptDetails)
                : throw new MarketException($"Receipt with id {receiptId} not found.");
        }

        public async Task<IEnumerable<ReceiptModel>> GetReceiptsByPeriodAsync(DateTime startDate, DateTime endDate)
        {
            var receipts = await this.UnitOfWork.ReceiptRepository.GetAllWithDetailsAsync()
                ?? throw new MarketException("No receipts found within the specified period.");

            var filteredReceipts = receipts.Where(r => r.OperationDate > startDate && r.OperationDate < endDate).ToList();

            return this.Mapper.Map<IEnumerable<ReceiptModel>>(filteredReceipts);
        }

        public async Task RemoveProductAsync(int productId, int receiptId, int quantity)
        {
            var receipt = await this.UnitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId)
                ?? throw new MarketException($"Receipt with id {receiptId} not found.");
            var receiptDetailProduct = receipt.ReceiptDetails.FirstOrDefault(rd => rd.ProductId == productId)
                ?? throw new MarketException($"Product with id {productId} not found in receipt {receiptId}.");

            if (receiptDetailProduct.Quantity <= quantity)
            {
                this.UnitOfWork.ReceiptDetailRepository.Delete(receiptDetailProduct);
            }
            else
            {
                receiptDetailProduct.Quantity -= quantity;
            }

            await this.UnitOfWork.SaveAsync();
        }

        public async Task<decimal> ToPayAsync(int receiptId)
        {
            var receipt = await this.UnitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId)
                ?? throw new MarketException($"Receipt with id {receiptId} not found.");

            receipt.OperationDate = DateTime.Now;
            return receipt.ReceiptDetails.Sum(x => x.DiscountUnitPrice * x.Quantity);
        }

        public override async Task DeleteAsync(int modelId)
        {
            var receipt = await this.UnitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(modelId)
                ?? throw new MarketException($"Receipt with id {modelId} not found.");

            foreach (var receiptDetail in receipt.ReceiptDetails)
            {
                this.UnitOfWork.ReceiptDetailRepository.Delete(receiptDetail);
            }

            await this.GetRepository().DeleteByIdAsync(modelId);
            await this.UnitOfWork.SaveAsync();
        }

        protected override IReceiptRepository GetRepository()
        {
            return this.UnitOfWork.ReceiptRepository;
        }

        protected override void Validation(ReceiptModel model)
        {
            var projectCreationDate = new DateTime(1950, 1, 1);

            if (model == null
                || model.OperationDate > DateTime.UtcNow
                || model.OperationDate < projectCreationDate)
            {
                throw new MarketException();
            }
        }

        public async Task<IEnumerable<ReceiptModel>> GetAllAsync()
        {
            var entities = await this.UnitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
            return this.Mapper.Map<IEnumerable<ReceiptModel>>(entities);
        }

        public async Task<ReceiptModel> GetByIdAsync(int id)
        {
            var entity = await this.UnitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(id);
            return this.Mapper.Map<ReceiptModel>(entity);
        }
    }
}
