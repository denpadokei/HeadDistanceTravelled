using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using HeadDistanceTravelled.Configuration;
using HeadDistanceTravelled.Databases;
using HeadDistanceTravelled.Databases.Interfaces;
using HeadDistanceTravelled.Jsons;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;
using TMPro;
using UnityEngine;
using Zenject;
using static AlphabetScrollInfo;

namespace HeadDistanceTravelled.Views
{
    [HotReload]
    public class HeadDistanceTravelledMainViewController : ViewControllerBase
    {
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プロパティ
        /// <summary>説明 を取得、設定</summary>
        private string _hmdDistance = "";
        [UIValue("hmd-distance")]
        /// <summary>説明 を取得、設定</summary>
        public string HMDDistance
        {
            get => this._hmdDistance;

            set => this.SetProperty(ref this._hmdDistance, value);
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

        /// <summary>説明 を取得、設定</summary>
        private float _fontSize;
        /// <summary>説明 を取得、設定</summary>
        public float FontSize
        {
            get => this._fontSize;

            set => this.SetProperty(ref this._fontSize, value);
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
            this.Show = PluginConfig.Instance.DisplayViews.Any(x => x == PluginConfig.DisplayView.Main);
            var todayDt = _database.Find<DistanceInformation>(x => true).Sum(x => x.Distance);
            this.HMDDistance = $"<size=150%>{todayDt:#.000}</size> m";
            Plugin.Log.Info(this.HMDDistance);
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
                    && PluginConfig.Instance.DisplayViews.FirstOrDefault() == PluginConfig.DisplayView.Left;
                this.StartCoroutine(WaitAndSet(text, !b));
            }
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // パブリックメソッド
        

        public void OnEnable()
        {
            if (this._waitAndSetOnEnable && _textMeshProUGUI) {
                var b = PluginConfig.Instance.DisplayViews.Count == 1
                    && PluginConfig.Instance.DisplayViews.FirstOrDefault() == PluginConfig.DisplayView.Left;
                var text = _textMeshProUGUI.gameObject.GetComponentInChildren<FormattableText>();
                this.StartCoroutine(this.WaitAndSet(text, !b));
            }
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
                    && PluginConfig.Instance.DisplayViews.FirstOrDefault() == PluginConfig.DisplayView.Left;
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
        [UIObject("distance-text2")]
        private GameObject _textMeshProUGUI;
        private bool _waitAndSetOnEnable = false;
        private IHDTDatabase _database;
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // 構築・破棄
        [Inject]
        internal void Constractor(IHDTDatabase database)
        {
            _database = database;
        }
        #endregion
    }
}
