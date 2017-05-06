using System.Net;
using System.Net.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

namespace Beginor.Owin.WebApi.Windsor {

    public class ExceptionHandler : System.Web.Http.ExceptionHandling.ExceptionHandler {

        public override void Handle(ExceptionHandlerContext context) {
            var request = context.Request;
            var ex = context.Exception;
            var response = request.CreateErrorResponse(
                HttpStatusCode.InternalServerError,
                ex.Message
            );
            var result = new ResponseMessageResult(response);
            context.Result = result;
        }

    }

}
