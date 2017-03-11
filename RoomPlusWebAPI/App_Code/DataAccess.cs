using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using RoomPlustWebAPI.App_Code;

namespace RoomPlusWebAPI.App_Code
{
    public class DataAccess : IDisposable
    {
        public Database _db = DatabaseFactory.CreateDatabase("DBConnectionString");

        public DataTable GetMainImages()
        {
            try
            {
                using (DbCommand dbc = _db.GetStoredProcCommand("ServiceImageQuery"))
                {
                    return _db.ExecuteDataSet(dbc).Tables[0];
                }
            }
            catch
            {
                return null;
            }
        }

        public DataTable GetMainPageImages(int Id = 0)
        {
            try
            {
                using (DbCommand dbc = _db.GetStoredProcCommand("MainPageImagesQuery"))
                {
                    _db.AddInParameter(dbc, "@SID", DbType.String, Id);
                    return _db.ExecuteDataSet(dbc).Tables[0];
                }
            }
            catch
            {
                return null;
            }
        }

        public DataTable GetVideoList(int Id = 0)
        {
            try
            {
                using (DbCommand dbc = _db.GetStoredProcCommand("VideoListQuery"))
                {
                    _db.AddInParameter(dbc, "@SID", DbType.String, Id);
                    return _db.ExecuteDataSet(dbc).Tables[0];
                }
            }
            catch
            {
                return null;
            }
        }

        public DataTable GetTerminalKey(string SecrectKey)
        {
            try
            {
                using (DbCommand dbc = _db.GetStoredProcCommand("TerminalKeyQuery"))
                {
                    _db.AddInParameter(dbc, "@SecretKey", DbType.String, SecrectKey);
                    return _db.ExecuteDataSet(dbc).Tables[0];
                }
            }
            catch
            {
                return null;
            }
        }

        public DataTable GetHeaderLines(string Language = "en-us", int Id = 0)
        {
            try
            {
                using (DbCommand dbc = _db.GetStoredProcCommand("HeaderLinesQuery"))
                {
                    _db.AddInParameter(dbc, "@SID", DbType.Int32, Id);
                    _db.AddInParameter(dbc, "@Language", DbType.String, Language);

                    return _db.ExecuteDataSet(dbc).Tables[0];
                }
            }
            catch
            {
                return null;
            }
        }

        public DataTable GetWeatherDate(int CityId = 0)
        {
            try
            {
                using (DbCommand dbc = _db.GetStoredProcCommand("WeatherDateQuery"))
                {
                    _db.AddInParameter(dbc, "@CityId", DbType.Int32, CityId);
                    return _db.ExecuteDataSet(dbc).Tables[0];
                }
            }
            catch
            {
                return null;
            }
        }

        public DataTable GetServiceGroups(string language = "en-Us")
        {
            try
            {
                using (DbCommand dbc = _db.GetStoredProcCommand("ServiceGroupsQuery"))
                {
                    _db.AddInParameter(dbc, "@Language", DbType.String, language);
                    return _db.ExecuteDataSet(dbc).Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetServices(string language = "en-Us", int GorupId = 0, int Id = 0)
        {
            try
            {
                using (DbCommand dbc = _db.GetStoredProcCommand("ServicesQuery"))
                {
                    _db.AddInParameter(dbc, "@GroupId", DbType.Int32, GorupId);
                    _db.AddInParameter(dbc, "@SID", DbType.Int32, Id);
                    _db.AddInParameter(dbc, "@Language", DbType.String, language);

                    return _db.ExecuteDataSet(dbc).Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetServiceDetail(int ServiceId, string language = "en-Us")
        {
            try
            {
                using (DbCommand dbc = _db.GetStoredProcCommand("ServiceDetailQuery"))
                {
                    _db.AddInParameter(dbc, "@ServiceId", DbType.Int32, ServiceId);
                    _db.AddInParameter(dbc, "@Language", DbType.String, language);

                    return _db.ExecuteDataSet(dbc).Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int SaveTransactionion(TransactionRequest request)
        {
            int intTransactionId = 0;

            using (DbConnection conn = _db.CreateConnection())
            {
                DbTransaction tran = null;
                try
                {
                    conn.Open();
                    tran = conn.BeginTransaction();

                    using (DbCommand dbc = _db.GetStoredProcCommand("ServiceTransactionInsert"))
                    {
                        request.Status = RequestStatus.Sent;

                        _db.AddInParameter(dbc, "@ServiceId", DbType.Int32, request.ServiceId);
                        _db.AddInParameter(dbc, "@ServiceCode", DbType.String, request.ServiceCode);
                        _db.AddInParameter(dbc, "@Servicename", DbType.String, request.ServiceName);
                        _db.AddInParameter(dbc, "@TerminalId", DbType.String, request.TerminalId);
                        _db.AddInParameter(dbc, "@Location", DbType.String, request.Location);
                        _db.AddInParameter(dbc, "@TimeRemark", DbType.String, request.TimeRemak);
                        if (request.TimeRemak == ConstantValues.TIME_REMARK_NOW)
                        {
                            _db.AddInParameter(dbc, "@StartDateTime", DbType.DateTime, DateTime.Now);
                            _db.AddInParameter(dbc, "@EndDateTime", DbType.DateTime, DateTime.Now);
                        }
                        else
                        {
                            if (request.StartDatetime != null)
                                _db.AddInParameter(dbc, "@StartDateTime", DbType.DateTime, request.StartDatetime);

                            if (request.EndDatetime != null)
                                _db.AddInParameter(dbc, "@EndDateTime", DbType.DateTime, request.EndDatetime);
                        }
                        _db.AddInParameter(dbc, "@Remark", DbType.String, request.Remark);
                        _db.AddInParameter(dbc, "@Status", DbType.Int32, (int)request.Status);

                        intTransactionId = Convert.ToInt32(_db.ExecuteScalar(dbc, tran));

                        #region Detail
                        if (request.ServiceDetails != null)
                        {
                            foreach (TransactionRequest.TransactionDetail detail in request.ServiceDetails)
                            {
                                using (DbCommand dbcDetail = _db.GetStoredProcCommand("ServiceTransactionDetailInsert"))
                                {
                                    _db.AddInParameter(dbcDetail, "@TransactionId", DbType.Int32, intTransactionId);
                                    _db.AddInParameter(dbcDetail, "@TransactionLineNo", DbType.Int32, detail.LineNo);
                                    _db.AddInParameter(dbcDetail, "@ServiceDetailId", DbType.Int32, detail.ServiceDetailId);
                                    _db.AddInParameter(dbcDetail, "@DetailName", DbType.String, detail.DetailName);

                                    if (detail.StartDatetime != null)
                                        _db.AddInParameter(dbcDetail, "@StartDateTime", DbType.DateTime, detail.StartDatetime);

                                    if (detail.EndDatetime != null)
                                        _db.AddInParameter(dbcDetail, "@EndDateTime", DbType.DateTime, detail.EndDatetime);

                                    _db.ExecuteNonQuery(dbcDetail, tran);
                                }
                            }
                        }
                        #endregion

                        tran.Commit();
                    }
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    intTransactionId = 0;
                }
            }//end of using connection

            return intTransactionId;
        }

        public void Dispose()
        {
            _db = null;
        }
    }
}