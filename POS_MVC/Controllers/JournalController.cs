﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace REX_MVC.Controllers
{
    public class JournalController : Controller
    {
        // GET: Journal
        public ActionResult Index()
        {
            return View();
        }
    }
}