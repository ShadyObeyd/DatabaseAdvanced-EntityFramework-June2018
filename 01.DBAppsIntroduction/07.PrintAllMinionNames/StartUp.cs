namespace _07.PrintAllMinionNames
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using _01.InitialSetup;

    public class StartUp
    {
        public static void Main()
        {
            using (SqlConnection connection = new SqlConnection(Configuration.conncectionString))
            {
                connection.Open();

                List<string> minionNames = GetMinionNames(connection);
                List<string> minionNamesReversed = GetReversedNames(connection);

                for (int i = 0; i < minionNames.Count / 2; i++)
                {
                    Console.WriteLine(minionNames[i]);
                    Console.WriteLine(minionNamesReversed[i]);
                }

                connection.Close();
            }
        }

        private static List<string> GetReversedNames(SqlConnection connection)
        {
            List<string> minionNames = new List<string>();

            string minionNamesSql = "SELECT [Name] FROM Minions ORDER BY Id DESC";

            using (SqlCommand command = new SqlCommand(minionNamesSql, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        minionNames.Add((string)reader[0]);
                    }

                    return minionNames;
                }
            }
        }

        private static List<string> GetMinionNames(SqlConnection connection)
        {
            List<string> minionNames = new List<string>();

            string minionNamesSql = "SELECT [Name] FROM Minions";

            using (SqlCommand command = new SqlCommand(minionNamesSql, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        minionNames.Add((string)reader[0]);
                    }

                    return minionNames;
                }
            }
        }
    }
}
