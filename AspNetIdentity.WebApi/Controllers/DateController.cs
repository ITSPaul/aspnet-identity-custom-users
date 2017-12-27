namespace AspNetIdentity.WebApi.Controllers
{
    using System.Web.Http;

    using AspNetIdentity.WebApi.Models;

    [RoutePrefix("api/date")]
    public class DateController : BaseApiController
    {
        // POST api/<controller>
        public IHttpActionResult Post([FromBody]DateViewModel model)
        {
            return this.Ok(new
            {
               model.Date,
               model.Epoch,
               model.DateString,
               model.DateTimeFromString,
               model.DateTimeFromEpoch,
               DateAsString = model.Date.ToString()
            });
        }
    }
}
