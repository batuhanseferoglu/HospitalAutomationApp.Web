using AspNetCoreIdentityHospitalAutomationApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Web.Razor.Tokenizer;

namespace AspNetCoreIdentityHospitalAutomationApp.Web.DataAccessLayer
{
    public class DA
    {
        public string _DefaultAzureConnectionString { get; set; }


        public DA(string DefaultAzureConnectionString)
        {
            _DefaultAzureConnectionString = DefaultAzureConnectionString;
        }

        private SqlConnection GetConnection()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Server=tcp:hospitalappdb.database.windows.net,1433;Initial Catalog=HospitalDB;Persist Security Info=False;User ID=batuhan;Password=3956910Scs!!!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            conn.Open();

            return conn; 

        }

        private void CloseConnection(SqlConnection conn)
        {
            conn.Close();
        }

        public List<Event> GetCalendarEvents(string start, string end)
        
        {
            List<Event> events = new List<Event>();

            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(@"select
                                                            EventId
                                                            ,Title
                                                            ,Description
                                                            ,StartEvent
                                                            ,EndEvent
                                                            ,AllDay
                                                        from
                                                            [Events]
                                                        where
                                                            convert(datetime,StartEvent) between StartEvent and EndEvent", conn)
                {
                    CommandType = CommandType.Text
                })
                {
                    //cmd.Parameters.Add("StartEvent", SqlDbType.VarChar).Value = start;
                    //cmd.Parameters.Add("EndEvent", SqlDbType.VarChar).Value = end;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
                            DateTimeFormatInfo format = culture.DateTimeFormat;

                            events.Add(new Event()
                            {
                                EventId = Convert.ToInt32(dr["EventId"]),
                                Title = Convert.ToString(dr["Title"]),
                                Description = Convert.ToString(dr["Description"]),
                                StartEvent = Convert.ToDateTime(dr["StartEvent"]).ToString("MM/dd/yyyy hh:mm tt", culture),
                                EndEvent = Convert.ToDateTime(dr["EndEvent"]).ToString("MM/dd/yyyy hh:mm tt", culture),
                                AllDay = Convert.ToBoolean(dr["AllDay"])
                            });
                        }
                    }
                }
            }

            return events;
        }

        public string UpdateEvent(Event evt)
        {
            string message = "";
            SqlConnection conn = GetConnection();
            SqlTransaction trans = conn.BeginTransaction();

            try
            {
                SqlCommand cmd = new SqlCommand(@"update
	                                                [Events]
                                                set
	                                                [Description]=@description
                                                    ,Title=@title
	                                                ,StartEvent=@start
	                                                ,EndEvent=@end 
	                                                ,AllDay=@allDay
                                                where
	                                                EventId=@eventId", conn, trans)
                {
                    CommandType = CommandType.Text
                };
                cmd.Parameters.Add("@eventId", SqlDbType.Int).Value = evt.EventId;
                cmd.Parameters.Add("@title", SqlDbType.VarChar).Value = evt.Title;
                cmd.Parameters.Add("@description", SqlDbType.VarChar).Value = evt.Description;
                cmd.Parameters.Add("@start", SqlDbType.DateTime).Value = DateTime.ParseExact(evt.StartEvent, "MM/dd/yyyy h:mm tt", CultureInfo.InvariantCulture);
                cmd.Parameters.Add("@end", SqlDbType.DateTime).Value = DateTime.ParseExact(evt.EndEvent, "MM/dd/yyyy h:mm tt", CultureInfo.InvariantCulture);
                cmd.Parameters.Add("@allDay", SqlDbType.Bit).Value = evt.AllDay;
                cmd.ExecuteNonQuery();

                trans.Commit();
            }
            catch (Exception exp)
            {
                trans.Rollback();
                message = exp.Message;
            }
            finally
            {
                CloseConnection(conn);
            }

            return message;
        }

        public string AddEvent(Event evt, out int eventId)
        {
            string message = "";
            SqlConnection conn = GetConnection();
            SqlTransaction trans = conn.BeginTransaction();
            eventId = 0;


            try
            {
                SqlCommand cmd = new SqlCommand(@"insert into [Events]
                                                (
	                                                Title
	                                                ,[Description]
	                                                ,StartEvent
	                                                ,EndEvent
	                                                ,AllDay
                                                )
                                                values
                                                (
	                                                @title
	                                                ,@description
	                                                ,@start
	                                                ,@end
	                                                ,@allDay
                                                );
                                                select scope_identity()", conn, trans)
                {
                    CommandType = CommandType.Text
                };

                cmd.Parameters.Add("@title", SqlDbType.VarChar).Value = evt.Title;
                cmd.Parameters.Add("@description", SqlDbType.VarChar).Value = evt.Description;
                cmd.Parameters.Add("@start", SqlDbType.DateTime).Value = DateTime.ParseExact(evt.StartEvent, "MM/dd/yyyy h:mm tt", CultureInfo.InvariantCulture);
                cmd.Parameters.Add("@end", SqlDbType.DateTime).Value = DateTime.ParseExact(evt.EndEvent, "MM/dd/yyyy h:mm tt", CultureInfo.InvariantCulture);

                cmd.Parameters.Add("@allDay", SqlDbType.Bit).Value = evt.AllDay;

               


                eventId = Convert.ToInt32(cmd.ExecuteScalar());

                trans.Commit();
            }
            catch (Exception exp)
            {
                trans.Rollback();
                message = exp.Message;
            }
            finally
            {
                CloseConnection(conn);
            }

            return message;
        }



        public string DeleteEvent(int eventId)
        {
            string message = "";
            SqlConnection conn = GetConnection();
            SqlTransaction trans = conn.BeginTransaction();

            try
            {
                SqlCommand cmd = new SqlCommand(@"delete from 
	                                                [Events]
                                                where
	                                                EventId=@eventId", conn, trans)
                {
                    CommandType = CommandType.Text
                };
                cmd.Parameters.Add("@eventId", SqlDbType.Int).Value = eventId;
                cmd.ExecuteNonQuery();

                trans.Commit();
            }
            catch (Exception exp)
            {
                trans.Rollback();
                message = exp.Message;
            }
            finally
            {
                CloseConnection(conn);
            }

            return message;
        }
    }
}
