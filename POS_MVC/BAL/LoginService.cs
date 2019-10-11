using REX_MVC.BAL;
using REX_MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace REX_MVC.BAL
{
    public class LoginService
    {
        DBService<UserInfo> service = new DBService<UserInfo>();
        DBService<RoleWiseScreenPermission> roleService = new DBService<RoleWiseScreenPermission>();
        Entities entity = new Entities();
        public List<Screen> GetMenuPermission(int roleid)
        {
            var entryPoint = (from ep in entity.Screens
                              join e in entity.RoleWiseScreenPermissions on ep.ScreenId equals e.ScreenId
                              where e.RoleId == roleid.ToString() orderby e.Screen.OrderBy
                              select ep );
            return entryPoint.ToList();
        }

        public List<UserInfo> GetAll()
        {
            return service.GetAll();
        }
        public UserInfo GetById(int? id = 0)
        {
            return service.GetById(id);
        }

        public UserInfo Save(UserInfo cus)
        {
            return service.Save(cus);

        }
        public UserInfo Update(UserInfo t, int id)
        {
            return service.Update(t, id);

        }
        public RoleWiseScreenPermission SaveRoleWisePermission(RoleWiseScreenPermission cus)
        {
            return roleService.Save(cus);

        }
        public RoleWiseScreenPermission UpdateRoleWisePermission(RoleWiseScreenPermission t, int id)
        {
            return roleService.Update(t, id);

        }
        public List<RoleWiseScreenPermission> GetRoleWiseService()
        {
            return roleService.GetAll();

        }
        public int Delete(int id)
        {
            return service.Delete(id);
        }
    }
}