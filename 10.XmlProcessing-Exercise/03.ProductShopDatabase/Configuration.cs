namespace _03.ProductShopDatabase
{
    using System;
    using System.IO;
    using System.Xml.Serialization;
    using System.Collections.Generic;
    using System.Linq;

    using DataAnotations = System.ComponentModel.DataAnnotations;
    using Data.Models;
    using AutoMapper;
    using Data.ReadingDTOs;
    
    using Data;

    public class Configuration
    {
        public const string ConnectionString = @""; // <-- Insert connection string here!!!

        public static void ConfigureDatabase()
        {
            Mapper.Initialize(cfg => cfg.AddProfile<ProductShopProfile>());

            using (var context = new ProductShopContext())
            {
                if (!context.Users.Any() && !context.Categories.Any() && !context.Products.Any() && !context.CategoryProducts.Any())
                {
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();

                    var users = GetUsers();
                    context.Users.AddRange(users);
                    context.SaveChanges();

                    var categories = GetCategories();
                    context.Categories.AddRange(categories);
                    context.SaveChanges();

                    var products = GetProducts(users);
                    context.Products.AddRange(products);
                    context.SaveChanges();

                    var categoryProducts = GetCategoryProducts();
                    context.CategoryProducts.AddRange(categoryProducts);
                    context.SaveChanges();
                }
            }
        }

        private static List<CategoryProduct> GetCategoryProducts()
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

            return categoryProducts;
        }

        private static List<Product> GetProducts(List<User> users)
        {
            var serializer = new XmlSerializer(typeof(ProductDTO[]), new XmlRootAttribute("products"));

            List<Product> products = new List<Product>();

            using (var writer = new StreamReader("../../../XmlFiles/products.xml"))
            {
                ProductDTO[] productDTOs = (ProductDTO[])serializer.Deserialize(writer);

                var cntr = 1;

                foreach (ProductDTO productDTO in productDTOs)
                {
                    if (!IsValid(productDTO))
                    {
                        continue;
                    }

                    var product = Mapper.Map<Product>(productDTO);

                    if (cntr % 6 == 0)
                    {
                        product.BuyerId = null;
                    }
                    else
                    {
                        int buyerId = new Random().Next(1, (users.Count / 2) + 1);

                        product.BuyerId = buyerId;
                    }

                    int sellerId = new Random().Next((users.Count / 2) + 2, users.Count + 1);
                    product.SellerId = sellerId;

                    products.Add(product);

                    cntr++;
                }
            }

            return products;
        }

        private static List<Category> GetCategories()
        {
            var serializer = new XmlSerializer(typeof(CategoryDTO[]), new XmlRootAttribute("categories"));

            List<Category> categories = new List<Category>();

            using (var writer = new StreamReader("../../../XmlFiles/categories.xml"))
            {
                CategoryDTO[] categoryDTOs = (CategoryDTO[])serializer.Deserialize(writer);

                foreach (CategoryDTO categoryDTO in categoryDTOs)
                {
                    if (!IsValid(categoryDTO))
                    {
                        continue;
                    }

                    var category = Mapper.Map<Category>(categoryDTO);

                    categories.Add(category);
                }
            }

            return categories;
        }

        private static List<User> GetUsers()
        {
            var serializer = new XmlSerializer(typeof(UserDTO[]), new XmlRootAttribute("users"));

            List<User> users = new List<User>();

            using (var writer = new StreamReader("../../../XmlFiles/users.xml"))
            {
                UserDTO[] userDTOs = (UserDTO[])serializer.Deserialize(writer);

                foreach (UserDTO userDTO in userDTOs)
                {
                    if (!IsValid(userDTO))
                    {
                        continue;
                    }

                    var user = Mapper.Map<User>(userDTO);

                    users.Add(user);
                }
            }

            return users;
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new DataAnotations.ValidationContext(obj);
            var validationResults = new List<DataAnotations.ValidationResult>();

            return DataAnotations.Validator.TryValidateObject(obj, validationContext, validationResults, true);
        }
    }
}