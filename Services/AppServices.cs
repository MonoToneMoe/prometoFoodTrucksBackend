using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using prometoFoodTrucksBackend.Models;
using prometoFoodTrucksBackend.Services.DbContext;

namespace prometoFoodTrucksBackend.Services
{
    public class AppServices
    {
        private readonly DataContext _context;

        public AppServices(DataContext context)
        {
            _context = context;
        }

        public bool AddAppItemModelItem(AppItemModel newAppItem)
        {
            _context.Add(newAppItem);
            return _context.SaveChanges() != 0;
        }

        //IEnumerable enables iteration over collection of object (AppItemModel).  This allows access to each object one by one
        public IEnumerable<AppItemModel> GetAllAppItems()
        {
            return _context.AppInfo;
        }

        public IEnumerable<AppItemModel> GetItemsByUserId(int userId)
        {
            return _context.AppInfo.Where(item => item.UserID == userId);
        }


        public IEnumerable<AppItemModel> GetItemsByCategory(string category)
        {
            return _context.AppInfo.Where(item => item.Categories == category);
        }

        public IEnumerable<AppItemModel> GetPublishedItems()
        {
            return _context.AppInfo.Where(item => item.IsPublished == true);
        }

        public List<AppItemModel> GetAllItemsByTags(string tag)
        {
            var allItems = GetAllAppItems().ToList();

            var filteredItems = allItems.Where(item => item.Tags.Split(",").Contains(tag)).ToList();

            return filteredItems;
        }

        public AppItemModel GetAppItemById(int id)
        {
            return _context.AppInfo.SingleOrDefault(item => item.ID == id);
        }


        public bool UpdateAppItem(AppItemModel AppUpdate)
        {
            _context.Update<AppItemModel>(AppUpdate);
            return _context.SaveChanges() != 0;
        }


        public bool DeleteAppItem(AppItemModel AppToDelete)
        {
            AppToDelete.IsDeleted = true;
            _context.Update<AppItemModel>(AppToDelete);
            return _context.SaveChanges() != 0; 
        }
    }
}