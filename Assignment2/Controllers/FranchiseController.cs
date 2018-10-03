using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Assignment2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Assignment2.Data;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Assignment2.Controllers
{

    /**
* @author Zaid Aslam Shaikh (s3590683)
* @author Swapnil Guha (s3587683)
* This class is used to implement all the functionality related to Franchise.
*
*/

    [Authorize(Roles = Constants.FranchiseRole)]
    public class FranchiseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _UserManagercontext;

        public FranchiseController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _UserManagercontext = userManager;
        }

        /** 
   * 
   * ******************************************************************************************
   * This Action is used to display Franchise Inventory.
   * ******************************************************************************************
   */

        // GET: Franchise
        public async Task<IActionResult> Index(int storeID)
        {
            var user = await _UserManagercontext.GetUserAsync(User);

                ViewData["StoreID"] = new SelectList(_context.Stores, "StoreID", "Name", storeID);

            if (storeID == 0)
            {
                storeID = 1;
            }
            var storeName = _context.Stores
                              .Where(at => at.StoreID == storeID)
                              .Select(at => at.Name)
                              .SingleOrDefault() ?? string.Empty;

                ViewData["StoreName"] = storeName;
            if (user.StoreID == storeID)
            {
                var assignment2Context = _context.StoreInventory.Include(s => s.Product).Include(s => s.Store);
                var query = assignment2Context.Where(s => s.Store.StoreID == storeID);
                return View(await query.ToListAsync());
            }
            else {
                return View();
            }
        }

        /** 
   * 
   * ******************************************************************************************
   * This Action is used to Add new Item.
   * ******************************************************************************************
   */

        public async Task<IActionResult> AddNewItem(int storeID)
        {
            var user = await _UserManagercontext.GetUserAsync(User);
            
            ViewData["StoreID"] = new SelectList(_context.Stores, "StoreID", "Name", storeID);

            if (storeID == 0)
            {
                storeID = 1;
            }
            ViewData["StID"] = storeID;

            if (user.StoreID == storeID)
            {
                var sProducts = _context.StoreInventory.Where(x => x.StoreID == storeID).Select(x => x.ProductID).ToArray();
            List<int> pIds = new List<int>(sProducts);

            var query = _context.OwnerInventory.Include(x => x.Product).Select(x => x);
            query = query.Where(x => !pIds.ToList().Contains(x.ProductID));

            return View(query.ToList());
            }
            else
            {
                return View();
            }
        }

        /** 
 * 
 * ******************************************************************************************
 * This Action is used to Add new Item.
 * ******************************************************************************************
 */

        public IActionResult Add(int? id, int? store, int? stocklevel, string name)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            ViewData["ProductID"] = id;
            ViewData["StoreID"] = store;
            ViewData["ProductName"] = name;

            return View();
        }

        /** 
 * 
 * ******************************************************************************************
 * This Action is used to Add new Item.
 * ******************************************************************************************
 */

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int id, int store, int stocklevel, [Bind("ProductID,StoreID,StockLevel")] StoreInventory storeInventory)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var stock_request = new StockRequest { ProductID = id, Quantity = stocklevel, StoreID = store };
                    _context.Add(stock_request);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                }
                ViewData["Message"] = "Stock Request Created";
            }

            return View();
        }

        /** 
 * 
 * ******************************************************************************************
 * This Action is used to create Stock Request.
 * ******************************************************************************************
 */
        // GET: Franchise/Edit/5
        [Authorize(Roles = Constants.FranchiseRole)]
        public IActionResult Edit(int? id,int? store, int? stocklevel)
        {
        
            if (id == null)
            {
                return NotFound();
            }

            var query = _context.StoreInventory.Include(x => x.Product).Select(x => x);
            query = query.Where(x => x.StoreID == store);
            query = query.Where(x => x.ProductID == id);
            if (query.ToList() == null)
            {
                return NotFound();
            }
                ViewData["ProductID"] = query.First().ProductID;
                ViewData["StoreID"] = query.First().StoreID;
                ViewData["ProductName"] = query.First().Product.Name;
            

            return View();
        }

        /** 
* 
* ******************************************************************************************
* This Action is used to create Stock Request.
* ******************************************************************************************
*/

        // POST: Franchise/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constants.FranchiseRole)]
        public async Task<IActionResult> Edit(int id, int store, int stocklevel, [Bind("ProductID,StoreID,StockLevel")] StoreInventory storeInventory)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var stock_request = new StockRequest { ProductID = id , Quantity = stocklevel , StoreID = store};
                   _context.Add(stock_request);
                   await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                }
                ViewData["Message"] = "Stock Request Created";
            }
            
            return View();
        }

        private bool StoreInventoryExists(int id)
        {
            return _context.StoreInventory.Any(e => e.StoreID == id);
        }
    }
}
