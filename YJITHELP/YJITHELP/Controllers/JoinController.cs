using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Newtonsoft.Json;
using YJITHELP.Models;
using YJITHELP.Models.Query;

namespace YJITHELP.Controllers
{
    public class JoinController : Controller
    {
        string strJson = "";
        string strResult = "";
        DataTable dt = new DataTable();
        Sql_Cust SC = new Sql_Cust();
        DataTable ResultDt = new DataTable();

        public ActionResult Index()
        {           
            return View();
        }

        public class JsonData
        {
            public string vJsonData { get; set; }
        }

        [HttpPost]
        public string fnRegister(JsonData value)
        {
            int nResult = 0;
            int nResult1 = 0;
            string DB_con = _DataHelper.ConnectionString;

            strResult = value.vJsonData.ToString();

            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            try
            {
                nResult = _DataHelper.ExecuteNonQuery(SC.InsertRegister_Query(dt.Rows[0]), CommandType.Text);

                if (nResult == 1)
                {
                    nResult1 = _DataHelper.ExecuteNonQuery(SC.RegisterRequest_Query(dt.Rows[0]), CommandType.Text);

                    if (nResult1 == -1)
                    {
                        strJson = _common.MakeJson("Y", "CRM전송 성공");
                    }
                    else
                    {
                        strJson = _common.MakeJson("N", "CRM전송 실패");
                    }
                }
                else
                {
                    strJson = _common.MakeJson("N", "가입 실패");
                }
                return strJson;
            }
            catch (Exception e)
            {
                strJson = _common.MakeJson("E", e.Message);
                return strJson;
            }
        }

        [HttpPost]
        public string SearchCrn(JsonData value)
        {
            int nResult = 0;
            DataTable returndt = new DataTable();//데이터 테이블 변수선언
            string DB_con = _DataHelper.ConnectionString;

            string strResult = value.vJsonData.ToString();

            DataTable dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            try
            {
                if (dt.Rows.Count > 0)
                {
                    returndt = _DataHelper.ExecuteDataTable(SC.SearchCrn_Query(dt.Rows[0]), CommandType.Text);
                    if (returndt.Rows.Count > 0)
                    {
                        strJson = _common.MakeJson("Y", "확인 성공", returndt);
                    }
                    else {
                        strJson = _common.MakeJson("N", "확인 실패");
                    }                
                }
                else
                {
                    strJson = _common.MakeJson("E", "데이터가 없습니다.");
                }

                return strJson;
            }
            catch (Exception e)
            {
                strJson = _common.MakeJson("E", e.Message);
                return strJson;
            }
        }

    }
}