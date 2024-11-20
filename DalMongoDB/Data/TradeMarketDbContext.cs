using System;
using DalMongoDB.Entities;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

namespace DalMongoDB.Data
{
    public class TradeMarketDbContext
    {
        private readonly IMongoDatabase database;

        public TradeMarketDbContext()
        {
        }

        public TradeMarketDbContext(IMongoClient mongoClient, string databaseName)
        {
            this.database = mongoClient.GetDatabase(databaseName);
        }

        public IMongoDatabase Database => this.database;

        public IMongoCollection<Customer> Customers => database.GetCollection<Customer>("Customers");
        public IMongoCollection<Person> Persons => database.GetCollection<Person>("Persons");
        public IMongoCollection<Product> Products => database.GetCollection<Product>("Products");
        public IMongoCollection<ProductCategory> ProductCategories => database.GetCollection<ProductCategory>("ProductCategories");
        public IMongoCollection<Receipt> Receipts => database.GetCollection<Receipt>("Receipts");
        public IMongoCollection<ReceiptDetail> ReceiptsDetails => database.GetCollection<ReceiptDetail>("ReceiptDetails");
    }
}