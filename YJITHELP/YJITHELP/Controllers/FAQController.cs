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
    public class FAQController : Controller
    {
        string strJson = "";
        string strResult = "";
        DataTable dt = new DataTable();
        Sql_Cust SC = new Sql_Cust();
        DataTable ResultDt = new DataTable();

        public ActionResult Index()
        {
            ViewBag.MENU3 = "FAQ";
            return View();
        }

        public ActionResult view()
        {            
            return View();
        }

        public class JsonData
        {
            public string vJsonData { get; set; }
        }

        public string FaqList(JsonData value)
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
                    returndt = _DataHelper.ExecuteDataTable(SC.FaqList(dt.Rows[0]), CommandType.Text);
                    returndt.TableName = "FaqList";
                    if (returndt.Rows.Count > 0)
                    {
                        //var GRP_NM = returndt.Rows[0]["GRP_NM"].ToString().Trim();
                        strJson = _common.MakeJson("Y", "검색성공", returndt);
                    }
                    else
                    {
                        strJson = _common.MakeJson("N", "검색실패", returndt);
                    }
                }
                else
                {
                    strJson = _common.MakeJson("N", "오류");
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
        public string SearchFaq(JsonData value)
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
                    returndt = _DataHelper.ExecuteDataTable(SC.SearchFaq(dt.Rows[0]), CommandType.Text);
                    returndt.TableName = "SearchFaq";
                    if (returndt.Rows.Count > 0)
                    {
                        strJson = _common.MakeJson("Y", "검색성공", returndt);
                    }
                    else
                    {
                        strJson = _common.MakeJson("N", "검색실패", returndt);
                    }
                }
                else
                {
                    strJson = _common.MakeJson("N", "오류");
                }

                return strJson;
            }
            catch (Exception e)
            {
                strJson = _common.MakeJson("E", e.Message);
                return strJson;
            }
        }
        public string FLAG { get; set; }
        public string FaqView(JsonData value)
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
                    //인서트 로직
                    int rtn_status = _DataHelper.ExecuteNonQuery(SC.DUMPFAQ(dt.Rows[0]), CommandType.Text);
                    if(rtn_status > 0)
                    {
                        //FLAG = dr["FLAG"].ToString()
                        returndt = _DataHelper.ExecuteDataTable(SC.FaqView(dt.Rows[0]), CommandType.Text);
                        returndt.TableName = "FaqView";

                        int rtn_del_status = _DataHelper.ExecuteNonQuery(SC.DelDUMPFAQ(dt.Rows[0]), CommandType.Text);
                        if (returndt.Rows.Count > 0)
                        {
                            //Session["PARENT_NODE_ID"] = returndt.Rows[0]["PARENT_NODE_ID"].ToString().Trim();
                            strJson = _common.MakeJson("Y", "검색성공", returndt);
                        }
                        else
                        {
                            strJson = _common.MakeJson("N", "검색실패", returndt);
                        }
                    }
                    else
                    {
                        strJson = _common.MakeJson("N", "검색실패", returndt);
                    }
                }
                else
                {
                    strJson = _common.MakeJson("N", "오류");
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