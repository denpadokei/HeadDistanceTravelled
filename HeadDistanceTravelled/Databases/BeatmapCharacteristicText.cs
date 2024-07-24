using HeadDistanceTravelled.Databases.Interfaces;
using HeadDistanceTravelled.Jsons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadDistanceTravelled.Databases
{
    internal class BeatmapCharacteristicText : IIdEntity
    {
        public int ID { get; set; }
        public BeatmapCharacteristic BeatmapCharacteristicEnumValue { get; set; }
        public string Key { get; set; }
        public string DisplayName { get; set; }
    }
}
