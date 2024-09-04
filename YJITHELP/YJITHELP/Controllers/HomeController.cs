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
    public class HomeController : Controller
    {
        string strJson = "";
        string strResult = "";
        DataTable dt = new DataTable();
        Sql_Cust SC = new Sql_Cust();
        DataTable ResultDt = new DataTable();
        public ActionResult Index()
        {
            ViewBag.MENU1 = "HOME";
            return View();
        }
        public class JsonData
        {
            public string vJsonData { get; set; }
        }

        /// <summary>
        /// 로그인함수
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public string fnLogin(JsonData value)
        {
            string DB_con = _DataHelper.ConnectionString;

            strResult = value.vJsonData.ToString();

            dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            try
            {
                ResultDt = _DataHelper.ExecuteDataTable(SC.GetUserInfo(dt.Rows[0]), CommandType.Text);
                ResultDt.TableName = "Table";

                if (ResultDt.Rows.Count == 0)
                {
                    strJson = _common.MakeJson("N", "로그인 실패", ResultDt);
                }
                else
                {
                    strJson = _common.MakeJson("Y", "로그인 성공", ResultDt);
                }

                return strJson;
            }
            catch (Exception e)
            {
                strJson = _common.MakeJson("E", e.Message);
                return strJson;
            }
        }

        /// <summary>
        /// 로그인 후 데이터 세션 아이디 정보 저장
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveLogin(JsonData value)
        {
            DataSet ds = JsonConvert.DeserializeObject<DataSet>(value.vJsonData);
            DataTable rst = ds.Tables["Result"];
            DataTable dt = ds.Tables["Table"];

            try
            {
                if (rst.Rows[0]["trxCode"].ToString() == "N") return Content("N");

                if (rst.Rows[0]["trxCode"].ToString() == "Y")
                {
                    Session["PIC_NM"] = dt.Rows[0]["PIC_NM"].ToString().Trim();
                    Session["TEL_NO"] = dt.Rows[0]["TEL_NO"].ToString().Trim();
                    Session["EMAIL"] = dt.Rows[0]["EMAIL"].ToString().Trim();
                    Session["PIC_ID"] = dt.Rows[0]["PIC_ID"].ToString().Trim();
                    Session["CUST_CD"] = dt.Rows[0]["CUST_CD"].ToString().Trim();

                    return Content("Y");
                }

                return Content("N");
            }
            catch (Exception e)
            {
                return Content(e.Message);
            }
        }

        [HttpPost]
        public string LogOut()
        {
            Session.Clear();
            Session.RemoveAll();
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();

            return "Y";
        }


        //[HttpPost]
        //public string SearchElvis(JsonData value)
        //{
        //    int nResult = 0;
        //    DataTable returndt = new DataTable();//데이터 테이블 변수선언
        //    string DB_con = _DataHelper.ConnectionString;

        //    string strResult = value.vJsonData.ToString();

        //    DataTable dt = JsonConvert.DeserializeObject<DataTable>(strResult);
        //    try
        //    {
        //        if (dt.Rows.Count > 0)
        //        {
        //            returndt = _DataHelper.ExecuteDataTable(SC.SearchElvis(dt.Rows[0]), CommandType.Text);
        //            returndt.TableName = "ELVIS";

        //            if (returndt.Rows.Count > 0)
        //            {
        //                strJson = _common.MakeJson("Y", "검색성공", returndt);
        //            }
        //            else
        //            {
        //                strJson = _common.MakeJson("N", "검색실패", returndt);
        //            }
        //        }
        //        else
        //        {
        //            strJson = _common.MakeJson("N", "오류");
        //        }

        //        return strJson;
        //    }
        //    catch (Exception e)
        //    {
        //        strJson = _common.MakeJson("E", e.Message);
        //        return strJson;
        //    }
        //}

        //[HttpPost]
        //public string SearchElvis21(JsonData value)
        //{
        //    int nResult = 0;
        //    DataTable returndt = new DataTable();//데이터 테이블 변수선언
        //    string DB_con = _DataHelper.ConnectionString;

        //    string strResult = value.vJsonData.ToString();

        //    DataTable dt = JsonConvert.DeserializeObject<DataTable>(strResult);
        //    try
        //    {
        //        if (dt.Rows.Count > 0)
        //        {
        //            returndt = _DataHelper.ExecuteDataTable(SC.SearchElvis21(dt.Rows[0]), CommandType.Text);
        //            returndt.TableName = "ELVIS21";

        //            if (returndt.Rows.Count == 0)
        //            {
        //                strJson = _common.MakeJson("Y", "검색성공", returndt);
        //            }
        //            else
        //            {
        //                strJson = _common.MakeJson("N", "검색실패", returndt);
        //            }
        //        }
        //        else
        //        {
        //            strJson = _common.MakeJson("N", "오류");
        //        }

        //        return strJson;
        //    }
        //    catch (Exception e)
        //    {
        //        strJson = _common.MakeJson("E", e.Message);
        //        return strJson;
        //    }
        //}

        public class JsonGetData
        {
            public string FILE_NM { get; set; }
            public string FILE_PATH { get; set; }
            public string FILE_MNGTNO { get; set; }
            public string FILE_SEQ { get; set; }
            public string FILE_FORMID { get; set; }
            public string REAL_FILE_NM { get; set; }
        }
        private static void deleteFolder(string folderDir)
        {
            try
            {
                int deleteDay = 1;
                DirectoryInfo di = new DirectoryInfo(folderDir);
                if (di.Exists)
                {
                    DirectoryInfo[] dirInfo = di.GetDirectories();
                    string lDate = DateTime.Today.AddDays(-deleteDay).ToString("yyyyMMdd");
                    foreach (DirectoryInfo dir in dirInfo)
                    {
                        if (lDate.CompareTo(dir.Name.ToString()) > 0)
                        {
                            dir.Attributes = FileAttributes.Normal;
                            dir.Delete(true);
                        }
                    }
                }
            }
            catch (Exception) { }
        }

        [HttpGet]
        public ActionResult DownLoad_ELVIS_Menual(JsonGetData value)
        {
            FileInfo fi;
            string strServer_Path = "http://110.45.209.46:9632";
            string strFile_NM = value.FILE_NM;
            string strFile_MngtNo = value.FILE_MNGTNO;
            string strFile_SEQ = value.FILE_SEQ;
            string strFile_Path = value.FILE_PATH;
            string strFile_FormID = value.FILE_FORMID; //현재 OnlineHelp가 있을 경우는 FileNM이 다름
            string strRealFileNM = value.REAL_FILE_NM;
            string strLocalFilePath = Server.MapPath("~/Files/CRM") + "\\" + DateTime.Now.ToString("yyyyMMdd");

            //파일이 있는지 확인 후 다운로드
            try
            {
                deleteFolder(Server.MapPath("~/Files/CRM")); //파일 삭제 (Default 세팅 하루 이전 폴더 완전 삭제)

                //현재 날짜 파일 생성
                DirectoryInfo di = new DirectoryInfo(strLocalFilePath); //폴더 관련 객체
                if (di.Exists != true)
                {
                    di.Create();
                }

                di.Refresh();
                di = new DirectoryInfo(strLocalFilePath + "\\" + strFile_MngtNo);

                if (di.Exists != true)
                {
                    di.Create();
                }

                //똑같은 파일 있는지 확인.
                fi = new FileInfo(strLocalFilePath + "\\" + strFile_MngtNo + "\\" + strFile_NM);

                if (fi.Exists != true)
                {
                    //URL로 가져온 파일 내컴퓨터에 다운로드
                    WebClient wc = new WebClient();
                    wc.DownloadFile(strServer_Path + strFile_Path + strFile_NM, strLocalFilePath + "\\" + strFile_MngtNo + "\\" + strFile_NM);
                    fi.Refresh();
                    fi = new FileInfo(strLocalFilePath + "\\" + strFile_MngtNo + "\\" + strFile_NM);
                }

                //return File(fi.FullName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", strFile_NM);
                return File(fi.FullName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", strRealFileNM);
            }
            catch (Exception ex)
            {
                return Content("<script>alert('" + ex.Message + "'); window.history.back();</script>");
            }
        }
    }
}