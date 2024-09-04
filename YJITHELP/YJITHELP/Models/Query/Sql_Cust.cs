using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace YJITHELP.Models.Query
{

    public class Sql_Cust
    {
        private static string sSql = "";
        private static bool rtnBool = false;
        string sqlstr = "";
        DateTime Now = DateTime.Now;
        string KeyNo = DateTime.Now.ToString("yyyyMMddHHmmssffffff");
        string SavePath = "/EDMS/WEB/" + DateTime.Now.ToString("yyyyMMdd") + "/";

        private static DataTable dt = new DataTable();

        public string GetUserInfo(DataRow dr)
        {
            sqlstr = "";
            sqlstr += " SELECT B.PIC_NM AS PIC_NM ";
            sqlstr += "     , B.TEL_NO AS TEL_NO ";
            sqlstr += "     , B.EMAIL ";
            sqlstr += "     , B.PIC_ID AS PIC_ID ";
            sqlstr += "     , B.PIC_PWD AS PIC_PWD ";
            sqlstr += "     , A.CUST_NM ";
            sqlstr += "     , A.CUST_CD ";
            sqlstr += " FROM CRM_CUST_MST@DL_CRM A, CRM_CUST_PIC@DL_CRM B ";
            sqlstr += " WHERE A.CUST_CD = B.CUST_CD ";
            sqlstr += " AND (UPPER(PIC_ID) = '" + dr["USR_ID"].ToString() +  "') ";
            sqlstr += " AND PIC_PWD = '" + dr["PSWD"].ToString() +  "' ";
            

            return sqlstr;
        }

        public string InsertRegister_Query(DataRow dr)
        {
            sqlstr = "";
            sqlstr += "INSERT INTO CRM_CUST_PIC@DL_CRM(";
            sqlstr += "       CUST_CD "; 
            sqlstr += "     , SEQ"; 
            sqlstr += "     , PIC_NM   "; 
            sqlstr += "     , RANK"; 
            sqlstr += "     , TEL_NO"; 
            sqlstr += "     , EMAIL"; 
            sqlstr += "     , PIC_ID"; 
            sqlstr += "     , PIC_PWD"; 
            sqlstr += "       ) VALUES ("; 
            sqlstr += "     '" + dr["OFFICE_CD"].ToString() + "'";
            sqlstr += "     , (SELECT NVL(MAX(SEQ),0)+1 AS SEQ FROM CRM_CUST_PIC@DL_CRM WHERE CUST_CD = '" + dr["OFFICE_CD"].ToString() + "')"; 
            sqlstr += "     , '" + dr["PIC_CD"].ToString() + "'";
            sqlstr += "     , '" + dr["PIC_RANK"].ToString() + "'";
            sqlstr += "     , '" + dr["PIC_TEL"].ToString() + "'";
            sqlstr += "     , '" + dr["PIC_EMAIL"].ToString() + "'";
            sqlstr += "     , '" + dr["PIC_ID"].ToString() + "'";
            sqlstr += "     , '" + dr["PIC_PWD"].ToString() + "')";

            return sqlstr;
        }

        public string RegisterRequest_Query(DataRow dr)
        {
            sqlstr = "";
            sqlstr += "		BEGIN";
            sqlstr += "				INSERT INTO CRM_AS_MST@DL_CRM(";
            sqlstr += "					INS_USR	  , INS_YMD		  , INS_HM";
            sqlstr += "				  , UPD_USR	  , UPD_YMD		  , UPD_HM";
            sqlstr += "				  , REQ_YMD	  , REQ_HM";
            sqlstr += "				  , SYS_ID	  , FORM_ID";
            sqlstr += "				  , MNGT_NO";
            sqlstr += "				  , CUST_CD		";
            sqlstr += "				  , CONTENT	    ";
            sqlstr += "				  , REQ_USR	    ";
            sqlstr += "				  , TEL_NO	    ";
            sqlstr += "						)";
            sqlstr += "				VALUES (";
            sqlstr += "					'" + dr["PIC_CD"].ToString() + "'	, TO_CHAR(SYSDATE,'YYYYMMDD'),	TO_CHAR(SYSDATE,'HH24MI')";
            sqlstr += "				  , '" + dr["PIC_CD"].ToString() + "'	, TO_CHAR(SYSDATE,'YYYYMMDD'),	TO_CHAR(SYSDATE,'HH24MI')";
            sqlstr += "				  , TO_CHAR(SYSDATE,'YYYYMMDD')	,TO_CHAR(SYSDATE,'HH24MI')";
            sqlstr += "				  , 'WEB'	, 'OnlineHelp'";
            sqlstr += "				  , '" + KeyNo + "'";
            sqlstr += "				  , '" + dr["OFFICE_CD"].ToString() + "'";
            sqlstr += "				  , '온라인 회원가입 승인요청합니다. '||CHR(10)||CHR(13)||' ID : " + dr["PIC_ID"].ToString() + " '||CHR(10)||CHR(13)||' PW : " + dr["PIC_PWD"].ToString() + "'";
            sqlstr += "				  , '" + dr["PIC_CD"].ToString() + "'";
            sqlstr += "				  , '" + dr["PIC_TEL"].ToString() + "'";
            sqlstr += "						);";
            sqlstr += "				INSERT INTO CRM_AS_CUST@DL_CRM(";
            sqlstr += "					INS_USR	  , INS_YMD	, INS_HM";
            sqlstr += "				  , UPD_USR	  , UPD_YMD	, UPD_HM";
            sqlstr += "				  , OFFICE_CD	  	";
            sqlstr += "				  , MNGT_NO		  		";
            sqlstr += "				  , SUBJECT		  		";
            sqlstr += "				  , REQ_EMAIL			";
            sqlstr += "						)";
            sqlstr += "				VALUES (";
            sqlstr += "					'" + dr["PIC_CD"].ToString() + "'	, TO_CHAR(SYSDATE,'YYYYMMDD'),	TO_CHAR(SYSDATE,'HH24MI')";
            sqlstr += "				  ,'" + dr["PIC_CD"].ToString() + "'	, TO_CHAR(SYSDATE,'YYYYMMDD'),	TO_CHAR(SYSDATE,'HH24MI')";
            sqlstr += "				  , '" + dr["OFFICE_CD"].ToString() + "'	  		";
            sqlstr += "				  , '" + KeyNo + "'		  	";
            sqlstr += "				  , '온라인 회원가입 승인요청합니다.'		  	";
            sqlstr += "				  , '" + dr["PIC_EMAIL"].ToString() + "'			";
            sqlstr += "						);";
            sqlstr += "		END;";

            return sqlstr;
        }

        public string SearchCrn_Query(DataRow dr)
        {
            sqlstr = "";
            sqlstr += "SELECT CUST_LOC_NM AS CUST_CD, CUST_CD AS OFFICE_CD";            
            sqlstr += "     FROM CRM_CUST_MST@DL_CRM";
            sqlstr += "     WHERE 1 = 1";
            sqlstr += "     AND CUSTOMS_CD = UPPER('" + dr["CODE"].ToString().Trim() + "')";
            sqlstr += "     AND CRN = '" + dr["CRN"].ToString().Trim() + "'";

            return sqlstr;
        }

        public string SearchOnline(DataRow dr)
        {
            string sqlstr = "";

            sqlstr += "SELECT ROWNUM AS BOARD_ID, A.* ";
            sqlstr += "FROM ( ";
            sqlstr += "    SELECT A.MNGT_NO, ";
            sqlstr += "           B.SUBJECT AS BOARD_TITLE, ";
            sqlstr += "           REQ_YMD AS INS_DATE, ";
            sqlstr += "           CMPT_YMD AS ANS_DATE, ";
            sqlstr += "           REQ_SVC AS REQ_SVC, ";
            sqlstr += "           REQ_HM AS REQ_HM, ";//시간
            sqlstr += "           PROC_TYPE AS PROC_TYPE, ";
            sqlstr += "           (CASE WHEN A.PROC_TYPE IN ('1', '2') THEN '완료' ";
            sqlstr += "                 WHEN A.PROC_TYPE = '8' THEN '취소' ";
            sqlstr += "                 WHEN A.PROC_TYPE IN ('7', '9', 'Z') THEN '처리중' ";
            sqlstr += "                 WHEN A.WORK_USR IS NOT NULL THEN '접수' ";
            sqlstr += "                 ELSE '요청' END) AS G_STATUS ";
            sqlstr += "    FROM CRM_AS_MST@DL_CRM A ";
            sqlstr += "    INNER JOIN CRM_AS_CUST@DL_CRM B ON B.MNGT_NO = A.MNGT_NO ";
            sqlstr += "    WHERE SYS_ID IN ('ELVIS', 'WEB') AND A.ORGN_MNGT_NO IS NULL ";
            sqlstr += "    AND A.CUST_CD = '" + dr["UserCust"] + "' ";

            if(dr["frdt"].ToString() != "")
            {
                if(dr["ymd"].ToString() == "A")
                {
                    sqlstr += " AND REQ_YMD >= '" + dr["frdt"] + "'";
                }
                else
                {
                    sqlstr += " AND CMPT_YMD >= '" + dr["frdt"] + "'";
                }
            }
            if (dr["todt"].ToString() != "")
            {
                if (dr["ymd"].ToString() == "A")
                {
                    sqlstr += " AND REQ_YMD <= '" + dr["todt"] + "'";
                }
                else
                {
                    sqlstr += " AND CMPT_YMD <= '" + dr["todt"] + "'";
                }
            }

            if (dr["status"].ToString() != "A")
            {
                sqlstr += "AND (CASE WHEN A.PROC_TYPE IN ('1', '2') THEN 'F' ";
                sqlstr += "      WHEN A.PROC_TYPE = '8' THEN 'C' ";
                sqlstr += "      WHEN A.PROC_TYPE IN ('7', '9', 'Z') THEN 'P' ";
                sqlstr += "      WHEN A.WORK_USR IS NOT NULL THEN 'V' ";
                sqlstr += "      ELSE 'R' END) = '" + dr["status"].ToString().Trim() + "' ";
            }

            if (dr["L_title"].ToString() == "A")
            {
                sqlstr += "AND B.SUBJECT LIKE '%" + dr["TITLE"].ToString() + "%' ";
            }
            else if (dr["L_title"].ToString() == "B")
            {
                sqlstr += "AND A.CONTENT LIKE '%" + dr["TITLE"].ToString() + "%' ";
            }
            else
            {
                sqlstr += "AND A.CMPT_RMK LIKE '%" + dr["TITLE"].ToString() + "%' ";
            }
            sqlstr += "AND REQ_SVC IN(";
            sqlstr += "     SELECT COMN_CD";
            sqlstr += "     FROM MDM_COM_CODE@DL_CRM C";
            sqlstr += "     WHERE GRP_CD = 'T02' AND C.OPT_ITEM3 = 'Y'";
            if (dr["REQ"].ToString() != "") {
                sqlstr += "     AND COMN_CD = '" + dr["REQ"].ToString()+ "'";
            }
            sqlstr += ")";
            sqlstr += "ORDER BY INS_DATE DESC, REQ_HM DESC ";
            sqlstr += ") A ORDER BY ROWNUM ";

            return sqlstr;
        }

        public string SearchDetail(DataRow dr)
        {
            string sqlstr = "";

            sqlstr += " SELECT REQ_USR AS OP_CD, ";
            sqlstr += " TEL_NO AS TEL, ";
            sqlstr += " SUBJECT, ";
            sqlstr += " CONTENT AS CONTENTS, ";
            sqlstr += " REQ_EMAIL AS EMAIL, ";
            sqlstr += " CMPT_RMK AS ANSWER, ";
            sqlstr += " A.REQ_SVC AS REQ_SVC, ";
            sqlstr += " A.REQ_SVC2 AS REQ_SVC2, ";
            sqlstr += " A.MNGT_NO, ";
            sqlstr += " A.PROC_TYPE, ";
            sqlstr += " (CASE ";
            sqlstr += "     WHEN A.PROC_TYPE IN('1', '2') THEN 'F' ";
            sqlstr += "     WHEN A.PROC_TYPE = '8' THEN 'C' ";
            sqlstr += "     WHEN A.PROC_TYPE IN ('7', '9', 'Z') THEN 'P' ";
            sqlstr += "     WHEN A.WORK_USR IS NOT NULL THEN 'V' ";
            sqlstr += "     ELSE 'R' ";
            sqlstr += " END) AS G_STATUS "; // CASE 문을 SELECT 절의 마지막으로 이동
            sqlstr += " FROM CRM_AS_MST@DL_CRM A ";
            sqlstr += " INNER JOIN CRM_AS_CUST@DL_CRM B ON B.MNGT_NO = A.MNGT_NO ";
            sqlstr += " WHERE A.MNGT_NO = '" + dr["MNGT_NO"].ToString().Trim() + "' ";
            sqlstr += " AND A.CUST_CD = '" + dr["UserCust"] + "' ";


            //if (dr["G_STATUS"].ToString() != "A") 
            //{
            //    sqlstr += "AND (CASE WHEN A.PROC_TYPE IN ('1', '2') THEN 'F' ";
            //    sqlstr += "      WHEN A.PROC_TYPE = '8' THEN 'C' ";
            //    sqlstr += "      WHEN A.PROC_TYPE IN ('7', '9', 'Z') THEN 'P' ";
            //    sqlstr += "      WHEN A.WORK_USR IS NOT NULL THEN 'V' ";
            //    sqlstr += "      ELSE 'R' END) = '" + dr["G_STATUS"].ToString().Trim() + "' ";
            //}

            return sqlstr;
        }

        public string SearchFile(DataRow dr)
        {
            string sqlstr = "";

            sqlstr += " SELECT A.SEQ											  ";
            sqlstr += "		, A.MNGT_NO											  ";
            sqlstr += "		, A.FILE_NM											  ";
            sqlstr += "		, A.FILE_PATH											  ";
            sqlstr += "		, A.FORM_ID											  ";
            sqlstr += "      , ROUND(A.FILE_SIZE/1024,0) AS FILE_SIZE			  ";
            sqlstr += " FROM COM_DOC_MST@DL_CRM A										  ";
            sqlstr += " WHERE A.MNGT_NO = '" + dr["MNGT_NO"].ToString() + "'           ";
            sqlstr += " AND A.MNGT_NO IS NOT NULL AND (A.SEQ <> '0' OR A.SEQ IS NOT NULL) ";
            sqlstr += " ORDER BY A.SEQ											  ";

            return sqlstr;
        }

        public string SearchList()
        {
            string sqlstr = "";

            sqlstr += " SELECT A.CODE ";
            sqlstr += " ,DECODE(A.LOC_NM,NULL,A.NAME,A.LOC_NM) AS NAME ";
            sqlstr += " FROM (SELECT COMN_CD AS CODE, CD_NM AS NAME ";
            sqlstr += " , CD_NM AS LOC_NM ";
            sqlstr += " , SORT ";
            sqlstr += " , OPT_ITEM1 ";
            sqlstr += " FROM MDM_COM_CODE@DL_CRM ";
            sqlstr += " WHERE GRP_CD = 'T02' ";
            sqlstr += " AND USE_YN = 'Y' ";
            sqlstr += " AND OPT_ITEM3 LIKE '%Y%' ";
            sqlstr += " ) A ";
            sqlstr += " ORDER BY A.SORT, A.CODE";

            return sqlstr;
        }

        public string SearchList2(DataRow dr)
        {
            string sqlstr = "";

            sqlstr += " SELECT A.CODE ";
            sqlstr += " ,DECODE(A.LOC_NM,NULL,A.NAME,A.LOC_NM) AS NAME ";
            sqlstr += " FROM (SELECT COMN_CD AS CODE, CD_NM AS NAME ";
            sqlstr += " , CD_NM AS LOC_NM ";
            sqlstr += " , SORT ";
            sqlstr += " , OPT_ITEM1 ";
            sqlstr += " FROM MDM_COM_CODE@DL_CRM ";
            sqlstr += " WHERE GRP_CD = 'T10' ";
            sqlstr += " AND USE_YN = 'Y' ";
            sqlstr += " ) A ";
            sqlstr += " WHERE 1= 1 ";
            if(dr["REQ_SVC"].ToString() != "")
            {
                sqlstr += " AND OPT_ITEM1 LIKE '%" + dr["REQ_SVC"].ToString() + "%' ";
            }
            else
            {
                sqlstr += " AND OPT_ITEM1  LIKE '%" + dr["REQ_SVC"].ToString() + "%' ";
            }
         
            sqlstr += " ORDER BY A.SORT, A.CODE";

            return sqlstr;
        }


        public string CheckMNGT(string keyno)
        {
            string sqlstr = "";

            sqlstr += "SELECT A.MNGT_NO, ";
            sqlstr += "       A.REQ_YMD, ";
            sqlstr += "       A.CUST_CD, ";
            sqlstr += "       A.CONTENT, ";
            sqlstr += "       A.REQ_USR, ";
            sqlstr += "       A.TEL_NO, ";
            sqlstr += "       B.* ";
            sqlstr += "FROM CRM_AS_MST@DL_CRM A ";
            sqlstr += "INNER JOIN CRM_AS_CUST@DL_CRM B ";
            sqlstr += "ON A.MNGT_NO = B.MNGT_NO ";
            sqlstr += "WHERE A.MNGT_NO = '" + keyno + "'";

            return sqlstr;
        }

        public string SaveOnline(DataRow dr)
        {
            string sqlstr = "";

            sqlstr += "BEGIN ";
            sqlstr += "MERGE INTO CRM_AS_MST@DL_CRM D ";
            sqlstr += "USING (SELECT '" + dr["MNGT_NO"].ToString() + "' AS MNGT_NO, ";
            sqlstr += "'" + dr["PICID"] + "' AS USR_ID, ";
            sqlstr += "'WEB' AS SYS_ID, ";
            sqlstr += "'OnlineHelp' AS FORM_ID, ";
            sqlstr += "TO_CHAR(SYSDATE,'YYYYMMDD') AS USR_YMD, ";
            sqlstr += "TO_CHAR(SYSDATE,'HH24MISS') AS USR_HM ";
            sqlstr += "FROM DUAL) S ";
            sqlstr += "ON (D.MNGT_NO = S.MNGT_NO) ";
            sqlstr += "WHEN MATCHED THEN ";
            sqlstr += "UPDATE SET D.UPD_USR = '" + dr["PICNM"] + "', ";
            sqlstr += "D.UPD_YMD = S.USR_YMD, ";
            sqlstr += "D.UPD_HM = S.USR_HM, ";
            sqlstr += "D.PROC_TYPE = '0', ";
            sqlstr += "D.CONTENT = '" + dr["CONTENT"] + "', ";
            sqlstr += "D.CUST_CD = '" + dr["UserCust"] + "', ";
            sqlstr += "D.REQ_USR = '" + dr["PICNM"] + "', ";
            sqlstr += "D.TEL_NO = '" + dr["TEL"] + "', ";
            sqlstr += "D.REQ_SVC = '" + dr["REQ_SVC"].ToString() + "', ";
            sqlstr += "D.REQ_SVC2 = '" + dr["REQ_SVC2"].ToString() + "' ";
            sqlstr += "WHEN NOT MATCHED THEN ";
            sqlstr += "INSERT (D.INS_USR, D.INS_YMD, D.INS_HM, ";
            sqlstr += "D.UPD_USR, D.UPD_YMD, D.UPD_HM, ";
            sqlstr += "D.REQ_YMD, D.REQ_HM, ";
            sqlstr += "D.SYS_ID, D.FORM_ID, ";
            sqlstr += "D.MNGT_NO, D.CUST_CD, ";
            sqlstr += "D.CONTENT, D.REQ_USR, ";
            sqlstr += "D.TEL_NO, D.REQ_SVC, ";
            sqlstr += "D.REQ_SVC2, D.UPPER_MNGT_NO) ";
            sqlstr += "VALUES ('', S.USR_YMD, S.USR_HM, ";
            sqlstr += "S.USR_ID, S.USR_YMD, S.USR_HM, ";
            sqlstr += "S.USR_YMD, S.USR_HM, ";
            sqlstr += "S.SYS_ID, S.FORM_ID, ";
            sqlstr += "'" + dr["MNGT_NO"].ToString() + "', ";
            sqlstr += "'" + dr["UserCust"] + "', ";
            sqlstr += "'" + dr["CONTENT"] + "', ";
            sqlstr += "'" + dr["PICNM"] + "', ";
            sqlstr += "'" + dr["TEL"].ToString() + "', ";
            sqlstr += "'" + dr["REQ_SVC"].ToString() + "', ";
            sqlstr += "'" + dr["REQ_SVC2"].ToString() + "', ";
            sqlstr += "'" + dr["MNGT_NO"].ToString() + "');";
            sqlstr += "MERGE INTO CRM_AS_CUST@DL_CRM D ";
            sqlstr += "USING (SELECT '" + dr["UserCust"] + "' AS OFFICE_CD, ";
            sqlstr += "'" + dr["MNGT_NO"].ToString() + "' AS MNGT_NO, ";
            sqlstr += "'" + dr["PICID"] + "' AS USR_ID, ";
            sqlstr += "TO_CHAR(SYSDATE,'YYYYMMDD') AS USR_YMD, ";
            sqlstr += "TO_CHAR(SYSDATE,'HH24MISS') AS USR_HM ";
            sqlstr += "FROM DUAL) S ";
            sqlstr += "ON (D.OFFICE_CD = S.OFFICE_CD AND D.MNGT_NO = S.MNGT_NO) ";
            sqlstr += "WHEN MATCHED THEN ";
            sqlstr += "UPDATE SET D.UPD_USR = '" + dr["PICNM"] + "', ";
            sqlstr += "D.UPD_YMD = S.USR_YMD, ";
            sqlstr += "D.UPD_HM = S.USR_HM, ";
            sqlstr += "D.SUBJECT = '" + dr["SUBJECT"].ToString() + "', ";
            sqlstr += "D.REQ_EMAIL= '" + dr["EMAIL"].ToString() + "' ";
            sqlstr += "WHEN NOT MATCHED THEN ";
            sqlstr += "INSERT (D.INS_USR, D.INS_YMD, D.INS_HM, ";
            sqlstr += "D.UPD_USR, D.UPD_YMD, D.UPD_HM, ";
            sqlstr += "D.OFFICE_CD, D.MNGT_NO, ";
            sqlstr += "D.SUBJECT, D.REQ_EMAIL) ";
            sqlstr += "VALUES ('', S.USR_YMD, S.USR_HM, ";
            sqlstr += "S.USR_ID, S.USR_YMD, S.USR_HM, ";
            sqlstr += "'" + dr["UserCust"] + "', ";
            sqlstr += "'" + dr["MNGT_NO"].ToString() + "', ";
            sqlstr += "'" + dr["SUBJECT"].ToString() + "', ";
            sqlstr += "'" + dr["EMAIL"].ToString() + "'); ";
            sqlstr += "END; ";

            return sqlstr;
        }

        //public string SaveFile(DataRow dr)
        //{
        //    sqlstr = "";
        //    sqlstr += "		INSERT INTO COM_DOC_MST@DL_CRM( ";
        //    sqlstr += "			                     MNGT_NO";
        //    sqlstr += "			                   , SEQ";
        //    sqlstr += "				               , FILE_NM";
        //    sqlstr += "							   , FILE_PATH";
        //    sqlstr += "							   , FORM_ID";
        //    sqlstr += "							   , ONLINE_FILE_PATH";
        //    sqlstr += "						       , INS_USR";
        //    sqlstr += "							   , INS_YMD";
        //    sqlstr += "							   , INS_HM)";
        //    sqlstr += "						VALUES ( '" + dr["MNGT_NO"].ToString() + "'";
        //    sqlstr += "						       , (SELECT NVL (MAX(SEQ), 0) + 1 FROM COM_DOC_MST@DL_CRM WHERE MNGT_NO = '" + dr["MNGT_NO"].ToString() + "')";
        //    sqlstr += "					           , '" + dr["FILE_NM"].ToString() + "'";
        //    sqlstr += "					           , '" + SavePath + "'";
        //    sqlstr += "					           , 'OnlineHelp'";
        //    sqlstr += "					           , '" + dr["FILE_ATTACH"].ToString() + "'";
        //    sqlstr += "					           , ''";
        //    sqlstr += "					           , TO_CHAR(SYSDATE,'YYYYMMDD')";
        //    sqlstr += "					           , TO_CHAR(SYSDATE,'HH24MISS'))";

        //    return sqlstr;
        //}

        public string fnSetOnlineRS_MST_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " INSERT              ";
            sqlstr += "     INTO CRM_AS_MST@DL_CRM ";
            sqlstr += "     (MNGT_NO        ";
            sqlstr += "     , CUST_CD       ";
            sqlstr += "     , CONTENT       ";
            sqlstr += "     , TEL_NO        ";
            sqlstr += "     , REQ_SVC       ";
            sqlstr += "     , REQ_SVC2      ";
            sqlstr += "     , UPPER_MNGT_NO ";
            sqlstr += "     , FORM_ID       ";
            sqlstr += "     , SYS_ID        ";
            sqlstr += "     , REQ_USR       ";
            sqlstr += "     , REQ_YMD       ";
            sqlstr += "     , REQ_HM        ";
            sqlstr += "     , INS_USR       ";
            sqlstr += "     , INS_YMD       ";
            sqlstr += "     , INS_HM        ";
            sqlstr += "     , UPD_USR       ";
            sqlstr += "     , UPD_YMD       ";
            sqlstr += "     , UPD_HM)       ";
            sqlstr += "     VALUES          ";
            sqlstr += " 	('" + dr["MNGT_NO"].ToString() + "'   ";
            sqlstr += " 	,'" + dr["CUST_NM"].ToString() + "'   ";
            sqlstr += " 	,'" + dr["CONTENT"].ToString() + "'   ";
            sqlstr += " 	,'" + dr["TEL_NO"].ToString() + "'   ";
            sqlstr += " 	,'" + dr["REQ_SVC"].ToString() + "'   ";
            sqlstr += " 	,'" + dr["REQ_SVC2"].ToString() + "'   ";
            sqlstr += " 	,'" + dr["UPPER_MNGT_NO"].ToString() + "'   ";
            sqlstr += " 	,'" + dr["FORM_ID"].ToString() + "'   ";
            sqlstr += " 	,'" + dr["SYS_ID"].ToString() + "'   ";
            sqlstr += " 	,'" + dr["REQ_USR"].ToString() + "'   ";
            sqlstr += " 	,'" + dr["REQ_YMD"].ToString() + "'   ";
            sqlstr += " 	,'" + dr["REQ_HM"].ToString() + "'   ";
            sqlstr += " 	,'" + dr["INS_USR"].ToString() + "'   ";
            sqlstr += " 	,TO_CHAR(SYSDATE, 'YYYYMMDD')   ";
            sqlstr += " 	,TO_CHAR(SYSDATE, 'HHMI')   ";
            sqlstr += " 	,'" + dr["UPD_USR"].ToString() + "'   ";
            sqlstr += " 	,TO_CHAR(SYSDATE, 'YYYYMMDD')   ";
            sqlstr += " 	,TO_CHAR(SYSDATE, 'HHMI'))" + "  ";

            return sqlstr;
        }

        public string fnSetOnlineRS_Cust_Query(DataRow dr)
        {
            sqlstr = "";

            //여기는 Office_CD PK라서 지금 당장 못함. //이상한 코드로 일단 넣어두자
            sqlstr += " 	INSERT                                                                                     ";
            sqlstr += " 	    INTO CRM_AS_CUST@DL_CRM                                                                       ";
            sqlstr += " 	    (MNGT_NO                                                                               ";
            sqlstr += " 	    , OFFICE_CD                                                                            ";  //이거는 추후 지워야됩니다.
            sqlstr += " 	    , SUBJECT                                                                              ";
            sqlstr += " 	    , REQ_EMAIL                                                                            ";
            sqlstr += " 	    , INS_USR                                                                              ";
            sqlstr += " 	    , INS_YMD                                                                              ";
            sqlstr += " 	    , INS_HM                                                                               ";
            sqlstr += " 	    , UPD_USR                                                                              ";
            sqlstr += " 	    , UPD_YMD                                                                              ";
            sqlstr += " 	    , UPD_HM)                                                                              ";
            sqlstr += " 	    VALUES                                                                                 ";
            sqlstr += " 	    ('" + dr["MNGT_NO"].ToString() + "'                                                        ";
            sqlstr += " 		,''	                                                      				   ";
            sqlstr += " 		,'" + dr["SUBJECT"].ToString() + "'	                                                       ";
            sqlstr += " 		,'" + dr["REQ_EMAIL"].ToString() + "'	                                                   ";
            sqlstr += " 		,'" + dr["INS_USR"].ToString() + "'	                                                       ";
            sqlstr += " 	,TO_CHAR(SYSDATE, 'YYYYMMDD')   ";
            sqlstr += " 	,TO_CHAR(SYSDATE, 'HHMI')  ";
            sqlstr += " 		,'" + dr["UPD_USR"].ToString() + "'	                                                       ";
            sqlstr += " 	,TO_CHAR(SYSDATE, 'YYYYMMDD')   ";
            sqlstr += " 	,TO_CHAR(SYSDATE, 'HHMI'))  ";

            return sqlstr;
        }

        public string fnSetOnlineRS_Files_Query(DataRow dr)
        {
            sqlstr = "";

            sqlstr += " 	INSERT                                                                                     ";
            sqlstr += " 	    INTO COM_DOC_MST@DL_CRM                                                                       ";
            sqlstr += " 	    (MNGT_NO                                                                               ";
            sqlstr += " 	    , SEQ                                                                                  ";
            sqlstr += " 	    , FILE_NM                                                                              ";
            sqlstr += " 	    , FILE_PATH                                                                            ";
            sqlstr += " 	    , FILE_SIZE                                                                            ";
            sqlstr += " 	    , FORM_ID                                                                              ";
            sqlstr += " 	    , INS_USR                                                                              ";
            sqlstr += " 	    , INS_YMD                                                                              ";
            sqlstr += " 	    , INS_HM                                                                               ";
            sqlstr += " 	    , UPD_USR                                                                              ";
            sqlstr += " 	    , UPD_YMD                                                                              ";
            sqlstr += " 	    , UPD_HM)                                                                              ";
            sqlstr += " 	    VALUES                                                                                 ";
            sqlstr += " 	    ('" + dr["MNGT_NO"].ToString() + "'                                                                   ";
            sqlstr += " 		,(SELECT NVL (MAX(SEQ), 0) + 1 FROM COM_DOC_MST@DL_CRM WHERE MNGT_NO = '" + dr["MNGT_NO"].ToString() + "') ";
            sqlstr += " 		,'" + dr["FILE_NM"].ToString() + "'	                                                               ";
            sqlstr += " 		,'" + dr["FILE_PATH"].ToString() + "'	                                                               ";
            sqlstr += " 		,'" + dr["FILE_SIZE"].ToString() + "'	                                                               ";
            sqlstr += " 		,'" + dr["FORM_ID"].ToString() + "'	                                                               ";
            sqlstr += " 		,'" + dr["INS_USR"].ToString() + "'	                                                       ";
            sqlstr += " 	,TO_CHAR(SYSDATE, 'YYYYMMDD')   ";
            sqlstr += " 	,TO_CHAR(SYSDATE, 'HHMI')   ";
            sqlstr += " 		,'" + dr["UPD_USR"].ToString() + "'	                                                       ";
            sqlstr += " 	,TO_CHAR(SYSDATE, 'YYYYMMDD')  ";
            sqlstr += " 	,TO_CHAR(SYSDATE, 'HHMI'))  ";

            return sqlstr;
        }

        public string SelfOnline(DataRow dr)
        {

            string sqlstr = "";

            sqlstr += "UPDATE";
            sqlstr += " CRM_AS_MST@DL_CRM";
            sqlstr += " SET PROC_TYPE = '1'";
            sqlstr += " ,CONTENT = '" + dr["CONTENT"].ToString() + "'";
            sqlstr += " WHERE";
            sqlstr += " MNGT_NO = '" + dr["MNGT_NO"].ToString() + "'";
            sqlstr += "";

            return sqlstr;
        }

        public string FaqList(DataRow dr)
        {
            string sqlstr = "";
            sqlstr += " SELECT NODE_ID ";
            sqlstr += "    , NODE_TYPE ";
            sqlstr += "    , PARENT_NODE_ID ";
            sqlstr += "    , TITLE ";
            sqlstr += "    , DISPLAY_SEQ ";
            sqlstr += "    , LANG_CD ";
            sqlstr += "    , INS_USR ";
            sqlstr += "    , INS_DATE ";
            sqlstr += "    , UPD_USR ";
            sqlstr += "    , UPD_DATE ";
            sqlstr += "    , (SELECT TITLE FROM CRM_FAQ_MST@DL_CRM  S WHERE 1 = 1 AND NODE_TYPE = 'G' AND S.NODE_ID = X.PARENT_NODE_ID)  AS  GRP_NM ";
            sqlstr += " FROM CRM_FAQ_MST@DL_CRM X ";
            sqlstr += " WHERE 1 = 1 ";            
            sqlstr += " AND NODE_TYPE = 'N' ";
            sqlstr += " ORDER BY PARENT_NODE_ID, NODE_ID, DISPLAY_SEQ ";

            return sqlstr;
        }

        public string SearchFaq(DataRow dr)
        {
            string sqlstr = "";
            sqlstr += " SELECT NODE_ID ";
            sqlstr += "    , NODE_TYPE ";
            sqlstr += "    , PARENT_NODE_ID ";
            sqlstr += "    , TITLE ";
            sqlstr += "    , DISPLAY_SEQ ";
            sqlstr += "    , LANG_CD ";
            sqlstr += "    , INS_USR ";
            sqlstr += "    , INS_DATE ";
            sqlstr += "    , UPD_USR ";
            sqlstr += "    , UPD_DATE ";
            sqlstr += "    , (SELECT TITLE FROM CRM_FAQ_MST@DL_CRM  S WHERE 1 = 1 AND NODE_TYPE = 'G' AND S.NODE_ID = X.PARENT_NODE_ID)  AS  GRP_NM ";
            sqlstr += " FROM CRM_FAQ_MST@DL_CRM X ";
            sqlstr += " WHERE 1 = 1 ";
            sqlstr += " AND PARENT_NODE_ID = '" + dr["title"].ToString() + "'";
            sqlstr += " AND TITLE LIKE '%" + dr["content"].ToString() + "%'";
            sqlstr += " AND NODE_TYPE = 'N' ";
            sqlstr += " ORDER BY PARENT_NODE_ID, DISPLAY_SEQ, NODE_ID ";

            return sqlstr;
        }

        public string FaqView(DataRow dr)
        {
            string sqlstr = "";
            sqlstr += " SELECT  'NOW' AS FLAG ";
            sqlstr += "     ,NODE_ID";
            sqlstr += "     , PARENT_NODE_ID";
            sqlstr += "     , TITLE";
            sqlstr += "     , INS_DATE";
            sqlstr += "     , UPD_USR";
            //sqlstr += "     , TO_CHAR(CONTENT) ";
            sqlstr += "     , CONTENT";
            sqlstr += "      ,(SELECT TITLE";
            sqlstr += "         FROM CRM_FAQ_MST @DL_CRM S";
            sqlstr += "         WHERE 1 = 1 AND NODE_TYPE = 'G' AND S.NODE_ID = X.PARENT_NODE_ID)";
            sqlstr += "         AS GRP_NM";
            //sqlstr += " FROM CRM_FAQ_MST@DL_CRM ";
            sqlstr += " FROM TEMP_CRM_FAQ_MST X ";
            sqlstr += " WHERE ";
            sqlstr += " NODE_ID = '" + dr["NODE_ID"] + "'";
            sqlstr += " AND PARENT_NODE_ID = '" + dr["PARENT_NODE_ID"] + "'";

            sqlstr += "UNION ALL ";

            sqlstr += " SELECT  'PREV' AS FLAG ";
            sqlstr += "    , NODE_ID";
            sqlstr += "     , PARENT_NODE_ID";
            sqlstr += "     , TITLE";
            sqlstr += "     , INS_DATE";
            sqlstr += "     , UPD_USR";
            sqlstr += "     , (SELECT TO_CLOB('') FROM DUAL) AS CONTENT ";
            sqlstr += "      ,(SELECT TITLE";
            sqlstr += "         FROM CRM_FAQ_MST @DL_CRM S";
            sqlstr += "         WHERE 1 = 1 AND NODE_TYPE = 'G' AND S.NODE_ID = X.PARENT_NODE_ID)";
            sqlstr += "         AS GRP_NM";
            //sqlstr += " FROM CRM_FAQ_MST@DL_CRM ";
            sqlstr += " FROM CRM_FAQ_MST @DL_CRM X ";
            sqlstr += " WHERE ";
            sqlstr += " NODE_ID = (SELECT MAX(NODE_ID) FROM CRM_FAQ_MST @DL_CRM WHERE NODE_ID < '" + dr["NODE_ID"] + "' AND PARENT_NODE_ID = '" + dr["PARENT_NODE_ID"] + "' ) ";
            sqlstr += " AND PARENT_NODE_ID = '" + dr["PARENT_NODE_ID"] + "'";

            sqlstr += " UNION ALL ";

            sqlstr += " SELECT  'NEXT' AS FLAG ";
            sqlstr += "    , NODE_ID";
            sqlstr += "     , PARENT_NODE_ID";
            sqlstr += "     , TITLE";
            sqlstr += "     , INS_DATE";
            sqlstr += "     , UPD_USR";
            sqlstr += "     , (SELECT TO_CLOB('') FROM DUAL) AS CONTENT ";
            sqlstr += "      ,(SELECT TITLE";
            sqlstr += "         FROM CRM_FAQ_MST @DL_CRM S";
            sqlstr += "         WHERE 1 = 1 AND NODE_TYPE = 'G' AND S.NODE_ID = X.PARENT_NODE_ID)";
            sqlstr += "         AS GRP_NM";
            //sqlstr += " FROM CRM_FAQ_MST@DL_CRM ";
            sqlstr += " FROM CRM_FAQ_MST @DL_CRM X ";
            sqlstr += " WHERE ";
            sqlstr += " NODE_ID = (SELECT MIN(NODE_ID) FROM CRM_FAQ_MST @DL_CRM WHERE NODE_ID > '" + dr["NODE_ID"] + "' AND PARENT_NODE_ID = '" + dr["PARENT_NODE_ID"] + "' ) ";
            sqlstr += "  AND PARENT_NODE_ID = '" + dr["PARENT_NODE_ID"] + "'";


            return sqlstr;
        }

        public string DUMPFAQ(DataRow dr) 
        {
            string sqlstr = "";
            sqlstr += " INSERT INTO  TEMP_CRM_FAQ_MST ";
            sqlstr += " SELECT * FROM CRM_FAQ_MST@DL_CRM ";
            sqlstr += " WHERE NODE_ID = '" + dr["NODE_ID"] + "' ";

            return sqlstr;
        }

        public string DelDUMPFAQ(DataRow dr)
        {
            string sqlstr = "";
            sqlstr += "DELETE FROM TEMP_CRM_FAQ_MST ";
            sqlstr += " WHERE NODE_ID = '" + dr["NODE_ID"] + "' ";

            return sqlstr;
        }

        public string NoticeList(DataRow dr)
        {
            string sqlstr = "";

            sqlstr += " SELECT * FROM (                                                               ";
            sqlstr += "     SELECT                                                                    ";
            sqlstr += "         ROWNUM AS RNUM                                                        ";
            sqlstr += "          , FLOOR ( (ROWNUM - 1) / 10 + 1) AS PAGE                             ";
            sqlstr += "          , COUNT (*) OVER () AS TOTCNT                                        ";
            sqlstr += "          ,CASE WHEN R.NOTICE_YN = 'Y'                                         ";
            sqlstr += "                 THEN '[공지]'                                                   ";
            sqlstr += "                 ELSE BOARD_ID                                                 ";
            sqlstr += "            END AS BOARD_SEQ                                                   ";
            sqlstr += "          , R.BOARD_ID                                                         ";
            sqlstr += "          , R.TITLE                                                            ";
            sqlstr += "			, DECODE (R.BOARD_TYPE , 'N' ,'일반'                                 ";
            sqlstr += "							       , 'U' ,'업데이트'							";
            sqlstr += "							       , 'P' ,'팝업공지'							";
            sqlstr += "							       , 'D' ,'일일팝업공지'						  ";
            sqlstr += "							       , 'E' ,'이벤트공지') AS BOARD_TYPE			 ";
            sqlstr += "          , R.USR_NM                                                           ";
            sqlstr += "          , R.INS_YMD                                                          ";
            sqlstr += "          , R.STRT_YMD                                                        ";
            sqlstr += "          , R.EXP_YMD                                                        ";
            sqlstr += "          , R.EDIT                                                             ";
            sqlstr += "          , R.NOTICE_YN                                                        ";
            sqlstr += "       FROM (                                                                  ";
            sqlstr += "         SELECT A.BOARD_ID                                                     ";
            sqlstr += "              , A.TITLE                                                        ";
            sqlstr += "              , B.LOC_NM AS USR_NM                                             ";
            sqlstr += "              , A.BOARD_BODY                                                   ";
            sqlstr += "              , TO_CHAR(TO_DATE(A.INS_YMD,'yyyyMMdd'),'yyyy-MM-dd') AS INS_YMD ";
            sqlstr += "              , 'Edit' AS EDIT                                                 ";
            sqlstr += "              , CASE WHEN A.EXP_YMD < UFN_DATE_FORMAT@DL_CRM('DATE')                  ";
            sqlstr += "                     THEN 'N'                                                  ";
            sqlstr += "                     ELSE A.NOTICE_YN                                          ";
            sqlstr += "                END AS NOTICE_YN                                               ";
            sqlstr += "              , A.STRT_YMD													 ";
            sqlstr += "              , A.EXP_YMD														 ";
            sqlstr += "              , A.BOARD_TYPE													 ";
            sqlstr += "           FROM CRM_BOARD_MST@DL_CRM A                                                ";
            sqlstr += "              , CRM_USR_MST@DL_CRM B                                                  ";
            sqlstr += "           WHERE 1=1                                                           ";
            sqlstr += "           AND A.INS_USR = B.USR_ID                                            ";

            sqlstr += "           AND A.TITLE NOT LIKE '%송년회%'                                     ";

            if (dr["NOTICE_CS"].ToString() == "T")
            {
                sqlstr += "       AND A.TITLE LIKE '%" + dr["NOTICE_CS_RESULT"].ToString() + "%'                                            ";
            }
            else if (dr["NOTICE_CS"].ToString() == "C")
            {
                sqlstr += "       AND A.BOARD_BODY LIKE '%" + dr["NOTICE_CS_RESULT"].ToString() + "%'                         ";
            }
            else if (dr["NOTICE_CS"].ToString() == "")
            {
                sqlstr += "       AND ( 1 != 1                                            ";
                sqlstr += "       OR A.TITLE LIKE '%" + dr["NOTICE_CS_RESULT"].ToString() + "%'                                            ";
                sqlstr += "       OR B.LOC_NM LIKE '%" + dr["NOTICE_CS_RESULT"].ToString() + "%'                                            ";
                sqlstr += "       OR A.BOARD_BODY LIKE '%" + dr["NOTICE_CS_RESULT"].ToString() + "%'                          ";
                sqlstr += "       )                          ";
            }

            //sqlstr += "      ORDER BY  A.INS_YMD DESC ,A.NOTICE_YN									 ";
            sqlstr += "      ORDER BY  A.BOARD_ID DESC								 ";
            sqlstr += "           ) R                                                                 ";
            sqlstr += "      )                                                                        ";
            sqlstr += " WHERE 1 = 1                                       ";
            sqlstr += " ORDER BY RNUM ASC                                                               ";

            return sqlstr;
        }

        public string NoticeView(DataRow dr)
        {
            string sqlstr = "";

            sqlstr += " SELECT BRD.BOARD_ID                                                                                                                                 ";
            sqlstr += "      , USR.LOC_NM                                                                                                                                   ";
            sqlstr += "      , BRD.TITLE                                                                                                                                    ";
            sqlstr += "      , BRD.BOARD_BODY                                                                                                                               ";
            sqlstr += "      , TO_CHAR(TO_DATE(BRD.INS_YMD,'yyyyMMdd'),'yyyy-MM-dd') AS INS_YMD                                                                                       ";            
            sqlstr += "      , DECODE (BRD.BOARD_TYPE , 'N' ,'일반'                                                                                                           ";
            sqlstr += "                                             , 'U' ,'업데이트'                                                                                          ";
            sqlstr += "                                             , 'P' ,'팝업공지'                                                                                          ";
            sqlstr += "                                             , 'D' ,'일일팝업공지'                                                                                        ";
            sqlstr += "                                             , 'E' ,'이벤트공지') AS BOARD_TYPE                                                                          ";
            sqlstr += "      , BRD.STRT_YMD                                                                                                                                 ";
            sqlstr += "      , BRD.EXP_YMD                                                                                                                                  ";
            sqlstr += "      , BRD.LINK_URL                                                                                                                                 ";
            sqlstr += "      , BRD.NOTICE_YN                                                                                                                                ";
            sqlstr += "      , (SELECT max(A.BOARD_ID) KEEP(DENSE_RANK FIRST ORDER BY A.BOARD_ID DESC) FROM CRM_BOARD_MST@DL_CRM A WHERE A.BOARD_ID < '" + dr["NOTICE_BOARD_ID"].ToString() + "' AND A.TITLE NOT LIKE '%송년회%' ) AS PREV_BOARD_ID ";
            sqlstr += "      , (SELECT max(A.TITLE) KEEP(DENSE_RANK FIRST ORDER BY A.BOARD_ID DESC) FROM CRM_BOARD_MST@DL_CRM A WHERE A.BOARD_ID < '" + dr["NOTICE_BOARD_ID"].ToString() + "' AND A.TITLE NOT LIKE '%송년회%' ) AS PREV_TITLE       ";
            sqlstr += "      , (SELECT max(A.BOARD_ID) KEEP(DENSE_RANK FIRST ORDER BY A.BOARD_ID ASC) FROM CRM_BOARD_MST@DL_CRM A WHERE A.BOARD_ID > '" + dr["NOTICE_BOARD_ID"].ToString() + "' AND A.TITLE NOT LIKE '%송년회%' ) AS NEXT_BOARD_ID  ";
            sqlstr += "      , (SELECT max(A.TITLE) KEEP(DENSE_RANK FIRST ORDER BY A.BOARD_ID ASC) FROM CRM_BOARD_MST@DL_CRM A WHERE A.BOARD_ID > '" + dr["NOTICE_BOARD_ID"].ToString() + "' AND A.TITLE NOT LIKE '%송년회%' ) AS NEXT_TITLE        ";
            sqlstr += "   FROM TEMP_CRM_BOARD_MST BRD                                                                                                                            ";
            sqlstr += "   LEFT JOIN CRM_USR_MST@DL_CRM USR ON USR.USR_ID = BRD.INS_USR                                                                                             ";
            sqlstr += "   WHERE BRD.BOARD_ID = '" + dr["NOTICE_BOARD_ID"].ToString() + "'                                                                                                                   ";
            sqlstr += "   ORDER BY BRD.BOARD_ID DESC                                                                                                                        ";

            return sqlstr;
        }

        public string DUMPNOTICE(DataRow dr)
        {
            string sqlstr = "";
            sqlstr += " INSERT INTO TEMP_CRM_BOARD_MST ";
            sqlstr += " SELECT * FROM CRM_BOARD_MST@DL_CRM ";
            sqlstr += " WHERE BOARD_ID = '" + dr["NOTICE_BOARD_ID"] + "' ";

            return sqlstr;
        }
        public string DelDUMPNOTICE(DataRow dr)
        {
            string sqlstr = "";
            sqlstr += "DELETE FROM TEMP_CRM_BOARD_MST ";
            sqlstr += " WHERE BOARD_ID = '" + dr["NOTICE_BOARD_ID"] + "' ";

            return sqlstr;
        }

        //public string SearchElvis(DataRow dr)
        //{
        //    string sqlstr = "";

        //    sqlstr += "SELECT A.MNGT_NO ";
        //    sqlstr += "     , A.MANUAL_TYPE ";
        //    sqlstr += "     , A.REQ_SVC ";
        //    sqlstr += "     , A.SYS_ID ";
        //    sqlstr += "     , A.CONTENT AS TITLE";
        //    sqlstr += "     , A.RMK ";
        //    sqlstr += "     , C.FILE_PATH ";
        //    sqlstr += "     , C.FILE_NM ";
        //    sqlstr += "     , C.SEQ ";
        //    sqlstr += "     , (SELECT B.LOC_NM ";
        //    sqlstr += "         FROM CRM_USR_MST@DL_CRM B ";
        //    sqlstr += "         WHERE B.USR_ID = A.INS_USR) ";
        //    sqlstr += "         AS INS_USR ";
        //    sqlstr += ", TO_CHAR(TO_DATE(A.INS_YMD, 'YYYYMMDD'), 'YYYY-MM-DD') ";
        //    sqlstr += "||''|| ";
        //    sqlstr += "TO_CHAR(TO_DATE(A.INS_HM, 'HH24MISS'), 'HH24:MI:SS') ";
        //    sqlstr += "AS INS_YMD ";
        //    sqlstr += "       , (SELECT B.LOC_NM ";
        //    sqlstr += "         FROM CRM_USR_MST@DL_CRM B ";
        //    sqlstr += "         WHERE B.USR_ID = A.UPD_USR) ";
        //    sqlstr += "         AS UPD_USR ";
        //    sqlstr += "         , TO_CHAR (TO_DATE (A.UPD_YMD, 'YYYYMMDD'), 'YYYY-MM-DD') ";
        //    sqlstr += "         ||''|| ";
        //    sqlstr += "         TO_CHAR (TO_DATE (A.UPD_HM, 'HH24MISS'), 'HH24:MI:SS') ";
        //    sqlstr += "         AS UPD_YMD ";
        //    sqlstr += "FROM CRM_MANUAL_MST@DL_CRM A ";
        //    sqlstr += "LEFT JOIN COM_DOC_MST@DL_CRM C ON A.MNGT_NO = C.MNGT_NO ";
        //    sqlstr += "WHERE 1 = 1 AND A.SYS_ID = 'ELVIS' ";

        //    return sqlstr;
        //}

        //public string SearchElvis21(DataRow dr)
        //{
        //    string sqlstr = "";

        //    sqlstr += "SELECT A.MNGT_NO ";
        //    sqlstr += "     , A.MANUAL_TYPE ";
        //    sqlstr += "     , A.REQ_SVC ";
        //    sqlstr += "     , A.SYS_ID ";
        //    sqlstr += "     , A.CONTENT AS TITLE";
        //    sqlstr += "     , A.RMK ";
        //    sqlstr += "     , C.FILE_PATH ";
        //    sqlstr += "     , C.FILE_NM ";
        //    sqlstr += "     , C.SEQ ";
        //    sqlstr += "     , (SELECT B.LOC_NM ";
        //    sqlstr += "         FROM CRM_USR_MST@DL_CRM B ";
        //    sqlstr += "         WHERE B.USR_ID = A.INS_USR) ";
        //    sqlstr += "         AS INS_USR ";
        //    sqlstr += ", TO_CHAR(TO_DATE(A.INS_YMD, 'YYYYMMDD'), 'YYYY-MM-DD') ";
        //    sqlstr += "||''|| ";
        //    sqlstr += "TO_CHAR(TO_DATE(A.INS_HM, 'HH24MISS'), 'HH24:MI:SS') ";
        //    sqlstr += "AS INS_YMD ";
        //    sqlstr += "       , (SELECT B.LOC_NM ";
        //    sqlstr += "         FROM CRM_USR_MST@DL_CRM B ";
        //    sqlstr += "         WHERE B.USR_ID = A.UPD_USR) ";
        //    sqlstr += "         AS UPD_USR ";
        //    sqlstr += "         , TO_CHAR (TO_DATE (A.UPD_YMD, 'YYYYMMDD'), 'YYYY-MM-DD') ";
        //    sqlstr += "         ||''|| ";
        //    sqlstr += "         TO_CHAR (TO_DATE (A.UPD_HM, 'HH24MISS'), 'HH24:MI:SS') ";
        //    sqlstr += "         AS UPD_YMD ";
        //    sqlstr += "FROM CRM_MANUAL_MST@DL_CRM A ";
        //    sqlstr += "LEFT JOIN COM_DOC_MST@DL_CRM C ON A.MNGT_NO = C.MNGT_NO ";
        //    sqlstr += "WHERE 1 = 1 AND A.SYS_ID = 'ELVIS21' ";

        //    return sqlstr;
        //}

        public string AutoUserLogin(DataRow dr)
        {
            string sqlstr = "";
            
            sqlstr += "SELECT A.CUST_CD AS CUST_CD ";
            sqlstr += "     , A.CRN AS CRN ";
            sqlstr += "     , B.PIC_ID AS PIC_ID ";
            sqlstr += "     , B.PIC_NM AS PIC_NM ";
            sqlstr += "     , B.TEL_NO AS TEL_NO ";
            sqlstr += "     , B.EMAIL AS EMAIL ";
            sqlstr += "FROM CRM_CUST_MST@DL_CRM A ";
            sqlstr += "LEFT JOIN CRM_CUST_PIC@DL_CRM B ON A.CUST_CD = B.CUST_CD ";
            sqlstr += "WHERE A.CUSTOMS_CD = '" + dr["CUST_CD"].ToString().ToUpper() + "'";
            sqlstr += "     AND A.CRN = '" + dr["CRN"].ToString() + "'";
          
            return sqlstr;
        }
    }
    


}