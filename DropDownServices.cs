using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WoodStore.Domain.Models;
using WoodStore.Domain.Uow;

namespace WoodStore.UI.Helpers
{
    public class DropDownServices : IDisposable
    {
        UnitOfWork uow = new UnitOfWork();
        public IEnumerable<SelectListItem> GetLegalEntities(string selectedValue = null)
        {
            List<Organization_Legal_Entities> entities = uow.Organization_Legal_EntitiesRepository.Get(a => a.Entity_IsDeleted != true).ToList();

            var c =
             entities.Select(a => new SelectListItem()
             {
                 Text = a.Entity_Name,
                 Value = a.Entity_Id.ToString(),
                 Selected = selectedValue == a.Entity_Id.ToString() ? true : false
             });
            return c;
        }

        public IEnumerable<SelectListItem> GetIndustries(string selectedValue = null)
        {
            List<IndustryType> industries = uow.IndustryTypeRepository.Get(a => a.IndustryType_IsDeleted != true).ToList();
            var c =
             industries.Select(a => new SelectListItem()
             {
                 Text = a.IndustryType_Name,
                 Value = a.Type_Id.ToString(),
                 Selected = selectedValue == a.Type_Id.ToString() ? true : false
             });
            return c;
        }
        public IEnumerable<SelectListItem> GetRoles(string selectedValue = null)
        {
            IEnumerable<Role> roles = uow.RoleRepository.GetAll();
            var c =
             roles.Select(a => new SelectListItem()
             {
                 Text = a.Name,
                 Value = a.Id.ToString(),
                 Selected = selectedValue == a.Id.ToString() ? true : false
             });
            return c;
        }
        public IEnumerable<SelectListItem> GetCountries(string selectedValue = null)
        {
            List<Country> countries = uow.CountryRepository.Get(a => a.Country_IsDeleted != true).ToList();

            var c=
             countries.Select(a => new SelectListItem()
            {
                Text = a.Country_Name,
                Value = a.Country_Id.ToString(),
                Selected = selectedValue == a.Country_Id.ToString() ? true : false
            });
            return c;
        }
        public IEnumerable<SelectListItem> GetCountries(Controller ct, UnitOfWork db, int? selectedValue)
        {
            List<Country> countries = db.CountryRepository.Get(a => a.Country_IsDeleted != true).ToList();

            IEnumerable<SelectListItem> c = new List<SelectListItem>();
            if (ct.TempData["countries"] != null && selectedValue == null)
            {
                c = (IEnumerable<SelectListItem>)ct.TempData["countries"];
            }
            else
            {
                c = countries.Select(a => new SelectListItem()
                {
                    Text = a.Country_Name,
                    Value = a.Country_Id.ToString(),
                    Selected = selectedValue == a.Country_Id
                });
                if (selectedValue == null)
                {
                    ct.TempData["countries"] = c;
                }
            }
           
            return c;
        }
        public IEnumerable<SelectListItem> GetReceiveSerialNoTypes(Controller ct, UnitOfWork db, int? selectedValue)
        {
            List<SerialNoType> types = db.SerialNoTypeRepository.Get(a=>a.Id ==1 || a.Id == 2).ToList();
            IEnumerable<SelectListItem> c = new List<SelectListItem>();
           
                c = types.Select(a => new SelectListItem()
                {
                    Text = a.Name,
                    Value = a.Id.ToString(),
                    Selected = selectedValue == a.Id
                });
               
            return c;
        }
        public IEnumerable<SelectListItem> GetIssuingSerialNoTypes(Controller ct, UnitOfWork db, int? selectedValue)
        {
            List<SerialNoType> types = db.SerialNoTypeRepository.Get(a => a.Id == 2 || a.Id == 3).ToList();
            IEnumerable<SelectListItem> c = new List<SelectListItem>();
           
                c = types.Select(a => new SelectListItem()
                {
                    Text = a.Name,
                    Value = a.Id.ToString(),
                    Selected = selectedValue == a.Id
                });
              
            return c;
        }
        public IEnumerable<SelectListItem> GetCategory(Controller ct, UnitOfWork db, int? selectedValue)
        {
            List<Category> list = db.CategoryRepository.GetAll().ToList();
            IEnumerable<SelectListItem> c = new List<SelectListItem>();
            if (ct.TempData["Category"] != null && selectedValue == null)
            {
                c = (IEnumerable<SelectListItem>)ct.TempData["Category"];
            }
            else
            {
                c = list.Select(a => new SelectListItem()
                {
                    Text = a.Name,
                    Value = a.Id.ToString(),
                    Selected = selectedValue == a.Id
                });
                if (selectedValue == null)
                {
                    ct.TempData["Category"] = c;
                }
            }

            return c;
        }
        public IEnumerable<SelectListItem> GetStoreTypes(Controller ct, UnitOfWork db,int? selectedValue)
        {
            List<StoreType> list = db.StoreTypeRepository.GetAll().ToList();
            IEnumerable<SelectListItem> c = new List<SelectListItem>();
            if (ct.TempData["storeTypes"] != null && selectedValue == null)
            {
                c = (IEnumerable<SelectListItem>)ct.TempData["storeTypes"];
            }
            else
            {
                c = list.Select(a => new SelectListItem()
                {
                    Text = a.Type,
                    Value = a.Id.ToString(),
                    Selected = selectedValue == a.Id
                });
                if (selectedValue == null)
                {
                    ct.TempData["storeTypes"] = c;
                }
            }

            return c;
        }
        public IEnumerable<SelectListItem> GetPaymentTypes(Controller ct, UnitOfWork db, int? selectedValue)
        {
            List<PaymentType> list = db.PaymentTypeRepository.GetAll().ToList();
            IEnumerable<SelectListItem> c = new List<SelectListItem>();
            if (ct.TempData["PaymentType"] != null && selectedValue == null)
            {
                c = (IEnumerable<SelectListItem>)ct.TempData["PaymentType"];
            }
            else
            {
                c = list.Select(a => new SelectListItem()
                {
                    Text = a.Type,
                    Value = a.Id.ToString(),
                    Selected = selectedValue == a.Id
                });
                if (selectedValue == null)
                {
                    ct.TempData["PaymentType"] = c;
                }
            }

            return c;
        }
        public IEnumerable<SelectListItem> GetUsers(Controller ct, UnitOfWork db, string selectedValue)
        {
            List<User> list = db.UserRepository.GetAll().ToList();
            IEnumerable<SelectListItem> c = new List<SelectListItem>();
            if (ct.TempData["usersList"] != null && string.IsNullOrEmpty(selectedValue))
            {
                c = (IEnumerable<SelectListItem>)ct.TempData["usersList"];
            }
            else
            {
                c = list.Select(a => new SelectListItem()
                {
                    Text = a.FullName,
                    Value = a.Id,
                    Selected = selectedValue == a.Id
                });
                if (string.IsNullOrEmpty(selectedValue))
                {
                    ct.TempData["usersList"] = c;
                }
            }

            return c;
        }
        public IEnumerable<SelectListItem> GetUnits(List<Unit> units, string selectedValue = null)
        {
            var c =
             units.Select(a => new SelectListItem()
             {
                 Text = a.Unit_Name,
                 Value = a.Unit_Id.ToString(),
                 Selected = selectedValue == a.Unit_Id.ToString() ? true : false
             });
            return c;
        }
        public IEnumerable<SelectListItem> GetCurrencies(string selectedValue = null)
        {
            List<Currency> currencies = uow.CurrencyRepository.Get(a => a.Cur_Is_deleted != true).ToList();

            var c =
             currencies.Select(a => new SelectListItem()
             {
                 Text = a.Cur_Name,
                 Value = a.Cur_Id.ToString(),
                 Selected = selectedValue == a.Cur_Id.ToString() ? true : false
             });
            return c;
        }

        public IEnumerable<SelectListItem> GetCities(string selectedValue = null)
        {
            List<City> cities = uow.CityRepository.Get(a => a.City_IsDeleted != true).ToList();

            var c =
             cities.Select(a => new SelectListItem()
             {
                 Text = a.City_name,
                 Value = a.City_Id.ToString(),
                 Selected = selectedValue == a.City_Id.ToString() ? true : false
             });
            return c;
        }
        public IEnumerable<SelectListItem> GetCities(int? selectedValue)
        {
            List<City> cities = uow.CityRepository.Get(a => a.City_IsDeleted != true).ToList();

            var c =
             cities.Select(a => new SelectListItem()
             {
                 Text = a.City_name,
                 Value = a.City_Id.ToString(),
                 Selected = selectedValue == a.City_Id ? true : false
             });
            return c;
        }
        public IEnumerable<SelectListItem> GetRegions(string selectedValue = null)
        {
            List<District> regions = uow.DistrictRepository.GetAll().ToList();
            var c =
             regions.Select(a => new SelectListItem()
             {
                 Text = a.Name,
                 Value = a.Id.ToString(),
                 Selected = selectedValue == a.Id.ToString() ? true : false
             });
            return c;
        }
        public IEnumerable<SelectListItem> GetRegions(Controller ct, UnitOfWork db, int? selectedValue)
        {
            List<District> list = db.DistrictRepository.GetAll().ToList();
            IEnumerable<SelectListItem> c = new List<SelectListItem>();
            if (ct.TempData["regions"] != null && selectedValue == null)
            {
                c = (IEnumerable<SelectListItem>)ct.TempData["regions"];
            }
            else
            {
                c = list.Select(a => new SelectListItem()
                {
                    Text = a.Name,
                    Value = a.Id.ToString(),
                    Selected = selectedValue == a.Id
                });
                if (selectedValue == null)
                {
                    ct.TempData["regions"] = c;
                }
            }

            return c;
        }
       
        public IEnumerable<SelectListItem> GetLanguages(string selectedValue = null)
        {
            List<Language> languages = uow.LanguageRepository.Get(a => a.Language_IsDeleted != true).ToList();
            
            var c =
             languages.Select(a => new SelectListItem()
             {
                 Text = a.LanguageName,
                 Value = a.Language_Id.ToString(),
                 Selected = selectedValue == a.Language_Id.ToString() ? true : false
             });
            return c;
        }
        //public IEnumerable<SelectListItem> GetLegalEntities(string selectedValue = null)
        //{
        //    List<Organization_Legal_Entities> entities = uow.Organization_Legal_EntitiesRepository.Get(a => a.Entity_IsDeleted != true).ToList();

        //    var c =
        //     entities.Select(a => new SelectListItem()
        //     {
        //         Text = a.Entity_Name,
        //         Value = a.Entity_Id.ToString(),
        //         Selected = selectedValue == a.Entity_Id.ToString() ? true : false
        //     });
        //    return c;
        //}

        //public IEnumerable<SelectListItem> GetRelative_Types(int selectedValue = 0)
        //{
        //    IEnumerable<Relative_Types> types = uow.RelativeTypesRepository.GetAll();
        //    var c =
        //     types.Select(a => new SelectListItem()
        //     {
        //         Text = a.Name,
        //         Value = a.Type_Id.ToString(),
        //         Selected = selectedValue == a.Type_Id ? true : false
        //     });
        //    return c;
        //}
        public IEnumerable<SelectListItem> GetGenders(string selectedValue = null)
        {
            IEnumerable<Gender> e = uow.GenderRepository.GetAll();
            var c =
             e.Select(a => new SelectListItem()
             {
                 Text = a.Name,
                 Value = a.Gender_Id.ToString(),
                 Selected = selectedValue == a.Gender_Id.ToString() ? true : false
             });
            return c;
        }
        public IEnumerable<SelectListItem> GetJobTitles(int selectedValue=0)
        {
            IEnumerable<Job_Titles> e = uow.JobTitlesRepository.GetAll();
            var c =
             e.Select(a => new SelectListItem()
             {
                 Text = a.Name,
                 Value = a.Title_Id.ToString(),
                 Selected = selectedValue == a.Title_Id ? true : false
             });
            return c;
        }
        public IEnumerable<SelectListItem> GetCareerClasses(int selectedValue=0)
        {
            IEnumerable<Career_Classes> e = uow.CareerClassesRepository.GetAll();
            var c =
             e.Select(a => new SelectListItem()
             {
                 Text = a.Name,
                 Value = a.Classe_Id.ToString(),
                 Selected = selectedValue == a.Classe_Id ? true : false
             });
            return c;
        }
        public IEnumerable<SelectListItem> GetAllowances(string selectedValue = null)
        {
            IEnumerable<Allowance> e = uow.AllowanceRepository.GetAll();
            var c =
             e.Select(a => new SelectListItem()
             {
                 Text = a.Name,
                 Value = a.Allowance_Id.ToString(),
                 Selected = selectedValue == a.Allowance_Id.ToString() ? true : false
             });
            return c;
        }
        //public IEnumerable<SelectListItem> GetPetty_Cash_Types(int selectedValue=0)
        //{
        //    IEnumerable<Petty_Cash_Types> e = uow.Petty_Cash_TypesRepository.GetAll();
        //    var c =
        //     e.Select(a => new SelectListItem()
        //     {
        //         Text = a.Name,
        //         Value = a.CashType_Id.ToString(),
        //         Selected = selectedValue == a.CashType_Id ? true : false
        //     });
        //    return c;
        //}
        //public IEnumerable<SelectListItem> GetPetty_Cash_Types(string userId,int selectedValue = 0)
        //{
        //    if (string.IsNullOrEmpty(userId))
        //    {
        //        return null;
        //    }
        //    var userTyprId = uow.UserRepository.FindById(userId).Petty_Cash_TypeId;
        //    if (userTyprId == null)
        //    {
        //        return null;
        //    }
        //    List<Petty_Cash_Types> e = new List<Petty_Cash_Types>();
        //    if (userTyprId == 3)
        //    {
        //        e = uow.Petty_Cash_TypesRepository.Get(a=>a.CashType_Id != 3).ToList();
        //    }
        //    else
        //    {
        //        e.Add(uow.UserRepository.FindById(userId).Petty_Cash_Types);

        //    }
             
        //    var c =
        //     e.Select(a => new SelectListItem()
        //     {
        //         Text = a.Name,
        //         Value = a.CashType_Id.ToString(),
        //         Selected = selectedValue == a.CashType_Id ? true : false
        //     });
        //    return c;
        //}
        //public IEnumerable<SelectListItem> GetPetty_Cash_Periods(int selectedValue=0)
        //{
        //    IEnumerable<Petty_Cash_Periods> e = uow.Petty_Cash_PeriodsRepository.GetAll();
        //    var c =
        //     e.Select(a => new SelectListItem()
        //     {
        //         Text = a.Name,
        //         Value = a.Period_Id.ToString(),
        //         Selected = selectedValue == a.Period_Id ? true : false
        //     });
        //    return c;
        //}
        public IEnumerable<SelectListItem> GetNationalities(string selectedValue = null)
        {
            IEnumerable<Nationality> e = uow.NationalityRepository.GetAll();
            var c =
             e.Select(a => new SelectListItem()
             {
                 Text = a.Name,
                 Value = a.Id.ToString(),
                 Selected = selectedValue == a.Id.ToString() ? true : false
             });
            return c;
        }
        public IEnumerable<SelectListItem> GetBranches(int selectedValue = 0)
        {
            IEnumerable<Branch> e = uow.BranchRepository.Get(a=>a.Branch_IsDeleted != true);
            var c =
             e.Select(a => new SelectListItem()
             {
                 Text = a.Branch_Name,
                 Value = a.Branch_Id.ToString(),
                 Selected = selectedValue == a.Branch_Id 
             });
            return c;
        }
      
        public IEnumerable<SelectListItem> GetSupplierGroups(int selectedValue = 0)
        {
            IEnumerable<SuppliersGroup> e = uow.SuppliersGroupRepository.GetAll();
            var c =
             e.Select(a => new SelectListItem()
             {
                 Text = a.Group_Name,
                 Value = a.Id.ToString(),
                 Selected = selectedValue == a.Id
             });
            return c;
        }
        public IEnumerable<SelectListItem> GetSuppliersSupGroups(int selectedValue = 0)
        {
            IEnumerable<Suppliers_SubGroup> e = uow.Suppliers_SubGroupRepository.GetAll();
            var c =
             e.Select(a => new SelectListItem()
             {
                 Text = a.SubGroup_Name,
                 Value = a.SubGroup_Id.ToString(),
                 Selected = selectedValue == a.SubGroup_Id
             });
            return c;
        }
        public IEnumerable<SelectListItem> GetTimeZones(string selectedValue = null)
        {
            IEnumerable<TimeZoneInfo> e = TimeZoneInfo.GetSystemTimeZones(); 
            var c =
             e.Select(a => new SelectListItem()
             {
                 Text = a.DisplayName,
                 Value = a.Id,
                 Selected = selectedValue == a.Id? true : false
             });
            return c;
        }
      
        public void Dispose()
        {
            uow.Dispose();
            GC.SuppressFinalize(this);
        }

       
    }

    public class CheckModel
    {
        public int Id
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public bool Checked
        {
            get;
            set;
        }
    }
}
