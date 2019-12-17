using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageRoles.Filters;
using System.Text;
using System.Net;
using ManageRoles.ViewModels;
using ManageRoles.Repository;
using ManageRoles.Models;

namespace ManageRoles.Controllers
{
    [AuthorizeSuperAdminandAdmin]
    [SessionExpireFilter]
    public class AllUsersController : Controller
    {
        // GET: AllUsers
        public ActionResult Show()
        {
            return View();
        }

        [HttpPost]//Gets the todo Lists.  
        public JsonResult UserList(string username, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {

            try
            {
                var userCount = GetUserCount();

                var roles = GetUserList(username, jtStartIndex, jtPageSize, jtSorting);
                return Json(new { Result = "OK", Records = roles, TotalRecordCount = userCount });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetUserCount()
        {
            try
            {
                using (var db = new DatabaseContext())
                {
                    return db.SavedAssignedRoles.Count();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<UsermasterViewModel> GetUserList(string username, int startIndex, int count, string sorting)
        {
            // Instance of DatabaseContext
            try
            {
                using (var db = new DatabaseContext())
                {
                    var data = from usermaster in db.Usermasters
                               where usermaster.Status == true
                               select new UsermasterViewModel()
                               {
                                   UserId = usermaster.UserId,
                                   UserName = usermaster.UserName,
                                   FirstName = usermaster.FirstName,
                                   LastName = usermaster.LastName,
                                   EmailId = usermaster.EmailId,
                                   Gender = usermaster.Gender,
                                   Status = usermaster.Status,
                                   MobileNo = usermaster.MobileNo
                               };

                    IEnumerable<UsermasterViewModel> query = data.ToList();

                    //Search
                    if (username != null)
                    {
                        query = query.Where(p => p.UserName.Contains(username));
                    }

                    //Sorting Ascending and Descending
                    if (string.IsNullOrEmpty(sorting) || sorting.Equals("UserId ASC"))
                    {
                        query = query.OrderBy(p => p.UserId);
                    }
                    else if (sorting.Equals("UserId DESC"))
                    {
                        query = query.OrderByDescending(p => p.UserId);
                    }
                    else if (sorting.Equals("UserName ASC"))
                    {
                        query = query.OrderBy(p => p.UserName);
                    }
                    else if (sorting.Equals("UserName DESC"))
                    {
                        query = query.OrderByDescending(p => p.UserName);
                    }

                    else if (sorting.Equals("FirstName ASC"))
                    {
                        query = query.OrderBy(p => p.FirstName);
                    }
                    else if (sorting.Equals("FirstName DESC"))
                    {
                        query = query.OrderByDescending(p => p.FirstName);
                    }

                    else if (sorting.Equals("LastName ASC"))
                    {
                        query = query.OrderBy(p => p.LastName);
                    }
                    else if (sorting.Equals("LastName DESC"))
                    {
                        query = query.OrderByDescending(p => p.LastName);
                    }

                    else if (sorting.Equals("EmailId ASC"))
                    {
                        query = query.OrderBy(p => p.EmailId);
                    }

                    else if (sorting.Equals("EmailId DESC"))
                    {
                        query = query.OrderByDescending(p => p.EmailId);
                    }


                    else if (sorting.Equals("Gender ASC"))
                    {
                        query = query.OrderBy(p => p.Gender);
                    }

                    else if (sorting.Equals("Gender DESC"))
                    {
                        query = query.OrderByDescending(p => p.Gender);
                    }

                    else if (sorting.Equals("Status ASC"))
                    {
                        query = query.OrderBy(p => p.Status);
                    }

                    else if (sorting.Equals("Status DESC"))
                    {
                        query = query.OrderByDescending(p => p.Status);
                    }

                    else if (sorting.Equals("MobileNo ASC"))
                    {
                        query = query.OrderBy(p => p.MobileNo);
                    }

                    else if (sorting.Equals("MobileNo DESC"))
                    {
                        query = query.OrderByDescending(p => p.MobileNo);
                    }

                    else
                    {
                        query = query.OrderBy(p => p.UserId); //Default!
                    }

                    return count > 0
                               ? query.Skip(startIndex).Take(count).ToList()  //Paging
                               : query.ToList(); //No paging
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public JsonResult RemoveUser(int userId)
        {
            try
            {
                using (var db = new DatabaseContext())
                {
                    var roleId = (from sar in db.SavedAssignedRoles
                        where sar.UserId == userId
                        select sar.RoleId).FirstOrDefault();   

                    if (roleId != null)
                    {
                        var role = db.RoleMasters.Find(roleId);

                        if (role != null && role.RoleId == Convert.ToInt32(ConfigurationManager.AppSettings["SuperAdminRolekey"]))
                        {
                            return Json(new { Result = "ERROR", Message = "Cannot Delete Super Admin" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            var usermaster = db.Usermasters.Find(userId);
                            if (usermaster != null) db.Usermasters.Remove(usermaster);
                            db.SaveChanges();

                            var password = db.PasswordMaster.Find(userId);
                            if (password != null) db.PasswordMaster.Remove(password);
                            db.SaveChanges();

                            var savedAssignedRoles = db.SavedAssignedRoles.Find(userId);
                            if (savedAssignedRoles != null) db.SavedAssignedRoles.Remove(savedAssignedRoles);
                            db.SaveChanges();
                        }
                    }
                }
                return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        CommonFunction common = new CommonFunction();
        #region Start ProcessByUserGrid   
        public ActionResult ProcessByUserGrid()
        {
            ProcessByUser_Model objModel = new ProcessByUser_Model();
            objModel.page = 1;
            objModel.StaticPageSize = 10;
            BindProcessByUserGrid(objModel, Convert.ToInt32(objModel.page), objModel.StaticPageSize);
            return View(objModel);
        }
        public ActionResult ProcessByUserFilterSearch(ProcessByUser_Model objModel, int page = 1, int pageSize = 10)
        {
            BindProcessByUserGrid(objModel, page, pageSize);
            return PartialView("ProcessByUserList", objModel);
        }
        public void BindProcessByUserGrid(ProcessByUser_Model objModel, int page, int pageSize)
        {
            StringBuilder query = new StringBuilder();
            var colName = common.GetColumns(CommonFunction.module.ProcessByUser.ToString());
            query = common.GetSqlTableQuery(CommonFunction.module.ProcessByUser.ToString());
            if (objModel != null)
                objModel.StaticPageSize = pageSize;

            VW_ProcessByUserManager context = new VW_ProcessByUserManager(new DataContext());
            context.setModel(query, objModel, colName, "UserID", page, pageSize);
        }


        //Start Product Master
        [HttpPost]
        public ActionResult AddEditProcessByUser(int ID = 0)
        {
            ProcessListManager objProcessListManager = new ProcessListManager(new DataContext());
            UserMasterListManager objUserMasterListManager = new UserMasterListManager(new DataContext());
            //ProcessByUserManager context = new ProcessByUserManager(new DataContext());
            VW_ProcessByUserManager context = new VW_ProcessByUserManager(new DataContext());
            ProcessByUser_Model objModel = new ProcessByUser_Model();

            if (ID != 0)
            {
                objModel.Table = context.GetProcessByUserById(ID);
                objModel.UserId = objModel.Table.UserID;
                objModel.ProcessId= !string.IsNullOrEmpty(objModel.Table.ProcessID)? objModel.Table.ProcessID.Split(',') : null;
                objModel.UserList = Extens.ToSelectList(objUserMasterListManager.GetDtUserListForProcess(objModel.Table.UserID), "UserID", "UserName");
            }
            else
            {
                objModel.Table = new VW_ProcessByUser();
                objModel.UserList = Extens.ToSelectList(objUserMasterListManager.GetDtUserListForProcess(), "UserID", "UserName");
            }
            objModel.ProcessList = Extens.ToSelectList(objProcessListManager.GetDtProcess(), "ProcID", "Processname");

            return PartialView("ProcessByUserCRUD", objModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveProcessByUser(ProcessByUser_Model objModel, int page = 1, int pageSize = 10)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join("|", ModelState.Values.SelectMany(e => e.Errors).Select(em => em.ErrorMessage));
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, message);
            }

            //Save     
            ProcessByUserManager context = new ProcessByUserManager(new DataContext());
            VW_ProcessByUserManager context2 = new VW_ProcessByUserManager(new DataContext());
            List<ProcessByUser> lst = context.GetAll(c => c.UserID == objModel.UserId).ToList();
            context.DeleteProcessByUser(lst);
            foreach (string pId in objModel.ProcessId)
            {
                ProcessByUserManager context1 = new ProcessByUserManager(new DataContext());
                ProcessByUser obj = new ProcessByUser();
                obj.ProcessID = Convert.ToInt32(pId);
                obj.UserID = objModel.UserId;
                var msg = context1.SaveProcessByUser(obj);
            }
            int UserID =Convert.ToInt32(Session["UserID"]);
            VW_ProcessByUser objVW_ProcessByUser = context2.GetProcessByUserById(UserID);
            if (objVW_ProcessByUser != null)
            {
                Session["UserProcess"] = objVW_ProcessByUser.ProcessName != null ? objVW_ProcessByUser.ProcessName : "";
            }
            if (objModel.StaticPageSize != 0) { pageSize = objModel.StaticPageSize; }
            objModel.StaticPageSize = pageSize;

            BindProcessByUserGrid(objModel, page, pageSize);
            return PartialView("ProcessByUserList", objModel);
        }

        [HttpPost]
        public ActionResult DeleteProcessByUser(string ID, ProcessByUser_Model objModel, int page = 1, int pageSize = 10)
        {
            ProcessByUserManager context = new ProcessByUserManager(new DataContext());

            if (!string.IsNullOrEmpty(ID))
            {
                int uId = Convert.ToInt32(ID);
                List<ProcessByUser> lst = context.GetAll(c => c.UserID == uId).ToList();
                context.DeleteProcessByUser(lst);

                VW_ProcessByUserManager context2 = new VW_ProcessByUserManager(new DataContext());
                int UserID = Convert.ToInt32(Session["UserID"]);
                VW_ProcessByUser objVW_ProcessByUser = context2.GetProcessByUserById(UserID);
                if (objVW_ProcessByUser != null)
                {
                    Session["UserProcess"] = objVW_ProcessByUser.ProcessName != null ? objVW_ProcessByUser.ProcessName : "";
                }
            }

            if (objModel.StaticPageSize != 0) { pageSize = objModel.StaticPageSize; }
            objModel.StaticPageSize = pageSize;

            BindProcessByUserGrid(objModel, page, pageSize);
            return PartialView("ProcessByUserList", objModel);
        }
        #endregion

        #region Start ProductNameGrid   
        public ActionResult ProductNameGrid()
        {
            ProductName_Model objModel = new ProductName_Model();
            objModel.page = 1;
            objModel.StaticPageSize = 10;
            BindProductNameGrid(objModel, Convert.ToInt32(objModel.page), objModel.StaticPageSize);
            return View(objModel);
        }
        public ActionResult ProductNameFilterSearch(ProductName_Model objModel, int page = 1, int pageSize = 10)
        {
            BindProductNameGrid(objModel, page, pageSize);
            return PartialView("ProductNameList", objModel);
        }
        public void BindProductNameGrid(ProductName_Model objModel, int page, int pageSize)
        {
            StringBuilder query = new StringBuilder();
            var colName = common.GetColumns(CommonFunction.module.ProductName.ToString());
            query = common.GetSqlTableQuery(CommonFunction.module.ProductName.ToString());
            if (objModel != null)
                objModel.StaticPageSize = pageSize;

            ProductNameManager context = new ProductNameManager(new DataContext());
            context.setModel(query, objModel, colName, "Productname", page, pageSize);
        }


        //Start Product Master
        [HttpPost]
        public ActionResult AddEditProductname(int ID = 0)
        {
            ProductNameManager context = new ProductNameManager(new DataContext());
            ProductName_Model objModel = new ProductName_Model();

            if (ID != 0)
            {
                objModel.Table = context.GetProductNameById(ID);
            }
            else
            {
                objModel.Table = new Productname();
            }
            return PartialView("ProductNameCRUD", objModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveProductName(ProductName_Model objModel, int page = 1, int pageSize = 10)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join("|", ModelState.Values.SelectMany(e => e.Errors).Select(em => em.ErrorMessage));
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, message);
            }

            //Save     
            ProductNameManager context = new ProductNameManager(new DataContext());
            var msg = context.SaveProductName(objModel.Table);
            if (msg.Contains("exists"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "exists");
            }
            else
            {
                if (objModel.StaticPageSize != 0) { pageSize = objModel.StaticPageSize; }
                objModel.StaticPageSize = pageSize;

                BindProductNameGrid(objModel, page, pageSize);
                return PartialView("ProductNameList", objModel);
            }
        }

        [HttpPost]
        public ActionResult DeleteProductName(string ID, ProductName_Model objModel, int page = 1, int pageSize = 10)
        {
            ProductNameManager context = new ProductNameManager(new DataContext());

            if (!string.IsNullOrEmpty(ID))
            {
                int pId = Convert.ToInt32(ID);
                List<Productname> lst = context.GetAll(c => c.PrdID == pId).ToList();
                context.DeleteProductName(lst);
            }

            if (objModel.StaticPageSize != 0) { pageSize = objModel.StaticPageSize; }
            objModel.StaticPageSize = pageSize;

            BindProductNameGrid(objModel, page, pageSize);
            return PartialView("ProductNameList", objModel);
        }
        #endregion


        #region Start ProcessnameGrid   
        public ActionResult ProcessnameGrid()
        {
            Processname_Model objModel = new Processname_Model();
            objModel.page = 1;
            objModel.StaticPageSize = 10;
            BindProcessnameGrid(objModel, Convert.ToInt32(objModel.page), objModel.StaticPageSize);
            return View(objModel);
        }
        public ActionResult ProcessnameFilterSearch(Processname_Model objModel, int page = 1, int pageSize = 10)
        {
            BindProcessnameGrid(objModel, page, pageSize);
            return PartialView("ProcessnameList", objModel);
        }
        public void BindProcessnameGrid(Processname_Model objModel, int page, int pageSize)
        {
            StringBuilder query = new StringBuilder();
            var colName = common.GetColumns(CommonFunction.module.Processname.ToString());
            query = common.GetSqlTableQuery(CommonFunction.module.Processname.ToString());
            if (objModel != null)
                objModel.StaticPageSize = pageSize;

            ProcessnameManager context = new ProcessnameManager(new DataContext());
            context.setModel(query, objModel, colName, "Processname", page, pageSize);
        }


        //Start Processname Master
        [HttpPost]
        public ActionResult AddEditProcessname(int ID = 0)
        {
            ProcessnameManager context = new ProcessnameManager(new DataContext());
            Processname_Model objModel = new Processname_Model();

            if (ID != 0)
            {
                objModel.Table = context.GetProcessnameById(ID);
            }
            else
            {
                objModel.Table = new Processname();
            }
            return PartialView("ProcessnameCRUD", objModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveProcessname(Processname_Model objModel, int page = 1, int pageSize = 10)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join("|", ModelState.Values.SelectMany(e => e.Errors).Select(em => em.ErrorMessage));
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, message);
            }

            //Save     
            ProcessnameManager context = new ProcessnameManager(new DataContext());
            var msg = context.SaveProcessname(objModel.Table);
            if (msg.Contains("exists"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "exists");
            }
            else
            {
                if (objModel.StaticPageSize != 0) { pageSize = objModel.StaticPageSize; }
                objModel.StaticPageSize = pageSize;

                BindProcessnameGrid(objModel, page, pageSize);
                return PartialView("ProcessnameList", objModel);
            }
        }

        [HttpPost]
        public ActionResult DeleteProcessname(string ID, Processname_Model objModel, int page = 1, int pageSize = 10)
        {
            ProcessnameManager context = new ProcessnameManager(new DataContext());

            if (!string.IsNullOrEmpty(ID))
            {
                int pId = Convert.ToInt32(ID);
                List<Processname> lst = context.GetAll(c => c.ProcID == pId).ToList();
                context.DeleteProcessname(lst);
            }

            if (objModel.StaticPageSize != 0) { pageSize = objModel.StaticPageSize; }
            objModel.StaticPageSize = pageSize;

            BindProcessnameGrid(objModel, page, pageSize);
            return PartialView("ProcessnameList", objModel);
        }
        #endregion
                
    }
}