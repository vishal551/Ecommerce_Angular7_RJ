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
    [RoutePrefix("api/UserProfile")]
    public class UserProfileController : ApiController
    {
        [HttpPost]
        [Authorize]
        [Route("AddProfile")]
        public IHttpActionResult AddProfile(Profile P)
        {
            var DB = new AngularDemoEntities();
            try
            {
                if (P != null)
                {
                    byte[] ImageFile = null;
                    if (!string.IsNullOrEmpty(P.ProfilePicture))
                    {
                        ImageFile = Convert.FromBase64String(P.ProfilePicture.Split(',')[1]);
                    }
                    var user_id = User.Identity.GetUserId();
                    UserProfile_Master UP = new UserProfile_Master
                    {
                        UserID = user_id,
                        FirstName = P.FirstName,
                        City = P.City,
                        Gender = P.Gender,
                        LastName = P.LastName,
                        Mobile = P.Mobile,
                        ProfilePicture = ImageFile,
                        State = P.State
                    };
                    DB.UserProfile_Master.Add(UP);
                    DB.SaveChanges();
                    return Ok("success");
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
        [Route("GetCurrentUser")]
        public IHttpActionResult GetCurrentUser()
        {
            var DB = new AngularDemoEntities();
            var user_id = User.Identity.GetUserId();
            try
            {
                var user = DB.UserProfile_Master.Where(x => x.UserID == user_id).FirstOrDefault();
                if (user != null)
                    return Ok(user);
                else
                    return Ok("Not Found");
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        [HttpGet]
        [Authorize]
        [Route("CheckIsProfile")]
        public IHttpActionResult CheckIsProfile()
        {
            var DB = new AngularDemoEntities();
            var user_id = User.Identity.GetUserId();
            try
            {
                bool IsProfile = DB.UserProfile_Master.Any(x => x.UserID == user_id);
                return Ok(IsProfile);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }
    }
}
