namespace _05.ChangeTownNamesCasing
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using _01.InitialSetup;

    public class StartUp
    {
        public static void Main()
        {
            string countryName = Console.ReadLine();

            using (SqlConnection connection = new SqlConnection(Configuration.conncectionString))
            {
                connection.Open();

                int countryId = GetCountryId(countryName, connection);
                List<string> townsAffected = GetAffectedTowns(countryId, connection);

                string townOrTowns = townsAffected.Count == 1 ? "town" : "towns";

                Console.WriteLine($"{townsAffected.Count} {townOrTowns} were affected.");
                Console.WriteLine($"[{string.Join(", ", townsAffected)}]");

                connection.Close();
            }
        }

        private static List<string> GetAffectedTowns(int countryId, SqlConnection connection)
        {
            List<string> updatedTowns = new List<string>();

            string updateTowns = "UPDATE Towns SET Name = UPPER(Name) WHERE CountryCode = @countryId";

            using (SqlCommand command = new SqlCommand(updateTowns, connection))
            {
                command.Parameters.AddWithValue("@countryId", countryId);

                if (command.ExecuteNonQuery() == 0)
                {
                    Console.WriteLine("No town names were affected.");
                    Environment.Exit(0);
                }

                command.ExecuteNonQuery();
            }

            string getTowns = "SELECT Name FROM Towns WHERE CountryCode = @countryId";

            using (SqlCommand command = new SqlCommand(getTowns, connection))
            {
                command.Parameters.AddWithValue("@countryId", countryId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        updatedTowns.Add((string)reader[0]);
                    }
                }
            }

            return updatedTowns;
        }

        private static int GetCountryId(string countryName, SqlConnection connection)
        {
            string countryIdSql = "SELECT Id FROM Countries WHERE Name = @countryName";

            using (SqlCommand command = new SqlCommand(countryIdSql, connection))
            {
                command.Parameters.AddWithValue("@countryName", countryName);

                if (command.ExecuteScalar() == null)
                {
                    Console.WriteLine("No town names were affected.");
                    Environment.Exit(0);
                }

                return (int)command.ExecuteScalar();
            }
        }
    }
}