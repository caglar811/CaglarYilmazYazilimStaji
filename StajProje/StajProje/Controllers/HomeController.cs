using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StajProje.Models;
using StajProje.Models.MyRepo;

namespace StajProje.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepoSystem _repo;

        public HomeController(IRepoSystem repo)
        {
            _repo = repo;
        } 
        public IActionResult Index()
        {
            var mekanlar = _repo.TumIlanlar();
            return View(mekanlar.OrderBy(a => Guid.NewGuid()));
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
