namespace _02.CatalogOfMusicalAlbums.Models
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlType("album")]
    public class Album
    {
        public Album()
        {
            this.Songs = new List<Song>();
        }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("artist")]
        public string Artist { get; set; }

        [XmlElement("year")]
        public int Year { get; set; }

        [XmlElement("producer")]
        public string Producer { get; set; }

        [XmlIgnore]
        public string Price { get; set; }

        [XmlArray("songs")]
        public List<Song> Songs { get; set; }
    }
}
