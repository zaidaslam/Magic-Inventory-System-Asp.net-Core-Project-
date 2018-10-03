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



namespace Assignment2.Controllers
{
    /**
 * @author Zaid Aslam Shaikh (s3590683)
 * @author Swapnil Guha (s3587683)
 * This class is used to implement all the functionality related to Owner.
 *
 */

    [Authorize(Roles = Constants.OwnerRole)]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: OwnerInventory

        /** 
	 * 
	 * ******************************************************************************************
	 * This Action is used to display Owner Inventory.
	 * ******************************************************************************************
	 */

        public async Task<IActionResult> Index(string productName)
           {

            var query = _context.OwnerInventory.Include(x => x.Product).Select(x => x);

            if (!string.IsNullOrWhiteSpace(productName))
            {
                query = query.Where(x => x.Product.Name.Contains(productName));

                ViewBag.ProductName = productName;
            }

            query = query.OrderBy(x => x.Product.Name);

            return View(await query.ToListAsync());
        }

        /** 
   * 
   * ******************************************************************************************
   * This Action is used to display Stock Request.
   * ******************************************************************************************
   */

        public async Task<IActionResult> StockRequest()
        {
            var query = _context.StockRequests.Include(s => s.Product).Include(s => s.Store);
            return View(await query.ToListAsync());
        }

        /** 
   * 
   * ******************************************************************************************
   * This Action is used to Process the Stock Request based on Stock Request ID.
   * ******************************************************************************************
   */
        public IActionResult ProcessStockRequest(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var query = _context.StockRequests.Include(s => s.Product).Include(s => s.Store);
            var stock = query.Where(s => s.StockRequestID == id).SingleOrDefault();

            var request  = _context.OwnerInventory.Include(x => x.Product).Select(x => x);
            request = request.Where(s => s.ProductID == stock.ProductID);

            ViewData["ProductID"] = stock.StockRequestID;
            ViewData["ProductName"] = stock.Product.Name;
            ViewData["Quantity"] = stock.Quantity;
            ViewData["CurrentStock"] = request.SingleOrDefault().StockLevel;

            if (stock.Quantity <= request.SingleOrDefault().StockLevel)
            {
                ViewData["Availability"] = "True";
               
            }
            else {
                ViewData["Availability"] = "False";
            }

            return View();
        }

        /** 
          * 
          * ******************************************************************************************
          * This Action is used to Process the Stock Request based on Stock Request ID.
          * ******************************************************************************************
          */

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessStockRequest(int id)
        {
            var query = _context.StockRequests.Include(s => s.Product).Include(s => s.Store);
            var stock = query.Where(s => s.StockRequestID == id).SingleOrDefault();

            var request = _context.OwnerInventory.Include(x => x.Product).Select(x => x);
            request = request.Where(s => s.ProductID == stock.ProductID);

            var productID = stock.ProductID;
            var stocklevel = request.SingleOrDefault().StockLevel - stock.Quantity;

            var storeRequest = _context.StoreInventory.Include(s => s.Product).Include(s => s.Store); ;
            var store = storeRequest.Where(s => s.StoreID == stock.StoreID).Where(p => p.ProductID == stock.ProductID).SingleOrDefault();
            
            //Check StockLevel from Owner Inventory
            if (stock.Quantity <= request.SingleOrDefault().StockLevel)
            {
                
                  var storeInventory = new StoreInventory { StoreID = stock.StoreID, ProductID = stock.ProductID, StockLevel = stock.Quantity };
                //Update Store Inventory
                //Add New Item  
                if (store == null)
                {
                    _context.Add(storeInventory);
                }
                //Replenish Store Inventory StockLevel                
                else if (store != null) {
                    
                    StoreInventory stores = new StoreInventory();
                    stores = _context.StoreInventory.Find(stock.StoreID, stock.ProductID);
                    stores.StockLevel = (store.StockLevel + stock.Quantity);
                }
                _context.Remove(stock); //Upate Stock Request               
                OwnerInventory owner = new OwnerInventory();
                owner = _context.OwnerInventory.Find(productID);
                owner.StockLevel = stocklevel; //Update Owner Inventory
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(StockRequest));
            } else
            {
                ViewData["ProductID"] = stock.StockRequestID;
                ViewData["ProductName"] = stock.Product.Name;
                ViewData["Quantity"] = stock.Quantity;
                ViewData["CurrentStock"] = request.SingleOrDefault().StockLevel;
                ViewData["Availability"] = "False";
                ViewData["Aval"] = "Not enough stock";
            }
           
            return View();
        }

        /** 
         * 
         * ******************************************************************************************
         * This Action is used to Update Stock Level
         * ******************************************************************************************
         */

        // GET: OwnerInventory/Edit/5
        //[Authorize(Roles = Constants.WholeSaleRole)]
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var ownerInventory = await _context.OwnerInventory.SingleOrDefaultAsync(m => m.ProductID == id);
            await _context.Products.SingleOrDefaultAsync(m => m.ProductID == id); // assign vale to getter and setter

            if (ownerInventory == null)
            {
                return NotFound();
            }
            ViewData["ProductID"] = new SelectList(_context.Stores, "ProductID", "ProductID", ownerInventory.ProductID);
            return View(ownerInventory);
        }

        /** 
        * 
        * ******************************************************************************************
        * This Action is used to Update Stock Level
        * ******************************************************************************************
        */

        // POST: OwnerInventory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,StockLevel")] OwnerInventory ownerInventory)
        {
            if (id != ownerInventory.ProductID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ownerInventory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OwnerInventoryExists(ownerInventory.ProductID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductID", ownerInventory.ProductID);
            return View(ownerInventory);
        }

        private bool OwnerInventoryExists(int id)
        {
            return _context.OwnerInventory.Any(e => e.ProductID == id);
        }
    }
}
