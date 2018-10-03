using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assignment2WebApi.Data;
using Assignment2WebApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Assignment2WebApi.Controllers
{
    /**
* @author Zaid Aslam Shaikh (s3590683)
* @author Swapnil Guha (s3587683)
* This class is used to implement WebApi.
*
*/

    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ValuesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<OrderHistory> Get()
        {
            var query = _context.OrdersHistory.Select(x => x);
            List<OrderHistory> list = new List<OrderHistory>();

            return query;
        }

        //GET api/values/5
        [HttpGet("{custid}")]
        public IEnumerable<int> Get(string custid)
        {
            var query = _context.OrdersHistory.Where(s => s.CustomerID.Equals(custid)).Select(x => x.OrderID);
            return query.ToList();
        }

        // GET api/values/5

        //[HttpGet("{orderId:int}")]
        [HttpGet("order/{orderId:int}")]
        public IEnumerable<Order> Get(int orderId)
        {
            var query = _context.Orders.Include(s => s.Product).Include(s => s.Store).Where(s => s.OrderID == orderId).Select(x => x).OrderBy(s => s.StoreID);
            return query;
        }

    }
}
