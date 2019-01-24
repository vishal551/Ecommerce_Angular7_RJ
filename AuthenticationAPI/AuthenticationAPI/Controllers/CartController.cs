using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using AuthenticationAPI.Models;
using Microsoft.AspNet.Identity;

namespace AuthenticationAPI.Controllers
{
    [RoutePrefix("api/Cart")]
    public class CartController : ApiController
    {
        [HttpPost]
        [Authorize]
        [Route("AddToCart")]
        public IHttpActionResult AddToCart(Cart_Master C)
        {
            var DB = new AngularDemoEntities();
            try
            {
                if (C != null)
                {
                    var user_id = User.Identity.GetUserId();
                    bool IsThere = DB.Cart_Master.Any(x => x.ProductID == C.ProductID && x.UserID == user_id);
                    if (!IsThere)
                    {
                        C.UserID = User.Identity.GetUserId();
                        DB.Cart_Master.Add(C);
                        var Products = DB.Product_Master.Where(x => x.ProductID == C.ProductID).SingleOrDefault();
                        if (Products != null)
                        {
                            Products.ProductQuantity--;
                        }
                        DB.SaveChanges();
                        return Ok("success");
                    }
                    else
                    {
                        return Ok("already");
                    }
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }
        [HttpGet]
        [Authorize]
        [Route("CartItemTolal")]
        public IHttpActionResult CartItemTolal()
        {
            var DB = new AngularDemoEntities();
            try
            {
                var user_id = User.Identity.GetUserId();
                int TotalCartItems = DB.Cart_Master.Where(x => x.UserID == user_id).Count();
                return Ok(TotalCartItems);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        [HttpGet]
        [Authorize]
        [Route("CartItems")]
        public IHttpActionResult CartItems()
        {
            try
            {
                var user_id = User.Identity.GetUserId();
                var DB = new AngularDemoEntities();
                var Cart_products = DB.Cart_Master.Where(x => x.UserID == user_id).Select(y => new
                {
                    y.CartID,
                    y.ProductQuantity,
                    y.ProductID,
                    y.Product_Master.ProductName,
                    y.Product_Master.ProductPrice,
                    y.Product_Master.ProductImage,
                    y.UserID,
                    y.Product_Master.ProductDescription,
                    y.Product_Master.InStock
                }).ToList();
                return Ok(Cart_products);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        [HttpDelete]
        [Authorize]
        [Route("RemoveCartItem/{ID}")]
        public IHttpActionResult RemoveCartItem(int ID)
        {
            var user_id = User.Identity.GetUserId();
            var DB = new AngularDemoEntities();
            try
            {
                var Cart_item = DB.Cart_Master.Where(x => x.CartID == ID).FirstOrDefault();
                if (Cart_item != null)
                {
                    DB.Cart_Master.Remove(Cart_item);
                    DB.SaveChanges();
                    return Ok("success");
                }
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        [HttpGet]
        [Authorize]
        [Route("UpdateQty/{ID}/{Qty}")]
        public IHttpActionResult UpdateQty(int ID, int Qty)
        {
            var user_id = User.Identity.GetUserId();
            var DB = new AngularDemoEntities();
            try
            {
                var Cart_item = DB.Cart_Master.Where(x => x.CartID == ID).FirstOrDefault();
                if (Cart_item != null)
                {
                    var Product = DB.Product_Master.Where(x => x.ProductID == Cart_item.ProductID).FirstOrDefault();
                    if (Qty < Product.ProductQuantity)
                    {
                        if (Qty <= Product.ProductQuantity && Qty >= Cart_item.ProductQuantity)
                        {
                            Product.ProductQuantity -= Math.Abs(Qty);
                            Cart_item.ProductQuantity += Math.Abs(Qty);
                            DB.SaveChanges();
                            return Ok("success");
                        }
                        else
                        {
                            Product.ProductQuantity -= Qty;
                            Cart_item.ProductQuantity += Qty;
                            DB.SaveChanges();
                            return Ok("success");
                        }
                    }
                    else
                    {
                        return Ok("OutOfStock");
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        [HttpPost]
        [Authorize]
        [Route("Checkout")]
        public IHttpActionResult Checkout(Order_Master O)
        {
            var DB = new AngularDemoEntities();
            try
            {
                var user_id = User.Identity.GetUserId();
                O.OrderDate = DateTime.Now;
                O.DeliveryDate = DateTime.Now.AddDays(5);
                O.UserID = user_id;
                DB.Order_Master.Add(O);
                DB.SaveChanges();
                int InsertedOrderID = O.OrderID;
                List<Cart_Master> C = DB.Cart_Master.Where(x => x.UserID == user_id).ToList();
                if (C.Count > 0)
                {
                    foreach(var i in C)
                    {
                        OrderItems_Master OI = new OrderItems_Master
                        {
                            OrderID = InsertedOrderID,
                            ProductID = Convert.ToInt32(i.ProductID),
                            Quantity = Convert.ToDecimal(i.ProductQuantity),
                            UserID = i.UserID,
                            UnitPrice = Convert.ToDecimal(i.Product_Master.ProductPrice)
                        };
                        DB.OrderItems_Master.Add(OI);
                        DB.Cart_Master.Remove(i);
                        DB.SaveChanges();
                    }
                    return Ok("success");
                }
                else
                {
                    return Ok("Some problem");
                }               
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        [HttpGet]
        [Authorize]
        [Route("GetStates")]
        public IHttpActionResult GetStates()
        {
            var DB = new AngularDemoEntities();
            try
            {
                var states = DB.States_Master.OrderBy(x => x.StateName).Select(y => new
                {
                    y.StateID,
                    y.StateName
                }).ToList();
                return Ok(states);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }


        [HttpGet]
        [Authorize]
        [Route("GetCity/{ID}")]
        public IHttpActionResult GetCity(int ID)
        {
            var DB = new AngularDemoEntities();
            try
            {
                var City = DB.City_Master.Where(x => x.StateID == ID).OrderBy(x => x.CityName).Select(y => new
                {
                    y.CityID,
                    y.CityName
                }).ToList();
                return Ok(City);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }
    }
}
