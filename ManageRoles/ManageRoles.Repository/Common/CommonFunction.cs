using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace ManageRoles.Repository
{
    public class CommonFunction
    {
        public static string GetConnectionStringname() { return "DatabaseConnection"; }

        public Dictionary<string, string> GetColumns(string moduleName)
        {
            var colName = new Dictionary<string, string>();
            if (moduleName == module.ProductMaster.ToString())
            {
                colName.Add("ProductID", "ProductID");//ColumnName, DisplayColumn
                colName.Add("Buyername", "Buyername");
                colName.Add("BuyerOrderNumberName", "Buyer Order Number");               
                colName.Add("Processname", "Processname");
                colName.Add("Suppliername", "Suppliername");
                colName.Add("Productname", "Productname");             
                colName.Add("StartDate", "Start Date");
                colName.Add("SPCDate", "SPC Date");
                colName.Add("ArticleNumber", "Article Number");
                colName.Add("DesignNumber", "Design Number");
                colName.Add("Size", "Size");
                colName.Add("Colour", "Colour");
                colName.Add("CountConstruction", "Count Construction");
                colName.Add("Unit", "Unit");
                colName.Add("SetNote", "Set Note");
                colName.Add("ProductQty", "ProductQty");
                colName.Add("FinishedQty", "FinishedQty");
                colName.Add("BalanceQty", "BalanceQty");
                colName.Add("PlannedQty", "PlannedQty");
                colName.Add("Remarks", "Remarks");
                colName.Add("StoryName", "Story Name");
                colName.Add("Status", "Status");
                colName.Add("SKUStartDate", "SKU Start Date");
                colName.Add("SKUEndDate", "SKU Completed Date");
            }
            else if (moduleName == module.OPM.ToString())
            {
                colName.Add("OPM_ID", "OPM ID");//ColumnName, DisplayColumn
                colName.Add("BuyerReferenceName", "BuyerReferenceName");
                colName.Add("BuyerStoryName", "BuyerStoryName");
                colName.Add("OID", "OID");
                colName.Add("StartupLetterIssued", "Startup Letter Issued Date");
                colName.Add("Participants", "Participants");
                colName.Add("KrrMeetingHeldOn", "KrrMeetingHeldOn Date");
                colName.Add("PackingList", "PackingList Date");
                colName.Add("KarurDespatch", "KarurDespatch Date");
            }
            else if (moduleName == module.ProcessByUser.ToString())
            {
                colName.Add("UserID", "User ID");
                colName.Add("UserName", "User Name");
                colName.Add("ProcessID", "ProcessID");
                colName.Add("ProcessName", "Process Name");
            }
            else if (moduleName == module.ProductName.ToString())
            {
                colName.Add("PrdID", "Product ID");
                colName.Add("Productname", "Product Name");
            }
            else if (moduleName == module.Processname.ToString())
            {
                colName.Add("ProcID", "Process ID");
                colName.Add("Processname", "Process Name");
            }
            else if (moduleName == module.Suppliername.ToString())
            {
                colName.Add("SupID", "Supplier ID");
                colName.Add("Suppliername", "Supplier Name");
            }
            else if (moduleName == module.DSProduct.ToString())
            {
                colName.Add("ProductID", "Product ID");
                colName.Add("BuyerOrderNumberName", "Buyer Order Number");
                colName.Add("Processname", "Process Name");
                colName.Add("Productname", "Product Name");
                colName.Add("SPCDate", "Process ID");
                colName.Add("Processname", "Process Name");
                colName.Add("TodayDate", "Today Date");
                colName.Add("FinishedDays", "Finished Days");
                colName.Add("ProductQty", "Product Qty");
                colName.Add("FinishedQty", "Finished Qty");
                colName.Add("Unit", "Unit");
                colName.Add("RemainingDays", "Remaining Days");
                colName.Add("Done", "Done");
            }
            return colName;
        }

        public StringBuilder GetSqlTableQuery(string mod)
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT COUNT(1) OVER() AS rwcnt,");
            if (mod == module.ProductMaster.ToString())
            {
                query.Append("ProductID,Buyername,BuyerOrderNumberName,Processname,Suppliername,Productname,StartDate,SPCDate,ArticleNumber,DesignNumber,Size,Colour,CountConstruction," +
                    "Unit,SetNote,ProductQty,FinishedQty,(ProductQty - FinishedQty) AS BalanceQty,PlannedQty,Remarks,StoryName,Status,SKUStartDate,SKUEndDate FROM ProductUpdateGrid WHERE 1=1");
            }
            else if (mod == module.OPM.ToString())
            {
                query.Append(" [OPM_ID],[BuyerReferenceName],[BuyerStoryName],[OID],[StartupLetterIssued],[Participants],[KrrMeetingHeldOn],[PackingList],[KarurDespatch] FROM OPM");
            }
            else if (mod == module.ProcessByUser.ToString())
            {
                query.Append(" * FROM VW_ProcessByUser");
            }
            else if (mod == module.ProductName.ToString())
            {
                query.Append(" * FROM Productname");
            }
            else if (mod == module.Processname.ToString())
            {
                query.Append(" * FROM Processname");
            }
            
            else if (mod == module.Suppliername.ToString())
            {
                query.Append(" * FROM Suppliername");
            } 
            
            else if (mod == module.DSProduct.ToString())
            {
                query.Append(" * FROM VW_DSProductUpdateGrid");
            }
            return query;
        }

        public static string GenerateFilterCondition(List<GridFilter> filters)
        {
            string ret = " 1=1 ";
            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    if (!string.IsNullOrWhiteSpace(filter.ColumnName) && !string.IsNullOrEmpty(filter.Condition))
                    {
                        string value = "";
                        if (filter.Condition.Contains("is null") || filter.Condition.Contains("is not null"))
                        {
                        }
                        else
                        {
                            value = filter.Value == null ? string.Empty : filter.Value.Trim();
                        }

                        Regex regex = new Regex("[;\\\\/:*?\"<>|&']");
                        value = regex.Replace(value, "");

                        if (filter.Condition == "No" && !string.IsNullOrEmpty(value))
                            filter.Condition = "Contain";

                        ret += " AND";
                        ret += " (" + filter.ColumnName;
                        if (filter.Condition == "Contain")
                        {
                            ret += " LIKE '%" + value + "%'";
                        }
                        else if (filter.Condition == "Does not contain")
                        {
                            ret += " NOT LIKE '%" + value + "%'";
                        }
                        else if (filter.Condition == "Starts with")
                        {
                            ret += " LIKE '" + value + "%'";
                        }
                        else if (filter.Condition == "Ends with")
                        {
                            ret += " LIKE '%" + value + "' ";
                        }
                        else if (filter.Condition.Contains("is null") || filter.Condition.Contains("is not null"))
                        {
                            ret += " " + filter.Condition + " ";
                        }
                        else
                        {
                            ret += " " + filter.Condition + " '" + value + "'";
                        }
                        ret += ") ";
                    }
                }
            }
            return ret;
        }

        public static List<dynamic> GetDynamicListBySP(string SP, DynamicList_Model model)
        {
            using (DataContext context = new DataContext())
            {
                using (var connection = new SqlConnection(context.Database.Connection.ConnectionString))//,
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = SP;
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("fieldName", model.fieldName == null ? model.ColumnList[0] : model.fieldName));
                    command.Parameters.Add(new SqlParameter("sortOrder", model.sortOrder == null ? "ASC" : model.sortOrder));
                    command.Parameters.Add(new SqlParameter("pageNo", model.page > 0 ? model.page - 1 : 0));
                    command.Parameters.Add(new SqlParameter("pageSize", model.pageSize));
                    //For Extra Parameter
                    if (model.SqlParameterList != null)
                    {
                        foreach (var para in model.SqlParameterList)
                        {
                            command.Parameters.Add(para);
                        }
                    }
                    connection.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        var dt = new DataTable();
                        adapter.Fill(dt);
                        //totalCount = dt.Rows.Count;
                        //return dt.DynamicMapToList<dynamic>(Convert.ToInt32(pageNo), pageSize, true);
                        model.totalCount = (dt == null || dt.Rows.Count == 0 || dt.Rows[0]["rwcnt"] == null) ? 0 : Convert.ToInt32(dt.Rows[0]["rwcnt"].ToString());
                        dt.Columns.Remove("rwcnt");

                        DataView view = new DataView(dt);
                        DataTable table2 = view.ToTable(false, model.ColumnList);

                        return dt.DynamicMapToList<dynamic>(0, model.pageSize, true);//pageno=0 because always get pagesize wise data not get all data,so
                    }
                }
            }
        }
        public static List<dynamic> GetDynamicListBySP1(string SP, string[] columns, ref int totalCount, int pageNo = 0, int? pageSize = 10)
        {
            using (DataContext context = new DataContext())
            {
                using (var connection = new SqlConnection(context.Database.Connection.ConnectionString))//,
                {
                    pageNo = pageNo > 0 ? pageNo - 1 : 0;

                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = SP;
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        var dt = new DataTable();
                        adapter.Fill(dt);
                        //totalCount = dt.Rows.Count;
                        //return dt.DynamicMapToList<dynamic>(Convert.ToInt32(pageNo), pageSize, true);
                        totalCount = (dt == null || dt.Rows.Count == 0 || dt.Rows[0]["rwcnt"] == null) ? 0 : Convert.ToInt32(dt.Rows[0]["rwcnt"].ToString());
                        dt.Columns.Remove("rwcnt");

                        DataView view = new DataView(dt);
                        DataTable table2 = view.ToTable(false, columns);

                        return dt.DynamicMapToList<dynamic>(0, pageSize, true);//pageno=0 because always get pagesize wise data not get all data
                    }
                }
            }
        }

        public enum module
        {
            ProductMaster,
            Category,
            SubCategory,
            Status,
            OPM,
            Vendor,
            StyleInfo,
            ProcessByUser,
            ProductName,
            Processname,
            DSProduct,
            Supplier,
            Suppliername
        }


        public static string WriteErrorLog(string msg)
        {
            string errorLogFilename = "ErrorLog_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
            string filepath = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/ErrorLogFiles/");
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);

            }
            string path = filepath + errorLogFilename;
            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
            }
            if (File.Exists(path))
            {
                using (StreamWriter stwriter = new StreamWriter(path, true))
                {
                    stwriter.WriteLine(msg);
                    stwriter.Close();
                }
            }
            else
            {
                using (StreamWriter stwriter = File.CreateText(path))
                {
                    stwriter.WriteLine(msg);
                    stwriter.Close();
                }
            }
            return errorLogFilename;
        }
    }


    public class CommonModel<T>
    {
        public T Table { get; set; }
        public List<dynamic> dynamicList { get; set; }
        public PagedList<T> pageList { get; set; }
        public IPagedList dynamicListMetaData { get; set; }
        public int? page { get; set; }
        public string sortOrder { get; set; }
        public string fieldName { get; set; }
        public string SearchText { get; set; }
        public List<GridFilter> Filters { get; set; }

        public int StaticPageSize { get; set; }
        public string ViewType { get; set; }

    }
    public class GridFilter
    {
        public string Value { get; set; }
        public string ColumnName { get; set; }
        public string DisplayText { get; set; }
        public string Condition { get; set; }
    }
    public class MVCPagerModel
    {
        public List<dynamic> DynamicList { get; set; }
        public IPagedList DynamicListMetaData { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string UpdateTargetId { get; set; }
        public string TableUpdate { get; set; }

        public string sortOrder { get; set; }
        public string fieldName { get; set; }
        public string SearchText { get; set; }
        public string SearchType { get; set; }
        public string OnBegin { get; set; }
        public string OnComplete { get; set; }
        public string OnSuccess { get; set; }

        public int StaticPageSize { get; set; }

        public bool PagingWithoutFilter { get; set; }
    }

    public class MVCPagerModel1
    {
        public IPagedList pagedlist { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string UpdateTargetId { get; set; }
        public string TableUpdate { get; set; }
        public string OnBegin { get; set; }
        public string OnComplete { get; set; }
        public string OnSuccess { get; set; }
        public int OPM_ID { get; set; }
        public int StaticPageSize { get; set; }

        public bool PagingWithoutFilter { get; set; }
    }
}