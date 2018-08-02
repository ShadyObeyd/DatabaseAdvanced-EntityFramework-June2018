namespace _02.CatalogOfMusicalAlbums
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Serialization;
    using Models;

    public class StartUp
    {
        public static void Main()
        {
            Album[] albums = GetAlbums();

            var serializer = new XmlSerializer(typeof(Album[]));

            using (var writer = new StreamWriter("../../../catalog.xml"))
            {
                serializer.Serialize(writer, albums);
            }
        }

        private static Album[] GetAlbums()
        {
            List<Song> songs = GetSongs();

            Album album = new Album
            {
                Name = "The Sickness",
                Artist = "Disturbed",
                Producer = "Johny K",
                Year = 2000,
                Price = "$9.99",
                Songs = songs
            };

            return new Album[] { album };
        }

        private static List<Song> GetSongs()
        {
            List<Song> songs = new List<Song>
            {
                new Song{ Title = "Voices", Duration = "03:11" },
                new Song{ Title = "The Game", Duration = "03:47" },
                new Song{ Title = "Stupify", Duration = "04:34" },
                new Song{ Title = "Down With The Sickness", Duration = "04:38" },
                new Song{ Title = "Violece Fetish", Duration = "03:23" },
                new Song{ Title = "Fear", Duration = "03:46" },
                new Song{ Title = "Numb", Duration = "03:44" },
                new Song{ Title = "Want", Duration = "00:03:52" },
                new Song{ Title = "Conflict", Duration = "04:35" },
                new Song{ Title = "Shout 2000", Duration = "04:18" },
                new Song{ Title = "Dropping Plates", Duration = "03:48" },
                new Song{ Title = "Meaning of Life", Duration = "04:01" }
            };

            return songs;
        }
    }
}
