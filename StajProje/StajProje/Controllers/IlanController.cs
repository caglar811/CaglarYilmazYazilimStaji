using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StajProje.Models;
using StajProje.Models.MyRepo;
using StajProje.Models.Search;

namespace StajProje.Controllers
{
    public class IlanController : Controller
    {
        #region Constructors
        private readonly IRepoSystem _repo;
        private readonly MyDbContext _myDbContext;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHostingEnvironment _appEnvironment;

        public IlanController(IRepoSystem repo, SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager, MyDbContext myDbContext, IHostingEnvironment appEnvironment)
        {
            _myDbContext = myDbContext;
            _repo = repo;
            _signInManager = signInManager;
            _userManager = userManager;
            _appEnvironment = appEnvironment;
        }
        #endregion
        public async Task<IActionResult> Ara(string Keyword, string type, string city, string bedroom, string bathroom, string garage, string price)
        {
            var ilanlar = from m in _myDbContext.Evler.Include(c => c.user)
                         select m;
          

            if (!String.IsNullOrEmpty(Keyword))
            {
                ilanlar = ilanlar.Where(s => s.IlanBaslik.Contains(Keyword));
            }
            if (!String.IsNullOrEmpty(type) && type!="Hepsi")
            {
                ilanlar = ilanlar.Where(s => s.IlanTip==type);
            }
            if (!String.IsNullOrEmpty(city) && city != "Hepsi")
            {
                ilanlar = ilanlar.Where(s => s.Adress.Contains(city));
            }
            if (!String.IsNullOrEmpty(bedroom) && bedroom != "Hepsi")
            {
                int yatak = Convert.ToInt32(bedroom);
                ilanlar = ilanlar.Where(s => s.YatakOdasi >= yatak);
            }
            if (!String.IsNullOrEmpty(bathroom) && bathroom != "Hepsi")
            {
                int banyo = Convert.ToInt32(bathroom);
                ilanlar = ilanlar.Where(s => s.Banyo >= banyo);
            }
            if (!String.IsNullOrEmpty(garage) && garage != "Hepsi")
            {
                int garaj = Convert.ToInt32(garage);
                ilanlar = ilanlar.Where(s => s.Garaj >= garaj);
            }
            if (!String.IsNullOrEmpty(price) && price != "Hepsi")
            {
                int fiyat = Convert.ToInt32(price);
                ilanlar = ilanlar.Where(s => s.Fiyat <=fiyat);
            }

            return View(await ilanlar.ToListAsync());
        }
        public IActionResult Index()
        {
            return View(_repo.TumIlanlar());
        }
        [Authorize]
        public IActionResult IlanEkle()
        {
           
            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> IlanEkle(Ev ev)
        {
            // ModelState nesnesini temizliyoruz
            ModelState.Clear();
            // formda eksik olan kullanıcı alanını dolduruyoruz
            ev.user = await _userManager.GetUserAsync(HttpContext.User);
            // formu tekrar doğruluyoruz
            TryValidateModel(ev);
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                foreach (var Image in files)
                {
                    if (Image != null && Image.Length > 0)
                    {
                        var file = Image;
                        //There is an error here
                        var uploads = Path.Combine(_appEnvironment.WebRootPath, "img\\ilan");
                        if (file.Length > 0)
                        {
                            var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                            using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                                ev.resim = fileName;
                            }

                        }
                    }
                }

                _repo.IlanEkle(ev);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }

        [Authorize]
        public async Task<IActionResult> Ilanlarim()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            return View(_repo.BenimIlanlar(user));
        }
        [Authorize]
        public async Task<IActionResult> IlanSil(int id)
        {
            // giriş yapan kullanıcıyı alıyoruz
            var user = await _userManager.GetUserAsync(HttpContext.User);
            try
            {
                // ilgili post giriş yapan kullanıcıya mı ait?
                if (_repo.IlanGetir(id).user == user)
                    _repo.IlanSil(id);
                else
                    return NotFound("404 Sayfa Bulunamadı");
            }
            catch (Exception)
            {
                return NotFound("404 Sayfa Bulunamadı");
            }
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public async Task<IActionResult> IlanGuncelle(int id)
        {
            
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var ilan = _repo.IlanGetir(id);
            if (ilan == null) return NotFound("404 Sayfa Bulunamadı");
            if (ilan.user != user) return NotFound("404 Sayfa Bulunamadı");
            return View(ilan);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> IlanGuncelle(int id, Ev ilan)
        {
            ModelState.Clear();
            ilan.user = await _userManager.GetUserAsync(HttpContext.User);
            TryValidateModel(ilan);
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                foreach (var Image in files)
                {
                    if (Image != null && Image.Length > 0)
                    {
                        var file = Image;
                        //There is an error here
                        var uploads = Path.Combine(_appEnvironment.WebRootPath, "img\\ilan");
                        if (file.Length > 0)
                        {
                            var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                            using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                                ilan.resim = fileName;
                            }

                        }
                    }
                }
                _repo.IlanGuncelle(id, ilan);
                return RedirectToAction("Ilanlarim", "Ilan");
            }
            else
            {
                return View();
            }
        }

        [Route("{username}")]
        public async Task<IActionResult> IlanDetay(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound("404 Sayfa Bulunamadı");
            var temp = _repo.BenimIlanlar(user);
            return View("Index", temp);
        }

        [Route("{username}/ilan/{id?}")]
        public IActionResult IlanDetay(string username, int id)
        {
            return View(_repo.IlanGetir(id));

        }
    }
}