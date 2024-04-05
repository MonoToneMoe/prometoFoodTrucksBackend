using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace prometoFoodTrucksBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppController : Controller
    {
        private readonly UserService _data;

        public UserController(UserService data){
            _data = data;
        }


        [HttpPost]
        [Route("AddAppItem")]
        public bool AddAppItem(AppItemModel newAppItemModel){
            return _data.AddAppItemModel(newAppItemModel);
        }
        

        [HttpGet]
        [Route("GetAllAppItems")]
        public IEnumerable<AppItemModel> GetAllAppItems(){
            return _data.GetAllAppItems();
        }

        [HttpGet]
        [Route("GetItemsByUserId/{userId}")]
        public IEnumerable<AppItemModel> GetItemsByUserId(int userId){
            return _data.GetItemsByUserId(userId);
        }


        [HttpGet]
        [Route("GetItemsByCategory/{category}")]
        public IEnumerable<AppItemModel> GetItemsByCategory(string category){
            return _data.GetItemsByCategory(category);
        }

        [HttpGet]
        [Route("GetPublishedItems")]
        public IEnumerable<AppItemModel> GetPublishedItems(){
            return _data.GetPublishedItems();
        }


        [HttpGet]
        [Route("GetAllItemsByTags/{tag}")]
        public List<AppItemModel> GetAllItemsByTags(string tag){
            return _data.GetAllItemsByTags(tag);
        }

        [HttpGet]
        [Route("GetAppItemById/{id}")]
        public AppItemModel GetAppItemById(int id){
            return _data.GetAppItemById(id);
        }

        [HttpPut]
        [Route("UpdateAppItem")]
        public bool UpdateAppItem(AppItemModel AppUpdate){
            return _data.UpdateAppItem(AppUpdate);
        }


        [HttpDelete]
        [Route("DeleteAppItem")]
        public bool DeleteAppItem(AppItemModel AppToDelete){
            return _data.DeleteAppItem(AppToDelete);
        }


    }
}