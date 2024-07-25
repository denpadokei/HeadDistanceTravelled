using HeadDistanceTravelled.Databases;
using HeadDistanceTravelled.Databases.Interfaces;
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
        public static void Upgrade(IHDTDatabase db)
        {
            HDTData.Instance.Load();
            if (!db.AnyBeatmapCharacteristic()) {
                db.SetDefaultValue();
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
                    var charaText = db.Find<BeatmapCharacteristicText>(x => x.BeatmapCharacteristicEnumValue == item.BeatmapCharacteristic.Value).FirstOrDefault();
                    entity.BeatmapCharacteristicTextId = charaText?.ID ?? -1;
                }
                else {
                    entity.BeatmapCharacteristicTextId = db.Find<BeatmapCharacteristicText>(x => x.BeatmapCharacteristicEnumValue == BeatmapCharacteristic.UnknownValue).FirstOrDefault()?.ID ?? 0;
                }
                if (!string.IsNullOrEmpty(item.Difficurity)) {
                    entity.Difficurity = item.Difficurity;
                }
                oldDatas.Add(entity);
            }
            Plugin.Log.Info($"Result length:{oldDatas.Count}");
            db.InsertBulk(oldDatas);
        }
    }
}
