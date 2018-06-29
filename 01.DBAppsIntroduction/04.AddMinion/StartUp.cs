namespace _04.AddMinion
{
    using System;
    using System.Data.SqlClient;
    using _01.InitialSetup;

    public class StartUp
    {
        public static void Main()
        {
            string[] minionTokens = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            string minionName = minionTokens[1];
            string age = minionTokens[2];
            string town = minionTokens[3];

            string[] villainTokens = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string villainName = villainTokens[1];

            using (SqlConnection connection = new SqlConnection(Configuration.conncectionString))
            {
                connection.Open();

                int townId = GetTownId(town, connection);
                int minionId = InsertMinionAndGetId(minionName, age, townId, connection);
                int villainId = GetVillainId(villainName, connection);
                AssignMinionToVillain(minionId, minionName, villainId, villainName, connection);

                connection.Close();
            }
        }

        private static void AssignMinionToVillain(int minionId, string minionName, int villainId, string villianName, SqlConnection connection)
        {
            string assign = "INSERT INTO MinionsVillains (MinionId, villainId) VALUES (@minionId, @villianId)";

            using (SqlCommand command = new SqlCommand(assign, connection))
            {
                command.Parameters.AddWithValue("@minionId", minionId);
                command.Parameters.AddWithValue("@villianId", villainId);

                command.ExecuteNonQuery();

                Console.WriteLine($"Successfully added {minionName} to be minion of {villianName}.");
            }
        }

        private static int GetVillainId(string villainName, SqlConnection connection)
        {
            string villainSql = "SELECT Id FROM Villains WHERE [Name] = @villainName";

            using (SqlCommand command = new SqlCommand(villainSql, connection))
            {
                command.Parameters.AddWithValue("@villainName", villainName);

                if (command.ExecuteScalar() == null)
                {
                    InsertVillain(villainName, connection);
                    Console.WriteLine($"Villain {villainName} was added to the database.");
                }

                return (int)command.ExecuteScalar();
            }
        }

        private static void InsertVillain(string villainName, SqlConnection connection)
        {
            string insertVillain = "INSERT INTO Villains ([Name], EvilnessFactorId) VALUES (@villainName, 4)";

            using (SqlCommand command = new SqlCommand(insertVillain, connection))
            {
                command.Parameters.AddWithValue("@villainName", villainName);

                command.ExecuteNonQuery();
            }
        }

        private static int InsertMinionAndGetId(string minionName, string age, int townId, SqlConnection connection)
        {
            string minionSql = "INSERT INTO Minions ([Name], Age, TownId) VALUES (@minionName, @age, @townId)";

            using (SqlCommand command = new SqlCommand(minionSql, connection))
            {
                command.Parameters.AddWithValue("@minionName", minionName);
                command.Parameters.AddWithValue("@age", age);
                command.Parameters.AddWithValue("@TownId", townId);

                command.ExecuteNonQuery();
            }

            string minionIdSql = "SELECT Id FROM Minions WHERE [Name] = @name";

            using (SqlCommand command = new SqlCommand(minionIdSql, connection))
            {
                command.Parameters.AddWithValue("@name", minionName);

                return (int)command.ExecuteScalar();
            }
        }

        private static int GetTownId(string town, SqlConnection connection)
        {
            string townIdSql = "SELECT Id FROM Towns WHERE [Name] = @townName";

            using (SqlCommand command = new SqlCommand(townIdSql, connection))
            {
                command.Parameters.AddWithValue("@townName", town);

                if (command.ExecuteScalar() == null)
                {
                    InsertTown(town, connection);
                    Console.WriteLine($"Town {town} was added to the database.");
                }

                return (int)command.ExecuteScalar();
            }
        }

        private static void InsertTown(string town, SqlConnection connection)
        {
            string insertTown = "INSERT INTO Towns ([Name]) VALUES (@town)";

            using (SqlCommand command = new SqlCommand(insertTown, connection))
            {
                command.Parameters.AddWithValue("@town", town);

                command.ExecuteNonQuery();
            }
        }
    }
}
