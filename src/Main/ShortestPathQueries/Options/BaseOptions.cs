using System;
using System.Collections.Generic;
using System.Text;
using USC.GISResearchLab.Common.Core.Geocoders.FeatureMatching;
using USC.GISResearchLab.Common.Databases.Runners.Options;

namespace USC.GISResearchLab.Common.Core.ShortestPathFinders.ShortestPathQueries.Options
{
    public class BaseOptions : BatchDatabaseOptions
    {
        #region Properties

        private bool _ShouldDoShortestTime;
        public bool ShouldDoShortestTime
        {
            get { return _ShouldDoShortestTime; }
            set { _ShouldDoShortestTime = value; }
        }

        private bool _ShouldDoShortestDistance;
        public bool ShouldDoShortestDistance
        {
            get { return _ShouldDoShortestDistance; }
            set { _ShouldDoShortestDistance = value; }
        }

        #endregion

        public BaseOptions()
        {
            
        }
    }
}
