namespace _03.MinionNames
{
    using System;
    using System.Data.SqlClient;
    using _01.InitialSetup;

    public class StartUp
    {
        public static void Main()
        {
            int villainId = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(Configuration.conncectionString))
            {
                connection.Open();

                string villainName = null;

                string nameSql = "SELECT [Name] FROM Villains WHERE Id = @id";

                using (SqlCommand command = new SqlCommand(nameSql, connection))
                {
                    command.Parameters.AddWithValue("@id", villainId);

                    villainName = (string)command.ExecuteScalar();
                }

                if (villainName == null)
                {
                    Console.WriteLine($"No villain with ID {villainId} exists in the database.");
                }
                else
                {
                    Console.WriteLine($"Villian: {villainName}");

                    string minionNamesSql = "SELECT m.[Name], m.Age FROM Minions AS m JOIN MinionsVillains AS mv ON mv.MinionId = m.Id WHERE mv.VillainId = @id ORDER BY m.[Name] ASC";

                    using (SqlCommand command = new SqlCommand(minionNamesSql, connection))
                    {
                        command.Parameters.AddWithValue("id", villainId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                int cntr = 1;

                                while (reader.Read())
                                {
                                    string minionName = reader[0].ToString();
                                    string minionAge = reader[1].ToString();

                                    Console.WriteLine($"{cntr}. {minionName} {minionAge}");

                                    cntr++;
                                }
                            }
                            else
                            {
                                Console.WriteLine("(no minions)");
                            }
                        }
                    }
                }

                connection.Close();
            }
        }
    }
}