using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CDB_TSB
{
    class ManageData
    {
        public List<Tuple<string, string>> categoryCount = new List<Tuple<string, string>>();
        public List<Tuple<String, String, String, String, String, String, String>> callLogData = new List<Tuple<String, String, String, String, String, String, String>>();
        Tuple<String, String, String, String, String, String, String> CurrentRecord;

        //private List<Tuple<Int32,Int32,String,String,String,DateTime,String,DateTime>> callLogData = new List<Tuple<Int32, Int32, String, String, String, DateTime, String, DateTime>>();


        SqlConnection connection = new SqlConnection();
        public SqlConnection MyConnection() //TODO probably remove method and declare SQL connection in PullMainPage() or pass into method? issue currently is when trying to call MyConnection.Logoff() we are setting connection sting again and program thinks we are trying to change the value while the connection is open.
        {
            //string serverName = key.GetValue("");

            string conInput = "Server=ortho-sql; Database=CDB; Integrated Security=True";

            if (connection.State == ConnectionState.Closed) //StackOverFlow Exception
            {
                //Console.WriteLine("conInput:"+conInput + " ConnectionString:" + connection.ConnectionString);
                connection.ConnectionString = conInput;
            }

            //Console.WriteLine("SQLLogin Connection:" + connection.ConnectionString);
            //connection.Open();
            return connection;
        }

        public void Login()
        {

            try
            {
                MyConnection().Open();
                Console.WriteLine("Connected");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }

        public void Logoff()
        {
            try
            {
                MyConnection().Close();
                Console.WriteLine("Disconnected");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void PullLogs(DateTime startDate, DateTime endDate)
        {
            callLogData.Clear();
            string sql = "";

            sql = "select tiId, clId, clCategory, tiSubject, clLogText,clInsertUser, clInsertDate from tblCallLog "
                  +"inner join tblTicket on tiId = clTiId "
                  +"where tiLastSupportContactDate >= '" + startDate + "' "
                  +"and tiLastSupportContactDate <= '" + endDate + "' "
                  +"and(clCategory like 'Edge%' or clCategory like 'VP%' or clCategory like 'ViewPoint') and tiSubject<> 'EDGE- delete patients & contracts' and clDelete<> 1 "
                  +"order by clCategory, tiId,clId";

            //Console.WriteLine(sql);
            using (SqlCommand pullCallLogData = new SqlCommand(sql, MyConnection()))
            {
                SqlDataReader callLogDataReader = pullCallLogData.ExecuteReader();
                List<string> tempList = new List<string>();

                if (callLogDataReader.HasRows)
                {
                    while (callLogDataReader.Read())
                    {

                        for (int i = 0; i < callLogDataReader.FieldCount; i++)
                        {
                            if (callLogDataReader[i] == null)
                            {
                                Console.WriteLine("null");
                                
                            }
                            else
                            {
                                if(i == 6)
                                {
                                    tempList.Add(callLogDataReader[i].ToString());

                                    //Console.WriteLine(tempList[0]+" /"+ tempList[1]+ " /"+ tempList[2] + " /" + tempList[3] + " /" + tempList[4]+ " /"+ tempList[5]+ " /" + tempList[6]);
                                    callLogData.Add(new Tuple<String, String, String, String, String, String, String>(tempList[0],tempList[1],tempList[2],tempList[3],tempList[4],tempList[5],tempList[6]));
                                    //Console.WriteLine("data added");
                                    tempList.Clear();
                                }
                                else
                                {
                                    tempList.Add(callLogDataReader[i].ToString());
                                }
                            }
                        }
                    }
                    
                    CurrentRecord = callLogData[0];
                }
                callLogDataReader.Close();

            }

        }

        public void PullCounts(DateTime startDate, DateTime endDate)
        {
            categoryCount.Clear();
            string sql = "";

            sql = "select clCategory, COUNT(DISTINCT tiId) as Count from tblCallLog "
                  + "inner join tblTicket on tiId = clTiId "
                  + "where tiLastSupportContactDate >= '" + startDate + "' "
                  + "and tiLastSupportContactDate <= '" + endDate + "' "
                  + "and(clCategory like 'Edge%' or clCategory like 'VP%' or clCategory like 'ViewPoint') and tiSubject<> 'EDGE- delete patients & contracts' and clDelete<> 1 "
                  + "group by clCategory order by Count desc";

            //Console.WriteLine(sql);
            using (SqlCommand pullCallLogCount = new SqlCommand(sql, MyConnection()))
            {
                SqlDataReader callLogCountReader = pullCallLogCount.ExecuteReader();
                List<string> tempList = new List<string>();

                if (callLogCountReader.HasRows)
                {
                    while (callLogCountReader.Read())
                    {
                        for (int i = 0; i < callLogCountReader.FieldCount; i++)
                        {
                            if (callLogCountReader[i] == null)
                            {
                                Console.WriteLine("null");

                            }
                            else
                            {
                                if (i == 1)
                                {
                                    tempList.Add(callLogCountReader[i].ToString());

                                    //Console.WriteLine(tempList[0]+" /"+ tempList[1]+ " /"+ tempList[2] + " /" + tempList[3] + " /" + tempList[4]+ " /"+ tempList[5]+ " /" + tempList[6]);
                                    categoryCount.Add(new Tuple<string,string>(tempList[0], tempList[1]));
                                    //Console.WriteLine("data added");
                                    tempList.Clear();
                                }
                                else
                                {
                                    tempList.Add(callLogCountReader[i].ToString());
                                }
                                
                            }
                        }
                    }
                }
                //Console.WriteLine(categoryCount[0].ToString());
                callLogCountReader.Close();

            }
        }
    }
}
