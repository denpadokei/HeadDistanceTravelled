using HeadDistanceTravelled.Databases;
using HeadDistanceTravelled.Databases.Interfaces;
using HeadDistanceTravelled.Jsons;
using HeadDistanceTravelled.Models;
using IPA.Utilities;
using SiraUtil.Tools.FPFC;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;
using Zenject;

namespace HeadDistanceTravelled
{
    /// <summary>
    /// Monobehaviours (scripts) are added to GameObjects.
    /// For a full list of Messages a Monobehaviour can receive from the game, see https://docs.unity3d.com/ScriptReference/MonoBehaviour.html.
    /// </summary>
    internal class HeadDistanceTravelledController : MonoBehaviour, IHeadDistanceTravelledController
    {
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プロパティ
        public float HMDDistance => this._hmdDistance;
        public Vector3 HMDPositon => this._hmdPosition;
        public Quaternion HMDRotation => this._hmdRotation;
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プライベートメソッド
        private void RecordHMDPosition()
        {
            // OpenVR使っててID0で取れるかどうか知らん。Oculusしか持ってないからなガハハ。
            this._platformHelper.GetNodePose(XRNode.Head, 0, out this._hmdPosition, out this._hmdRotation);
            var distance = Vector3.Distance(this._hmdPosition, this._prevHMDPosition);
            if (this._movementSensitivityThreshold < distance) {
                this._hmdDistance += distance;
                this._prevHMDPosition = this._hmdPosition;
                this.OnDistanceChanged?.Invoke(this._hmdDistance, in this._hmdPosition, in this._hmdRotation);
            }
        }
        private void OnDidResumeEvent()
        {
            this._isPause = false;
        }

        private void OnDidPauseEvent()
        {
            this._isPause = true;
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // イベント
        public event HMDDistanceChangedEventHandler OnDistanceChanged;
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // メンバ変数
        private Vector3 _prevHMDPosition = Vector3.zero;
        private Vector3 _hmdPosition = Vector3.zero;
        private Quaternion _hmdRotation;
        private readonly float _movementSensitivityThreshold = 0.01f;
        private float _hmdDistance = 0;
        private IGamePause _pauseController;
        private float _startTime;
        private float _endTime;
        private IAudioTimeSource _audioTimeSource;
        private IVRPlatformHelper _platformHelper;
        private IDifficultyBeatmap _difficultyBeatmap;
        private IFPFCSettings _fpfc;
        private bool _isfpfc;
        private bool _isPause;
        private ManualMeasurementController _manualMeasurementController;
        private IHDTDatabase _hDTDatabase;
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // 構築・破棄
        [Inject]
        public void Constractor(IVRPlatformHelper helper, IGamePause pauseController, IAudioTimeSource timeSource, IReadonlyBeatmapData readonlyBeatmapData, IHDTDatabase hDTDatabase, GameplayCoreSceneSetupData gameplayCoreSceneSetupData, IFPFCSettings fpfc, ManualMeasurementController manualMeasurementController)        {
            this._platformHelper = helper;
            this._pauseController = pauseController;
            this._audioTimeSource = timeSource;
            this._difficultyBeatmap = gameplayCoreSceneSetupData.difficultyBeatmap;
            this._fpfc = fpfc;
            this._manualMeasurementController = manualMeasurementController;
            this._hDTDatabase = hDTDatabase;
            // ライトショーとかやられるとマジ死ぬ
#if VER_1_20_0
            var firstNote = readonlyBeatmapData.allBeatmapDataItems.OfType<NoteData>().FirstOrDefault();
            var lastNote = readonlyBeatmapData.allBeatmapDataItems.OfType<NoteData>().LastOrDefault();
#else
            var firstNote = gameplayCoreSceneSetupData.difficultyBeatmap.beatmapData.beatmapObjectsData.OrderBy(x => x.time).OfType<NoteData>().FirstOrDefault();
            var lastNote = gameplayCoreSceneSetupData.difficultyBeatmap.beatmapData.beatmapObjectsData.OrderBy(x => x.time).OfType<NoteData>().LastOrDefault();
#endif
            if (firstNote != null) {
                this._startTime = firstNote.time;
            }
            else {
                this._startTime = 0;
            }
            if (lastNote != null) {
                this._endTime = lastNote.time;
            }
            else {
#if VER_1_20_0
                this._endTime = this._audioTimeSource.songLength;
#else
                this._endTime = this._audioTimeSource.songEndTime;
#endif

            }
            this._pauseController.didPauseEvent += this.OnDidPauseEvent;
            this._pauseController.didResumeEvent += this.OnDidResumeEvent;
            if (this._fpfc != null) {
                this._isfpfc = this._fpfc.Enabled;
                this._fpfc.Changed += this.OnFPFCChanged;
            }
            else {
                this._isfpfc = false;
            }
        }

        /// <summary>
        /// SiraUtilが入ってるときは<see cref="FirstPersonFlyingController"/>のメソッドがそもそも実行されないらしい。
        /// </summary>
        /// <param name="obj"></param>
        private void OnFPFCChanged(IFPFCSettings obj)
        {
            this._isfpfc = obj.Enabled;
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        // These methods are automatically called by Unity, you should remove any you aren't using.
        #region Monobehaviour Messages
        public void Start()
        {
            this._platformHelper.GetNodePose(XRNode.Head, 0, out this._prevHMDPosition, out _);
        }
        public void Update()
        {
            if (this._isPause) {
                this._platformHelper.GetNodePose(XRNode.Head, 0, out this._prevHMDPosition, out _);
                return;
            }
            if (this._audioTimeSource.songTime < this._startTime || this._endTime < this._audioTimeSource.songTime) {
                this._platformHelper.GetNodePose(XRNode.Head, 0, out this._prevHMDPosition, out _);
                return;
            }
            if (this._isfpfc) {
                return;
            }
            this.RecordHMDPosition();
        }
        public void OnDestroy()
        {
            this._pauseController.didPauseEvent -= this.OnDidPauseEvent;
            this._pauseController.didResumeEvent -= this.OnDidResumeEvent;
            this._fpfc.Changed -= this.OnFPFCChanged;
            var info = new DistanceInformation
            {
                LevelID = this._difficultyBeatmap.level.levelID,
                SongName = this._difficultyBeatmap.level.songName,
                Difficurity = this._difficultyBeatmap.difficulty.ToString(),
                Distance = this._hmdDistance,
                CreatedAt = DateTime.Now,
            };
            var include = EnumUtl.TryGetEnumValue<BeatmapCharacteristic>(this._difficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic.characteristicNameLocalizationKey, out var chara);
            var bc = _hDTDatabase.Find<BeatmapCharacteristicText>(x => x.BeatmapCharacteristicEnumValue == chara).FirstOrDefault();
            var unknownBc = _hDTDatabase.Find<BeatmapCharacteristicText>(x => x.BeatmapCharacteristicEnumValue == BeatmapCharacteristic.UnknownValue).FirstOrDefault();
            info.BeatmapCharacteristicTextId = include ? bc.ID : unknownBc.ID;
            var inserted = _hDTDatabase.Insert(info);
            Plugin.Log.Info($"Id={info.ID}");
            this._manualMeasurementController.Save(info);
        }
        #endregion
    }
}
