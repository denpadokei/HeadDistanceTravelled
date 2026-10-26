using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.FloatingScreen;
using BeatSaberMarkupLanguage.ViewControllers;
using HeadDistanceTravelled.Configuration;
using HeadDistanceTravelled.Databases;
using HeadDistanceTravelled.Databases.Interfaces;
using HeadDistanceTravelled.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

namespace HeadDistanceTravelled.Views
{
    [HotReload]
    internal class HMDDistanceFloatingScreen : BSMLAutomaticViewController, IInitializable
    {
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プロパティ
        /// <summary>HMDの距離のテキスト を取得、設定</summary>
        private string _hmdDistanceText;
        [UIValue("hmd-distance-text")]
        /// <summary>HMDの距離のテキスト を取得、設定</summary>
        public string HMDDistanceText
        {
            get => this._hmdDistanceText;

            set => this.SetProperty(ref this._hmdDistanceText, value);
        }

        [UIValue("show-hand-distance")]
        public bool ShowHandDistance => PluginConfig.Instance.ShowHandDistanceOnHMD;

        /// <summary>左手の距離のテキスト を取得、設定</summary>
        private string _leftHandDistanceText;
        [UIValue("left-hand-distance-text")]
        /// <summary>左手の距離のテキスト を取得、設定</summary>
        public string LeftHandDistanceText
        {
            get => this._leftHandDistanceText;

            set => this.SetProperty(ref this._leftHandDistanceText, value);
        }

        /// <summary>右手の距離のテキスト を取得、設定</summary>
        private string _rightHandDistanceText;
        [UIValue("right-hand-distance-text")]
        /// <summary>右手の距離のテキスト を取得、設定</summary>
        public string RightHandDistanceText
        {
            get => this._rightHandDistanceText;

            set => this.SetProperty(ref this._rightHandDistanceText, value);
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // コマンド
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // コマンド用メソッド
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // オーバーライドメソッド
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // パブリックメソッド
        public void Initialize()
        {
            var height = PluginConfig.Instance.ShowHandDistanceOnHMD ? 60 : 40;
            this._floatingScreen = FloatingScreen.CreateFloatingScreen(new Vector2(120, height), false, Vector3.zero, Quaternion.identity);
            // UI Layer
            this._floatingScreen.gameObject.layer = 5;
            this._floatingScreen.SetRootViewController(this, AnimationType.None);
            var oldScale = this._floatingScreen.transform.localScale;
            this._floatingScreen.transform.localScale = new Vector3(oldScale.x, oldScale.y, -oldScale.z);
        }

        public void Update()
        {
            this.ManualUpdate();
        }

        public void ManualUpdate()
        {
            this._floatingScreen.transform.position = this._controller.HMDPositon + s_localOffset;
            this.CreateDistanceText();
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プライベートメソッド
        private bool SetProperty<T>(ref T field, T value, [CallerMemberName] string memberName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) {
                return false;
            }
            field = value;
            this.OnPropertyChanged(new PropertyChangedEventArgs(memberName));
            return true;
        }

        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            this.NotifyPropertyChanged(e.PropertyName);
        }
        private void CreateDistanceText()
        {
            this.HMDDistanceText = $"{this._controller.HMDDistance + this._startDistance:#0.00} <size=50%>m</size>";
            if (PluginConfig.Instance.ShowHandDistanceOnHMD) {
                this.LeftHandDistanceText = $"L {this._controller.LeftHandDistance + this._startLeftHandDistance:#0.00} <size=50%>m</size>";
                this.RightHandDistanceText = $"R {this._controller.RightHandDistance + this._startRightHandDistance:#0.00} <size=50%>m</size>";
            }
        }

        protected override void OnDestroy()
        {
            if (this._floatingScreen != null) {
                Destroy(this._floatingScreen.gameObject);
            }
            base.OnDestroy();
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // メンバ変数
        private IHeadDistanceTravelledController _controller;
        private FloatingScreen _floatingScreen;
        private static readonly Vector3 s_localOffset = new Vector3(0, 0.4f, 0);
        private float _startDistance = 0;
        private float _startLeftHandDistance = 0;
        private float _startRightHandDistance = 0;
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // 構築・破棄
        [Inject]
        public void Constarctor(IHeadDistanceTravelledController controller, IHDTDatabase database, ManualMeasurementController manualMeasurementController)
        {
            this._controller = controller;
            switch (PluginConfig.Instance.DistanceTypeValue) {
                case PluginConfig.DistanceType.Song:
                    _startDistance = 0;
                    _startLeftHandDistance = 0;
                    _startRightHandDistance = 0;
                    break;
                case PluginConfig.DistanceType.Today:
                    _startDistance = database.Find<DistanceInformation>(x => Plugin.LastLaunchDate <= x.CreatedAt).Sum(x => x.Distance);
                    _startLeftHandDistance = database.Find<HandDistanceInformation>(x => Plugin.LastLaunchDate <= x.CreatedAt).Sum(x => x.LeftDistance);
                    _startRightHandDistance = database.Find<HandDistanceInformation>(x => Plugin.LastLaunchDate <= x.CreatedAt).Sum(x => x.RightDistance);
                    break;
                case PluginConfig.DistanceType.Total:
                    _startDistance = database.Find<DistanceInformation>(x => true).Sum(x => x.Distance);
                    _startLeftHandDistance = database.Find<HandDistanceInformation>(x => true).Sum(x => x.LeftDistance);
                    _startRightHandDistance = database.Find<HandDistanceInformation>(x => true).Sum(x => x.RightDistance);
                    break;
                case PluginConfig.DistanceType.Manual:
                    _startDistance = manualMeasurementController.GetTotalDistance(manualMeasurementController.CurrentSessionGUID);
                    _startLeftHandDistance = database.Find<HandDistanceInformation>(x => x.SessionGUID == manualMeasurementController.CurrentSessionGUID).Sum(x => x.LeftDistance);
                    _startRightHandDistance = database.Find<HandDistanceInformation>(x => x.SessionGUID == manualMeasurementController.CurrentSessionGUID).Sum(x => x.RightDistance);
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}
