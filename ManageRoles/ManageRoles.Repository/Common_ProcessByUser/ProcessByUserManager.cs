using ManageRoles.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ManageRoles.Repository
{
    #region ProcessByUserManager
    public class ProcessByUser_Model : CommonModel<VW_ProcessByUser>
    {
        public SelectList ProcessList { get; set; }
        public SelectList UserList { get; set; }
        public ProcessByUser CTable { get; set; }
        public string[] ProcessId { get; set; }
        public int UserId { get; set; }
    }
    public class VW_ProcessByUserManager : CommonRepository<VW_ProcessByUser>
    {
        public VW_ProcessByUserManager(DbContext context) : base(context) { }

        public VW_ProcessByUser GetProcessByUserById(int Id)
        {
            return this.GetById(Id);
        }
    }
    public class ProcessByUserManager : CommonRepository<ProcessByUser>
    {
        public ProcessByUserManager(DbContext context) : base(context) { }

        public List<ProcessByUser> GetProcessByUserList()
        {
            var lst = this.GetAll().ToList();
            return lst;
        }

        public void AddProcessByUser(ProcessByUser obj)
        {
            this.Add(obj);
        }

        public void UpdateProcessByUser(ProcessByUser obj)
        {
            this.Update(obj);
        }

        public ProcessByUser GetProcessByUserById(int Id)
        {
            return this.GetById(Id);
        }
        public void DeleteProcessByUser(List<ProcessByUser> lst)
        {
            this.Delete(lst);
        }

        public void DeleteProcessByUser(int Id)
        {
            this.Delete(Id);
        }

        public string SaveProcessByUser(ProcessByUser objProcessByUser)
        {
            using (var context = new CommonRepository<ProcessByUser>(this.DbContext))
            {
                if (objProcessByUser.ID == 0)
                {
                    context.Add(objProcessByUser);
                    return "success";
                }
                else
                {
                    context.Update(objProcessByUser);
                    return "success";
                }
            }
        }
    }
    #endregion ProcessByUser


    public class UserMasterListManager : CommonRepository<Usermaster>
    {
        public UserMasterListManager(DbContext context) : base(context) { }
        public DataTable GetDtUserListForProcess()
        {
            return GetDataTable("SELECT UserID,UserName FROM Usermaster WHERE UserID NOT IN(SELECT DISTINCT UserID FROM[dbo].[ProcessByUser]) ORDER BY UserName");
        }

        public DataTable GetDtUserListForProcess(int UserId)
        {
            return GetDataTable("SELECT UserID,UserName FROM Usermaster WHERE UserID NOT IN(SELECT DISTINCT UserID FROM[dbo].[ProcessByUser] WHERE UserId != '"+ UserId + "') ORDER BY UserName");
        }
    }

    #region ProductNameManager
    public class ProductName_Model : CommonModel<Productname>
    {
    }
    public class ProductNameManager : CommonRepository<Productname>
    {
        public ProductNameManager(DbContext context) : base(context) { }

        public List<Productname> GetProductNameList()
        {
            var lst = this.GetAll().ToList();
            return lst;
        }

        public void AddProductName(Productname obj)
        {
            this.Add(obj);
        }

        public void UpdateProductName(Productname obj)
        {
            this.Update(obj);
        }

        public Productname GetProductNameById(int Id)
        {
            return this.GetById(Id);
        }
        public void DeleteProductName(List<Productname> lst)
        {
            this.Delete(lst);
        }

        public void DeleteProductName(int Id)
        {
            this.Delete(Id);
        }

        public string SaveProductName(Productname objProductName)
        {
            using (var context = new CommonRepository<Productname>(this.DbContext))
            {
                if (objProductName.PrdID == 0)
                {
                    context.Add(objProductName);
                    return "success";
                }
                else
                {
                    context.Update(objProductName);
                    return "success";
                }
            }
        }
    }
    #endregion ProductName

    #region ProcessnameManager
    public class Processname_Model : CommonModel<Processname>
    {
    }
    public class ProcessnameManager : CommonRepository<Processname>
    {
        public ProcessnameManager(DbContext context) : base(context) { }

        public List<Processname> GetProcessnameList()
        {
            var lst = this.GetAll().ToList();
            return lst;
        }

        public void AddProcessname(Processname obj)
        {
            this.Add(obj);
        }

        public void UpdateProcessname(Processname obj)
        {
            this.Update(obj);
        }

        public Processname GetProcessnameById(int Id)
        {
            return this.GetById(Id);
        }
        public void DeleteProcessname(List<Processname> lst)
        {
            this.Delete(lst);
        }

        public void DeleteProcessname(int Id)
        {
            this.Delete(Id);
        }

        public string SaveProcessname(Processname objProcessname)
        {
            using (var context = new CommonRepository<Processname>(this.DbContext))
            {
                if (objProcessname.ProcID == 0)
                {
                    context.Add(objProcessname);
                    return "success";
                }
                else
                {
                    context.Update(objProcessname);
                    return "success";
                }
            }
        }
    }
    #endregion Processname

  }
