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
    /// <summary>
    /// 手の位置を起点に表示されるフローティングスクリーンのベースクラス。
    /// </summary>
    internal abstract class HandDistanceFloatingScreen : BSMLAutomaticViewController, IInitializable
    {
        public enum HandSide
        {
            Left,
            Right
        }

        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プロパティ
        protected abstract HandSide Side { get; }

        /// <summary>手の距離のテキスト を取得、設定</summary>
        private string _handDistanceText;
        [UIValue("hand-distance-text")]
        /// <summary>手の距離のテキスト を取得、設定</summary>
        public string HandDistanceText
        {
            get => this._handDistanceText;

            set => this.SetProperty(ref this._handDistanceText, value);
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // パブリックメソッド
        public void Initialize()
        {
            this._floatingScreen = FloatingScreen.CreateFloatingScreen(new Vector2(120, 30), false, Vector3.zero, Quaternion.identity);
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
            if (this._floatingScreen == null) {
                return;
            }
            if (!this.IsTracked) {
                this.HandDistanceText = string.Empty;
                return;
            }
            this._floatingScreen.transform.position = this.HandPosition + s_localOffset;
            this.CreateDistanceText();
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プライベートメソッド
        private bool IsTracked => this.Side == HandSide.Left ? this._controller.LeftHandTracked : this._controller.RightHandTracked;
        private Vector3 HandPosition => this.Side == HandSide.Left ? this._controller.LeftHandPosition : this._controller.RightHandPosition;

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
            var prefix = this.Side == HandSide.Left ? "L" : "R";
            var distance = this.Side == HandSide.Left ? this._controller.LeftHandDistance : this._controller.RightHandDistance;
            this.HandDistanceText = $"{prefix} {distance + this._startDistance:#0.00} <size=50%>m</size>";
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
        private static readonly Vector3 s_localOffset = new Vector3(0, 0.12f, 0);
        private float _startDistance = 0;
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // 構築・破棄
        [Inject]
        public void Constractor(IHeadDistanceTravelledController controller, IHDTDatabase database, ManualMeasurementController manualMeasurementController)
        {
            this._controller = controller;
            Func<HandDistanceInformation, float> sideDistance = x => this.Side == HandSide.Left ? x.LeftDistance : x.RightDistance;
            switch (PluginConfig.Instance.DistanceTypeValue) {
                case PluginConfig.DistanceType.Song:
                    _startDistance = 0;
                    break;
                case PluginConfig.DistanceType.Today:
                    _startDistance = database.Find<HandDistanceInformation>(x => Plugin.LastLaunchDate <= x.CreatedAt).Sum(sideDistance);
                    break;
                case PluginConfig.DistanceType.Total:
                    _startDistance = database.Find<HandDistanceInformation>(x => true).Sum(sideDistance);
                    break;
                case PluginConfig.DistanceType.Manual:
                    var sessionGuid = manualMeasurementController.CurrentSessionGUID;
                    _startDistance = database.Find<HandDistanceInformation>(x => x.SessionGUID == sessionGuid).Sum(sideDistance);
                    break;
                default:
                    break;
            }
        }
        #endregion
    }

    [HotReload]
    [ViewDefinition("HeadDistanceTravelled.Views.HandDistanceFloatingScreen.bsml")]
    internal class LeftHandDistanceFloatingScreen : HandDistanceFloatingScreen
    {
        protected override HandSide Side => HandSide.Left;
    }

    [HotReload]
    [ViewDefinition("HeadDistanceTravelled.Views.HandDistanceFloatingScreen.bsml")]
    internal class RightHandDistanceFloatingScreen : HandDistanceFloatingScreen
    {
        protected override HandSide Side => HandSide.Right;
    }
}
