using System;

namespace HeadDistanceTravelled.Databases
{
    internal class ManualMeasurement
    {
        /// <summary>
        /// 自動採番
        /// </summary>
        public int ID { get; set; }
        public Guid SessionGUID { get; set; }
        public DateTime StartDate { get; set; }
        public int DistanceInfoID { get; set; }
    }
}
