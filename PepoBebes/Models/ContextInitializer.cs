using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Devtalk.EF.CodeFirst;

namespace PepoBebes.Models
{
    public class ContextInitializer
    {
        public static void Init()
        {
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<Context>());
            Database.SetInitializer(new DontDropDbJustCreateTablesIfModelChanged<Context>());
        }

    }
}