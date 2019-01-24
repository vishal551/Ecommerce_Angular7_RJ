using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using AuthenticationAPI.Models;
namespace AuthenticationAPI.Controllers
{
    [RoutePrefix("api/Product")]
    public class ProductController : ApiController
    {
        [HttpGet]
        [Authorize]
        [Route("Category")]
        public IHttpActionResult Category()
        {
            try
            {
                var DB = new AngularDemoEntities();
                var Category = DB.Category_Master.Select(x => new
                {
                    x.CategoryID,
                    x.CategoryName
                }).ToList();
                if (Category.Count == 0)
                {
                    return NotFound();
                }
                return Ok(Category);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        [HttpPost]
        [Authorize]
        [Route("AddProduct")]
        public IHttpActionResult AddProduct(Products P)
        {
            var DB = new AngularDemoEntities();
            try
            {
                if (P != null)
                {
                    byte[] ImageFile = null;
                    if (!string.IsNullOrEmpty(P.ProductImage))
                    {
                        ImageFile = Convert.FromBase64String(P.ProductImage.Split(',')[1]);
                    }
                    Product_Master Obj_P = new Product_Master
                    {
                        CategoryID = P.CategoryID,
                        InStock = P.InStock,
                        ProductImage = ImageFile,
                        ProductName = P.ProductName,
                        ProductPrice = P.ProductPrice,
                        ProductQuantity = P.ProductQuantity,
                        ProductDescription = P.ProductDescription
                    };
                    DB.Product_Master.Add(Obj_P);
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
        [Route("ProductList")]
        public IHttpActionResult ProductList()
        {
            try
            {
                var DB = new AngularDemoEntities();
                var P_List = DB.Product_Master.Select(x => new
                {
                    x.ProductID,
                    x.CategoryID,
                    x.Category_Master.CategoryName,
                    x.ProductImage,
                    x.ProductPrice,
                    x.InStock,
                    x.ProductQuantity,
                    x.ProductName,
                    x.ProductDescription
                }).OrderByDescending(y => y.ProductID).ToList();
                return Ok(P_List);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        [HttpDelete]
        [Authorize]
        [Route("DeleteProduct/{P_ID}")]
        public IHttpActionResult DeleteProduct(int P_ID)
        {
            try
            {
                var DB = new AngularDemoEntities();
                var Product = DB.Product_Master.Where(x => x.ProductID == P_ID).FirstOrDefault();
                if (Product != null)
                {
                    DB.Product_Master.Remove(Product);
                    DB.SaveChanges();
                    return Ok("success");
                }
                else
                {
                    return Ok("Record not found");
                }
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        [HttpGet]
        [Route("GetProByID/{ID}")]
        public IHttpActionResult GetProByID(int ID)
        {
            try
            {
                var DB = new AngularDemoEntities();
                var Product = DB.Product_Master.Where(x => x.ProductID == ID).Select(x => new
                {
                    x.ProductID,
                    x.CategoryID,
                    x.Category_Master.CategoryName,
                    x.ProductImage,
                    x.ProductPrice,
                    x.InStock,
                    x.ProductQuantity,
                    x.ProductName,
                    x.ProductDescription
                }).FirstOrDefault();
                if (Product != null)
                    return Ok(Product);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }
    }
}
