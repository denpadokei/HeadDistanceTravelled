using HeadDistanceTravelled.Databases.Interfaces;
using HeadDistanceTravelled.Jsons;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
