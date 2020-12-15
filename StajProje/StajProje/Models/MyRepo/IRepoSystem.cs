using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StajProje.Models.MyRepo
{
    public interface IRepoSystem
    {
        void IlanEkle(Ev ev);
        Ev IlanGetir(int id);
        List<Ev> TumIlanlar();
        void IlanSil(int id);
        void IlanGuncelle(int id, Ev ev);
        List<Ev> BenimIlanlar(IdentityUser user);
    }
}
