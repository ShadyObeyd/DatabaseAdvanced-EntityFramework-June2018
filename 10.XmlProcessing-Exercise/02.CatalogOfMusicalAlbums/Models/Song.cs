namespace _02.CatalogOfMusicalAlbums.Models
{
    using System.Xml.Serialization;

    [XmlType("song")]
    public class Song
    {
        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("duration")]
        public string Duration { get; set; }
    }
}
