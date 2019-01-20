
using ParkManagement2.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ParkManagement.Controllers
{
    public class ReportController : ApiController
    {

        ISTParkManagementEntities DB;
        public ReportController()
        {
            DB = new ISTParkManagementEntities();
        }


        public IEnumerable<Park> LoadParks()
        {
            return DB.Parks;
        }

        // GET: api/Report
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Report/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Report
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Report/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Report/5
        public void Delete(int id)
        {
        }
    }
}
