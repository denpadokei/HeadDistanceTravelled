﻿using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using HeadDistanceTravelled.Configuration;
using HeadDistanceTravelled.Databases;
using HeadDistanceTravelled.Databases.Interfaces;
using HeadDistanceTravelled.Jsons;
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
    public class HeadDistanceTravelledLeftViewController : ViewControllerBase
    {
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プロパティ
        /// <summary>説明 を取得、設定</summary>
        private string _todaysDistance;
        /// <summary>説明 を取得、設定</summary>
        [UIValue("today-distance")]
        public string TodaysDistance
        {
            get => this._todaysDistance;

            set => this.SetProperty(ref this._todaysDistance, value);
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
            this.Show = PluginConfig.Instance.DisplayViews.Any(x => x == PluginConfig.DisplayView.Left);
            var todayDt = _database.Find<DistanceInformation>(x => Plugin.LastLaunchDate <= x.CreatedAt).Sum(x => x.Distance);
            this.TodaysDistance = $"<size=150%>{todayDt:#.000}</size> m";
            Plugin.Log.Info(this.TodaysDistance);
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
        [UIObject("distance-text1")]
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
