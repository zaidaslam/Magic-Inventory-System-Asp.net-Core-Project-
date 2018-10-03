using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Assignment2.Data;
using Assignment2.Models;
using Microsoft.AspNetCore.Identity;
using CreditCardValidator;
using Newtonsoft.Json;
using Assignment2;

namespace Assignment2.Controllers
{

    /**
* @author Zaid Aslam Shaikh (s3590683)
* @author Swapnil Guha (s3587683)
* This class is used to implement all the functionality related to Customer.
*
*/
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _UserManagercontext;
        public CustomerController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _UserManagercontext = userManager;
        }

        /** 
  * 
  * ******************************************************************************************
  * This Action is used to display Store Inventory and Filter Product by Product Name.
  * ******************************************************************************************
  */
        // GET: Customer
        public async Task<IActionResult> Index(int storeID, string productName, string currentFilter, int? page)
        {
            ViewData["StoreID"] = new SelectList(_context.Stores, "StoreID", "Name", storeID);

            if (storeID == 0)
            {
                storeID = 1;
            }
            else
            {
                page = 1;
            }
            var storeName = _context.Stores
                              .Where(at => at.StoreID == storeID)
                              .Select(at => at.Name)
                              .SingleOrDefault() ?? string.Empty;

            ViewData["StoreName"] = storeName;

            var assignment2Context = _context.StoreInventory.Include(s => s.Product).Include(s => s.Store);

            if (productName != null)
            {
                page = 1;
            }
            else
            {
                productName = currentFilter;
            }
            
            if (!string.IsNullOrWhiteSpace(productName))
            {
                int pagesize = 3;
            
                var product = assignment2Context.Where(x => x.Product.Name.Contains(productName)).Where(x => x.StoreID == storeID);

                ViewBag.ProductName = productName;

                return View(await PaginatedList<StoreInventory>.CreateAsync(product.AsNoTracking(), page ?? 1, pagesize));
            }

            var query = assignment2Context.Where(s => s.Store.StoreID == storeID);
            int pageSize = 3;
            return View(await PaginatedList<StoreInventory>.CreateAsync(query.AsNoTracking(), page ?? 1, pageSize));
        }

        /** 
  * 
  * ******************************************************************************************
  * This Action is used to Add Product to Cart.
  * ******************************************************************************************
  */

        // GET: Customer/Edit/5
        public IActionResult Edit(int? id, int? store)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewData["StoreID"] = store;
            ViewData["ProductID"] = id;
            ViewData["ProductName"] = _context.Products.Where(p => p.ProductID == id).SingleOrDefault().Name;
            ViewData["StoreName"] = _context.Stores.Where(s => s.StoreID == store).SingleOrDefault().Name;
            return View();
        }

        /** 
 * 
 * ******************************************************************************************
 * This Action is used to Add Product to Cart.
 * ******************************************************************************************
 */

        // POST: Customer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int store, [Bind("StoreID,ProductID,StockLevel")] StoreInventory storeInventory)
        {
            ViewData["StoreID"] = store;
            ViewData["ProductID"] = id;
            ViewData["ProductName"] = _context.Products.Where(p => p.ProductID == id).SingleOrDefault().Name;
            ViewData["StoreName"] = _context.Stores.Where(s => s.StoreID == store).SingleOrDefault().Name;

            var user = await _UserManagercontext.GetUserAsync(User);
            var userid = user.Id;

            if (ModelState.IsValid)
            {
                try
                {
                    var cart = new Cart { CustomerID = userid, ProductID = id, StoreID = store, Quantity = storeInventory.StockLevel };


                    var checkItem = _context.Cart.Where(x => x.CustomerID == userid).Where(x => x.StoreID == store).Where(x => x.ProductID == id).SingleOrDefault();
                    if (checkItem == null)
                    {
                        _context.Add(cart);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Cart));
                    }
                    else
                    {
                        ModelState.AddModelError("StockLevel", "Item already present in Cart");
                        ViewData["ProductName"] = _context.Products.Where(p => p.ProductID == id).SingleOrDefault().Name;
                        ViewData["StoreName"] = _context.Stores.Where(s => s.StoreID == store).SingleOrDefault().Name;
                        return View(storeInventory);
                    }

                }
                catch (DbUpdateConcurrencyException)
                {

                }
            }
            return View();
        }

        /** 
 * 
 * ******************************************************************************************
 * This Action is used to Edit Cart.
 * ******************************************************************************************
 */

        // GET: Customer/Edit/5
        public IActionResult EditCart(int? id, int? store)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewData["StoreID"] = store;
            ViewData["ProductID"] = id;
            ViewData["ProductName"] = _context.Products.Where(p => p.ProductID == id).SingleOrDefault().Name;
            ViewData["StoreName"] = _context.Stores.Where(s => s.StoreID == store).SingleOrDefault().Name;
            return View();
        }

        /** 
* 
* ******************************************************************************************
* This Action is used to Edit Cart.
* ******************************************************************************************
*/

        // POST: Customer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCart(int id, int store, [Bind("CustomerID,StoreID,ProductID,Quantity")] Cart carts)
        {
            ViewData["StoreID"] = store;
            ViewData["ProductID"] = id;
            ViewData["ProductName"] = _context.Products.Where(p => p.ProductID == id).SingleOrDefault().Name;
            ViewData["StoreName"] = _context.Stores.Where(s => s.StoreID == store).SingleOrDefault().Name;

            var user = await _UserManagercontext.GetUserAsync(User);
            var userid = user.Id;

            if (ModelState.IsValid)
            {
                try
                {
                    {
                        Cart custCart = new Cart();
                        custCart = _context.Cart.Where(s => s.CustomerID == userid).Where(s => s.ProductID == id). Where(s => s.StoreID == store).SingleOrDefault();
                        custCart.Quantity = carts.Quantity;
                        await _context.SaveChangesAsync();

                    }

                }
                catch (DbUpdateConcurrencyException)
                {

                }
            }
            return RedirectToAction(nameof(Cart));
        }

        /** 
* 
* ******************************************************************************************
* This Action is used to Display Customer Cart.
* ******************************************************************************************
*/

        public async Task<IActionResult> Cart()
        {
            var user = await _UserManagercontext.GetUserAsync(User);
            var userid = user.Id;

            // Eager loading the Product table - join between OwnerInventory and the Product table.
            var query = _context.Cart.Include(x => x.Product).Include(x => x.Store).Where(x => x.CustomerID == userid).Select(x => x);
            
            // Adding an order by to the query for the Product name.
            query = query.OrderBy(x => x.StoreID); 

            if (query.Count() == 0)
            {
                ViewData["check"] = "Cart is Empty";
            }
                return View(await query.ToListAsync());
        }

        /** 
* 
* ******************************************************************************************
* This Action is used to Delete Cart Item.
* ******************************************************************************************
*/

        public async Task<IActionResult> Delete(int id, int store)
        {
            var user = await _UserManagercontext.GetUserAsync(User);
            var userid = user.Id;

            Cart custCart = new Cart();
            custCart = _context.Cart.Where(s => s.CustomerID == userid).Where(s => s.ProductID == id).Where(s => s.StoreID == store).SingleOrDefault();
            _context.Cart.Remove(custCart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Cart));
        }

        /** 
* 
* ******************************************************************************************
* This Action is used to Validate Credit Card Details.
* ******************************************************************************************
*/

        public async  Task<IActionResult> CreditCard()
        {
            var user = await _UserManagercontext.GetUserAsync(User);
            var userid = user.Id;

            var query = _context.Cart.Include(x => x.Product).Include(x => x.Store).Where(x => x.CustomerID == userid).Select(x => x);
            
            // Adding an order by to the query for the Product name.
            query = query.OrderBy(x => x.StoreID);

            if (query.Count() == 0)
            {
                ViewData["check"] = "Cart is Empty";
                return RedirectToAction(nameof(Cart));
            }
            else
            {
                return View();
            }
            }

        /** 
* 
* ******************************************************************************************
* This Action is used to Display Order Receipt.
* ******************************************************************************************
*/

        public async Task<IActionResult> OrderConfirmation(String creditCard)
        {
            CreditCardDetector detector = new CreditCardDetector(creditCard);

            if (detector.IsValid())
            {
                var user = await _UserManagercontext.GetUserAsync(User);
                var userid = user.Id;
                bool save = true;
                var query = _context.Cart.Where(x => x.CustomerID == userid).Select(x => x);
                var storeInventory = _context.StoreInventory.Include(s => s.Product).Select(x => x);
                var orderID = 1;
                var lastOrderID = _context.OrdersHistory.FirstOrDefault();

                if (lastOrderID == null)
                {
                    orderID = 1;
                }
                else
                {
                    var lastOrder = _context.OrdersHistory.Select(x => x).ToList();
                    orderID = lastOrder.Count() + 1;
                }
               
                List<string> list = new List<string>();
                OrderHistory odrHist = new OrderHistory { OrderID = orderID, CustomerID = userid };
                _context.Add(odrHist);

                foreach (var item in query) {

                    foreach (var product in storeInventory) {
                        if (product.StoreID == item.StoreID) {
                            if (product.ProductID == item.ProductID) {
                                if (item.Quantity <= product.StockLevel)
                                {
                                  
                                    Order ord = new Order { OrderID = orderID, ProductID = item.ProductID, StoreID = item.StoreID, Quantity = item.Quantity };
                                    _context.Add(ord);
                                    product.StockLevel = (product.StockLevel - item.Quantity);
                                    Cart custCart = new Cart();
                                    custCart = _context.Cart.Where(s => s.CustomerID == userid).Where(s => s.ProductID == product.ProductID).Where(s => s.StoreID == product.StoreID).SingleOrDefault();
                                    _context.Cart.Remove(custCart);

                                }
                                else {
                                    save = false;                                
                                    string str = product.Product.Name.ToString();                                   
                                    list.Add(str);
                                }
                            }
                        }
                    }

                }
                if (save)
                {

                    await _context.SaveChangesAsync();
                    var confirm = _context.Orders.Include(s => s.Product).Include(s => s.Store).Where(s => s.OrderID == orderID).Select(x => x).OrderBy(s => s.StoreID);
                    ViewData["Creditcheck"] = orderID;
                    return View(confirm.ToList());
                }
                else {                   
                    return View("InsufficientQuantity",list);
                } 
            }
                        
            else
            {
                ViewData["check"] = creditCard;
                ViewData["Message"] = "Invalid Credit Card Number";
                return View("CreditCard", creditCard);
            }
            
        }

        /** 
* 
* ******************************************************************************************
* This Action is used to Display Order History. Using WebApi
* ******************************************************************************************
*/
        public async Task<IActionResult> OrderHistory()
        {
            var user = await _UserManagercontext.GetUserAsync(User);
            var userid = user.Id;

            string path = "api/values/"+userid;
            var response = await Helper.InitializeClient().GetAsync(path);

            if (!response.IsSuccessStatusCode)
                throw new Exception();

            // Storing the response details recieved from web api.
            var result = response.Content.ReadAsStringAsync().Result;

            // Deserializing the response recieved from web api and storing into a list.
            var orderHistory = JsonConvert.DeserializeObject<List<int>>(result);

            return View(orderHistory);
        }


        /** 
* 
* ******************************************************************************************
* This Action is used to Display Order History Details. Using WebApi
* ******************************************************************************************
*/
        public async Task<IActionResult> OrderDetails(int id)
        {
            string path = "api/values/order/" + id;
            var response = await Helper.InitializeClient().GetAsync(path);

            if (!response.IsSuccessStatusCode)
                throw new Exception();

            // Storing the response details recieved from web api.
            var result = response.Content.ReadAsStringAsync().Result;

            // Deserializing the response recieved from web api and storing into a list.
            var orderHistory = JsonConvert.DeserializeObject<List<Order>>(result);

            return View(orderHistory);
        }

        private bool StoreInventoryExists(int id)
        {
            return _context.StoreInventory.Any(e => e.StoreID == id);
        }
    }
}
