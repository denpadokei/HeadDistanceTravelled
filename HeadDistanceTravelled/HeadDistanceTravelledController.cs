using HeadDistanceTravelled.Jsons;
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
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プライベートメソッド
        private void RecordHMDPosition()
        {
            // OpenVR使っててID0で取れるかどうか知らん。Oculusしか持ってないからなガハハ。
            this._platformHelper.GetNodePose(XRNode.Head, 0, out var hmdpos, out _);
            var distance = Vector3.Distance(hmdpos, this._prevHMDPosition);
            if (this._movementSensitivityThreshold < distance) {
                this._hmdDistance += distance;
                this._prevHMDPosition = hmdpos;
            }
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // メンバ変数
        private Vector3 _prevHMDPosition = Vector3.zero;
        private readonly float _movementSensitivityThreshold = 0.01f;
        private float _hmdDistance = 0;
        private IGamePause _pauseController;
        private float _startTime;
        private float _endTime;
        private IAudioTimeSource _audioTimeSource;
        private IVRPlatformHelper _platformHelper;
        private IDifficultyBeatmap _difficultyBeatmap;
        private bool _fpfc;
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // 構築・破棄
        [Inject]
        public void Constractor(IVRPlatformHelper helper, IGamePause pauseController, IAudioTimeSource timeSource, IReadonlyBeatmapData readonlyBeatmapData, GameplayCoreSceneSetupData gameplayCoreSceneSetupData)
        {
            this._platformHelper = helper;
            this._pauseController = pauseController;
            this._audioTimeSource = timeSource;
            this._difficultyBeatmap = gameplayCoreSceneSetupData.difficultyBeatmap;
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
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        // These methods are automatically called by Unity, you should remove any you aren't using.
        #region Monobehaviour Messages
        public void Start()
        {
            var args = Environment.GetCommandLineArgs();
            this._fpfc = args.Any(x => x.Contains("fpfc"));

            this._platformHelper.GetNodePose(XRNode.Head, 0, out var hmdpos, out _);
            this._prevHMDPosition = hmdpos;
        }
        public void Update()
        {
            if (this._pauseController.isPaused) {
                return;
            }
            if (this._audioTimeSource.songTime < this._startTime || this._endTime < this._audioTimeSource.songTime) {
                return;
            }
            if (this._fpfc) {
                return;
            }
            this.RecordHMDPosition();
        }
        public void OnDestroy()
        {
            var data = new HDTData();
            data.Load();
            data.HeadDistanceTravelled += this._hmdDistance;
            var oldResults = data.BeatmapResults.ToList();
            oldResults.Add(new HDTData.BeatmapResult(this._difficultyBeatmap.level.levelID, this._difficultyBeatmap.level.songName, this._hmdDistance, DateTime.Now));
            data.BeatmapResults = new ReadOnlyCollection<HDTData.BeatmapResult>(oldResults);
            data.Save();
        }
        #endregion
    }
}
