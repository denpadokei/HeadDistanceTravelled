using IPA.Config.Stores;
using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using static HeadDistanceTravelled.Models.ManualMeasurementController;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace HeadDistanceTravelled.Configuration
{
    internal class PluginConfig
    {
        public static PluginConfig Instance { get; set; }
        public virtual bool ShowDistanceOnHMD { get; set; } = true; // Must be 'virtual' if you want BSIPA to detect a value change and save the config automatically.
        [UseConverter(typeof(ListConverter<DisplayView>))]
        public virtual List<DisplayView> DisplayViews { get; set; } = new List<DisplayView> { DisplayView.Main, DisplayView.Left, DisplayView.Right };
        [UseConverter(typeof(EnumConverter<DistanceType>))]
        public virtual DistanceType DistanceTypeValue { get; set; } = DistanceType.Song;
        [UseConverter(typeof(EnumConverter<MeasurementStatus>))]
        public virtual MeasurementStatus MeasurementStatusValue { get; set; } = MeasurementStatus.NotMeasuring;

        /// <summary>
        /// This is called whenever BSIPA reads the config from disk (including when file changes are detected).
        /// </summary>
        public virtual void OnReload()
        {
            // Do stuff after config is read from disk.
        }

        /// <summary>
        /// Call this to force BSIPA to update the config file. This is also called by BSIPA if it detects the file was modified.
        /// </summary>
        public virtual void Changed()
        {
            // Do stuff when the config is changed.
        }

        /// <summary>
        /// Call this to have BSIPA copy the values from <paramref name="other"/> into this config.
        /// </summary>
        public virtual void CopyFrom(PluginConfig other)
        {
            // This instance's members populated from other
        }

        public enum DisplayView
        {
            Main,
            Left,
            Right,
        }

        public enum DistanceType
        {
            Song,
            Today,
            Total,
            Manual
        }
    }
}