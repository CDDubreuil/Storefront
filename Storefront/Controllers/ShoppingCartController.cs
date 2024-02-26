using Microsoft.AspNetCore.Mvc;

using Storefront.DATA.EF.Models;
using Microsoft.AspNetCore.Identity;
using Storefront.Models;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Storefront.UI.MVC.Controllers
{

	#region Steps to Implement Session Based Shopping Cart
	/*
    * Session is a tool available on the server-side that can store information for a user while they are actively using your site.
    * Typically the session lasts for 20 minutes (this can be adjusted in Program.cs).
    * Once the 20 minutes is up, the session variable is disposed.
    * 
    * Values that we can store in Session are limited to: string, int
    * - Because of this we have to get creative when trying to store complex objects (like CartItemViewModel).
    * To keep the info separated into properties we will convert the C# object to a JSON string.
    * */
	/*
     * 1) Register Session in program.cs (builder.Services.AddSession... && app.UseSession())
     * 2) Create the CartItemViewModel class in [ProjName].UI.MVC/Models folder
     * 3) Add the 'Add To Cart' button in the Index and/or Details view of your Products
     * 4) Create the ShoppingCartController (empty controller -> named ShoppingCartController)
     *      - add using statements
     *          - using CORE3.DATA.EF.Models;
     *          - using Microsoft.AspNetCore.Identity;
     *          - using CORE3.UI.MVC.Models;
     *          - using Newtonsoft.Json;
     *      - Add props for the GadgetStoreContext && UserManager
     *      - Create a constructor for the controller - assign values to context && usermanager
     *      - Code the AddToCart() action
     *      - Code the Index() action
     *      - Code the Index View
     *          - Start with the basic table structure
     *          - Show the items that are easily accessible (like the properties from the model)
     *          - Calculate/show the lineTotal
     *          - Add the RemoveFromCart <a>
     *      - Code the RemoveFromCart() action
     *          - verify the button for RemoveFromCart in the Index view is coded with the controller/action/id
     *      - Add UpdateCart <form> to the Index View
     *      - Code the UpdateCart() action
     *      - Add Submit Order button to Index View
     *      - Code SubmitOrder() action
     * */
	#endregion

	public class ShoppingCartController : Controller
	{
		private readonly StorefrontProjectContext _context;
		private readonly UserManager<IdentityUser> _userManager;
		private int orderId;

		public string CustomerId { get; private set; }

		public ShoppingCartController(StorefrontProjectContext context, UserManager<IdentityUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		public IActionResult Index()
		{
			var localCart = GetCart();
			if (!localCart.Any())
			{
				ViewBag.Message = "There are no items in your cart";
			}
			return View(localCart);
		}

		public IActionResult AddToCart(int id)
		{
			//get the cart.
			Dictionary<int, CartItemViewModel> localCart = GetCart();
			//If the new item already exists in the cart, update the qty. Otherwise, add the new item to the cart.

			//make sure the item exists
			Record? r = _context.Records.Find(id);
			if (r == null)
			{
				ViewBag.Message = "Invalid Product ID.";
				return RedirectToAction(nameof(Index));
			}

			//check if the item is already in the cart.
			if (localCart.ContainsKey(r.RecordId))
			{
				localCart[r.RecordId].Qty++;
			}
			else
			{

				localCart.Add(r.RecordId, new CartItemViewModel(1, r));
			}
			//Save the cart back to JSON
			SetCart(localCart);
			//Refresh the page.
			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult RemoveFromCart(int id)
		{
			Dictionary<int, CartItemViewModel> localCart = GetCart();
			if (localCart.ContainsKey(id))
			{
				localCart.Remove(id);
			}
			SetCart(localCart);

			return RedirectToAction(nameof(Index));
			//return RedirectToAction("Index");
		}

		[HttpPost]
		public IActionResult UpdateCart(int id, int qty)
		{
			if (qty <= 0)
			{
				RemoveFromCart(id);//without return RedirectToAction, invoking an IActionResult acts as a void method.
			}
			var localCart = GetCart();
			if (localCart.ContainsKey(id))
			{
				localCart[id].Qty = qty;
			}
			SetCart(localCart);

			return RedirectToAction(nameof(Index));
		}

		//GET
		[Authorize]
		public async Task<IActionResult> Checkout()
		{
			//If there aren't any items in the cart, send them back to the Index.
			if (!GetCart().Any())
			{
				return RedirectToAction(nameof(Index));
			}
			var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var cd = await _context.CustomerData.FindAsync(customerId);
			//populate the model with default info from the UserDetail
			CheckoutViewModel model = new()
			{
				FullName = cd?.FullName,
				CustomerCity = cd?.CustomerCity,
				CustomerState = cd?.CustomerState,
				CustomerZip = cd?.CustomerZip
			};
			return View(model);
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> Checkout(CheckoutViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			//Create the Order Object and assign values
			Order o = new()
			{
				OrderId = orderId,
				CustomerId = CustomerId,
				OrderDate = DateTime.Now
			};
			var localCart = GetCart();
			_context.Orders.Add(o);//add our order to the context before populating the OrderProducts so that we have an OrderId to use.
			foreach (var item in localCart.Values)
			{
				//for every CartItemViewModel in the Dictionary Cart, create an OrderProduct object
				RecordOrder ro = new()
				{
					OrderId = o.OrderId,
					RecordId = item.Record.RecordId
				};
				//Add each of those OrderProducts to the collection property of the Order
				o.RecordOrders.Add(ro);
			}
			//commit those changes to the DB
			await _context.SaveChangesAsync();

			SetCart(new());//send an empty cart back to the session.

			return RedirectToAction(nameof(Index), "Orders");
		}
		#region Json Methods

		private Dictionary<int, CartItemViewModel> GetCart()
		{
			//Retrieve the contents from the Session shopping cart (JSON) and convert those to C#
			var jsonCart = HttpContext.Session.GetString("cart");//name of the session variable
			ViewBag.Session = jsonCart;
			if (string.IsNullOrEmpty(jsonCart))
			{
				//If a cart doesn't currently exist, return an empty Dictionary cart
				return new();
			}

			//if a session cart does exist, deserialize it and return the Dictionary.
			return JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(jsonCart);
		}

		private void SetCart(Dictionary<int, CartItemViewModel> localCart)
		{
			if (!localCart.Any()) // localCart.Count == 0
			{
				//if there isn't anything in the session variable, remove it!
				HttpContext.Session.Remove("cart");
			}
			else
			{
				//if there is, turn the cart into json and store it in the session variable.
				var jsonCart = JsonConvert.SerializeObject(localCart);
				HttpContext.Session.SetString("cart", jsonCart);
			}
		}
		#endregion
	}
}
