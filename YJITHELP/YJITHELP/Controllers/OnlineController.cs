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
    public class OnlineController : Controller
    {

        string strJson = "";
        string strResult = "";
        DataTable dt = new DataTable();
        Sql_Cust SC = new Sql_Cust();
        DataTable ResultDt = new DataTable();

        public ActionResult Index()
        {
            ViewBag.MENU2 = "ONLINE";
            return View();
        }

        public class JsonData
        {
            public string vJsonData { get; set; }
        }

        [HttpPost]
        public string SearchOnline(JsonData value)
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
                    returndt = _DataHelper.ExecuteDataTable(SC.SearchOnline(dt.Rows[0]), CommandType.Text);
                    returndt.TableName = "Online";
                    if(returndt.Rows.Count > 0)
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
        public string SearchDetail (JsonData value)
        {
            int nResult = 0;
            DataTable returndt = new DataTable();//데이터 테이블 변수선언
            string DB_con = _DataHelper.ConnectionString;

            string strResult = value.vJsonData.ToString();

            DataTable dt = JsonConvert.DeserializeObject<DataTable>(strResult);
            DataSet ds = new DataSet();
            try
            {
                if (dt.Rows.Count > 0)
                {
                    returndt = _DataHelper.ExecuteDataTable(SC.SearchDetail(dt.Rows[0]), CommandType.Text);
                    returndt.TableName = "Detail";
                    ds.Tables.Add(returndt);

                    returndt = _DataHelper.ExecuteDataTable(SC.SearchFile(dt.Rows[0]), CommandType.Text);
                    returndt.TableName = "File";
                    ds.Tables.Add(returndt);


                    strJson = _common.DS_MakeJson("Y", "검색성공", ds);
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
        public string SearchList()
        {
            int nResult = 0;
            DataTable returndt = new DataTable();//데이터 테이블 변수선언
            string DB_con = _DataHelper.ConnectionString;

            try
            {
                returndt = _DataHelper.ExecuteDataTable(SC.SearchList(), CommandType.Text);
                returndt.TableName = "List";
                if (returndt.Rows.Count > 0)
                {
                    strJson = _common.MakeJson("Y", "검색성공", returndt);
                }
                else
                {
                    strJson = _common.MakeJson("N", "검색실패", returndt);
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
        public string SearchList2(JsonData value)
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
                    returndt = _DataHelper.ExecuteDataTable(SC.SearchList2(dt.Rows[0]), CommandType.Text);
                    returndt.TableName = "List2";
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
        public string SaveOnline(JsonData value)
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
                    if (dt.Rows[0]["MNGT_NO"].ToString() == "") {

                        string KeyNo = DateTime.Now.ToString("yyyyMMddHHmmssffffff");

                        returndt = _DataHelper.ExecuteDataTable(SC.CheckMNGT(KeyNo), CommandType.Text);
                        if (returndt.Rows.Count > 0)
                        {
                            strJson = _common.MakeJson("N", "저장실패", returndt);
                            return strJson;
                        }
                        else
                        {
                            dt.Rows[0]["MNGT_NO"] = KeyNo;
                            returndt = _DataHelper.ExecuteDataTable(SC.SaveOnline(dt.Rows[0]), CommandType.Text);
                            strJson = _common.MakeJson("Y", "저장성공" , dt);
                        }
                        
                    }
                    else { 
                         int i = _DataHelper.ExecuteNonQuery(SC.SaveOnline(dt.Rows[0]), CommandType.Text);
                        if (i != 0)
                        {
                            strJson = _common.MakeJson("Y", "저장성공" , dt);
                        }
                        else {
                            strJson = _common.MakeJson("N", "저장성공");
                        }
                    }
                    //BEGIN QUERY
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
        public class JsonGetData
		{
			public string FILE_NM { get; set; }
			public string FILE_PATH { get; set; }
			public string FILE_MNGTNO { get; set; }
			public string FILE_SEQ { get; set; }
			public string FILE_FORMID { get; set; }
			public string REAL_FILE_NM { get; set; }
		}
        /// <summary>
		/// 파일 DOWNLOAD
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		[HttpGet]
        public ActionResult DownLoad_Files(JsonGetData value)
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

        [HttpPost]
        public ActionResult OnlineUpload()
        {
            
            Con_Main Con_Main = new Con_Main(); //Con_Main.cs 컨트롤러 호출
            Encryption ec = new Encryption(); //암호화, 새 객체 생성
            DataSet ds = new DataSet(); //클라이언트 메모리 상에 존재하는 테이블들, 개발자가 직접 모든 테이블 구조를 만들고 데이터 삽입
            DataTable dt= new DataTable(); //데이터가 모인 열들의 집합체
            DataRow dr; //데이터가 모인 한 열
            //DataRow → DataTable → Dataset  
            FileInfo fi; //파일의 정보를 제공하는 클래스
            string strResult = ""; //문자열이 값이 텍스트인 strResult 변수 선언
            string SavePath = "/EDMS/WEB/" + DateTime.Now.ToString("yyyyMMdd") + "/"; //CRM에 파일이 저장될 경로(현재 날짜, 년월일로 변환)
            string strFilePath = ""; //문자열이 값이 텍스트인 strFilPath 변수 선언
            UTF8Encoding UTF8_Encodeing = new UTF8Encoding(); //문자 인코딩. 유니코드 문자 집합을 바이트 시퀀스로 변환

            string JsonVal = "";

            string strMNGT_NO = DateTime.Now.ToString("yyyyMMddHHmmssffffff"); //문자열 strMNGT_NO 변수를 선언하고 현재 날짜, 년/월/일/시/분/초/초 뒤의 소수점 6자리까지로 변환            
            string strCustNM = Request.Form["UserCust"].ToString();            
            string strName = Request.Form["OP_CD"].ToString();
            string strHP = Request.Form["TEL"].ToString();
            string strEmail = Request.Form["EMAIL"].ToString();
            string strWorkDiv1 = Request.Form["REQ_SVC"].ToString();
            string strWorkDiv2 = Request.Form["REQ_SVC2"].ToString();
            string strTitle = Request.Form["SUBJECT"].ToString();
            string strContent = Request.Form["CONTENT"].ToString();

            string stryyyyMMdd = DateTime.Now.ToString("yyyyMMdd");
            string strHHmmss = DateTime.Now.ToString("HHmmss");

            //Request 개체는 GET 또는 POST에서 전송된 데이터를 받고자 할 때 사용
            //POST방식으로 전송된 모든 HTML 컨트롤 요소 값을 받아올 때는 Request.Form[""]
            //GET 방식으로 전송된 모든 HTML 컨트롤 요소 값을 받아올 때는 Request.QueryString[""]
            //POST/GET 상관없이 받을 때는 Request.Params[""] 또는 Request[""]

            /*사용자의 시스템에서 요청과 함께 전달된 모든 쿠키값을 받을 때는
            ex) Response.Cookies["NOW"].Value = DateTime.Now.ToShortTimeString(); → 현재 시간을 쿠키에 저장
             string now = Request.Cookies["NOW"].Value; → 쿠키 읽어오기*/

            HttpFileCollectionBase files = Request.Files; //파일 업로드할 때 사용

            //CRM 온라인 접수 데이터 테이블 생성
            dt = new DataTable("CRM_INFO");
            dt.Columns.Add("INS_USR");
            dt.Columns.Add("INS_YMD");
            dt.Columns.Add("INS_HM");
            dt.Columns.Add("UPD_USR");
            dt.Columns.Add("UPD_YMD");
            dt.Columns.Add("UPD_HM");
            dt.Columns.Add("REQ_YMD");
            dt.Columns.Add("REQ_HM");
            dt.Columns.Add("SYS_ID");
            dt.Columns.Add("FORM_ID");
            dt.Columns.Add("MNGT_NO");
            dt.Columns.Add("CUST_NM");
            dt.Columns.Add("CONTENT");
            dt.Columns.Add("REQ_USR");
            dt.Columns.Add("TEL_NO");
            dt.Columns.Add("REQ_SVC");
            dt.Columns.Add("REQ_SVC2");
            dt.Columns.Add("UPPER_MNGT_NO");
            dt.Columns.Add("SUBJECT");
            dt.Columns.Add("REQ_EMAIL");

            dr = dt.NewRow();

            dr["INS_USR"] = "";
            dr["INS_YMD"] = stryyyyMMdd;
            dr["INS_HM"] = strHHmmss;
            dr["UPD_USR"] = strName;
            dr["UPD_YMD"] = stryyyyMMdd;
            dr["UPD_HM"] = strHHmmss;
            dr["REQ_USR"] = strName;
            dr["REQ_YMD"] = stryyyyMMdd;
            dr["REQ_HM"] = strHHmmss;
            dr["SYS_ID"] = "WEB";
            dr["FORM_ID"] = "OnlineHelp";
            dr["MNGT_NO"] = strMNGT_NO;
            dr["CUST_NM"] = strCustNM;
            dr["CONTENT"] = strContent;
            dr["TEL_NO"] = strHP;
            dr["REQ_SVC"] = strWorkDiv1;
            dr["REQ_SVC2"] = strWorkDiv2;
            dr["UPPER_MNGT_NO"] = strMNGT_NO;
            dr["SUBJECT"] = strTitle;
            dr["REQ_EMAIL"] = strEmail;

            dt.Rows.Add(dr);

            ds.Tables.Add(dt);

            //파일 관련 데이터 set
            try
            {
                string sUploadHandler = "http://CRM.YJIT.CO.KR:9632/wcf/UploadHandler.aspx";
                System.Net.WebClient wc = new System.Net.WebClient(); 

                //WebClient wc = new WebClient();
                //WebClient 클래스는 System.Net 네임스페이스에 있는 클래스
                /*
                WebClient 클래스는 4종류의 기능을 제공.
                1. 데이터를 가져오기 위해 여러 Download 메서드들
                2. 데이터를 보내기 위한 여러 Upload 메서드들
                3. 데이터를 스트림 형태로 읽어오기 위한 OpenRead 메서드들
                4. 데이터를 스트림 형태로 쓰기 위한 OpenWrite 메서드들
                * Stream(스트림)이란 일련의 연속성을 갖는 흐름
                * 대용량 데이터일 경우 서버에서 파일을 받아올 때 한번에 보내는게 아닌 파일을 잘게 쪼개서 전송
                */
                byte[] responseArray; //배열선언

                dt = new DataTable("FILE_UPLOAD");
                dt.Columns.Add("MNGT_NO");
                dt.Columns.Add("FILE_NM");
                dt.Columns.Add("FILE_SIZE");
                dt.Columns.Add("FILE_PATH");
                dt.Columns.Add("FORM_ID");
                dt.Columns.Add("INS_USR");
                dt.Columns.Add("INS_YMD");
                dt.Columns.Add("INS_HM");
                dt.Columns.Add("UPD_USR");
                dt.Columns.Add("UPD_YMD");
                dt.Columns.Add("UPD_HM");

                NameValueCollection myQueryStringCollection = new NameValueCollection();
                // 상대경로
                myQueryStringCollection.Add("SavePath", SavePath); // Ex)  SavePath  :  "/EMAIL/SEND/" 				

                //foreach는 사용할 수 없습니다.
                for (int i = 0; i < files.Count; i++)
                {

                    string InputFileName = "";
                    string strFileSize = "";
                    string strNowTime = "_" + DateTime.Now.ToString("yyyyMMddHHmmssffffff");

                    HttpPostedFileBase file = files[i]; //스플릿 for문 마지막에 있는것만 확장자로 ㄱㄱ

                    if (file.FileName != "" && file.ContentLength != 0)
                    {
                        if (file != null)
                        {
                            string strFileName = "";

                            InputFileName = Path.GetFileName(file.FileName); //파일 이름

                            string[] fileinfo = InputFileName.Split('.');
                            for (int j = 0; j < fileinfo.Length - 1; j++)
                            {
                                //.이 파일명 사이에 있을 경우						
                                if (j == fileinfo.Length - 2)
                                {
                                    strFileName += fileinfo[j];
                                }
                                else
                                {
                                    strFileName += fileinfo[j] + ".";
                                }
                            }

                            InputFileName = strFileName + strNowTime + "." + fileinfo[fileinfo.Length - 1]; //파일 이름_생성 시간.확장자

                            strFileSize = file.ContentLength.ToString();
                            //파일 사이즈						
                            strFilePath = Path.Combine(Server.MapPath("~/Files/TEMP/") + InputFileName); //날짜 붙혀서 보내기

                            //Save file to server folder  
                            file.SaveAs(strFilePath);
                        }

                        if (file != null && file.ContentLength > 0)
                        {
                            wc.QueryString = myQueryStringCollection;
                            //responseArray = wc.UploadFile(sUploadHandler, "POST", strFilePath);
                            responseArray = wc.UploadFile(sUploadHandler, "POST", strFilePath);
                            strResult = UTF8_Encodeing.GetString(responseArray);
                        }
                        else
                        {
                            responseArray = System.Text.Encoding.ASCII.GetBytes("N\n Upload failed!");
                            strResult = UTF8_Encodeing.GetString(responseArray);
                        }

                        //foreach
                        if (strResult.StartsWith("Y"))
                        {
                            dr = dt.NewRow();

                            //파일 사이즈도 넣어야됨.					

                            string strTimeYMD = DateTime.Now.ToString("yyyyMMdd");
                            string strTimeHMS = DateTime.Now.ToString("HHmmss");

                            dr["MNGT_NO"] = strMNGT_NO;
                            dr["FILE_NM"] = InputFileName;
                            dr["FILE_PATH"] = SavePath;
                            dr["FILE_SIZE"] = strFileSize;
                            dr["FORM_ID"] = "OnlineHelp";
                            dr["INS_USR"] = strName;
                            dr["INS_YMD"] = strTimeYMD;
                            dr["INS_HM"] = strTimeHMS;
                            dr["UPD_USR"] = strName;
                            dr["UPD_YMD"] = strTimeYMD;
                            dr["UPD_HM"] = strTimeHMS;
                            dt.Rows.Add(dr);
                        }
                    }

                    //파일 TEMP 삭제 로직
                    fi = new System.IO.FileInfo(strFilePath);

                    if (fi.Exists)
                    {
                        fi.Delete();
                    }
                }//end for

                ds.Tables.Add(dt);

                strResult = Con_Main.Con_fnSetOnlineRS(ds);


                strJson = _common.MakeJson("Y", "성공");
                return Json(strJson);

            }
            catch (Exception e)
            {
                return Json("[Error]" + e.Message);
            }
        }

        [HttpPost]
        public string SelfOnline(JsonData value)
        {
            int nResult = 0;
            DataTable returndt = new DataTable();//데이터 테이블 변수선언
            string DB_con = _DataHelper.ConnectionString;

            string strResult = value.vJsonData.ToString();

            DataTable dt = JsonConvert.DeserializeObject<DataTable>(strResult);

            try
            {
                nResult = _DataHelper.ExecuteNonQuery(SC.SelfOnline(dt.Rows[0]), CommandType.Text);
                returndt.TableName = "Self";
                if (nResult > 0)
                {
                    strJson = _common.MakeJson("Y", "저장성공", dt);                    
                }
                else
                {
                    strJson = _common.MakeJson("N", "저장실패");
                }

                return strJson;
            }
            catch (Exception e)
            {
                strJson = _common.MakeJson("E", e.Message);
                return strJson;
            }
        }

        [HttpGet]
        public string AutoUserLogin(JsonData value)
        {
            
            string strResult = value.vJsonData.ToString();
            DataTable returndt = new DataTable();//데이터 테이블 변수선언
            //DataSet ds = JsonConvert.DeserializeObject<DataSet>(value.vJsonData);
            //DataTable rst = ds.Tables["Result"];
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(strResult);
            //DataTable dt = ds.Tables["Table"];

            try
            {
                if (dt.Rows.Count > 0)
                {
                    returndt = _DataHelper.ExecuteDataTable(SC.AutoUserLogin(dt.Rows[0]), CommandType.Text);
                    returndt.TableName = "AutoUserLogin";
                    if (returndt.Rows.Count > 0)
                    {
                        Session["PIC_NM"] = returndt.Rows[0]["PIC_NM"].ToString().Trim();
                        Session["TEL_NO"] = returndt.Rows[0]["TEL_NO"].ToString().Trim();
                        Session["EMAIL"] = returndt.Rows[0]["EMAIL"].ToString().Trim();
                        Session["PIC_ID"] = returndt.Rows[0]["PIC_ID"].ToString().Trim();
                        Session["CUST_CD"] = returndt.Rows[0]["CUST_CD"].ToString().Trim();
                    }                    

                    strJson = _common.MakeJson("Y", "검색성공", returndt);
                }
                else
                {
                    strJson = _common.MakeJson("N", "검색실패", returndt);
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