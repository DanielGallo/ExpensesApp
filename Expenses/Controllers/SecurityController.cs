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
    public class SecurityController : ApiController
    {
        private ExpensesEntities db = new ExpensesEntities() { Configuration = { LazyLoadingEnabled = false } };
     
        public class LoginCredentials
        {
            public string EmailAddress { get; set; }
            public string Password { get; set; }
        }

        [HttpPost]
        public HttpResponseMessage Login(LoginCredentials credentials)
        {
            int UserID = -1;

            if (IsValid(credentials.EmailAddress, credentials.Password, out UserID))
            {
                FormsAuthentication.SetAuthCookie(UserID.ToString(), false);
                return Request.CreateResponse(HttpStatusCode.OK, UserID);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
        }

        [HttpGet]
        public HttpResponseMessage Logout()
        {
            FormsAuthentication.SignOut();

            return Request.CreateResponse(HttpStatusCode.OK);
        }
        
        [HttpGet]
        public HttpResponseMessage IsAuthenticated()
        {
            HttpContext context = HttpContext.Current;

            if ((context == null) || (context.User == null) || (context.User.Identity == null))
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            else if (context.User.Identity.IsAuthenticated) 
            {
                return Request.CreateResponse(HttpStatusCode.OK, int.Parse(context.User.Identity.Name));
            }

            return Request.CreateResponse(HttpStatusCode.Unauthorized);
        }

        private bool IsValid(string emailAddress, string password, out int UserID)
        {
            var crypto = new SimpleCrypto.PBKDF2();
            bool isValid = false;
            int userID = -1;

            var user = db.Users.FirstOrDefault(u => u.EmailAddress == emailAddress);

            if (user != null)
            {
                if (user.Password == crypto.Compute(password, user.PasswordSalt))
                {
                    isValid = true;
                    userID = user.ID;
                }
            }

            UserID = userID;

            return isValid;
        }

    }
}
