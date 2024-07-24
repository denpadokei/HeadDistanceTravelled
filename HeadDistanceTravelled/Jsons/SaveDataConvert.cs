using HeadDistanceTravelled.Databases;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadDistanceTravelled.Jsons
{
    internal class SaveDataConvert
    {
        public static void Upgrade()
        {
            HDTData.Instance.Load();
            using (var dbInstance = new HDTDatabase()) {
                if (!dbInstance.AnyBeatmapCharacteristic()) {
                    dbInstance.SetDefaultValue();
                }
                var oldDatas = new List<DistanceInformation>();
                foreach (var item in HDTData.Instance.BeatmapResults) {
                    var entity = new DistanceInformation
                    {
                        LevelID = item.LevelID,
                        Distance = item.Distance,
                        SongName = item.SongName,
                        CreatedAt = item.CreatedAt,
                    };
                    if (item.BeatmapCharacteristic != null) {
                        var charaText = dbInstance.Find<BeatmapCharacteristicText>(x => x.BeatmapCharacteristicEnumValue == item.BeatmapCharacteristic.Value).FirstOrDefault();
                        entity.BeatmapCharacteristicTextId = charaText?.ID ?? -1;
                    }
                    else {
                        entity.BeatmapCharacteristicTextId = dbInstance.Find<BeatmapCharacteristicText>(x => x.BeatmapCharacteristicEnumValue == BeatmapCharacteristic.UnknownValue).FirstOrDefault()?.ID ?? 0;
                    }
                    if (!string.IsNullOrEmpty(item.Difficurity)) {
                        entity.Difficurity = item.Difficurity;
                    }
                    oldDatas.Add(entity);
                }
                Plugin.Log.Info($"Result length:{oldDatas.Count}");
                dbInstance.InsertBulk(oldDatas);
            }
        }
    }
}
