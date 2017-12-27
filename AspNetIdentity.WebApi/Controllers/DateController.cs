namespace AspNetIdentity.WebApi.Controllers
{
    using System.Linq;
    using System.Web.Http;

    using AspNetIdentity.WebApi.Models;

    [RoutePrefix("api/date")]
    public class DateController : BaseApiController
    {
        private readonly XAppDbContext db;

        public DateController(XAppDbContext dbContext)
        {
            this.db = dbContext;
        }
        // POST api/<controller>
        public IHttpActionResult Post([FromBody]DateViewModel model)
        {
            var order = this.db.UserOrders.FirstOrDefault();
            var oldDate = order.CreationDate;

            order.CreationDate = model.Date;
            this.db.SaveChanges();

            var updatedOrder = this.db.UserOrders.Find(order.Id);
            
            return this.Ok(new
            {
               model.Date,
               model.Epoch,
               model.DateString,
               model.DateTimeFromString,
               model.DateTimeFromEpoch,
               DateAsString = model.Date.ToString(),
               DateUtc = model.Date.ToUniversalTime(),
               order = new {
                    OldDate = oldDate,
                    NewDate = updatedOrder.CreationDate
               }
            });
        }
    }
}
