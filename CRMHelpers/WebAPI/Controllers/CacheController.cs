using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class CacheController : Controller
    {
        // GET: Cache
        public ActionResult Index()
        {
            try
            {
                var viewModel = new CacheViewModel();

                return View(viewModel);
            }
            catch (Exception ex)
            {
                string additionalErrorMessage = ex.Message;

                additionalErrorMessage += " Ensure Redis Server is Running on localhost: ";
                try
                {
                    additionalErrorMessage += " " + ConfigurationManager.AppSettings["RedisConnectionString"];
                    additionalErrorMessage += " " + ConfigurationManager.AppSettings["ReddisServerName"];
                }
                catch
                {
                    additionalErrorMessage +=
                        "Configuration Settings RedisConnectionString, ReddisServerName Not Found";
                }

                throw new Exception(additionalErrorMessage);
            }
        }

        public HttpResponseMessage AddEntityToCache(string EntityName)
        {
            try
            {

                var viewModel = new CacheViewModel();

                return viewModel.AddEntityToCache(EntityName);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public HttpResponseMessage Delete(string key)
        {
            try
            {

                var viewModel = new CacheViewModel();

                return viewModel.DeleteFromCache(key);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public HttpResponseMessage DeleteAllKeys()
        {
            try
            {
                var viewModel = new CacheViewModel();

                return viewModel.DeleteAllFromCache();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}