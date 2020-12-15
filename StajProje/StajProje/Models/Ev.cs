using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StajProje.Models
{
    public class Ev
    {
        [Key]
        public int IlanID { get; set; }
        [Display(Name = "İlan Başlığı")]
        public string IlanBaslik { get; set; }
        [Display(Name = "İlan Tipi")]
        public string IlanTip { get; set; }
        [Display(Name = "Resim")]
        public string resim { get; set; }
        [Display(Name = "Fiyat")]
        public double Fiyat { get; set; }
        [Display(Name = "Alan Büyüklüğü m2")]
        public int YuzOlcumu { get; set; }
        [Display(Name = "Yatak Odası")]
        public int YatakOdasi { get; set; }
        [Display(Name = "Banyo")]
        public int Banyo { get; set; }
        [Display(Name = "Garaj")]
        public int Garaj { get; set; }
        [Display(Name = "İlan Açıklaması")]
        public string IlanAciklama { get; set; }
        [Display(Name = "Açık Adres")]
        public string Adress { get; set; }
        public IdentityUser user { get; set; }
    }
}
