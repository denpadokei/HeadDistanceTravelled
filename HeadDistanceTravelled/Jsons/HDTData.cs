using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using UnityEngine;

namespace HeadDistanceTravelled.Jsons
{
    public class HDTData
    {
        public class BeatmapResult
        {
            public string LevelID { get; set; }
            public string SongName { get; set; }
            public float Distance { get; set; }
            public DateTime CreatedAt { get; set; }
            public BeatmapResult()
            {

            }
            public BeatmapResult(string levelID, string songName, float Distance, DateTime now)
            {
                this.LevelID = levelID;
                this.SongName = songName;
                this.Distance = Distance;
                this.CreatedAt = now;
            }
        }
        public float HeadDistanceTravelled { get; set; }
        public ReadOnlyCollection<BeatmapResult> BeatmapResults { get; set; }
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
            }
            catch (Exception e) {
                Plugin.Log.Error(e);
            }
        }

        public HDTData()
        {
            this.HeadDistanceTravelled = 0;
            this.BeatmapResults = new ReadOnlyCollection<BeatmapResult>(new List<BeatmapResult>());
        }
    }
}
