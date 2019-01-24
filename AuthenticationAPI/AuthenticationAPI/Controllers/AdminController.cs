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
    [RoutePrefix("api/Admin")]
    public class AdminController : ApiController
    {
        [HttpGet]
        [Authorize]
        [Route("GetAllCount")]
        public IHttpActionResult GetAllCount()
        {
            var DB = new AngularDemoEntities();
            var user_id = User.Identity.GetUserId();
            try
            {
                int TotalUsers = DB.AspNetUsers.Count();
                int TotalOrders = DB.Order_Master.Count();
                int TotalProducts = DB.Product_Master.Count();

                IDictionary<string, int> C = new Dictionary<string, int>()
                                            {
                                                {"TotalRegisteredUser",TotalUsers},
                                                {"TotalOrders", TotalOrders},
                                                {"TotalProducts",TotalProducts}
                                            };
                return Ok(C);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }
        [HttpGet]
        [Authorize]
        [Route("GetAllOrders")]
        public IHttpActionResult GetAllOrders()
        {
            var DB = new AngularDemoEntities();
            var user_id = User.Identity.GetUserId();
            try
            {
                var O = DB.Order_Master.Select(x => new {
                    x.BankName,
                    x.BillingAddress,
                    x.ChequeNo,
                    x.City,
                    x.DeliveryDate,
                    x.Mobile,
                    x.Name,
                    x.OrderDate,
                    x.OrderID,
                    x.PaymentMethod,
                    x.ShippingAddress,
                    x.State,
                    x.States_Master.StateName,
                    x.City_Master.CityName,
                    x.UserID,
                    x.Zipcode,
                    x.TotalAmount
                }).ToList();
                return Ok(O);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        [HttpGet]
        [Authorize]
        [Route("GetRegisteredCustomers")]
        public IHttpActionResult GetRegisteredCustomers()
        {
            var DB = new AngularDemoEntities();
            try
            {
                var cust = DB.UserProfile_Master.Select(x=> new {
                    x.City,
                    x.FirstName,
                    x.Gender,
                    x.LastName,x.Mobile,x.ProfilePicture,x.State,x.UserID,x.U_ID,
                    x.AspNetUser.Email,x.AspNetUser.Id,x.AspNetUser.UserName
                }).ToList();
                return Ok(cust);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }
    }
}
