namespace _01.InitialSetup
{
    using System.Data.SqlClient;

    public class StartUp
    {
        public static void Main()
        {
            using (SqlConnection connection = new SqlConnection(Configuration.conncectionString))
            {
                connection.Open();

                string createDatabaseCommand = "CREATE DATABASE MinionsDB";

                ExecNonQuerry(connection, createDatabaseCommand);

                connection.ChangeDatabase(Configuration.databaseName);

                string countriesTable = "CREATE TABLE Countries (Id INT PRIMARY KEY IDENTITY,Name VARCHAR(50))";

                ExecNonQuerry(connection, countriesTable);

                string townsTable = "CREATE TABLE Towns(Id INT PRIMARY KEY IDENTITY,Name VARCHAR(50), CountryCode INT FOREIGN KEY REFERENCES Countries(Id))";

                ExecNonQuerry(connection, townsTable);

                string minionsTable = "CREATE TABLE Minions(Id INT PRIMARY KEY IDENTITY,Name VARCHAR(30), Age INT, TownId INT FOREIGN KEY REFERENCES Towns(Id))";

                ExecNonQuerry(connection, minionsTable);

                string evilnessFactorsTable = "CREATE TABLE EvilnessFactors(Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50))";

                ExecNonQuerry(connection, evilnessFactorsTable);

                string villainsTable = "CREATE TABLE Villains (Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50), EvilnessFactorId INT FOREIGN KEY REFERENCES EvilnessFactors(Id))";

                ExecNonQuerry(connection, villainsTable);

                string minionsVillainsTable = "CREATE TABLE MinionsVillains (MinionId INT FOREIGN KEY REFERENCES Minions(Id),VillainId INT FOREIGN KEY REFERENCES Villains(Id),CONSTRAINT PK_MinionsVillains PRIMARY KEY (MinionId, VillainId))";

                ExecNonQuerry(connection, minionsVillainsTable);

                string countriesInsert = "INSERT INTO Countries ([Name]) VALUES ('Bulgaria'),('England'),('Cyprus'),('Germany'),('Norway')";

                ExecNonQuerry(connection, countriesInsert);

                string townsInsert = "INSERT INTO Towns ([Name], CountryCode) VALUES ('Plovdiv', 1),('Varna', 1),('Burgas', 1),('Sofia', 1),('London', 2),('Southampton', 2),('Bath', 2),('Liverpool', 2),('Berlin', 3),('Frankfurt', 3),('Oslo', 4)";

                ExecNonQuerry(connection, townsInsert);

                string minionsInsert = "INSERT INTO Minions (Name,Age, TownId) VALUES('Bob', 42, 3),('Kevin', 1, 1),('Bob ', 32, 6),('Simon', 45, 3),('Cathleen', 11, 2),('Carry ', 50, 10),('Becky', 125, 5),('Mars', 21, 1),('Misho', 5, 10),('Zoe', 125, 5),('Json', 21, 1)";

                ExecNonQuerry(connection, minionsInsert);

                string evilnessFactorsInsert = "INSERT INTO EvilnessFactors (Name) VALUES ('Super good'),('Good'),('Bad'), ('Evil'),('Super evil')";

                ExecNonQuerry(connection, evilnessFactorsInsert);

                string villainsInsert = "INSERT INTO Villains (Name, EvilnessFactorId) VALUES ('Gru',2),('Victor',1),('Jilly',3),('Miro',4),('Rosen',5),('Dimityr',1),('Dobromir',2)";

                ExecNonQuerry(connection, villainsInsert);

                string minionsVillainsInsert = "INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (4,2),(1,1),(5,7),(3,5),(2,6),(11,5),(8,4),(9,7),(7,1),(1,3),(7,3),(5,3),(4,3),(1,2),(2,1),(2,7)";

                ExecNonQuerry(connection, minionsVillainsInsert);

                connection.Close();
            }
        }

        private static void ExecNonQuerry(SqlConnection connection, string commandStr)
        {
            using (SqlCommand command = new SqlCommand(commandStr, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}
