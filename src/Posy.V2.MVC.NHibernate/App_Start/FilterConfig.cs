﻿using System.Web;
using System.Web.Mvc;

namespace Posy.V2.MVC.NHibernate
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
