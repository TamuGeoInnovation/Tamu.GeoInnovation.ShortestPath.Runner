namespace USC.GISResearchLab.Common.Core.ShortestPathFinders.ShortestPathQueries.Options
{
  public class BatchOptions : BaseOptions
  {
    #region Properties
    public bool NonProcessedOnly { get; set; }

    public string Table { get; set; }

    public string FieldId { get; set; }

    public string FieldFromLat { get; set; }

    public string FieldFromLon { get; set; }

    public string FieldToLat { get; set; }

    public string FieldToLon { get; set; }

    public string FieldShortestTime { get; set; }

    public string FieldShortestDistance { get; set; }

    public string FieldProcessed { get; set; }

    public string FieldKML { get; set; }

    public string FieldTravelTime { get; set; }

    public string FieldTravelDistance { get; set; }

    #endregion
  }
}