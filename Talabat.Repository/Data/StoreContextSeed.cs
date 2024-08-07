﻿using System.Text.Json;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Entities.Product_Aggregate;

namespace Talabat.Infrastructure.Data
{
    public class StoreContextSeed
    {
        public async static Task SeedAsync(StoreContext context)
        {
            if (context.ProductBrands.Count() == 0)
            {

                var Brands = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/brands.json");
                var brandsObjects = JsonSerializer.Deserialize<List<ProductBrand>>(Brands);

                if (brandsObjects?.Count() > 0)
                {
                    foreach (var brand in brandsObjects)
                    {
                        context.Set<ProductBrand>().Add(brand);
                    }
                    context.SaveChanges();
                }
            }
            if (context.ProductCategories.Count() == 0)
            {

                var categories = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/categories.json");
                var categoriesObjects = JsonSerializer.Deserialize<List<ProductCategory>>(categories);

                if (categoriesObjects?.Count() > 0)
                {
                    foreach (var category in categoriesObjects)
                    {
                        context.Set<ProductCategory>().Add(category);
                    }
                    context.SaveChanges();
                }
            }
            if (context.Products.Count() == 0)
            {

                var Products = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/products.json");
                var productsObjects = JsonSerializer.Deserialize<List<Product>>(Products);

                if (productsObjects?.Count() > 0)
                {
                    foreach (var product in productsObjects)
                    {
                        context.Set<Product>().Add(product);
                    }
                    context.SaveChanges();
                }
            }
            if (context.DeliveryMethod.Count() == 0)
            {

                var methods = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/delivery.json");
                var deliveryMethodsObjects = JsonSerializer.Deserialize<List<DeliveryMethod>>(methods);

                if (deliveryMethodsObjects?.Count() > 0)
                {
                    foreach (var method in deliveryMethodsObjects)
                    {
                        context.Set<DeliveryMethod>().Add(method);
                    }
                    context.SaveChanges();
                }
            }


        }
    }
}
