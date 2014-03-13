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
using System.Web.Security;

namespace Expenses.Controllers
{
    public class UserController : ApiController
    {
        private ExpensesEntities db = new ExpensesEntities() { Configuration = { LazyLoadingEnabled = false } };

        // GET api/User
        [Authorize]
        public dynamic GetUsers()
        {
            // Custom LINQ query, as we don't want to return password values
            var users = from oUser in db.Users
                        select new
                        {
                            oUser.ID,
                            oUser.Name,
                            oUser.EmailAddress,
                            oUser.ApproverID
                        };

            return users.AsEnumerable();
        }

        // GET api/User/5
        /*public User GetUser(string emailAddress, string password)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return user;
        }*/

        // PUT api/User/5
        [Authorize]
        public HttpResponseMessage PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != user.ID)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                var crypto = new SimpleCrypto.PBKDF2();
                var encryptedPassword = crypto.Compute(user.Password);

                user.ApproverID = 1;
                user.Password = encryptedPassword;
                user.PasswordSalt = crypto.Salt;

                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }
        
        // POST api/User
        [Authorize]
        public HttpResponseMessage PostUser(User user)
        {
            if (ModelState.IsValid)
            {
                var crypto = new SimpleCrypto.PBKDF2();
                var encryptedPassword = crypto.Compute(user.Password);

                user.ApproverID = 1;
                user.Password = encryptedPassword;
                user.PasswordSalt = crypto.Salt;

                db.Users.Add(user);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, user);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = user.ID }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }
        
        // DELETE api/User/5
        [Authorize]
        public HttpResponseMessage DeleteUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Users.Remove(user);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, user);
        }




        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}