//using Microsoft.AspNetCore.Mvc;
//using Storefront.DATA.EF.Models;
//using Microsoft.AspNetCore.Identity;
//using Newtonsoft.Json;
//using Storefront.Models;

//namespace Storefront.UI.MVC.Controllers { 


//    public class ShoppingCartController : Controller
//{
//    private readonly StorefrontProjectContext _context;
//    private readonly UserManager<IdentityUser> _userManager;
//    public ShoppingCartController(StorefrontProjectContext context, UserManager<IdentityUser> userManager)
//        {
//            _context = context;
//            _userManager = userManager;
//        }
//        public IActionResult Index()
//        {
//            var localCart = GetCart();
//            if (!localCart.Any())
//            {
//                ViewBag.Message = "There are no items in your cart";
//            }
//            return View(localCart);
//        }

//        public IActionResult AddToCart(int id)
//        {
//            Dictionary<int, CartItemViewModel> localCart = GetCart();
//            Record? r = _context.Records.Find(id);
//            if (r == null)
//            {
//                ViewBag.Message = "Invalid product ID";
//                return RedirectToAction(nameof(Index));
//            }

//            if(localCart.ContainsKey(r.RecordId))
//            {
//                localCart[r.RecordId].Qty++;
//            }
//            else
//            {
//                    localCart.Add(r.RecordId, new CartItemViewModel(1, r));
//            }
//            SetCart(localCart);
//            return RedirectToAction(nameof(Index));
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult RemoveFromCart(int id)
//        {
//            Dictionary<int, CartItemViewModel> localCart = GetCart();
//            if(localCart.ContainsKey(id))
//            {
//                localCart.Remove(id);
//            }
//            SetCart(localCart);
//            return RedirectToAction(nameof(Index));
//        }

//        [HttpPost]
//        public IActionResult UpdateCart(int id, int qty)
//        {
//            if (qty <= 0)
//            {
//                RemoveFromCart(id);
//            }
//            var localCart = GetCart();
//            if (localCart.ContainsKey(id))
//            {
//                localCart[id].Qty = qty;
//            }
//            SetCart(localCart);
//            return RedirectToAction(nameof(Index));
//        }

//        [Authorize]
//        public async Task<IActionResult> Checkout()
//        {

//        }

//        private string? GetCart()
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
////}