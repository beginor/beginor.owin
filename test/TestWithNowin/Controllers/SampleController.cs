using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using Microsoft.Owin.Security.Cookies;

namespace TestWithNowin.Controllers {

    [RoutePrefix("api/sample")]
    public class SampleController : ApiController {

        [HttpGet, Route("anonymous")]
        public IHttpActionResult Anonymous() {
            return Ok("result from anonymous action");
        }

        [HttpGet, Route("security"), Authorize]
        public IHttpActionResult Security() {
            return Ok("result from security action, your name is: " + User.Identity.Name);
        }


        [HttpGet, Route("login/{user}")]
        public IHttpActionResult Login(string user) {
            var authMgr = Request.GetOwinContext().Authentication;

            var claim = new Claim(ClaimTypes.Name, user);
            var identity = new ClaimsIdentity(new[] { claim }, CookieAuthenticationDefaults.AuthenticationType);
            authMgr.SignIn(identity);
            return Ok(string.Format("user {0} logged in!", user));
        }
    }
}
