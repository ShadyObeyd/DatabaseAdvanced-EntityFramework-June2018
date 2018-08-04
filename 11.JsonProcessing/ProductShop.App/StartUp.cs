namespace ProductShop.App
{
    using System.IO;
    using System.Linq;

    using AutoMapper;
    using Data;
    using AutoMapper.QueryableExtensions;
    using Dto;
    using Newtonsoft.Json;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            Mapper.Initialize(cfg => cfg.AddProfile<ProductShopProfile>());

            using (var context = new ProductShopContext())
            {
                Configuration.Configure(context);

                GetProductsInRange(context);
                GetSuccessfullySoldProducts(context);
                GetCategoriesByProductsCount(context);
                GetUsersAndProducts(context);
            }
        }

        //Query 4: Users and Products
        private static void GetUsersAndProducts(ProductShopContext context)
        {
            var users = new UsersDTO
            {
                UsersCount = context.Users.Where(u => u.ProductsSold.Count >= 1).Count(),
                Users = context.Users.Where(u => u.ProductsSold.Count >= 1)
                .OrderByDescending(u => u.ProductsSold.Count)
                .ThenBy(u => u.LastName)
                .Select(u => new UserWithCountDTO
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age.ToString(),
                    Products = new ProductsDTO
                    {
                        ProductCount = u.ProductsSold.Count().ToString(),
                        Products = u.ProductsSold.Select(p => new ProductWithCountDTO
                        {
                            Name = p.Name,
                            Price = p.Price.ToString()
                        }).ToList()
                    }
                }).ToList()
            };

            string serializedUsers = JsonConvert.SerializeObject(users, Formatting.Indented);

            File.WriteAllText("../../../Json/users-and-products.json", serializedUsers);
        }

        //Query 3 Categories By Products Count
        private static void GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .OrderByDescending(c => c.CategoryProducts.Count)
                .ProjectTo<CategoryDTO>()
                .ToArray();

            string serializedCategories = JsonConvert.SerializeObject(categories, Formatting.Indented);

            File.WriteAllText("../../../Json/categories-by-products.json", serializedCategories);
        }

        // Query 2: Successfully Sold Products
        private static void GetSuccessfullySoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Count() >= 1 && u.ProductsSold.All(p => p.Buyer != null))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ProjectTo<UserDTO>()
                .ToArray();

            string serializedUsers = JsonConvert.SerializeObject(users, Formatting.Indented);

            File.WriteAllText("../../../Json/users-sold-products.json", serializedUsers);
        }

        // Query 1: Products In Range
        private static void GetProductsInRange(ProductShopContext context)
        {
            var users = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .ProjectTo<ProductDTO>()
                .ToArray();

            string serializedProducts = JsonConvert.SerializeObject(users, Formatting.Indented);

            File.WriteAllText("../../../Json/products-in-range.json", serializedProducts);
        }
    }
}