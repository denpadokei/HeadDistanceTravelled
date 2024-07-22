using IPA.Utilities.Async;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using UnityEngine;

namespace HeadDistanceTravelled.Jsons
{
    public class HDTData
    {
        [JsonIgnore]
        public static HDTData Instance { get; } = new HDTData();
        public class BeatmapResult
        {
            public string LevelID { get; set; }
            public string SongName { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate, Required = Required.Default)]
            [DefaultValue(null)]
            public string Difficurity { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate, Required = Required.Default)]
            [DefaultValue(null)]
            public string BeatmapCharacteristic { get; set; }
            public float Distance { get; set; }
            public DateTime CreatedAt { get; set; }
            public BeatmapResult()
            {

            }
            public BeatmapResult(string levelID, string songName, string diff, string beatmapChara, float Distance, DateTime now)
            {
                this.LevelID = levelID;
                this.SongName = songName;
                this.Difficurity = diff;
                this.BeatmapCharacteristic = beatmapChara;
                this.Distance = Distance;
                this.CreatedAt = now;
            }
        }
        public float HeadDistanceTravelled { get; set; }
        public ReadOnlyCollection<BeatmapResult> BeatmapResults { get; set; }

        
        public event Action<object> OnLoaded;
        public event Action<object> OnSaved;

        public void Load()
        {
            try {
                var filePath = Path.Combine(Application.persistentDataPath, "HMDDistance.dat");
                if (File.Exists(filePath)) {
                    var text = File.ReadAllText(filePath);
                    var data = JsonConvert.DeserializeObject<HDTData>(text);
                    this.HeadDistanceTravelled = data.HeadDistanceTravelled;
                    this.BeatmapResults = data.BeatmapResults;
                }
                this.OnLoaded?.Invoke(this);
            }
            catch (Exception e) {
                Plugin.Log.Error(e);
            }
        }
        public void Save()
        {
            try {
                var filePath = Path.Combine(Application.persistentDataPath, "HMDDistance.dat");
                var text = JsonConvert.SerializeObject(this, Formatting.Indented);
                File.WriteAllText(filePath, text);
                this.OnSaved?.Invoke(this);
            }
            catch (Exception e) {
                Plugin.Log.Error(e);
            }
        }

        public void UpdateTotalDistance()
        {
            this.HeadDistanceTravelled = this.BeatmapResults.Sum(x => x.Distance);
        }

        private HDTData()
        {
            this.HeadDistanceTravelled = 0;
            this.BeatmapResults = new ReadOnlyCollection<BeatmapResult>(new List<BeatmapResult>());
        }
    }
}
