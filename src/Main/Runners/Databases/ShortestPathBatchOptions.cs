using System;

namespace USC.GISResearchLab.Common.Core.ShortestPathFinders.ShortestPathQueries.Options
{


    [Serializable]
    public class ShortestPathBatchOptions : ShortestPathBaseOptions
    {
        #region Properties
        
        public string FieldFromLat { get; set; }

        public string FieldFromLon { get; set; }

        public string FieldToLat { get; set; }

        public string FieldToLon { get; set; }

        public string FieldShortestTime { get; set; }

        public string FieldShortestDistance { get; set; }

		public string FieldNearestNodeDistance { get; set; }

        public string FieldKMLDistance { get; set; }

        public string FieldKMLTime { get; set; }

        public string FieldTravelTime { get; set; }

        public string FieldTravelDistance { get; set; }

        public bool RecordKML { get; set; }
        public bool UpdateOnly { get; set; }
        public uint MaxMiles2Go { get; set; }
        public uint MaxHours2Go { get; set; }
        public double MaxNearestNodeDistance { get; set; }

        #endregion
    }
}