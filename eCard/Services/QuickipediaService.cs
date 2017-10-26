using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eCard.Models;

namespace eCard.Services
{
    public class QuickipediaService
    {
        public static List<ClientDropDown> GetClientDropDown(out string message)
        {
            try
            {
                message = "";

                using (var db = new QuickipediaEntities())
                {
                    var query = from c in db.ClientProfile
                                orderby c.ClientName ascending
                                select new ClientDropDown
                                {
                                    ID = c.ClientCode,
                                    Value = c.ClientName
                                };

                    return query.ToList();
                }
            }
            catch(Exception error)
            {
                message = error.Message;

                return null;
            }
        }
    }
}