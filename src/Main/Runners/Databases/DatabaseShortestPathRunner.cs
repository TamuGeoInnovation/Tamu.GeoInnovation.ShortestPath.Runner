using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Threading;
using System.Xml.Serialization;
using USC.GISResearchLab.Common.Core.Databases;
using USC.GISResearchLab.Common.Core.ShortestPathFinders.ShortestPathQueries.Options;
using USC.GISResearchLab.Common.Databases.QueryManagers;
using USC.GISResearchLab.Common.Diagnostics.TraceEvents;
using USC.GISResearchLab.Common.Threading;
using USC.GISResearchLab.Common.Utils.Databases;

namespace USC.GISResearchLab.ShortestPath.Core.Runners.Databases
{

    public class DatabaseShortestPathRunner : SuspendableThread
    {
        public static string SHORTESTPATH_NAME = "USCShortestPath";

        #region Properties
        public int MaxMiles2Go;
        public int MaxHours2Go;
        private string _ConnectionStringInputData;
        private DataProviderType _DataProviderInputData;
        private DatabaseType _DatabaseTypeInputData;
        private string _ConnectionStringStatus;
        private OleDbConnection _OleDbConnectionProcessStatusDatabase;
        private string _UpdateProcessStatusSQLNumberCompleted;
        private string _UpdateProcessStatusSQLFinished;
        private string _UpdateProcessStatusSQLAborted;
        private string _UpdateProcessStatusSQLParameterTotal;
        private string _UpdateProcessStatusSQLParameterCompleted;
        private string _UpdateProcessStatusSQLParameterStatus;
        private string _UpdateProcessStatusSQLParameterResultStatus;
        private string _UpdateProcessStatusSQLParameterTimeUpdated;
        private string _UpdateProcessStatusSQLParameterGUID;
        private ShortestPathService myShortestPathService;
        private ShortestPathQuery spQuery;

        private Guid _Guid;
        private BackgroundWorker _BackgroundWorker;
        private DoWorkEventArgs _DoWorkEventArgs;
        private RecordCompletedProgressState _ProgressState;
        private TraceSource _TraceSource;
        private OleDbConnection _OleDbConnection;
        private ThreadStart _ThreadStart;
        private Thread _Thread;
        private bool _IsRunning;
        private DateTime _Created;

        private string _NavteqDataSourceConnectionString;

        private DataProviderType _DataProviderStatus;
        private QueryManager _DBManagerStatus;
        private QueryManager _DBManagerInputData;
        private DatabaseType _DatabaseTypeStatus;

        public DatabaseType DatabaseTypeStatus
        {
            get { return _DatabaseTypeStatus; }
            set { _DatabaseTypeStatus = value; }
        }


        public QueryManager DBManagerInputData
        {
            get { return _DBManagerInputData; }
            set { _DBManagerInputData = value; }
        }

        private BatchOptions _BatchOptions;
        public BatchOptions BatchOptions
        {
            get { return _BatchOptions; }
            set { _BatchOptions = value; }
        }

        public QueryManager DBManagerStatus
        {
            get { return _DBManagerStatus; }
            set { _DBManagerStatus = value; }
        }

        public DataProviderType DataProviderStatus
        {
            get { return _DataProviderStatus; }
            set { _DataProviderStatus = value; }
        }

        public DataProviderType DataProviderInputData
        {
            get { return _DataProviderInputData; }
            set { _DataProviderInputData = value; }
        }

        public DatabaseType DatabaseTypeInputData
        {
            get { return _DatabaseTypeInputData; }
            set { _DatabaseTypeInputData = value; }
        }

        public string NavteqDataSourceConnectionString
        {
            get { return _NavteqDataSourceConnectionString; }
            set { _NavteqDataSourceConnectionString = value; }
        }

        public string UpdateProcessStatusSQLParameterStatus
        {
            get { return _UpdateProcessStatusSQLParameterStatus; }
            set { _UpdateProcessStatusSQLParameterStatus = value; }
        }

        public string UpdateProcessStatusSQLParameterResultStatus
        {
            get { return _UpdateProcessStatusSQLParameterResultStatus; }
            set { _UpdateProcessStatusSQLParameterResultStatus = value; }
        }

        public string UpdateProcessStatusSQLParameterCompleted
        {
            get { return _UpdateProcessStatusSQLParameterCompleted; }
            set { _UpdateProcessStatusSQLParameterCompleted = value; }
        }

        public string UpdateProcessStatusSQLParameterTimeUpdated
        {
            get { return _UpdateProcessStatusSQLParameterTimeUpdated; }
            set { _UpdateProcessStatusSQLParameterTimeUpdated = value; }
        }

        public string UpdateProcessStatusSQLParameterGUID
        {
            get { return _UpdateProcessStatusSQLParameterGUID; }
            set { _UpdateProcessStatusSQLParameterGUID = value; }
        }

        public string UpdateProcessStatusSQLParameterTotal
        {
            get { return _UpdateProcessStatusSQLParameterTotal; }
            set { _UpdateProcessStatusSQLParameterTotal = value; }
        }

        public OleDbConnection OleDbConnectionProcessStatusDatabase
        {
            get { return _OleDbConnectionProcessStatusDatabase; }
            set { _OleDbConnectionProcessStatusDatabase = value; }
        }

        public string UpdateProcessStatusSQLNumberCompleted
        {
            get { return _UpdateProcessStatusSQLNumberCompleted; }
            set { _UpdateProcessStatusSQLNumberCompleted = value; }
        }

        public string UpdateProcessStatusSQLFinished
        {
            get { return _UpdateProcessStatusSQLFinished; }
            set { _UpdateProcessStatusSQLFinished = value; }
        }

        public string UpdateProcessStatusSQLAborted
        {
            get { return _UpdateProcessStatusSQLAborted; }
            set { _UpdateProcessStatusSQLAborted = value; }
        }

        public string ConnectionStringStatus
        {
            get { return _ConnectionStringStatus; }
            set { _ConnectionStringStatus = value; }
        }

        public DateTime Created
        {
            get { return _Created; }
            set { _Created = value; }
        }

        public bool IsRunning
        {
            get { return _IsRunning; }
            set { _IsRunning = value; }
        }

        public Thread Thread
        {
            get { return _Thread; }
            set { _Thread = value; }
        }


        public ThreadStart ThreadStart
        {
            get { return _ThreadStart; }
            set { _ThreadStart = value; }
        }

        public Guid Guid
        {
            get { return _Guid; }
            set { _Guid = value; }
        }

        public OleDbConnection OleDbConnection
        {
            get { return _OleDbConnection; }
            set { _OleDbConnection = value; }
        }

        [XmlIgnore]
        public TraceSource TraceSource
        {
            get { return _TraceSource; }
            set { _TraceSource = value; }
        }

        public RecordCompletedProgressState ProgressState
        {
            get { return _ProgressState; }
            set { _ProgressState = value; }
        }

        public string ConnectionStringInputData
        {
            get { return _ConnectionStringInputData; }
            set { _ConnectionStringInputData = value; }
        }


        public BackgroundWorker BackgroundWorker
        {
            get { return _BackgroundWorker; }
            set { _BackgroundWorker = value; }
        }

        public DoWorkEventArgs DoWorkEventArgs
        {
            get { return _DoWorkEventArgs; }
            set { _DoWorkEventArgs = value; }
        }

        #endregion

        #region Constructors
        public DatabaseShortestPathRunner(BackgroundWorker backgroundWorker, TraceSource traceSource)
        {
            Created = DateTime.Now;
            BackgroundWorker = backgroundWorker;
            TraceSource = traceSource;
        }

        public DatabaseShortestPathRunner(BackgroundWorker backgroundWorker)
        {
            Created = DateTime.Now;
            BackgroundWorker = backgroundWorker;
            TraceSource = null;
        }

        public DatabaseShortestPathRunner(TraceSource traceSource)
        {
            Created = DateTime.Now;
            BackgroundWorker = null;
            TraceSource = traceSource;
        }

        public DatabaseShortestPathRunner()
        {
            Created = DateTime.Now;
            BackgroundWorker = null;
            TraceSource = null;
        }
        #endregion

        protected new bool HasTerminateRequest()
        {
            bool ret = base.HasTerminateRequest();
            if (BackgroundWorker != null)
            {
                ret = ret || BackgroundWorker.CancellationPending;
            }
            return ret;
        }

        protected override void OnDoWork()
        {
            Run(null);
        }

        public bool Run(DoWorkEventArgs e)
        {
            bool ret = false;
            ProgressState = new RecordCompletedProgressState();
            if (e != null && DoWorkEventArgs == null)
            {
                DoWorkEventArgs = e;
            }

            if (DoWorkEventArgs != null)
            {

                BatchOptions = (BatchOptions)DoWorkEventArgs.Argument;
                DBManagerInputData = new QueryManager(BatchOptions.DataProviderType, BatchOptions.DatabaseType, BatchOptions.DatabaseConnectionString);

                myShortestPathService = new ShortestPathService();
                string msg = string.Empty;
                myShortestPathService.Timeout = 900000;
                if (!myShortestPathService.IsAlive())
                {
                    msg = myShortestPathService.Setup(NavteqDataSourceConnectionString, this.MaxMiles2Go, this.MaxHours2Go);
                    if (msg != string.Empty) throw new Exception(msg);
                    // Debug.WriteLine(msg);
                }
                spQuery = new ShortestPathQuery();
                spQuery.ShouldDoShortestDistance = BatchOptions.ShouldDoShortestDistance;
                spQuery.ShouldDoShortestTime = BatchOptions.ShouldDoShortestTime;

                try
                {
                    DataTable dataTable = getWork();
                    ret = ProcessRecords(dataTable);

                    //if ((IntPtr.Size == 8) && (DatabaseTypeInputData == DatabaseType.Text))
                    //{
                    //    FlushCSVData();
                    //    cachedInputDataCSV = null;
                    //    cachedInpurtDataFirstLine = "";
                    //}

                    if (TraceSource != null)
                    {
                        TraceSource.TraceEvent(TraceEventType.Information, (int)ProcessEvents.Completed);
                    }

                    if (!HasTerminateRequest())
                    {
                        if (ConnectionStringStatus != null)
                        {
                            UpdateProcessingStatusFinished();
                        }
                    }

                    if (BackgroundWorker != null)
                    {
                        BackgroundWorker.ReportProgress(Convert.ToInt32(ProgressState.PercentCompleted), ProgressState);
                    }

                }
                catch (Exception ex)
                {
                    ProgressState.Error = true;
                    string message = "Error: " + ex.Message + ex.ToString() + ex.StackTrace;
                    if (TraceSource != null)
                    {
                        TraceSource.TraceEvent(TraceEventType.Error, (int)ExceptionEvents.ExceptionOccurred, message);
                    }
                    if (ConnectionStringStatus != null)
                    {
                        UpdateProcessingStatusAborted(ex);
                    }
                }
                finally
                {
                    if (DBManagerInputData != null)
                    {
                        DBManagerInputData.Dispose();
                        DBManagerInputData = null;
                    }
                }
            }
            return ret;
        }

        protected DataTable getWork()
        {
            DataTable ret = null;
            try
            {
                if (TraceSource != null)
                {
                    TraceSource.TraceEvent(TraceEventType.Information, (int)ProcessEvents.Completing, "Getting records");
                }

                string sql = "SELECT ";
                sql += " " + BatchOptions.FieldId + " AS id, ";
                sql += " " + BatchOptions.FieldFromLat + " AS fromlat, ";
                sql += " " + BatchOptions.FieldFromLon + " AS fromlon, ";
                sql += " " + BatchOptions.FieldToLat + " AS tolat, ";
                sql += " " + BatchOptions.FieldToLon + " AS tolon ";
                sql += " FROM [" + BatchOptions.Table + "]";

                if (BatchOptions.NonProcessedOnly)
                {
                    //sql += " WHERE ";
                    //sql += " ([" + OutputFieldProcessed + "] <> 1 or  [" + OutputFieldProcessed + "] is null)";
                }
                sql += " ORDER BY " + BatchOptions.FieldId + " ASC";

                ret = DBManagerInputData.ExecuteDataTable(CommandType.Text, sql, true);

            }
            catch (Exception e)
            {
                throw new Exception("Error occured getting work: " + e.Message, e);
            }
            return ret;
        }

        /*
        protected DataTable getWork()
        {
            try
            {
                if (DBManagerInputData == null)
                {
                    DBManagerInputData = new QueryManager(DataProviderInputData, DatabaseTypeInputData, ConnectionStringInputData);
                }

                if (TraceSource != null)
                {
                    TraceSource.TraceEvent(TraceEventType.Information, (int)ProcessEvents.Completing, "Getting records");
                }

                string sql = "SELECT ";
                sql += " [" + BatchOptions.FieldId + "] AS Id, ";
                sql += " [" + BatchOptions.FieldFromLat + "] AS Fromlat, ";
                sql += " [" + BatchOptions.FieldFromLon + "] AS fromlon, ";
                sql += " [" + BatchOptions.FieldToLat + "] AS tolat, ";
                sql += " [" + BatchOptions.FieldToLon + "] AS tolon ";
                sql += " FROM [" + BatchOptions.Table + "]";

                if (BatchOptions.NonProcessedOnly)
                {
                    //sql += " WHERE ";
                    //sql += " ([" + OutputFieldProcessed + "] <> 1 or  [" + OutputFieldProcessed + "] is null)";
                }
                sql += " ORDER BY [" + BatchOptions.FieldId + "] ASC";

                if ((IntPtr.Size == 8) && (DataProviderInputData == DataProviderType.Odbc))
                {
                    cachedInputDataCSV = new DataTable();
                    StreamReader sr = File.OpenText(ConnectionStringInputData.Substring(4 + ConnectionStringInputData.IndexOf("DBQ=")) + Table + ".csv");
                    string line = "", col = "";
                    string[] lineSplits = null, colSplits = null;
                    DataRow r = null;
                    col = sr.ReadLine();
                    cachedInpurtDataFirstLine = col;
                    colSplits = col.Split(',');

                    foreach (string c in colSplits)
                    {
                        if (c == InputFieldId) cachedInputDataCSV.Columns.Add("Id", c.GetType());
                        else if (c == InputFieldFromLat) cachedInputDataCSV.Columns.Add("Fromlat", c.GetType());
                        else if (c == InputFieldFromLon) cachedInputDataCSV.Columns.Add("fromlon", c.GetType());
                        else if (c == InputFieldToLat) cachedInputDataCSV.Columns.Add("tolat", c.GetType());
                        else if (c == InputFieldToLon) cachedInputDataCSV.Columns.Add("tolon", c.GetType());
                        else cachedInputDataCSV.Columns.Add(c, c.GetType());
                    }
                    while (!sr.EndOfStream)
                    {
                        r = cachedInputDataCSV.NewRow();
                        line = sr.ReadLine();
                        lineSplits = line.Split(',');
                        for (int i = 0; i < colSplits.Length; i++) r[i] = lineSplits[i];
                        cachedInputDataCSV.Rows.Add(r);
                    }
                    sr.Close();
                    cachedInputDataCSV.DefaultView.Sort = "[Id] asc";
                }
                else
                {
                    DBManagerInputData.Open();
                    cachedInputDataCSV = DBManagerInputData.ExecuteDataTable(CommandType.Text, sql);
                    DBManagerInputData.Close();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error occured getting work: " + e.Message, e);
            }
            return cachedInputDataCSV;
        }
         * */

        protected bool ProcessRecords(DataTable dataTable)
        {
            bool ret = false;
            DataRow dataRow = null;
            try
            {
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    if (TraceSource != null)
                    {
                        TraceSource.TraceEvent(TraceEventType.Information, (int)ProcessEvents.Completing, "Processing records");
                    }
                    ProgressState.Total = dataTable.Rows.Count;

                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        if (!HasTerminateRequest())
                        {
                            Boolean awokenByTerminate = SuspendIfNeeded();
                            if (awokenByTerminate) return true;
                            ProgressState.Completed = i;
                            dataRow = dataTable.Rows[i];

                            string id = DatabaseUtils.StringIfNull(dataRow["id"]);
                            double fromlat = DatabaseUtils.DoubleIfNull(dataRow["fromlat"]);
                            double fromlon = DatabaseUtils.DoubleIfNull(dataRow["fromlon"]);
                            double tolat = DatabaseUtils.DoubleIfNull(dataRow["tolat"]);
                            double tolon = DatabaseUtils.DoubleIfNull(dataRow["tolon"]);

                            if (TraceSource != null)
                            {
                                TraceSource.TraceEvent(TraceEventType.Information, (int)ProcessEvents.Running, "{0}: {1}, {2} {3} {4}", new object[] { id, fromlat, fromlon, tolat, tolon });
                            }
                            if (id != string.Empty) ProcessRecord(id, fromlat, fromlon, tolat, tolon);

                            if (ConnectionStringStatus != null)
                            {
                                UpdateProcessingStatusNumberCompleted(dataTable.Rows.Count, i + 1);
                            }
                        }
                        else
                        {
                            if (TraceSource != null)
                                TraceSource.TraceEvent(TraceEventType.Information, (int)ProcessEvents.Cancelling, "Cancelling");

                            if (DoWorkEventArgs != null) DoWorkEventArgs.Cancel = true;
                            break;
                        }
                    }
                    ret = true;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error occured processing records: " + e.Message, e);
            }
            return ret;
        }

        protected bool ProcessRecord(string id, double fromlat, double fromlon, double tolat, double tolon)
        {
            bool ret = false;
            try
            {
                ProgressState.Id = id;
                ProgressState.FromLat = fromlat.ToString();
                ProgressState.FromLon = fromlon.ToString();
                ProgressState.ToLat = tolat.ToString();
                ProgressState.ToLon = tolon.ToString();

                if (BackgroundWorker != null)
                {
                    BackgroundWorker.ReportProgress(Convert.ToInt32(ProgressState.PercentCompleted), ProgressState);
                }
                spQuery.FromLat = fromlat;
                spQuery.FromLon = fromlon;
                spQuery.ToLat = tolat;
                spQuery.ToLon = tolon;
                ShortestPathResult result = myShortestPathService.Calculate(spQuery);
                if (result.Code == ShortestPathResultCode.ERROR) throw new Exception(result.Message);
                UpdateRecord(id, result);

                ProgressState.Completed = 1;
                ret = true;
            }
            catch (Exception e)
            {
                throw new Exception("Error occured processing record: " + e.Message, e);
            }
            return ret;
        }

        protected void UpdateRecord(string recordId, ShortestPathResult result)
        {
            try
            {
                string sql = "";

                // for SqlServer, first turn off warnings so fields are truncated if neccessary
                if (DBManagerInputData.DatabaseType == DatabaseType.SqlServer)
                {
                    sql = " SET ANSI_WARNINGS OFF; ";
                }

                sql += " UPDATE [" + BatchOptions.Table + "] SET ";
                sql += " " + BatchOptions.FieldShortestDistance + " =@d, ";
                sql += " " + BatchOptions.FieldShortestTime + " =@t, ";
                sql += " " + BatchOptions.FieldTravelDistance + " =@td, ";
                sql += " " + BatchOptions.FieldTravelTime + " =@tt, ";
                sql += " " + BatchOptions.FieldKML + " =@kml ";
                sql += " WHERE " + BatchOptions.FieldId + " =@ID ";

                DBManagerInputData.CreateParameters(6);

                DBManagerInputData.AddParameters(0, "@d", result.ShortestDistance);
                DBManagerInputData.AddParameters(1, "@t", result.ShortestTime);
                DBManagerInputData.AddParameters(2, "@td", result.TraveledDistance);
                DBManagerInputData.AddParameters(3, "@tt", result.TraveledTime);
                DBManagerInputData.AddParameters(4, "@kml", result.KML);
                DBManagerInputData.AddParameters(5, "@ID", recordId);

                DBManagerInputData.ExecuteNonQuery(CommandType.Text, sql, true);

            }
            catch (Exception e)
            {
                throw new Exception("Error occured updating record: " + recordId + " : " + e.Message, e);
            }
        }

        /*
        protected void UpdateRecord(string recordId, ShortestPathResult result)
        {
            try
            {
                if ((IntPtr.Size == 8) && (DatabaseTypeInputData == DatabaseType.Text))
                {
                    foreach (DataRow r in cachedInputDataCSV.Rows)
                        if (r["Id"].ToString() == recordId)
                        {
                            r[OutputFieldShortestDistance] = result.ShortestDistance;
                            r[OutputFieldShortestTime] = result.ShortestTime;
                            break;
                        }
                    FlushCSVData();
                }
                else
                {
                    if (DBManagerInputData == null)
                    {
                        DBManagerInputData = new QueryManager(DataProviderInputData, DatabaseTypeInputData, ConnectionStringInputData);
                    }

                    string sql = " UPDATE [" + BatchOptions.Table + "] SET ";
                    sql += " [" + BatchOptions.FieldShortestDistance + "] =@d, ";
                    sql += " [" + BatchOptions.FieldShortestTime + "] =@t ";
                    sql += " WHERE [" + BatchOptions.FieldId + "] =@ID ";

                    DBManagerInputData.Open();
                    DBManagerInputData.CreateParameters(3);

                    DBManagerInputData.AddParameters(0, "@d", result.ShortestDistance);
                    DBManagerInputData.AddParameters(1, "@t", result.ShortestTime);
                    DBManagerInputData.AddParameters(2, "@ID", recordId);

                    DBManagerInputData.ExecuteNonQuery(CommandType.Text, sql);
                    DBManagerInputData.Close();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error occured updating record: " + recordId + " : " + e.Message, e);
            }
        }
         */

        protected void UpdateProcessingStatusAborted(Exception ex)
        {
            try
            {
                string status = ex.ToString();

                if (DBManagerStatus == null)
                {
                    DBManagerStatus = new QueryManager(DataProviderStatus, DatabaseTypeStatus, ConnectionStringStatus);
                }

                DBManagerStatus.Open();
                DBManagerStatus.CreateParameters(3);
                DBManagerStatus.AddParameters(0, "@" + UpdateProcessStatusSQLParameterTimeUpdated, DateTime.Now);
                DBManagerStatus.AddParameters(1, "@" + UpdateProcessStatusSQLParameterResultStatus, status);
                DBManagerStatus.AddParameters(2, "@" + UpdateProcessStatusSQLParameterGUID, Guid.ToString());

                DBManagerStatus.ExecuteNonQuery(CommandType.Text, UpdateProcessStatusSQLAborted);
                DBManagerStatus.Close();

            }
            catch (Exception e)
            {
                throw new Exception("Error occured UpdateProcessingStatus: " + e.Message, e);
            }
        }

        protected void UpdateProcessingStatusFinished()
        {
            try
            {
                if (DBManagerStatus == null)
                {
                    DBManagerStatus = new QueryManager(DataProviderStatus, DatabaseTypeStatus, ConnectionStringStatus);
                }

                DBManagerStatus.Open();
                DBManagerStatus.CreateParameters(3);
                DBManagerStatus.AddParameters(0, "@" + UpdateProcessStatusSQLParameterTimeUpdated, DateTime.Now);
                DBManagerStatus.AddParameters(1, "@" + UpdateProcessStatusSQLParameterResultStatus, "Completed Successfully");
                DBManagerStatus.AddParameters(2, "@" + UpdateProcessStatusSQLParameterGUID, Guid.ToString());

                DBManagerStatus.ExecuteNonQuery(CommandType.Text, UpdateProcessStatusSQLFinished);
                DBManagerStatus.Close();

            }
            catch (Exception e)
            {
                throw new Exception("Error occured UpdateProcessingStatus: " + e.Message, e);
            }
        }

        /*
        private void FlushCSVData()
        {
            DataRow r = null;
            string[] data = new string[cachedInputDataCSV.Rows.Count + 1];
            string rowLine = "";
            data[0] = cachedInpurtDataFirstLine;

            for (int x = 0; x < cachedInputDataCSV.Rows.Count; x++)
            {
                r = cachedInputDataCSV.Rows[x];
                for (int y = 0; y < cachedInputDataCSV.Columns.Count; y++)
                    if (y == 0) rowLine = r[0].ToString(); else rowLine += "," + r[y].ToString();
                data[x + 1] = rowLine;
            }
            File.WriteAllLines(ConnectionStringInputData.Substring(4 + ConnectionStringInputData.IndexOf("DBQ=")) + Table + ".csv", data);
        }
         */

        protected void UpdateProcessingStatusNumberCompleted(int recordsTotal, int recordsCompleted)
        {
            try
            {
                if (DBManagerStatus == null)
                {
                    DBManagerStatus = new QueryManager(DataProviderStatus, DatabaseTypeStatus, ConnectionStringStatus);
                }

                DBManagerStatus.Open();
                DBManagerStatus.CreateParameters(4);
                DBManagerStatus.AddParameters(0, "@" + UpdateProcessStatusSQLParameterTotal, recordsTotal);
                DBManagerStatus.AddParameters(1, "@" + UpdateProcessStatusSQLParameterCompleted, recordsCompleted);
                DBManagerStatus.AddParameters(2, "@" + UpdateProcessStatusSQLParameterTimeUpdated, DateTime.Now);
                DBManagerStatus.AddParameters(3, "@" + UpdateProcessStatusSQLParameterGUID, Guid.ToString());

                DBManagerStatus.ExecuteNonQuery(CommandType.Text, UpdateProcessStatusSQLNumberCompleted);
                DBManagerStatus.Close();

            }
            catch (Exception e)
            {
                throw new Exception("Error occured UpdateProcessingStatus: " + e.Message, e);
            }
        }
    }
}