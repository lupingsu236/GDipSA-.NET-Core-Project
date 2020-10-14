using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Layout.Db;
using Layout.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Layout.Controllers
{
    public class LoginController : Controller
    {
        private readonly Database db;

        public LoginController(Database db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            ViewData["Is_Login"] = "menu_highlight";
            ViewData["Title"] = "Login";
            return View();
        }
        public IActionResult Authenticate(string username, string password)
        {
            User user = db.Users.FirstOrDefault(x => x.Username == username && x.Password == password);

            if (user == null)
            {
                ViewData["lasttypedusername"] = username;
                ViewData["errMsg"] = "Incorrect username or password.";
                ViewData["Is_Login"] = "menu_highlight";
                return View("Index");
            }
            else
            {
                Session session = new Session()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = user.Id,
                    Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds()
                };
                db.Sessions.Add(session);
                db.SaveChanges();

                Response.Cookies.Append("sessionId", session.Id);

                string currentGuestId = Request.Cookies["guestId"];
                User currentGuestUser = db.Users.FirstOrDefault(x => x.Id == currentGuestId);
                Cart cartMadeByGuest = null;

                if (currentGuestUser!=null)
                {
                    cartMadeByGuest = db.Carts.FirstOrDefault(x => x.UserId == currentGuestUser.Id);
                    db.Remove(currentGuestUser);
                    db.SaveChanges();
                }

                //if guest user did have items in cart
                if (cartMadeByGuest != null)
                {
                    List<CartDetail> cartDetailsMadeByGuest = cartMadeByGuest.CartDetails.ToList();

                    Cart existingCart = db.Carts.FirstOrDefault(x => x.UserId == user.Id);

                    //if logged in user does not have existing cart
                    if (existingCart == null)
                    {
                        //assign logged in user id to the cart made originally as guest
                        cartMadeByGuest.UserId = user.Id;

                        foreach (CartDetail cd in cartDetailsMadeByGuest)
                        {
                            cd.UserId = user.Id;
                        }
                    }

                    //logged in user has an existing cart
                    else
                    {
                        //retrieve all cart details of the existing cart
                        List<CartDetail> cartDetailsOfExistingCart = existingCart.CartDetails.ToList();

                        //compare cart details in existing cart and cart details in cart made while as guest
                        //and merge into one cart
                        foreach (CartDetail cd1 in cartDetailsOfExistingCart)
                        {
                            foreach (CartDetail cd2 in cartDetailsMadeByGuest)
                            {
                                if(cd1.ProductId == cd2.ProductId)
                                {
                                    cd1.Quantity = cd1.Quantity + cd2.Quantity;

                                    //product in cart made while as guest has been combined to user's existing cart
                                    //remove this product/cart detail from the database
                                    db.Remove(cd2);
                                    db.SaveChanges();
                                    break;
                                }
                            }
                        }


                        //as user's existing cart may not have all products same as products in cart made while as guest
                        //account for the remaining products in the cart made while as guest
                        List<CartDetail> remainingCartDetailsMadeByGuest = cartMadeByGuest.CartDetails.ToList();
                        foreach (CartDetail cd in remainingCartDetailsMadeByGuest)
                        {
                            int tempProductId = cd.ProductId;
                            int tempQuantity = cd.Quantity;
                            db.Remove(cd);
                            db.SaveChanges();
                            CartDetail temp = new CartDetail
                            {
                                CartId = existingCart.Id,
                                ProductId = tempProductId,
                                Quantity = tempQuantity,
                                UserId = user.Id
                            };
                            db.Add(temp);
                            db.SaveChanges();
                        }
                        db.Remove(cartMadeByGuest);
                    }
                }

                Response.Cookies.Delete("guestId");

                return RedirectToAction("Index", "Home");
            }
        }
        public IActionResult NoEntry()
        {
            return View();
        }
    }
}
