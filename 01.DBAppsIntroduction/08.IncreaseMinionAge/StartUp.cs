namespace _08.IncreaseMinionAge
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using _01.InitialSetup;

    public class StartUp
    {
        public static void Main()
        {
            int[] minionIdTokens = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

            using (SqlConnection connection = new SqlConnection(Configuration.conncectionString))
            {
                connection.Open();

                for (int i = 0; i < minionIdTokens.Length; i++)
                {
                    int inputId = minionIdTokens[i];
                    int minionId = ValidateId(inputId, connection);
                    IncreaseAge(minionId, connection);
                    ChangeName(minionId, connection);
                }

                PrintNames(connection);

                connection.Close();
            }
        }

        private static void PrintNames(SqlConnection connection)
        {
            string getMinionsSql = "SELECT [Name], Age FROM Minions";

            using (SqlCommand command = new SqlCommand(getMinionsSql, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader[0]} {reader[1]}");
                    }
                }
            }
        }

        private static void ChangeName(int minionId, SqlConnection connection)
        {
            List<string> nameParts = new List<string>();

            string minionNameSql = "SELECT [Name] FROM Minions WHERE Id = @id";

            string minionName = string.Empty;

            using (SqlCommand command = new SqlCommand(minionNameSql, connection))
            {
                command.Parameters.AddWithValue("@id", minionId);
                minionName = (string)command.ExecuteScalar();
            }

            string[] minionNameTokens = minionName.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < minionNameTokens.Length; i++)
            {
                string namePart = minionNameTokens[i];

                string newNamePart = char.ToUpper(namePart[0]) + namePart.Substring(1);

                nameParts.Add(newNamePart);
            }

            string newNameStr = string.Join(" ", nameParts);

            string updateDatabase = "UPDATE Minions SET [Name] = @name WHERE Id = @id";

            using (SqlCommand command = new SqlCommand(updateDatabase, connection))
            {
                command.Parameters.AddWithValue("@name", newNameStr);
                command.Parameters.AddWithValue("@id", minionId);

                command.ExecuteNonQuery();
            }
        }

        private static int ValidateId(int inputId, SqlConnection connection)
        {
            string minionIdSql = "SELECT Id FROM Minions WHERE Id = @id";

            using (SqlCommand command = new SqlCommand(minionIdSql, connection))
            {
                command.Parameters.AddWithValue("@id", inputId);

                if (command.ExecuteScalar() == null)
                {
                    Console.WriteLine("Invalid Minion Id!");
                    Environment.Exit(0);
                }

                return (int)command.ExecuteScalar();
            }
        }

        private static void IncreaseAge(int minionId, SqlConnection connection)
        {
            string increaseAgeSql = "UPDATE Minions SET Age += 1 WHERE Id = @id";

            using (SqlCommand command = new SqlCommand(increaseAgeSql, connection))
            {
                command.Parameters.AddWithValue("@id", minionId);

                command.ExecuteNonQuery();
            }
        }
    }
}