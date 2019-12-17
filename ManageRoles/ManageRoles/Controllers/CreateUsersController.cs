using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageRoles.Algorithm;
using ManageRoles.Filters;
using ManageRoles.Models;
using ManageRoles.Repository;
using ManageRoles.ViewModels;
using System.IO;

namespace ManageRoles.Controllers
{
    [AuthorizeSuperAdminandAdmin]
    [SessionExpireFilter]
    public class CreateUsersController : Controller
    {
        private readonly IUserMaster _iUserMaster;
        private readonly IPassword _iPassword;
        private readonly ISavedAssignedRoles _savedAssignedRoles;
        private readonly IRole _iRole;
        public CreateUsersController(IUserMaster userMaster, IPassword password, ISavedAssignedRoles savedAssignedRoles, IRole role)
        {
            _iUserMaster = userMaster;
            _iPassword = password;
            _savedAssignedRoles = savedAssignedRoles;
            _iRole = role;
        }

        // GET: CreateUsers
        public ActionResult Create(int? Id = null)
        {
            try
            {
                if (Id == null)
                {
                    var createUserViewModel = new CreateUserViewModel()
                    {
                        ListRole = _iRole.GetAllActiveRoles()
                    };
                    return View(createUserViewModel);
                }
                else
                {
                    CreateUserViewModel createUserViewModel = new CreateUserViewModel();
                    var user = _iUserMaster.GetUserById(Id);
                    createUserViewModel.EmailId = user.EmailId;
                    createUserViewModel.FirstName = user.FirstName;
                    createUserViewModel.Gender = user.Gender;
                    createUserViewModel.ImageName = user.ImageName;
                    createUserViewModel.LastName = user.LastName;
                    createUserViewModel.MobileNo = user.MobileNo;
                    createUserViewModel.Status = user.Status;
                    createUserViewModel.UserName = user.UserName;
                    createUserViewModel.UserId = user.UserId;
                    return View(createUserViewModel);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult Create(CreateUserViewModel createUserViewModel, HttpPostedFileBase fileUpload)
        {
            try
            {
                if(createUserViewModel.UserId > 0)
                {
                    ModelState.Remove("UserName");
                    ModelState.Remove("Password");
                    ModelState.Remove("ConfirmPassword");
                    ModelState.Remove("RoleId");
                }
                if (ModelState.IsValid)
                {
                    if (createUserViewModel.UserId == 0)
                    {

                        var isUser = _iUserMaster.CheckUsernameExists(createUserViewModel.UserName);
                        if (isUser)
                        {
                            ModelState.AddModelError("", "Username already exists");
                        }

                        AesAlgorithm aesAlgorithm = new AesAlgorithm();


                        var usermaster = AutoMapper.Mapper.Map<Usermaster>(createUserViewModel);
                        string path = Server.MapPath("~/Content/UserImage/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        if (fileUpload != null && fileUpload.ContentLength > 0)
                        {
                            string extension = Path.GetExtension(fileUpload.FileName);
                            string newFileName = "u_" + DateTime.Now.Ticks + extension;
                            usermaster.ImageName = newFileName;
                            fileUpload.SaveAs(path + newFileName);
                        }

                        usermaster.Status = true;
                        usermaster.CreateDate = DateTime.Now;
                        usermaster.UserId = 0;
                        usermaster.CreatedBy = Convert.ToInt32(Session["UserID"]);

                        var userId = _iUserMaster.AddUser(usermaster);
                        if (userId != -1)
                        {
                            var passwordMaster = new PasswordMaster
                            {
                                CreateDate = DateTime.Now,
                                UserId = userId,
                                PasswordId = 0,
                                Password = aesAlgorithm.EncryptString(createUserViewModel.Password)
                            };

                            var passwordId = _iPassword.SavePassword(passwordMaster);
                            if (passwordId != -1)
                            {
                                var savedAssignedRoles = new SavedAssignedRoles()
                                {
                                    RoleId = createUserViewModel.RoleId,
                                    UserId = userId,
                                    AssignedRoleId = 0,
                                    Status = true,
                                    CreateDate = DateTime.Now
                                };
                                _savedAssignedRoles.AddAssignedRoles(savedAssignedRoles);

                                TempData["MessageCreateUsers"] = "User Created Successfully";
                            }
                        }
                    }
                    else
                    {
                        var usermaster = AutoMapper.Mapper.Map<Usermaster>(createUserViewModel);
                        string path = Server.MapPath("~/Content/UserImage/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        if (fileUpload != null && fileUpload.ContentLength > 0)
                        {
                            string extension = Path.GetExtension(fileUpload.FileName);
                            string newFileName = "u_" + DateTime.Now.Ticks + extension;
                            usermaster.ImageName = newFileName;
                            fileUpload.SaveAs(path + newFileName);
                        }

                        usermaster.Status = true;
                        usermaster.CreateDate = DateTime.Now;
                        usermaster.UserId = createUserViewModel.UserId;
                        usermaster.CreatedBy = Convert.ToInt32(Session["UserID"]);

                        var userId = _iUserMaster.UpdateUser(usermaster);
                    }
                    return RedirectToAction("Create", "CreateUsers");
                }
                else
                {
                    return View("Create", createUserViewModel);
                }
            }
            catch
            {
                throw;
            }
        }

        public bool UserImageDelete(int userId, string fileName)
        {
            try
            {
                var user = _iUserMaster.GetUserById(userId);
                if (user != null)
                {
                    user.ImageName = "";
                   _iUserMaster.UpdateUser(user);
                }
                string path = Server.MapPath("~/UserImage/" + fileName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}