namespace ProductShop.App
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System;
    using DataAnnotations = System.ComponentModel.DataAnnotations;
    
    using Data;
    using Models;
    using Newtonsoft.Json;

    public class Configuration
    {
        public static void Configure(ProductShopContext context)
        {
            var users = GetUsers(context);
            var products = GetProducts(context);
            var categories = GetCategories(context);
            var categoryProducts = GetCategoryProducts(context);
        }

        private static List<CategoryProduct> GetCategoryProducts(ProductShopContext context)
        {
            List<CategoryProduct> categoryProducts = new List<CategoryProduct>();

            for (int productId = 1; productId <= 200; productId++)
            {
                int categoryId = new Random().Next(1, 12);

                var categoryProduct = new CategoryProduct
                {
                    ProductId = productId,
                    CategoryId = categoryId
                };

                categoryProducts.Add(categoryProduct);
            }

            if (!context.CategoryProducts.Any())
            {
                context.CategoryProducts.AddRange(categoryProducts);
                context.SaveChanges();
            }

            return categoryProducts;
        }

        private static List<Category> GetCategories(ProductShopContext context)
        {
            string jsonString = File.ReadAllText("../../../Json/categories.json");

            var desirializedCategories = JsonConvert.DeserializeObject<List<Category>>(jsonString);

            var categories = new List<Category>();

            foreach (Category category in desirializedCategories)
            {
                if (!IsValid(category))
                {
                    continue;
                }

                categories.Add(category);
            }

            if (!context.Categories.Any())
            {
                context.Categories.AddRange(categories);
                context.SaveChanges();
            }

            return categories;
        }

        private static List<Product> GetProducts(ProductShopContext context)
        {
            string jsonString = File.ReadAllText("../../../Json/products.json");

            var desirializedProducts = JsonConvert.DeserializeObject<List<Product>>(jsonString);

            var products = new List<Product>();

            int counter = 1;

            foreach (Product product in desirializedProducts)
            {
                if (!IsValid(product))
                {
                    continue;
                }

                int sellerId = new Random().Next(1, 31);

                product.SellerId = sellerId;

                if (counter % 6 == 0)
                {
                    product.BuyerId = null;
                }
                else
                {
                    int buyerId = new Random().Next(31, 57);
                    product.BuyerId = buyerId;
                }

                products.Add(product);

                counter++;
            }

            if (!context.Products.Any())
            {
                context.AddRange(products);
                context.SaveChanges();
            }

            return products;
        }

        private static List<User> GetUsers(ProductShopContext context)
        {
            string jsonString = File.ReadAllText("../../../Json/users.json");

            var desirializedUsers = JsonConvert.DeserializeObject<List<User>>(jsonString);

            List<User> users = new List<User>();

            foreach (User user in desirializedUsers)
            {
                if (!IsValid(user))
                {
                    continue;
                }

                users.Add(user);
            }

            if (!context.Users.Any())
            {
                context.Users.AddRange(users);
                context.SaveChanges();
            }

            return users;
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new DataAnnotations.ValidationContext(obj);

            var validationResults = new List<DataAnnotations.ValidationResult>();

            return DataAnnotations.Validator.TryValidateObject(obj, validationContext, validationResults, true);
        }
    }
}