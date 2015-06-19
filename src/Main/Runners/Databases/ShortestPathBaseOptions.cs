using System;
using USC.GISResearchLab.Common.Databases.Runners.Options;

namespace USC.GISResearchLab.Common.Core.ShortestPathFinders.ShortestPathQueries.Options
{
    [Serializable]
    public class ShortestPathBaseOptions : BatchDatabaseWebOptions
    {
        #region Properties

        public bool ShouldDoShortestTime { get; set; }

        public bool ShouldDoShortestDistance { get; set; }

        #endregion

        public ShortestPathBaseOptions()
        {

        }
    }
}