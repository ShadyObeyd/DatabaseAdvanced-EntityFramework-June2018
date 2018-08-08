namespace FastFood.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Dto.Import;
    using FastFood.Models;
    using FastFood.Models.Enums;
    using Newtonsoft.Json;
    using DataAnnotations = System.ComponentModel.DataAnnotations;

    public static class Deserializer
	{
		private const string FailureMessage = "Invalid data format.";
		private const string SuccessMessage = "Record {0} successfully imported.";

		public static string ImportEmployees(FastFoodDbContext context, string jsonString)
		{
            var desirializedEmployees = JsonConvert.DeserializeObject<EmployeeDTO[]>(jsonString);

            StringBuilder sb = new StringBuilder();

            List<Employee> employees = new List<Employee>();

            foreach (EmployeeDTO employeeDTO in desirializedEmployees)
            {
                if (!IsValid(employeeDTO))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                Position position = context.Positions.FirstOrDefault(p => p.Name == employeeDTO.Position);

                if (position == null)
                {
                    position = new Position
                    {
                        Name = employeeDTO.Position
                    };

                    context.Positions.Add(position);
                    context.SaveChanges();
                }

                Employee employee = new Employee
                {
                    Name = employeeDTO.Name,
                    Age = employeeDTO.Age,
                    Position = position
                };

                employees.Add(employee);

                sb.AppendLine(string.Format(SuccessMessage, employee.Name));
            }

            context.AddRange(employees);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
		}

		public static string ImportItems(FastFoodDbContext context, string jsonString)
		{
            var desirializedItems = JsonConvert.DeserializeObject<ItemDTO[]>(jsonString);

            StringBuilder sb = new StringBuilder();

            List<Item> items = new List<Item>();

            foreach (ItemDTO itemDTO in desirializedItems)
            {
                if (!IsValid(itemDTO))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                if (items.Any(i => i.Name == itemDTO.Name))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                Category category = context.Categories.FirstOrDefault(c => c.Name == itemDTO.Category);

                if (category == null)
                {
                    category = new Category
                    {
                        Name = itemDTO.Category
                    };

                    context.Categories.Add(category);
                    context.SaveChanges();
                }

                Item item = new Item
                {
                    Category = category,
                    Name = itemDTO.Name,
                    Price = itemDTO.Price
                };

                items.Add(item);

                sb.AppendLine(string.Format(SuccessMessage, item.Name));
            }

            context.AddRange(items);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

		public static string ImportOrders(FastFoodDbContext context, string xmlString)
		{
            var serializer = new XmlSerializer(typeof(OrderDTO[]), new XmlRootAttribute("Orders"));

            StringBuilder sb = new StringBuilder();

            List<Order> orders = new List<Order>();
            List<OrderItem> orderItems = new List<OrderItem>();

            using (var reader = new StringReader(xmlString))
            {
                OrderDTO[] desirializedOrders = (OrderDTO[])serializer.Deserialize(reader);

                foreach (OrderDTO orderDTO in desirializedOrders)
                {
                    if (!IsValid(orderDTO))
                    {
                        sb.AppendLine(FailureMessage);
                        continue;
                    }

                    bool allItemsValid = true;

                    foreach (OrderItemDTO orderItemDTO in orderDTO.OrderItemDTOs)
                    {
                        if (!IsValid(orderItemDTO))
                        {
                            allItemsValid = false;
                            break;
                        }
                    }

                    if (!allItemsValid)
                    {
                        sb.AppendLine(FailureMessage);
                        continue;
                    }

                    bool allItemsExist = true;

                    foreach (OrderItemDTO orderItemDTO in orderDTO.OrderItemDTOs)
                    {
                        if (!context.Items.Any(i => i.Name == orderItemDTO.Name))
                        {
                            allItemsExist = false;
                            break;
                        }
                    }

                    if (!allItemsExist)
                    {
                        sb.AppendLine(FailureMessage);
                        continue;
                    }

                    Employee employee = context.Employees.FirstOrDefault(e => e.Name == orderDTO.Employee);

                    if (employee == null)
                    {
                        sb.AppendLine(FailureMessage);
                        continue;
                    }

                    Order order = new Order
                    {
                        Customer = orderDTO.Customer,
                        Employee = employee,
                        DateTime = DateTime.ParseExact(orderDTO.DateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                        Type = Enum.Parse<OrderType>(orderDTO.Type)
                    };

                    orders.Add(order);

                    foreach (OrderItemDTO orderItemDTO in orderDTO.OrderItemDTOs)
                    {
                        Item item = context.Items.FirstOrDefault(i => i.Name == orderItemDTO.Name);

                        OrderItem orderItem = new OrderItem
                        {
                            Order = order,
                            Item = item,
                            Quantity = int.Parse(orderItemDTO.Quantity)
                        };

                        orderItems.Add(orderItem);
                    }

                    sb.AppendLine($"Order for {order.Customer} on {order.DateTime.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)} added");
                }

                context.Orders.AddRange(orders);
                context.SaveChanges();

                context.OrderItems.AddRange(orderItems);
                context.SaveChanges();

                return sb.ToString().TrimEnd();
            }
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new DataAnnotations.ValidationContext(obj);

            var validationResults = new List<DataAnnotations.ValidationResult>();

            return DataAnnotations.Validator.TryValidateObject(obj, validationContext, validationResults, true);
        }
	}
}