using USC.GISResearchLab.Common.Databases.Runners.Options;

namespace USC.GISResearchLab.Common.Core.ShortestPathFinders.ShortestPathQueries.Options
{
  public class BaseOptions : BatchDatabaseOptions
  {
    #region Properties

    public bool ShouldDoShortestTime { get; set; }

    public bool ShouldDoShortestDistance { get; set; }

    #endregion

    public BaseOptions()
    {

    }
  }
}