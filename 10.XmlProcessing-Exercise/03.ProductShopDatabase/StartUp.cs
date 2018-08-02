namespace _03.ProductShopDatabase
{
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;

    using AutoMapper.QueryableExtensions;
    using Data;
    using ToExportDTOs = Data.ToExportDTOs;

    public class StartUp
    {
        public static void Main()
        {
            Configuration.ConfigureDatabase();

            using (ProductShopContext context = new ProductShopContext())
            {
                GetProductsInRange(context);
                GetSoldProducts(context);
                GetCategoriesByProductsCount(context);
                GetUsersAndProducts(context);
            }
        }

        // Querry 1: Products In Range
        private static void GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(p => p.Price >= 1000 && p.Price <= 2000 && p.Buyer != null)
                .OrderBy(p => p.Price)
                .ProjectTo<ToExportDTOs.ProductInfoDTO>()
                .ToArray();

            var serializer = new XmlSerializer(typeof(ToExportDTOs.ProductInfoDTO[]), new XmlRootAttribute("products"));

            using (var writer = new StreamWriter("../../../CreatedXmlFiles/products-in-range.xml"))
            {
                serializer.Serialize(writer, products);
            }
        }

        // Querry 2: Sold Products
        private static void GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Count >= 1)
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ProjectTo<ToExportDTOs.UserDTO>()
                .ToArray();

            var serializer = new XmlSerializer(typeof(ToExportDTOs.UserDTO[]), new XmlRootAttribute("users"));

            using (var writer = new StreamWriter("../../../CreatedXmlFiles/users-sold-products.xml"))
            {
                serializer.Serialize(writer, users);
            }
        }

        // Querry 3: Categories By Products Count
        private static void GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .OrderByDescending(c => c.CategoryProducts.Count)
                .ProjectTo<ToExportDTOs.CategoryDTO>()
                .ToArray();

            var serializer = new XmlSerializer(typeof(ToExportDTOs.CategoryDTO[]), new XmlRootAttribute("categories"));

            using (var writer = new StreamWriter("../../../CreatedXmlFiles/categories-by-products.xml"))
            {
                serializer.Serialize(writer, categories);
            }
        }

        // Querry 4: Users And Products
        private static void GetUsersAndProducts(ProductShopContext context)
        {

            var users = new ToExportDTOs.UsersDTO
            {
                UsersCount = context.Users.Where(u => u.ProductsSold.Count >= 1).Count(),
                Users = context.Users
                .Where(u => u.ProductsSold.Count >= 1)
                .OrderByDescending(u => u.ProductsSold.Count)
                .ThenBy(u => u.LastName)
                .Select(u => new ToExportDTOs.UserFullInfoDTO
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age.ToString(),
                    SoldProduct = new ToExportDTOs.SoldProductDTO
                    {
                        Count = u.ProductsSold.Count,
                        SoldProducts = u.ProductsSold.Select(p => new ToExportDTOs.ProductWithAttributesDTO
                        {
                            Name = p.Name,
                            Price = p.Price.ToString()
                        }).ToList()
                    }
                }).ToList()
            };

            var serializer = new XmlSerializer(typeof(ToExportDTOs.UsersDTO));

            using (var writer = new StreamWriter("../../../CreatedXmlFiles/users-and-products.xml"))
            {
                serializer.Serialize(writer, users);
            }
        }
    }
}