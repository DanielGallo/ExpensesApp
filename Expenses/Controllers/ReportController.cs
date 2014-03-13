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
    //[Authorize]
    public class ReportController : ApiController
    {
        private ExpensesEntities db = new ExpensesEntities() { Configuration = { LazyLoadingEnabled = false } };

        // GET api/Report
        public IEnumerable<ReportView> GetReports()
        {
            int? UserID = Security.GetUserID();
            
            if (UserID == null) // TODO: Only using this during testing.
                UserID = 1;

            return db.ReportViews.Where(oReport => oReport.UserID == UserID).AsEnumerable();
        }

        // GET api/Report/5
        public Report GetReport(int id)
        {
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return report;
        }

        // PUT api/Report/5
        public HttpResponseMessage PutReport(int id, Report report)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != report.ID)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(report).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/Report
        public HttpResponseMessage PostReport(Report report)
        {
            if (ModelState.IsValid)
            {
                report.DateCreated = DateTime.Now;
                report.UserID = Security.GetUserID().Value;
                report.Submitted = false;
                report.Approved = false;
                report.Reimbursed = false;

                db.Reports.Add(report);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, report);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = report.ID }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Report/5
        public HttpResponseMessage DeleteReport(int id)
        {
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Reports.Remove(report);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, report);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}