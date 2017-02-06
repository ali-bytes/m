using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Web;
using System.Web.Mvc;
using System.Web.Security.AntiXss;
using Microsoft.Ajax.Utilities;
using WoodStore.Domain.Helpers;
using WoodStore.Domain.Models;
using WoodStore.Domain.Resources;
using WoodStore.Domain.Uow;

namespace WoodStore.UI.Controllers
{
    public class CommonController : BaseController
    {
        private UnitOfWork uow = new UnitOfWork();
        private Security sec = new Security();

        [HttpPost]
        public JsonResult AddLegalEntity(string entityName)
        {

            var check = uow.Organization_Legal_EntitiesRepository.Get(a => a.Entity_Name.Equals(entityName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (check != null)
            {
                return new JsonResult { Data = new { msg = Token.UnitAlreadyExists, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            if (!sec.IsValidString(entityName))
            {
                return new JsonResult { Data = new { msg = Token.ErrEnterValidName, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            Organization_Legal_Entities ct = new Organization_Legal_Entities();
            ct.Entity_Name = entityName;
            ct.Entity_RegId = 1;
            ct.Entity_RegDate = GetCurrentDateTime();
            uow.Organization_Legal_EntitiesRepository.Add(ct);
            if (uow.Save())
            {
                return new JsonResult { Data = new { msg = Token.Saved, status = true }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return new JsonResult { Data = new { msg = Token.NotSaved, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public JsonResult GetProductQty(int? productId,int? storeId)
        {
            if (productId == null || storeId == null)
            {
                return new JsonResult { Data = new { msg = Token.ErrPleaseEnterValidData, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            decimal? total=0;
            decimal? storeTotal=0;
            bool m3 = false;
            if (uow.ProductRepository.FindById(productId??0).CategoryId == 1002)
            {
                total = uow.StoreTransactionRepository.Get(a => a.ProductId == productId).Sum(a => a.TotalQty);
                storeTotal = uow.StoreTransactionRepository.Get(a => a.ProductId == productId && a.StoreId == storeId).Sum(a => a.TotalQty);
            }
            else
            {
                total = uow.StoreTransactionRepository.Get(a => a.ProductId == productId).Sum(a => a.TotalSize);
                storeTotal = uow.StoreTransactionRepository.Get(a => a.ProductId == productId && a.StoreId == storeId).Sum(a => a.TotalSize);
                m3 = true;
            }

            return new JsonResult { Data = new { msg = "", Total = total, StoreTotal = storeTotal,m=m3, status = true }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
       
        public JsonResult GetLegalEntities()
        {
            var Entities = uow.Organization_Legal_EntitiesRepository.Get(a => a.Entity_IsDeleted != true).Select(a => new
            {
                a.Entity_Id,
                a.Entity_Name
            }).ToList();

            return new JsonResult { Data = Entities, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetAdditionTypes()
        {
            string routLang = (string)ControllerContext.RouteData.Values["lang"];
            if (!string.IsNullOrEmpty(routLang) && routLang.ToLower() == "ar")
            {
                var e = uow.AdditionTypeRepository.Get(a => a.Id != 2 && a.Id != 3).Select(a => new
                {
                    a.Id,
                   Name= a.ArName
                }).ToList();
                return new JsonResult { Data = e, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            else
            {
                var e = uow.AdditionTypeRepository.Get(a => a.Id != 2 && a.Id != 3).Select(a => new
                {
                    a.Id,
                    Name = a.EnName
                }).ToList();
                return new JsonResult { Data = e, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }

            //return new JsonResult { Data = null, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetIssueTypes()
        {
            string routLang = (string)ControllerContext.RouteData.Values["lang"];
            if (!string.IsNullOrEmpty(routLang) && routLang.ToLower() == "ar")
            {
                var e = uow.IssueTypeRepository.Get(a=>a.Id != 2).Select(a => new
                {
                    a.Id,
                    Name = a.ArName
                }).ToList();
                return new JsonResult { Data = e, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            else
            {
                var e = uow.IssueTypeRepository.Get(a => a.Id != 2).Select(a => new
                {
                    a.Id,
                    Name = a.EnName
                }).ToList();
                return new JsonResult { Data = e, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }

            //return new JsonResult { Data = null, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetIndustry()
        {
            var industry = uow.IndustryTypeRepository.Get(a => a.IndustryType_IsDeleted != true).Select(a => new
            {
                a.Type_Id,
                a.IndustryType_Name
            }).ToList();

            return new JsonResult { Data = industry, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public JsonResult AddIndustry(string industryName)
        {

            var check = uow.IndustryTypeRepository.Get(a => a.IndustryType_Name.Equals(industryName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (check != null)
            {
                return new JsonResult { Data = new { msg = Token.UnitAlreadyExists, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            if (!sec.IsValidString(industryName))
            {
                return new JsonResult { Data = new { msg = Token.ErrEnterValidName, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            IndustryType ct = new IndustryType();
            ct.IndustryType_Name = industryName;
            ct.IndustryType_RegId = 1;
            ct.IndustryType_RegDate = GetCurrentDateTime();
            uow.IndustryTypeRepository.Add(ct);
            if (uow.Save())
            {
                return new JsonResult { Data = new { msg = Token.Saved, status = true }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return new JsonResult { Data = new { msg = Token.NotSaved, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetDryingTypes()
        {
            var e = uow.DryingTypeRepository.GetAll().Select(a => new
            {
                a.Id,
                a.Type
            }).ToList();

            return new JsonResult { Data = e, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetGrades()
        {
            var e = uow.GradeRepository.GetAll().Select(a => new
            {
                a.Id,
                a.Name
            }).ToList();

            return new JsonResult { Data = e, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public JsonResult GetGradeQty(int? gradeId,int? productId, int? storeId)
        {
            var e = uow.StoreTransactionRepository.Get(a => a.ProductId == productId && a.StoreId == storeId && a.GradeId == gradeId).Sum(a => a.TotalSize);
          

            return new JsonResult { Data = e, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetUnits()
        {
            var e = uow.UnitRepository.GetAll().Select(a => new
            {
                a.Unit_Id,
                a.Unit_Name
            }).ToList();

            return new JsonResult { Data = e, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetSuppliers()
        {
            var e = uow.SupplierRepository.GetAll().Select(a => new
            {
                a.Id,
                a.Name
            }).ToList();

            return new JsonResult { Data = e, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetBanks()
        {
            var e = uow.BankRepository.GetAll().Select(a => new
            {
                a.Bank_Id,
                a.Bank_Name
            }).ToList();

            return new JsonResult { Data = e, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetNationalities()
        {
            var e = uow.NationalityRepository.GetAll().Select(a => new
            {
                a.Id,
                a.Name
            }).ToList();

            return new JsonResult { Data = e, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetSuppliersGroups()
        {
            var e = uow.SuppliersGroupRepository.GetAll().Select(a => new
            {
                a.Id,
                a.Group_Name
            }).ToList();

            return new JsonResult { Data = e, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public JsonResult GetSupplierProducts(int? supplierId,int? categoryId)
        {
            if (categoryId == null || supplierId == null)
            {
                return new JsonResult { Data = null, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            var e = uow.SupplierRepository.Get(a => a.Id == supplierId.Value).FirstOrDefault().Products.Where(o=>o.CategoryId == categoryId.Value).Select(a => new
            {
                a.Id,
                a.Name
            }).ToList();

            return new JsonResult { Data = e, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public JsonResult GetSuppliersSubGroup(int? groupId)
        {
            if (groupId == null)
            {
                return new JsonResult { Data = null, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            var sgroups = uow.Suppliers_SubGroupRepository.Get(a => a.SubGroup_IsDeleted != true && a.Groups_Id == groupId).Select(a => new
            {
                a.SubGroup_Id,
                a.SubGroup_Name
            }).ToList();

            return new JsonResult { Data = sgroups, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public JsonResult GetSuppliersCategories(int? supplierId)
        {
            if (supplierId == null)
            {
                return new JsonResult { Data = null, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            var categories = uow.SupplierRepository.Get(a => a.Id == supplierId).FirstOrDefault().Products.Select(a => new
            {
                a.Category.Id,
                a.Category.Name
               
            }).DistinctBy(a=>a.Id).ToList();
         
            return new JsonResult { Data = categories, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult GetProducts(int? supplierId, int? storeId, int? categoryId)
        {
            if (supplierId == null || storeId == null)
            {
                return new JsonResult { Data = null, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
          
            //var supplierProds = uow.SupplierRepository.Get(a => a.Id == supplierId.Value).FirstOrDefault().Products.Where(a => a.CategoryId == categoryId.Value).Select(a => new
            //{
            //    a.Id,
            //    a.Name
            //}).ToList();

            var storeProds = uow.StoreRepository.Get(a => a.Id == storeId.Value).FirstOrDefault().Products.Where(a => a.CategoryId == categoryId.Value && a.Suppliers.Any(o=>o.Id == supplierId.Value)).Select(a => new
            {
                a.Id,
                a.Name
            }).ToList();


            return new JsonResult { Data = storeProds, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult GetProductsByStoreAndCategory(int? storeId, int? categoryId)
        {
            if (categoryId == null || storeId == null)
            {
                return new JsonResult { Data = null, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            
            var storeProds = uow.StoreRepository.Get(a => a.Id == storeId.Value).FirstOrDefault().Products.Where(a => a.CategoryId == categoryId.Value).Select(a => new
            {
                a.Id,
                a.Name
            }).ToList();


            return new JsonResult { Data = storeProds, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult GetStoreProducts(int? storeId, int? categoryId)
        {
            if (categoryId == null || storeId == null)
            {
                return new JsonResult { Data = null, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            var storeProds = uow.StoreRepository.Get(a => a.Id == storeId.Value).FirstOrDefault().Products.Where(a => a.CategoryId == categoryId.Value).Select(a => new
            {
                a.Id,
                a.Name
            }).ToList();

            return new JsonResult { Data = storeProds, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        [HttpPost]
        public JsonResult GetCategories(int? supplierId, int? storeId)
        {
            if (supplierId == null || storeId == null)
            {
                return new JsonResult { Data = null, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            var storeCats = uow.StoreRepository.Get(a => a.Id == storeId.Value).FirstOrDefault().Products.Where(a => a.Suppliers.Any(o => o.Id == supplierId.Value)).Select(a => new
            {
               Id= a.CategoryId,
                a.Category.Name
            }).DistinctBy(a=>a.Id).ToList();

            return new JsonResult { Data = storeCats, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult GetStoreCategories(int? storeId)
        {
            if (storeId == null)
            {
                return new JsonResult { Data = null, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            var storeCats = uow.StoreRepository.Get(a => a.Id == storeId.Value).FirstOrDefault().Products.Select(a => new
            {
                Id = a.CategoryId,
                a.Category.Name
            }).DistinctBy(a => a.Id).ToList();

            return new JsonResult { Data = storeCats, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult GetPaymentTypes()
        {
            var e = uow.PaymentTypeRepository.GetAll().Select(a => new
            {
                a.Id,
                a.Type
            }).ToList();

            return new JsonResult { Data = e, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetStoreTypes()
        {
            var e = uow.StoreTypeRepository.GetAll().Select(a=> new 
            {
                a.Id,
                a.Type
            }).ToList();

            return new JsonResult { Data = e, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public JsonResult GetStores(int? categoryId)
        {
            if (categoryId == null)
            {
                return new JsonResult { Data = null, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            var st = uow.StoreRepository.Get(a => a.Products.Any(o => o.CategoryId == categoryId.Value)).Select(a => new
            {
                a.Id,
                a.StoreName

            }).ToList();

            return new JsonResult { Data = st, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetAllStores()
        {
           
            var st = uow.StoreRepository.GetAll().Select(a => new
            {
                a.Id,
                a.StoreName

            }).ToList();

            return new JsonResult { Data = st, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult Getcoutries()
        {
            var coutries = uow.CountryRepository.Get(a => a.Country_IsDeleted != true).Select(a => new
            {
                a.Country_Id,
                a.Country_Name
            }).ToList();

            return new JsonResult { Data = coutries, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public JsonResult GetCities(int? countryId)
        {
            if (countryId == null)
            {
                var cities = uow.CityRepository.Get(a => a.City_IsDeleted != true).Select(a => new
                {
                    a.City_Id,
                    a.City_name
                }).ToList();

                return new JsonResult { Data = cities, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            var cities2 = uow.CityRepository.Get(a => a.City_IsDeleted != true && a.Country_Id == countryId).Select(a => new
            {
                a.City_Id,
                a.City_name
            }).ToList();

            return new JsonResult { Data = cities2, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public JsonResult GetRegions(int? cityId)
        {
            if (cityId == null)
            {
                var regions = uow.DistrictRepository.GetAll().Select(a => new
                {
                    a.Id,
                    a.Name
                }).ToList();

                return new JsonResult { Data = regions, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            var regions2 = uow.DistrictRepository.Get(a => a.City_Id == cityId).Select(a => new
            {
                a.Id,
                a.Name
            }).ToList();

            return new JsonResult { Data = regions2, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetLanguages()
        {
            var Languages = uow.LanguageRepository.Get(a => a.Language_IsDeleted != true).Select(a => new
            {
                a.Language_Id,
                a.LanguageName
            }).ToList();

            return new JsonResult { Data = Languages, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetCurrencies()
        {
            var Currency = uow.CurrencyRepository.Get(a => a.Cur_Is_deleted != true).Select(a => new
            {
                a.Cur_Id,
                a.Cur_Name
            }).ToList();

            return new JsonResult { Data = Currency, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

       
        public JsonResult GetJobTitles()
        {
            var Entities = uow.JobTitlesRepository.GetAll().Select(a => new
            {
                a.Title_Id,
                a.Name
            }).ToList();

            return new JsonResult { Data = Entities, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetCareerClasses()
        {
            var Entities = uow.CareerClassesRepository.GetAll().Select(a => new
            {
                a.Classe_Id,
                a.Name
            }).ToList();

            return new JsonResult { Data = Entities, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetAllowances()
        {
            var Entities = uow.AllowanceRepository.GetAll().Select(a => new
            {
                a.Allowance_Id,
                a.Name
            }).ToList();

            return new JsonResult { Data = Entities, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult AddPaymentType(string paymentType)
        {

            var check = uow.PaymentTypeRepository.Get(a => a.Type.Equals(paymentType, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (check != null)
            {
                return new JsonResult { Data = new { msg = Token.errUnitAlreadyExists, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            if (!sec.IsValidString(paymentType))
            {
                return new JsonResult { Data = new { msg = Token.ErrEnterValidName, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            PaymentType ct = new PaymentType();
            ct.Type = paymentType;
            uow.PaymentTypeRepository.Add(ct);
            if (uow.Save())
            {
                return new JsonResult { Data = new { msg = Token.Saved, status = true }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return new JsonResult { Data = new { msg = Token.NotSaved, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult AddStoreType(string storeType)
        {

            var check = uow.StoreTypeRepository.Get(a => a.Type.Equals(storeType, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (check != null)
            {
                return new JsonResult { Data = new { msg = Token.errUnitAlreadyExists, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            if (!sec.IsValidString(storeType))
            {
                return new JsonResult { Data = new { msg = Token.ErrEnterValidName, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            StoreType ct = new StoreType();
            ct.Type = storeType;
            uow.StoreTypeRepository.Add(ct);
            if (uow.Save())
            {
                return new JsonResult { Data = new { msg = Token.Saved, status = true }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return new JsonResult { Data = new { msg = Token.NotSaved, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public JsonResult AddCountry(string countryName)
        {

            var check = uow.CountryRepository.Get(a => a.Country_Name.Equals(countryName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (check != null)
            {
                return new JsonResult { Data = new { msg = Token.errCountryAlreadyExists, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            if (!sec.IsValidString(countryName))
            {
                return new JsonResult { Data = new { msg = Token.ErrEnterValidName, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            Country ct = new Country();
            ct.Country_Name = countryName;
            ct.Country_reg_id = 1;
            ct.Country_regdate = GetCurrentDateTime();
            uow.CountryRepository.Add(ct);
            if (uow.Save())
            {
                return new JsonResult { Data = new { msg = Token.Saved, status = true }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return new JsonResult { Data = new { msg = Token.NotSaved, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public JsonResult AddNationality(string nationality)
        {

            var check = uow.NationalityRepository.Get(a => a.Name.Equals(nationality, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (check != null)
            {
                return new JsonResult { Data = new { msg = Token.errCountryAlreadyExists, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            if (!sec.IsValidString(nationality))
            {
                return new JsonResult { Data = new { msg = Token.ErrEnterValidName, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            Nationality ct = new Nationality();
            ct.Name = nationality;
            ct.Nationality_regId = 1;
            ct.Nationality_regDate = GetCurrentDateTime();
            uow.NationalityRepository.Add(ct);
            if (uow.Save())
            {
                return new JsonResult { Data = new { msg = Token.Saved, status = true }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return new JsonResult { Data = new { msg = Token.NotSaved, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult AddSuplierSubGroup(string subGroup, int? groupId)
        {
            if (groupId == null || string.IsNullOrWhiteSpace(subGroup))
            {
                return new JsonResult { Data = new { msg = Token.ErrPleaseEnterValidData, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            if (!sec.IsValidString(subGroup))
            {
                return new JsonResult { Data = new { msg = Token.ErrEnterValidName, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            var check = uow.Suppliers_SubGroupRepository.Get(a => a.SubGroup_Name.Equals(subGroup, StringComparison.OrdinalIgnoreCase) && a.Groups_Id == groupId).FirstOrDefault();
            if (check != null)
            {
                return new JsonResult { Data = new { msg = Token.errUnitAlreadyExists, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }


            Suppliers_SubGroup ct = new Suppliers_SubGroup();
            ct.SubGroup_Name = subGroup;
            ct.Groups_Id = groupId.Value;
            ct.SubGroup_RegId = 1;
            ct.SubGroup_RegDate = GetCurrentDateTime();
            uow.Suppliers_SubGroupRepository.Add(ct);
            if (uow.Save())
            {
                return new JsonResult { Data = new { msg = Token.Saved, status = true }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return new JsonResult { Data = new { msg = Token.NotSaved, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult AddCity(string cityName,int? countryId)
        {
            if (countryId == null || string.IsNullOrWhiteSpace(cityName))
            {
                return new JsonResult { Data = new { msg = Token.ErrPleaseEnterValidData, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            if (!sec.IsValidString(cityName))
            {
                return new JsonResult { Data = new { msg = Token.ErrEnterValidName, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            var check = uow.CityRepository.Get(a => a.City_name.Equals(cityName, StringComparison.OrdinalIgnoreCase) && a.Country_Id == countryId).FirstOrDefault();
            if (check != null)
            {
                return new JsonResult { Data = new { msg = Token.errUnitAlreadyExists, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

           
            City ct = new City();
            ct.City_name = cityName;
            ct.Country_Id = countryId.Value;
            ct.City_RegId = 1;
            ct.City_RegDate = GetCurrentDateTime();
            uow.CityRepository.Add(ct);
            if (uow.Save())
            {
                return new JsonResult { Data = new { msg = Token.Saved, status = true }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return new JsonResult { Data = new { msg = Token.NotSaved, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public JsonResult AddRegion(string regionName, int? cityId)
        {
            if (cityId == null || string.IsNullOrWhiteSpace(regionName))
            {
                return new JsonResult { Data = new { msg = Token.ErrPleaseEnterValidData, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            if (!sec.IsValidString(regionName))
            {
                return new JsonResult { Data = new { msg = Token.ErrEnterValidName, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            var check = uow.DistrictRepository.Get(a => a.Name.Equals(regionName, StringComparison.OrdinalIgnoreCase) && a.City_Id == cityId).FirstOrDefault();
            if (check != null)
            {
                return new JsonResult { Data = new { msg = Token.errUnitAlreadyExists, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }


            District ct = new District();
            ct.Name = regionName;
            ct.City_Id = cityId.Value;
            uow.DistrictRepository.Add(ct);
            if (uow.Save())
            {
                return new JsonResult { Data = new { msg = Token.Saved, status = true }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return new JsonResult { Data = new { msg = Token.NotSaved, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public JsonResult AddLanguage(string languageName)
        {

            var check = uow.LanguageRepository.Get(a => a.LanguageName.Equals(languageName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (check != null)
            {
                return new JsonResult { Data = new { msg = Token.UnitAlreadyExists, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            if (!sec.IsValidString(languageName))
            {
                return new JsonResult { Data = new { msg = Token.ErrEnterValidName, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            Language ct = new Language();
            ct.LanguageName = languageName;
            ct.Language_RegId = 1;
            ct.Language_RegDate = GetCurrentDateTime();
            uow.LanguageRepository.Add(ct);
            if (uow.Save())
            {
                return new JsonResult { Data = new { msg = Token.Saved, status = true }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return new JsonResult { Data = new { msg = Token.NotSaved, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult AddCurrency(string currencyName, string currencySample)
        {
            if (string.IsNullOrWhiteSpace(currencyName) || string.IsNullOrWhiteSpace(currencySample))
            {
                return new JsonResult { Data = new { msg = Token.ErrPleaseEnterValidData, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            if (!sec.IsValidString(currencyName))
            {
                return new JsonResult { Data = new { msg = Token.ErrEnterValidName, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            var check = uow.CurrencyRepository.Get(a => a.Cur_Name.Equals(currencyName, StringComparison.OrdinalIgnoreCase) && a.Cur_Sample.Equals(currencySample, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (check != null)
            {
                return new JsonResult { Data = new { msg = Token.errUnitAlreadyExists, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }


            Currency ct = new Currency();
            ct.Cur_Name = currencyName;
            ct.Cur_Sample = AntiXssEncoder.HtmlEncode(currencySample,true);
            ct.Cur_reg_id = 1;
            ct.Cur_regdate = GetCurrentDateTime();
            uow.CurrencyRepository.Add(ct);
            if (uow.Save())
            {
                return new JsonResult { Data = new { msg = Token.Saved, status = true }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return new JsonResult { Data = new { msg = Token.NotSaved, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public JsonResult AddBank(string bankName, int? countryId)
        {
          
            if (string.IsNullOrWhiteSpace(bankName) || countryId == null)
            {
                return new JsonResult { Data = new { msg = Token.ErrPleaseEnterValidData, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            if (!sec.IsValidString(bankName))
            {
                return new JsonResult { Data = new { msg = Token.ErrEnterValidName, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            var check = uow.BankRepository.Get(a => a.Bank_Name.Equals(bankName, StringComparison.OrdinalIgnoreCase) && a.Bank_Country_id == countryId).FirstOrDefault();
            if (check != null)
            {
                return new JsonResult { Data = new { msg = Token.errUnitAlreadyExists, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }


            Bank ct = new Bank();
            ct.Bank_Name = bankName;
            ct.Bank_Country_id = countryId.Value;
            ct.Bank_RegId = 1;
            ct.Bank_RegDate = GetCurrentDateTime();
            uow.BankRepository.Add(ct);
            if (uow.Save())
            {
                return new JsonResult { Data = new { msg = Token.Saved, status = true }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return new JsonResult { Data = new { msg = Token.NotSaved, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        [HttpPost]
        public JsonResult AddSupplierGroup(string supplierGroup)
        {

            var check = uow.SuppliersGroupRepository.Get(a => a.Group_Name.Equals(supplierGroup, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (check != null)
            {
                return new JsonResult { Data = new { msg = Token.UnitAlreadyExists, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            if (!sec.IsValidString(supplierGroup))
            {
                return new JsonResult { Data = new { msg = Token.ErrEnterValidName, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            SuppliersGroup ct = new SuppliersGroup();
            ct.Group_Name = supplierGroup;

            uow.SuppliersGroupRepository.Add(ct);
            if (uow.Save())
            {
                return new JsonResult { Data = new { msg = Token.Saved, status = true }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return new JsonResult { Data = new { msg = Token.NotSaved, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult AddJobTitle(string jobTitle)
        {

            var check = uow.JobTitlesRepository.Get(a => a.Name.Equals(jobTitle, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (check != null)
            {
                return new JsonResult { Data = new { msg = Token.UnitAlreadyExists, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            if (!sec.IsValidString(jobTitle))
            {
                return new JsonResult { Data = new { msg = Token.ErrEnterValidName, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            Job_Titles ct = new Job_Titles();
            ct.Name = jobTitle;

            uow.JobTitlesRepository.Add(ct);
            if (uow.Save())
            {
                return new JsonResult { Data = new { msg = Token.Saved, status = true }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return new JsonResult { Data = new { msg = Token.NotSaved, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult AddCareerClass(string careerClass)
        {

            var check = uow.CareerClassesRepository.Get(a => a.Name.Equals(careerClass, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (check != null)
            {
                return new JsonResult { Data = new { msg = Token.UnitAlreadyExists, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            if (!sec.IsValidString(careerClass))
            {
                return new JsonResult { Data = new { msg = Token.ErrEnterValidName, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            Career_Classes ct = new Career_Classes();
            ct.Name = careerClass;

            uow.CareerClassesRepository.Add(ct);
            if (uow.Save())
            {
                return new JsonResult { Data = new { msg = Token.Saved, status = true }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return new JsonResult { Data = new { msg = Token.NotSaved, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult AddAllowance(string allowance)
        {

            var check = uow.AllowanceRepository.Get(a => a.Name.Equals(allowance, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (check != null)
            {
                return new JsonResult { Data = new { msg = Token.UnitAlreadyExists, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            if (!sec.IsValidString(allowance))
            {
                return new JsonResult { Data = new { msg = Token.ErrEnterValidName, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            Allowance ct = new Allowance();
            ct.Name = allowance;

            uow.AllowanceRepository.Add(ct);
            if (uow.Save())
            {
                return new JsonResult { Data = new { msg = Token.Saved, status = true }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return new JsonResult { Data = new { msg = Token.NotSaved, status = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetBranches()
        {
            var Entities = uow.BranchRepository.Get(a=>a.Branch_IsDeleted != true).Select(a => new
            {
                a.Branch_Id,
                a.Branch_Name
            }).ToList();

            return new JsonResult { Data = Entities, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public JsonResult GetResourceString(string key)
        {
            ResourceManager rm = Token.ResourceManager;
            var s = rm.GetString(key);
            return new JsonResult { Data = s, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        protected override void Dispose(bool disposing)
        {
            sec.Dispose();
            uow.Dispose();
            base.Dispose(disposing);
        }
    }
}