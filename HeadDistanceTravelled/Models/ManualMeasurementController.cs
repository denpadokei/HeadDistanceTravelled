using HeadDistanceTravelled.Configuration;
using HeadDistanceTravelled.Databases;
using HeadDistanceTravelled.Databases.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace HeadDistanceTravelled.Models
{
    public class ManualMeasurementController : IInitializable, IDisposable
    {
        public enum MeasurementStatus
        {
            /// <summary>
            /// 計測中
            /// </summary>
            Measuring,
            /// <summary>
            /// 計測してない
            /// </summary>
            NotMeasuring
        }
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プロパティ
        public Guid CurrentSessionGUID { get; set; } = Guid.Empty;
        public DateTime StartDateTime { get; set; } = DateTime.MinValue;
        public MeasurementStatus MeasurementStatusValue { get; set; } = MeasurementStatus.NotMeasuring;
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // イベント
        public event Action<ManualMeasurementController> OnStarted;
        public event Action<ManualMeasurementController> OnStopped;
        public event Action<ManualMeasurementController> OnSaved;
        public event Action<ManualMeasurementController> OnConfigLoaded;
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // オーバーライドメソッド
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // パブリックメソッド
        public void Start()
        {
            if (this.MeasurementStatusValue == MeasurementStatus.Measuring) {
                return;
            }
            this.CurrentSessionGUID = Guid.NewGuid();
            this.MeasurementStatusValue = MeasurementStatus.Measuring;
            this.StartDateTime = DateTime.Now;
            PluginConfig.Instance.MeasurementStatusValue = this.MeasurementStatusValue;
            this.OnStarted?.Invoke(this);
        }

        public void Stop()
        {
            if (this.MeasurementStatusValue == MeasurementStatus.NotMeasuring) {
                return;
            }
            this.CurrentSessionGUID = Guid.Empty;
            this.MeasurementStatusValue = MeasurementStatus.NotMeasuring;
            this.StartDateTime = DateTime.MinValue;
            PluginConfig.Instance.MeasurementStatusValue = this.MeasurementStatusValue;
            this.OnStopped?.Invoke(this);
        }

        public void Reset()
        {
            this.Stop();
            this.Start();
        }

        public void LoadConfig()
        {
            switch (PluginConfig.Instance.MeasurementStatusValue) {
                case MeasurementStatus.Measuring:
                    if (this.MeasurementStatusValue == MeasurementStatus.Measuring) {
                        break;
                    }
                    var lastInfo = _database.RawDatabase.GetCollection<ManualMeasurement>().FindAll().OrderByDescending(x => x.StartDate).FirstOrDefault();
                    if (lastInfo == null) {
                        this.Stop();
                    }
                    else {
                        this.CurrentSessionGUID = lastInfo.SessionGUID;
                        this.MeasurementStatusValue = MeasurementStatus.Measuring;
                        this.StartDateTime = lastInfo.StartDate;
                    }
                    break;
                case MeasurementStatus.NotMeasuring:
                    this.Stop();
                    break;
                default:
                    break;
            }
            this.OnConfigLoaded?.Invoke(this);
        }

        public float GetTotalDistance(Guid sessionGuid)
        {
            if (sessionGuid == Guid.Empty) {
                return 0;
            }
            var MeasurementInfos = _database.Find<ManualMeasurement>(x => x.SessionGUID == sessionGuid).ToArray();
            var distanceInfos = new List<DistanceInformation>();
            foreach (var measurement in MeasurementInfos) {
                distanceInfos.AddRange(_database.Find<DistanceInformation>(x => x.ID == measurement.DistanceInfoID).ToArray());
            }
            return distanceInfos.Sum(x => x.Distance);
        }

        public void Save(DistanceInformation information)
        {
            if (this.MeasurementStatusValue == MeasurementStatus.NotMeasuring || this.CurrentSessionGUID == Guid.Empty) {
                return;
            }
            var info = new ManualMeasurement
            {
                DistanceInfoID = information.ID,
                SessionGUID = this.CurrentSessionGUID,
                StartDate = this.StartDateTime,
            };
            _database.Insert(info);
            try {
                this.OnSaved?.Invoke(this);
            }
            catch (Exception e) {
                Plugin.Log.Error(e);
            }
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プライベートメソッド

        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // メンバ変数
        private bool _disposedValue;
        private IHDTDatabase _database;
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // 構築・破棄
        [Inject]
        public ManualMeasurementController(IHDTDatabase database)
        {
            _database = database;
        }

        public void Initialize()
        {
            this.Stop();
            this.LoadConfig();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue) {
                if (disposing) {
                    // TODO: マネージド状態を破棄します (マネージド オブジェクト)
                    this.Stop();
                }

                // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
                // TODO: 大きなフィールドを null に設定します
                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
