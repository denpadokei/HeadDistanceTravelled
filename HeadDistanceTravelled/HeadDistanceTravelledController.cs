using HeadDistanceTravelled.Databases;
using HeadDistanceTravelled.Databases.Interfaces;
using HeadDistanceTravelled.Jsons;
using HeadDistanceTravelled.Models;
using SiraUtil.Tools.FPFC;
using System;
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
        public float LeftHandDistance => this._leftHandDistance;
        public float RightHandDistance => this._rightHandDistance;
        public float TotalHandDistance => this._leftHandDistance + this._rightHandDistance;
        public Vector3 LeftHandPosition => this._leftHandPosition;
        public Vector3 RightHandPosition => this._rightHandPosition;
        public bool LeftHandTracked => this._leftHandTracked;
        public bool RightHandTracked => this._rightHandTracked;
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
        private void FetchHandPositions()
        {
            // コネクト解除で失敗する可能性があるため戻り値を見てTrackedフラグを更新する
            this._leftHandTracked = this._platformHelper.GetNodePose(XRNode.LeftHand, 0, out this._leftHandPosition, out _);
            this._rightHandTracked = this._platformHelper.GetNodePose(XRNode.RightHand, 0, out this._rightHandPosition, out _);
        }
        private void RecordHandPositions()
        {
            if (this._leftHandTracked) {
                var distance = Vector3.Distance(this._leftHandPosition, this._prevLeftHandPosition);
                if (this._movementSensitivityThreshold < distance) {
                    this._leftHandDistance += distance;
                    this._prevLeftHandPosition = this._leftHandPosition;
                }
            }
            if (this._rightHandTracked) {
                var distance = Vector3.Distance(this._rightHandPosition, this._prevRightHandPosition);
                if (this._movementSensitivityThreshold < distance) {
                    this._rightHandDistance += distance;
                    this._prevRightHandPosition = this._rightHandPosition;
                }
            }
        }
        private void ResetPreviousPositions()
        {
            this._platformHelper.GetNodePose(XRNode.Head, 0, out this._prevHMDPosition, out _);
            if (this._leftHandTracked) {
                this._prevLeftHandPosition = this._leftHandPosition;
            }
            if (this._rightHandTracked) {
                this._prevRightHandPosition = this._rightHandPosition;
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
        public event Action<IHeadDistanceTravelledController> OnDestroied;
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // メンバ変数
        private Vector3 _prevHMDPosition = Vector3.zero;
        private Vector3 _hmdPosition = Vector3.zero;
        private Quaternion _hmdRotation;
        private Vector3 _prevLeftHandPosition = Vector3.zero;
        private Vector3 _prevRightHandPosition = Vector3.zero;
        private Vector3 _leftHandPosition = Vector3.zero;
        private Vector3 _rightHandPosition = Vector3.zero;
        private bool _leftHandTracked;
        private bool _rightHandTracked;
        private float _leftHandDistance = 0;
        private float _rightHandDistance = 0;
        private readonly float _movementSensitivityThreshold = 0.01f;
        private float _hmdDistance = 0;
        private IGamePause _pauseController;
        private float _startTime;
        private float _endTime;
        private IAudioTimeSource _audioTimeSource;
        private IVRPlatformHelper _platformHelper;
        private BeatmapLevel _beatmapLevel;
        private BeatmapKey _key;
        private IReadonlyBeatmapData _readonlyBeatmapData;
        private IFPFCSettings _fpfc;
        private bool _isfpfc;
        private bool _isPause;
        private ManualMeasurementController _manualMeasurementController;
        private IHDTDatabase _hDTDatabase;
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // 構築・破棄
        [Inject]
        public void Constractor(IVRPlatformHelper helper, IGamePause pauseController, IAudioTimeSource timeSource, IHDTDatabase hDTDatabase, IReadonlyBeatmapData readonlyBeatmapData, GameplayCoreSceneSetupData gameplayCoreSceneSetupData, IFPFCSettings fpfc, ManualMeasurementController manualMeasurementController)
        {
            this._platformHelper = helper;
            this._pauseController = pauseController;
            this._audioTimeSource = timeSource;
            this._key = gameplayCoreSceneSetupData.beatmapKey;
            this._beatmapLevel = gameplayCoreSceneSetupData.beatmapLevel;
            this._readonlyBeatmapData = readonlyBeatmapData;
            this._fpfc = fpfc;
            this._manualMeasurementController = manualMeasurementController;
            this._hDTDatabase = hDTDatabase;
            // ライトショーとかやられるとマジ死ぬ
            var firstNote = readonlyBeatmapData.allBeatmapDataItems.OfType<NoteData>().FirstOrDefault();
            var lastNote = readonlyBeatmapData.allBeatmapDataItems.OfType<NoteData>().LastOrDefault();
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
                this._endTime = this._audioTimeSource.songLength;
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
            this.FetchHandPositions();
            this.ResetPreviousPositions();
        }
        public void Update()
        {
            this.FetchHandPositions();
            if (this._isPause) {
                this.ResetPreviousPositions();
                return;
            }
            if (this._audioTimeSource.songTime < this._startTime || this._endTime < this._audioTimeSource.songTime) {
                this.ResetPreviousPositions();
                return;
            }
            if (this._isfpfc) {
                return;
            }
            this.RecordHMDPosition();
            this.RecordHandPositions();
        }
        public void OnDestroy()
        {
            this._pauseController.didPauseEvent -= this.OnDidPauseEvent;
            this._pauseController.didResumeEvent -= this.OnDidResumeEvent;
            this._fpfc.Changed -= this.OnFPFCChanged;
            var info = new DistanceInformation
            {
                LevelID = this._beatmapLevel.levelID,
                SongName = this._beatmapLevel.songName,
                Difficurity = this._key.difficulty.ToString(),
                Distance = this._hmdDistance,
                CreatedAt = DateTime.Now,
            };
            var include = EnumUtl.TryGetEnumValue<BeatmapCharacteristic>(this._key.beatmapCharacteristic.characteristicNameLocalizationKey, out var chara);
            var bc = _hDTDatabase.Find<BeatmapCharacteristicText>(x => x.BeatmapCharacteristicEnumValue == chara).FirstOrDefault();
            var unknownBc = _hDTDatabase.Find<BeatmapCharacteristicText>(x => x.BeatmapCharacteristicEnumValue == BeatmapCharacteristic.UnknownValue).FirstOrDefault();
            info.BeatmapCharacteristicTextId = include ? bc.ID : unknownBc.ID;
            var inserted = _hDTDatabase.Insert(info);
            Plugin.Log.Info($"Id={info.ID}");
            this._manualMeasurementController.Save(info);
            var handInfo = new HandDistanceInformation
            {
                LevelID = this._beatmapLevel.levelID,
                SongName = this._beatmapLevel.songName,
                Difficurity = this._key.difficulty.ToString(),
                BeatmapCharacteristicTextId = info.BeatmapCharacteristicTextId,
                LeftDistance = this._leftHandDistance,
                RightDistance = this._rightHandDistance,
                CreatedAt = DateTime.Now,
                SessionGUID = this._manualMeasurementController.MeasurementStatusValue == ManualMeasurementController.MeasurementStatus.Measuring
                    ? this._manualMeasurementController.CurrentSessionGUID
                    : Guid.Empty,
            };
            _hDTDatabase.Insert(handInfo);
            Plugin.Log.Info($"HandId={handInfo.ID}");
            try {
                this.OnDestroied?.Invoke(this);
            }
            catch (Exception e) {
                Plugin.Log.Error(e);
            }
        }
        #endregion
    }
}
