using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace StajProje.Models.MyRepo
{
    public class RepoSystem : IRepoSystem
    {
        private readonly MyDbContext _myDbContext;
        public RepoSystem(MyDbContext myDbContext)
        {
            _myDbContext = myDbContext;
        }
        public List<Ev> BenimIlanlar(IdentityUser user)
        {
            var x = _myDbContext.Evler.Include(i => i.user).ToList<Ev>();
            foreach (var i in x.ToArray())
            {
                if (i.user != user) x.Remove(i);
            }
            return x;
        }

        public void IlanEkle(Ev ev)
        {
            _myDbContext.Evler.Add(ev);
            _myDbContext.SaveChanges();
        }

        public Ev IlanGetir(int id)
        {
            return _myDbContext.Evler
               .FirstOrDefault(p => p.IlanID == id);
        }

        public void IlanGuncelle(int id, Ev ev)
        {
            var temp = IlanGetir(id);
            temp.IlanBaslik = ev.IlanBaslik;
            temp.IlanTip = ev.IlanTip;
            temp.resim = ev.resim;
            temp.Fiyat = ev.Fiyat;
            temp.YuzOlcumu = ev.YuzOlcumu;
            temp.YatakOdasi = ev.YatakOdasi;
            temp.Banyo = ev.Banyo;
            temp.Garaj = ev.Garaj;
            temp.IlanAciklama = ev.IlanAciklama;
            temp.Adress = ev.Adress;
            _myDbContext.Update(temp);
            _myDbContext.SaveChanges();
        }

        public void IlanSil(int id)
        {
            Ev temp = _myDbContext.Evler.FirstOrDefault(i => i.IlanID == id);
            _myDbContext.Evler.Remove(temp);
            _myDbContext.SaveChanges();
        }

        

        public List<Ev> TumIlanlar()
        {
            return _myDbContext.Evler.Include(c => c.user).ToList<Ev>();

        }
    }
}
