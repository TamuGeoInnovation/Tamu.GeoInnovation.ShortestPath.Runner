using USC.GISResearchLab.Common.Threading.ProgressStates;

namespace USC.GISResearchLab.ShortestPath.Core.Runners.Databases
{
    public class RecordCompletedProgressState : PercentCompletableProgressState
    {
        #region Properties
        private string _FromLat;
        private string _FromLon;
        private string _ToLat;
        private string _ToLon;
        private string _Id;

        public string Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
	

        public string ToLon
        {
            get { return _ToLon; }
            set { _ToLon = value; }
        }

        public string ToLat
        {
            get { return _ToLat; }
            set { _ToLat = value; }
        }

        public string FromLon
        {
            get { return _FromLon; }
            set { _FromLon = value; }
        }
	

        public string FromLat
        {
            get { return _FromLat; }
            set { _FromLat = value; }
        }
	

        
        #endregion

        public RecordCompletedProgressState()
            :base(){}
    }
}
