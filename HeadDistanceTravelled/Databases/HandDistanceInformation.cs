using HeadDistanceTravelled.Databases.Interfaces;
using System;

namespace HeadDistanceTravelled.Databases
{
    /// <summary>
    /// 1プレイごとの左右手の移動距離を記録するエンティティ。
    /// </summary>
    public class HandDistanceInformation : IIdEntity
    {
        public int ID { get; set; }
        public DateTime CreatedAt { get; set; }
        public string LevelID { get; set; }
        public string SongName { get; set; }
        public string Difficurity { get; set; }
        public int BeatmapCharacteristicTextId { get; set; }
        public float LeftDistance { get; set; }
        public float RightDistance { get; set; }
        /// <summary>
        /// Manual計測中に記録された場合のセッションGUID。計測外なら<see cref="Guid.Empty"/>。
        /// </summary>
        public Guid SessionGUID { get; set; } = Guid.Empty;
    }
}
