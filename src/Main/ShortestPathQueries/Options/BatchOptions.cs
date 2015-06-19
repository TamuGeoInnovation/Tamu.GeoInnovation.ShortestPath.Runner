using System;
using System.Collections.Generic;
using System.Text;
using USC.GISResearchLab.Common.Core.Databases;

namespace USC.GISResearchLab.Common.Core.ShortestPathFinders.ShortestPathQueries.Options
{
    public class BatchOptions : BaseOptions
    {
        #region Properties
        private bool _NonProcessedOnly;
        public bool NonProcessedOnly
        {
            get { return _NonProcessedOnly; }
            set { _NonProcessedOnly = value; }
        }

        private string _Table;
        public string Table
        {
            get { return _Table; }
            set { _Table = value; }
        }

        private string _FieldId;
        public string FieldId
        {
            get { return _FieldId; }
            set { _FieldId = value; }
        }

        private string _FieldFromLat;
        public string FieldFromLat
        {
            get { return _FieldFromLat; }
            set { _FieldFromLat = value; }
        }

        private string _FieldFromLon;
        public string FieldFromLon
        {
            get { return _FieldFromLon; }
            set { _FieldFromLon = value; }
        }

        private string _FieldToLat;
        public string FieldToLat
        {
            get { return _FieldToLat; }
            set { _FieldToLat = value; }
        }

        private string _FieldToLon;
        public string FieldToLon
        {
            get { return _FieldToLon; }
            set { _FieldToLon = value; }
        }

        private string _FieldShortestTime;
        public string FieldShortestTime
        {
            get { return _FieldShortestTime; }
            set { _FieldShortestTime = value; }
        }

        private string _FieldShortestDistance;
        public string FieldShortestDistance
        {
            get { return _FieldShortestDistance; }
            set { _FieldShortestDistance = value; }
        }

        private string _FieldProcessed;
        public string FieldProcessed
        {
            get { return _FieldProcessed; }
            set { _FieldProcessed = value; }
        }

        private string _FieldKML;
        public string FieldKML
        {
            get { return _FieldKML; }
            set { _FieldKML = value; }
        }

        private string _FieldTravelTime;
        public string FieldTravelTime
        {
            get { return _FieldTravelTime; }
            set { _FieldTravelTime = value; }
        }

        private string _FieldTravelDistance;
        public string FieldTravelDistance
        {
            get { return _FieldTravelDistance; }
            set { _FieldTravelDistance = value; }
        }


        #endregion
    }
}
