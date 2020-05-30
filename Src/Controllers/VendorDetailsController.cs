using CommunitiesWinApi.Models;
using CommunitiesWinFrameworkApi.classes;
using CommunitiesWinFrameworkAPI.classes;
using CommunitiesWinFrameworkAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace CommunitiesWinFrameworkAPI.Controllers
{
    public class VendorDetailsController : ApiController
    {
        private readonly communitieswinEntities communitieswinEntities;

        private const string InvalidPhoneNumberMessage = "Invalid new phone number passed. Expecting phone number to be greater than zero and 10 digits in length.";
        public VendorDetailsController()
        {
            communitieswinEntities = new communitieswinEntities();
        }

        /// <summary>
        /// Invoke the method based on the URL below:
        /// http://localhost:54605/api/VendorDetails/GetVendor?phoneNumber=8888888888
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        [AcceptVerbs("GET")]
        [System.Web.Mvc.ActionName("GetVendor")]
        public IHttpActionResult GetVendor([FromBody]VendorDetail vendorDetailItem)
        {
            
            var vendorDetail = communitieswinEntities.vendor_details.FirstOrDefault(x => x.phone == vendorDetailItem.Phone);
            if (vendorDetail == null)
            {
                return BadRequest("Couldn't find a vendor profile with phone number: " + vendorDetailItem.Phone);
            }
            var categories = communitieswinEntities.categories.ToList();
            Dictionary<long, string> categoriesDict = new Dictionary<long, string>();
            foreach (var category in categories)
            {
                categoriesDict.Add(category.category_id, category.category_name);
            }
            
            var vendorProducts = communitieswinEntities.vendor_product.Where(x => x.vendor_id == vendorDetail.vendor_id).ToList();
            List<object> vendorProductsData = new List<object>();
            foreach (var vendorProduct in vendorProducts)
            {
                var product = communitieswinEntities.products.FirstOrDefault(x => x.product_id == vendorProduct.product_id);
                if (product != null)
                {
                    vendorProductsData.Add(new
                    {
                        ProductName = product.product_name,
                        MinOrderQuantity = vendorProduct.min_order_quantity,
                        Units = vendorProduct.Units,
                        Price = vendorProduct.price,
                        CategoryName = categoriesDict[product.category_id]
                    });
                }
            }
            var data = new
            {
                vendor_name = vendorDetail.vendor_name,
                VendorPin = vendorDetail.pin,
                VendorCity = vendorDetail.city,
                Vendorcountry = vendorDetail.country,
                VendorState = vendorDetail.state,
                VendorSantizier = vendorDetail.is_sanitizer_used,
                VendorFeverScreening = vendorDetail.is_fever_screen,
                VendorStampCheck = vendorDetail.is_stamp_check,
                VendorSocialDistanced = vendorDetail.is_social_distance,
                VendorContactLessPay = vendorDetail.contactless_pay,
                Vendorlatitude = vendorDetail.latitude,
                Vendorlongitude = vendorDetail.longitude,                
                VendorProducts = vendorProductsData
            };
            return Json(data);
        }

        /// <summary>
        /// This method is used for new vendor registration. If we can't find phoneNumber in the database, create a new record. 
        /// else update the existing record with the new phone number.
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="vendorName"></param>
        /// <param name="country"></param>
        /// <param name="state"></param>
        /// <param name="city"></param>
        /// <param name="pin"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="newPhoneNumber"></param>
        /// <returns></returns>
        /// http://localhost:54605/api/VendorDetails/VendorRegistrationAddUpdate?Phone=9999999999&VendorName=Harry&Country=India&State=AP&City=Vizag&Pin=530013&Latitude=null&Longitude=null
        [AcceptVerbs("POST")]
        [System.Web.Mvc.ActionName("VendorRegistrationAddUpdate")]
        public IHttpActionResult VendorRegistrationAddUpdate([FromBody]VendorDetail vendorDetails)
        {
            if (vendorDetails.Phone <= 0 || vendorDetails.Phone.ToString().Length < 10)
            {
                return BadRequest(InvalidPhoneNumberMessage);
            }
            if (string.IsNullOrEmpty(vendorDetails.VendorName))
            {
                return BadRequest("Invalid vendor name");
            }

            var existingVendorDetail = communitieswinEntities.vendor_details.FirstOrDefault(x => x.phone == vendorDetails.Phone);
            if (existingVendorDetail == null)
            {
                communitieswinEntities.vendor_details.Add(new vendor_details
                {
                    phone = vendorDetails.Phone,
                    vendor_name = vendorDetails.VendorName,
                    country = vendorDetails.Country,
                    city = vendorDetails.City,
                    state = vendorDetails.State,
                    pin = vendorDetails.Pin,
                    latitude = vendorDetails.Latitude,
                    longitude = vendorDetails.Longitude
                });
            }
            else
            {
                existingVendorDetail.vendor_name = vendorDetails.VendorName;
                existingVendorDetail.latitude = vendorDetails.Latitude;
                existingVendorDetail.longitude = vendorDetails.Longitude;
                existingVendorDetail.city = vendorDetails.City;
                existingVendorDetail.state = vendorDetails.State;
                existingVendorDetail.pin = vendorDetails.Pin;
                existingVendorDetail.country = vendorDetails.Country;
            }
            communitieswinEntities.SaveChanges();
            return Ok();
        }

        [AcceptVerbs("POST")]
        [System.Web.Mvc.ActionName("UpdateVendorCoronaPrecautions")]
        public IHttpActionResult UpdateVendorCoronaPrecautions([FromBody] VendorCoronaPrecaution coronaPrecautions)
        {
            if (coronaPrecautions.Phone <= 0 || coronaPrecautions.Phone.ToString().Length < 10)
            {
                return BadRequest(InvalidPhoneNumberMessage);
            }

            var existingVendorDetail = communitieswinEntities.vendor_details.FirstOrDefault(x => x.phone == coronaPrecautions.Phone);
            if (existingVendorDetail == null)
            {
                return BadRequest("Couldn't find an existing vendor detail entry. Please add the vendor details first.");
            }
            else
            {
                existingVendorDetail.is_fever_screen = coronaPrecautions.IsFeverScreen;
                existingVendorDetail.is_sanitizer_used = coronaPrecautions.IsSanitizerUsed;
                existingVendorDetail.is_stamp_check = coronaPrecautions.IsStampCheck;
                existingVendorDetail.is_social_distance = coronaPrecautions.IsSocialDistance;
                existingVendorDetail.contactless_pay = coronaPrecautions.IsContactLessPay;
            }
            communitieswinEntities.SaveChanges();
            return Ok();
        }

        [AcceptVerbs("POST")]
        [System.Web.Mvc.ActionName("VendorProductPrices")]
        public IHttpActionResult VendorProductPrices([FromBody] VendorProductPrice vendorProductPrice)
        {
            product currentProduct;
            if (string.IsNullOrEmpty(vendorProductPrice.ProductName))
            {
                throw new ArgumentException("Product name cannot be empty");
            }
            var vendorDetail = communitieswinEntities.vendor_details.FirstOrDefault(x => x.phone == vendorProductPrice.Phone);
            if (vendorDetail == null)
            {
                throw new ArgumentException("Couldn't find a vendor profile with phone number: " + vendorProductPrice.Phone);
            }

            long categoryId = SaveCategory(vendorProductPrice.CategoryName);

            var product = communitieswinEntities.products.FirstOrDefault(x => x.product_name.ToLower() == vendorProductPrice.ProductName.ToLower());
            if (product == null)
            {
                currentProduct = new product()
                {
                    product_name = vendorProductPrice.ProductName,
                    category_id = categoryId
                };
                communitieswinEntities.products.Add(currentProduct);                
            }
            else
            {
                currentProduct = product;
                currentProduct.product_name = vendorProductPrice.ProductName;
            }
            communitieswinEntities.SaveChanges();

            var vendorProductDetail = communitieswinEntities.vendor_product.FirstOrDefault(x => x.vendor_id == vendorDetail.vendor_id);
            if (vendorProductDetail == null)
            {
                communitieswinEntities.vendor_product.Add(new vendor_product()
                {
                    vendor_id = vendorDetail.vendor_id,
                    product_id = currentProduct.product_id,
                    min_order_quantity = vendorProductPrice.MinOrderQuantity,
                    price = vendorProductPrice.Price,
                    Units = vendorProductPrice.Units
                });
            }
            else
            {
                vendorProductDetail.product_id = currentProduct.product_id;
                vendorProductDetail.min_order_quantity = vendorProductPrice.MinOrderQuantity;
                vendorProductDetail.price = vendorProductPrice.Price;
                vendorProductDetail.Units = vendorProductPrice.Units;
            }
            communitieswinEntities.SaveChanges();
            return Ok();
        }

        private long SaveCategory(string categoryName)
        {
            var category = communitieswinEntities.categories.FirstOrDefault(x => x.category_name.ToLower() == categoryName.ToLower());
            if (category == null)
            {
                category = new category()
                {
                    category_name = categoryName
                };
                communitieswinEntities.categories.Add(category);
            }
            communitieswinEntities.SaveChanges();
            return category.category_id;
        }

        [AcceptVerbs("POST")]
        [System.Web.Mvc.ActionName("VendorCategory")]
        public IHttpActionResult VendorCategory([FromBody] VendorCategoryItem vendorCategoryItem)
        {
            if (string.IsNullOrEmpty(vendorCategoryItem.CategoryName))
            {
                return BadRequest("Category name cannot be empty.");                
            }
            var vendorDetail = communitieswinEntities.vendor_details.FirstOrDefault(x => x.phone == vendorCategoryItem.Phone);
            if (vendorDetail == null)
            {
                return BadRequest("Couldn't find a vendor profile with phone number: " + vendorCategoryItem.Phone);                
            }
            long categoryId = SaveCategory(vendorCategoryItem.CategoryName);
            var vendorCategory = communitieswinEntities.vendor_category.FirstOrDefault(x => x.vendor_id == vendorDetail.vendor_id && x.category_id == categoryId);
            if (vendorCategory == null)
            {
                communitieswinEntities.vendor_category.Add(new vendor_category()
                {
                    vendor_id = vendorDetail.vendor_id,
                    category_id = categoryId,
                    is_active = true
                });
            }
            else
            {
                if (vendorCategoryItem.Enable.GetValueOrDefault())
                {
                    return BadRequest("There is an existing relation to category and vendor with category name: " + vendorCategoryItem.CategoryName + " and its enabled.");
                }                    
                else
                {
                    vendorCategory.is_active = false;                    
                }
            }
            communitieswinEntities.SaveChanges();
            return Ok();
        }
        /// <summary>
        /// Invoke the method based on the URL below:
        /// https://localhost:44355/api/VendorDetails/ProductsList
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        
        [AcceptVerbs("GET")]
        [System.Web.Mvc.ActionName("ProductsList")]
        public IHttpActionResult ProductsList(string categoryName = null)
        {
            Dictionary<long, string> categoriesDict = new Dictionary<long, string>();
            var categories = communitieswinEntities.categories.ToList();
            long currentCategoryId = 0;
            foreach (var category in categories)
            {
                if (string.IsNullOrEmpty(categoryName) || category.category_name.ToLower() == categoryName.ToLower())
                {
                    categoriesDict.Add(category.category_id, category.category_name);
                    currentCategoryId = category.category_id;
                }                               
            }
            var products = string.IsNullOrEmpty(categoryName) ? communitieswinEntities.products.ToList() :
                           communitieswinEntities.products.Where(x => x.category_id == currentCategoryId).ToList();
            List<object> productsData = new List<object>();
            foreach (product item in products)
            {
                productsData.Add(new
                {
                    ProductName = item.product_name,
                    Category = categoriesDict[item.category_id],
                    ImageUrl = ""
                });
            }
            return Json(productsData);
        }
    }    
 }
