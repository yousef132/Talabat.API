using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Repository.Data
{
    public class StoreContextSeed
    {
        public async static Task SeedAsync(StoreContext context)
        {
            if (context.ProductBrands.Count()==0)
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
            if (context.ProductCategories.Count()==0)
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
        }
    }
}
