using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Expenses.Controllers
{
    [Authorize]
    public class ChartingController : ApiController
    {
        private ExpensesEntities db = new ExpensesEntities() { Configuration = { LazyLoadingEnabled = false } };

        public List<UserExpenseView> GetUserExpenseData()
        {
            int UserID = Security.GetUserID().Value;

            return db.UserExpenseViews.Where(oUserExpense => oUserExpense.UserID == UserID && oUserExpense.CurrencyID == 1).OrderBy(oUserExpense => oUserExpense.CategoryID).ToList();
        }
    }
}