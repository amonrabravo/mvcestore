using MvcEStoreData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCEStoreWeb.Models
{
    public class RegisterViewModel
    {
        [Display(Name = "E-Posta")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Lütfen geçerli bir e-posta adresi yazınız!")]
        public string UserName { get; set; }

        [Display(Name = "Parola")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Parola Tekrar")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "{0} alanı ile {1} alanı aynı olmalıdır!")]
        public string PasswordVerify { get; set; }

        [Display(Name = "Ad Soyad")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        [MaxLength(50, ErrorMessage = "{0} alanı en fazla {1} karakter olmalıdır!")]
        public string Name { get; set; }

        [Display(Name = "Cinsiyetiniz")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        public Genders Gender { get; set; }

    }
}
