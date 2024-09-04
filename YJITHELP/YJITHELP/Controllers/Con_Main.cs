using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using YJITHELP.Models;
using YJITHELP.Models.Query;

namespace YJITHELP.Controllers
{
    public class Con_Main
    {
        Sql_Cust SC = new Sql_Cust();

        //전역 변수
        DataTable dt = new DataTable();
        DataTable Resultdt = new DataTable();
        DataSet ds = new DataSet();
        string rtnJson = "";

        /// <summary>
        /// Online 접수 로직
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string Con_fnSetOnlineRS(DataSet Online_ds)
        {
            int nResult = 0;


            try
            {
                DataTable Resultdt = new DataTable();

                dt = Online_ds.Tables["CRM_INFO"];
                nResult = _DataHelper.ExecuteNonQuery(SC.fnSetOnlineRS_MST_Query(dt.Rows[0]), CommandType.Text);
                if (nResult == 0)
                {
                    rtnJson = _common.MakeJson("N", "fail");
                    return rtnJson;
                }
                nResult = _DataHelper.ExecuteNonQuery(SC.fnSetOnlineRS_Cust_Query(dt.Rows[0]), CommandType.Text);
                if (nResult == 0)
                {
                    rtnJson = _common.MakeJson("N", "fail");
                    return rtnJson;
                }
                dt = Online_ds.Tables["FILE_UPLOAD"];

                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        nResult = _DataHelper.ExecuteNonQuery(SC.fnSetOnlineRS_Files_Query(dt.Rows[0]), CommandType.Text);
                    }
                }
                if (nResult == 0)
                {
                    rtnJson = _common.MakeJson("N", "fail");
                    return rtnJson;
                }
                else {
                    rtnJson = _common.MakeJson("Y", "success");
                }

                return rtnJson;

            }
            catch (Exception e)
            {
                //만약 오류가 발생 하였을 경우
                rtnJson = _common.MakeJson("E", e.Message);
                return rtnJson;
            }
        }
    }
}