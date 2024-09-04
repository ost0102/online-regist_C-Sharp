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
    public class NoticeController : Controller
    {
        string strJson = "";
        string strResult = "";
        DataTable dt = new DataTable();
        Sql_Cust SC = new Sql_Cust();
        DataTable ResultDt = new DataTable();

        public ActionResult Index()
        {
            ViewBag.MENU4 = "Notice";
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

        [HttpPost]
        public string GetNoticeList(JsonData value)
        {
            DataTable returndt = new DataTable();//데이터 테이블 변수선언
            string DB_con = _DataHelper.ConnectionString;

            string strResult = value.vJsonData.ToString();

            DataTable dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            try
            {
                if (dt.Rows.Count > 0)
                {
                    returndt = _DataHelper.ExecuteDataTable(SC.NoticeList(dt.Rows[0]), CommandType.Text);
                    returndt.TableName = "NoticeList";
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

        [HttpPost]
        public string GetNoticeSearch(JsonData value)
        {
            DataTable returndt = new DataTable();//데이터 테이블 변수선언
            string DB_con = _DataHelper.ConnectionString;

            string strResult = value.vJsonData.ToString();

            DataTable dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            try
            {
                if (dt.Rows.Count > 0)
                {
                    returndt = _DataHelper.ExecuteDataTable(SC.NoticeList(dt.Rows[0]), CommandType.Text);
                    returndt.TableName = "NoticeSearch";
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

        public string NoticeView(JsonData value)
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
                    int rtn_status = _DataHelper.ExecuteNonQuery(SC.DUMPNOTICE(dt.Rows[0]), CommandType.Text);
                    if (rtn_status > 0)
                    {
                        //FLAG = dr["FLAG"].ToString()
                        returndt = _DataHelper.ExecuteDataTable(SC.NoticeView(dt.Rows[0]), CommandType.Text);
                        returndt.TableName = "NoticeView";

                        int rtn_del_status = _DataHelper.ExecuteNonQuery(SC.DelDUMPNOTICE(dt.Rows[0]), CommandType.Text);
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