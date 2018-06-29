namespace _06.RemoveVillain
{
    using System;
    using System.Data.SqlClient;
    using _01.InitialSetup;

    public class StartUp
    {
        public static void Main()
        {
            int inputVillianId = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(Configuration.conncectionString))
            {
                connection.Open();

                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    int villianId = GetVillianId(inputVillianId, connection, transaction);

                    int affectedRows = ReleaseMinions(villianId, connection, transaction);

                    string deletedVillain = DeleteVillainAndGetName(villianId, connection, transaction);

                    Console.WriteLine($"{deletedVillain} was deleted.");
                    Console.WriteLine($"{affectedRows} minions were released.");
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                }

                connection.Close();
            }
        }

        private static string DeleteVillainAndGetName(int villianId, SqlConnection connection, SqlTransaction transaction)
        {
            string villainNameSql = "SELECT [Name] FROM Villains WHERE Id = @id";

            string villainName = string.Empty;

            using (SqlCommand command = new SqlCommand(villainNameSql, connection, transaction))
            {
                command.Parameters.AddWithValue("@id", villianId);

                villainName = (string)command.ExecuteScalar();
            }

            string deleteVillainSql = "DELETE FROM Villains WHERE Id = @id";

            using (SqlCommand command = new SqlCommand(deleteVillainSql, connection, transaction))
            {
                command.Parameters.AddWithValue("@id", villianId);

                command.ExecuteNonQuery();
            }

            return villainName;
        }

        private static int ReleaseMinions(int villianId, SqlConnection connection, SqlTransaction transaction)
        {
            string minionsRelease = "DELETE FROM MinionsVillains WHERE VillainId = @id";

            using (SqlCommand command = new SqlCommand(minionsRelease, connection, transaction))
            {
                command.Parameters.AddWithValue("@id", villianId);

                return command.ExecuteNonQuery();
            }
        }

        private static int GetVillianId(int inputVillianId, SqlConnection connection, SqlTransaction transaction)
        {
            string villianSql = "SELECT Id FROM Villains WHERE Id = @id";

            using (SqlCommand command = new SqlCommand(villianSql, connection, transaction))
            {
                command.Parameters.AddWithValue("@id", inputVillianId);

                if (command.ExecuteScalar() == null)
                {
                    Console.WriteLine("No such villain was found.");
                    Environment.Exit(0);
                }

                return (int)command.ExecuteScalar();
            }
        }
    }
}
