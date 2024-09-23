using HeadDistanceTravelled.Databases.Interfaces;
using System;

namespace HeadDistanceTravelled.Databases
{
    public class DistanceInformation : IIdEntity
    {
        public int ID { get; set; }
        public DateTime CreatedAt { get; set; }
        public string LevelID { get; set; }
        public string SongName { get; set; }
        public string Difficurity { get; set; }
        public int BeatmapCharacteristicTextId { get; set; }
        public float Distance { get; set; }
    }
}
