using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using FastFood.Data;
using FastFood.DataProcessor.Dto.Export;
using FastFood.Models.Enums;
using Newtonsoft.Json;

namespace FastFood.DataProcessor
{
	public class Serializer
	{
        public static string ExportOrdersByEmployee(FastFoodDbContext context, string employeeName, string orderType)
        {
            var orderTypeEnum = Enum.Parse<OrderType>(orderType);

            var orders = new
            {
                Name = employeeName,
                Orders = context.Orders
                                .Where(o => o.Employee.Name == employeeName && o.Type == orderTypeEnum)
                                .OrderByDescending(o => o.TotalPrice)
                                .ThenByDescending(o => o.OrderItems.Select(oi => oi.Quantity))
                                .Select(o => new
                                {
                                    Customer = o.Customer,
                                    Items = o.OrderItems.Select(oi => new
                                    {
                                        Name = oi.Item.Name,
                                        Price = oi.Item.Price,
                                        Quantity = oi.Quantity
                                    }).ToArray(),
                                    TotalPrice = o.TotalPrice
                                }).ToArray(),
                           TotalMade = context.Orders.Where(o => o.Employee.Name == employeeName && o.Type == orderTypeEnum)
                                              .Sum(o => o.TotalPrice)
            };

            return JsonConvert.SerializeObject(orders, Newtonsoft.Json.Formatting.Indented); 
        }

		public static string ExportCategoryStatistics(FastFoodDbContext context, string categoriesString)
		{
            string[] categoriesArr = categoriesString.Split(',', StringSplitOptions.RemoveEmptyEntries);

            var categories = context.Categories
                .Where(c => categoriesArr.Contains(c.Name))
                .Select(c => new CategoryDTO
                {
                    Name = c.Name,
                    Item = c.Items.Select(i => new ItemDTO
                    {
                        Name = i.Name,
                        TimesSold = i.OrderItems.Sum(oi => oi.Quantity),
                        TotalMade = i.OrderItems.Sum(oi => oi.Item.Price * oi.Quantity)
                    }).OrderByDescending(i => i.TotalMade)
                      .ThenByDescending(i => i.TimesSold)
                      .FirstOrDefault()
                }).OrderByDescending(c => c.Item.TotalMade)
                  .ThenByDescending(c => c.Item.TimesSold)
                  .ToArray();


            StringBuilder sb = new StringBuilder();

            var xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            var serializer = new XmlSerializer(typeof(CategoryDTO[]), new XmlRootAttribute("Categories"));

            serializer.Serialize(new StringWriter(sb), categories, xmlNamespaces);

            return sb.ToString().TrimEnd();
		}
	}
}