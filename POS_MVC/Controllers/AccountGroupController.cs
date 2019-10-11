using REX_MVC.BAL;
using REX_MVC.Models;
using REX_MVC.Util;
using REX_MVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace REX_MVC.Controllers
{
    public class AccountGroupController : Controller
    {
        // GET: AccountGroup
        AccountGroupService service = new AccountGroupService();
        public ActionResult Index()
        {

            ViewBag.Title = "Account Group";
            return View();
        }
        public ActionResult GetAllAccountGroup()
        {
            List<AccountGroup> category = service.GetAll();
            if (category == null)
            {
                return HttpNotFound();
            }
            var result = AutoMapper.Mapper.Map<List<AccountGroup>, List<AccountGroupResponse>>(category);
            foreach (var item in result)
            {
                if (item.GroupUnder==0 || item.GroupUnder==-1)
                {
                    item.Under = "Primary";
                }
                else
                {
                    item.Under = result.Where(a => a.Id == item.GroupUnder).FirstOrDefault().AccountGroupName;

                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllGroupByNature(string nature)
        {            
            List<AccountGroup> category = service.GetAll(nature);
            if (category == null)
            {

                return HttpNotFound();
            }
            var result = AutoMapper.Mapper.Map<List<AccountGroup>, List<AccountGroupResponse>>(category);
            foreach (var item in result)
            {
                if (item.GroupUnder == 0 || item.GroupUnder == -1)
                {
                    item.Under = "Primary";
                }
                else
                {
                    item.Under = result.Where(a => a.Id == item.GroupUnder).FirstOrDefault().AccountGroupName;

                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetById(int groupId)
        {
            AccountGroup category = service.GetById(groupId);
            if (category == null)
            {
                return HttpNotFound();
            }
            var result = AutoMapper.Mapper.Map<AccountGroup, AccountGroupResponse>(category);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(AccountGroup category)
        {
            var result = category;
            if (ModelState.IsValid)
            {
                category.IsDefault = false;
                result = service.Save(category);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit(AccountGroup category)
        {
            var result = category;
            if (ModelState.IsValid)
            {
                category.IsDefault = false;
                result = service.Update(category,category.Id);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int accountGroupId)
        {
            var result = 0;
            if (ModelState.IsValid)
            {
                 result=service.Delete(accountGroupId);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}