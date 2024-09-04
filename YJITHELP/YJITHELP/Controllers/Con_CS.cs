using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Newtonsoft.Json;
using YJITHELP.Models;
using YJITHELP.Models.Query;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Specialized;

namespace YJITHELP.Controllers
{
    public class Con_CS
    {
        Encryption String_Encrypt = new Encryption();
        Sql_Cust SC = new Sql_Cust();

        DataTable dt = new DataTable();
        DataTable Resultdt = new DataTable();
        DataSet ds = new DataSet();
        string rtnJson = "";

        public class JsonData
        {
            public string vJsonData { get; set; }
        }

        public string Con_GetNoticeList(string strValue)
        {
            string strResult = String_Encrypt.decryptAES256(strValue);

            //데이터
            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            try
            {
                DataTable Resultdt = new DataTable();

                Resultdt = _DataHelper.ExecuteDataTable(SC.NoticeList(dt.Rows[0]), CommandType.Text);
                Resultdt.TableName = "Notice";

                if (Resultdt.Rows.Count == 0)
                {
                    rtnJson = _common.MakeJson("N", "Fail", Resultdt);
                }
                else
                {
                    rtnJson = _common.MakeJson("Y", "Success", Resultdt);
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