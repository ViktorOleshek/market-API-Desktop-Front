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
            //InitializeCollections();
        }

        public IMongoDatabase Database => this.database;

        public IMongoCollection<Customer> Customers => database.GetCollection<Customer>("Customers");
        public IMongoCollection<Person> Persons => database.GetCollection<Person>("Persons");
        public IMongoCollection<Product> Products => database.GetCollection<Product>("Products");
        public IMongoCollection<ProductCategory> ProductCategories => database.GetCollection<ProductCategory>("ProductCategories");
        public IMongoCollection<Receipt> Receipts => database.GetCollection<Receipt>("Receipts");
        public IMongoCollection<ReceiptDetail> ReceiptsDetails => database.GetCollection<ReceiptDetail>("ReceiptDetails");

        //private void InitializeCollections()
        //{
        //    CreateCollectionIfNotExists<Customer>("Customers");
        //    CreateCollectionIfNotExists<Person>("Persons");
        //    CreateCollectionIfNotExists<Product>("Products");
        //    CreateCollectionIfNotExists<ProductCategory>("ProductCategories");
        //    CreateCollectionIfNotExists<Receipt>("Receipts");
        //    CreateCollectionIfNotExists<ReceiptDetail>("ReceiptDetails");
        //}

        //private void CreateCollectionIfNotExists<T>(string collectionName)
        //{
        //    var collections = database.ListCollectionNames().ToList();

        //    if (!collections.Contains(collectionName))
        //    {
        //        database.CreateCollection(collectionName);
        //        Console.WriteLine($"Collection {collectionName} is created.");
        //    }
        //    else
        //    {
        //        Console.WriteLine($"Collection {collectionName} is already існує.");
        //    }
        //}
    }
}