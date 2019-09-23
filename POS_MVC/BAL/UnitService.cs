using RiceMill_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RiceMill_MVC.BAL
{
    public class UnitService
    {
        DBService<Unit> service = new DBService<Unit>();
        public List<Unit> GetAll()
        {
            return service.GetAll();
        }
        public Unit GetById(int? id = 0)
        {
            return service.GetById(id);
        }

        public Unit Save(Unit cus)
        {
            service.Save(cus);
            return cus;

        }
        public Unit Update(Unit t, int id)
        {
            service.Update(t, id);
            return t;

        }
        public int Delete(int id)
        {
            return service.Delete(id);
        }
    }
}