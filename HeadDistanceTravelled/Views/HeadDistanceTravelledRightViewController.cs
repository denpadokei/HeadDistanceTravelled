using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using HeadDistanceTravelled.Configuration;
using HeadDistanceTravelled.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TMPro;
using UnityEngine;
using Zenject;


namespace HeadDistanceTravelled.Views
{
    [HotReload]
    public class HeadDistanceTravelledRightViewController : ViewControllerBase
    {
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プロパティ
        /// <summary>説明 を取得、設定</summary>
        private string _status = "";
        /// <summary>説明 を取得、設定</summary>
        [UIValue("status")]
        public string Status
        {
            get => this._status;

            set => this.SetProperty(ref this._status, value);
        }

        /// <summary>計測距離 を取得、設定</summary>
        private string _distance;
        /// <summary>計測距離 を取得、設定</summary>
        [UIValue("manual-distance")]
        public string Distance
        {
            get => this._distance;

            set => this.SetProperty(ref this._distance, value);
        }

        /// <summary>説明 を取得、設定</summary>
        private bool _show;
        /// <summary>説明 を取得、設定</summary>
        [UIValue("show-view")]
        public bool Show
        {
            get => this._show;

            set => this.SetProperty(ref this._show, value);
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
        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);
            this.Show = PluginConfig.Instance.DisplayViews.Any(x => x == PluginConfig.DisplayView.Right);
            this.UpdateDistanceText(_manualMeasurementController.GetTotalDistance(_manualMeasurementController.CurrentSessionGUID));
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var text = _textMeshProUGUI.gameObject.GetComponentInChildren<FormattableText>();
            if (text) {
                text.enableAutoSizing = false;
                text.richText = false;
            }
            base.OnPropertyChanged(e);
            if (!this.gameObject.activeInHierarchy) {
                _waitAndSetOnEnable = true;
            }
            else {
                var b = PluginConfig.Instance.DisplayViews.Count == 1
                    && PluginConfig.Instance.DisplayViews.FirstOrDefault() == PluginConfig.DisplayView.Right;
                this.StartCoroutine(WaitAndSet(text, !b));
            }
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // パブリックメソッド
        [UIAction("on-start")]
        public void OnStart()
        {
            this._manualMeasurementController?.Start();
            this.Status = "Recording";
            this.UpdateDistanceText(_manualMeasurementController.GetTotalDistance(_manualMeasurementController.CurrentSessionGUID));
        }
        [UIAction("on-stop")]
        public void OnStop()
        {
            this._manualMeasurementController?.Stop();
            this.Status = "";
        }
        [UIAction("on-reset")]
        public void OnReset()
        {
            this._manualMeasurementController?.Reset();
            this.Status = "Recording";
            this.UpdateDistanceText(_manualMeasurementController.GetTotalDistance(_manualMeasurementController.CurrentSessionGUID));
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プライベートメソッド
        [UIAction("#post-parse")]
        internal void PostParse()
        {
            try {
                // Code to run after BSML finishes
                var text = _textMeshProUGUI.gameObject.GetComponentInChildren<FormattableText>();
                if (text) {
                    text.enableWordWrapping = false;
                    var b = PluginConfig.Instance.DisplayViews.Count == 1
                    && PluginConfig.Instance.DisplayViews.FirstOrDefault() == PluginConfig.DisplayView.Right;
                    if (!b && text) {
                        text.fontSizeMax = 40;
                        text.fontSizeMin = 1;
                    }
                }
            }
            catch (System.Exception e) {
                Plugin.Log.Error(e);
            }
        }

        private void UpdateDistanceText(float distance)
        {
            this.Distance = $"<size=150%>{distance:#.000}</size> m";
        }

        private IEnumerator WaitAndSet(TextMeshProUGUI text, bool enableAutoSizing)
        {
            if (!text) {
                yield break;
            }
            yield return new WaitForEndOfFrame();
            text.enableAutoSizing = enableAutoSizing;
            text.richText = true;
            _waitAndSetOnEnable = false;
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // メンバ変数
        [UIObject("distance-text3")]
        private GameObject _textMeshProUGUI;
        private bool _waitAndSetOnEnable = false;
        private ManualMeasurementController _manualMeasurementController;
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // 構築・破棄
        [Inject]
        internal void Constractor(ManualMeasurementController manualMeasurementController)
        {
            this._manualMeasurementController = manualMeasurementController;
        }
        #endregion
    }
}
