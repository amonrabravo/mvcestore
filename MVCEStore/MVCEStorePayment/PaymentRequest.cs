using System.ComponentModel.DataAnnotations;

namespace MVCEStorePayment
{
    public class PaymentRequest
    {
        [Display(Name = "Kart Sahibinin Adı")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        public string CardHolderName { get; set; }

        [Display(Name = "Kart Numarası")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        [RegularExpression(@"[0-9]{20}", ErrorMessage = "Lütfen geçerli bir kart numarası yazınız!")]
        public string CardNumber { get; set; }

        [Display(Name = "Son Kullanma T. Ay")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        public int ExpireMonth { get; set; }

        [Display(Name = "Son Kullanma T. Yıl")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        public int ExpireYear { get; set; }

        [Display(Name = "Güvenlik Kodu (Cv2)")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        [RegularExpression(@"[0-9]{3}", ErrorMessage = "Lütfen geçerli bir güvenlik kodu yazınız!")]
        public string SecurityCode { get; set; }

        [Display(Name = "Taksit Adedi")]
        public int Instalments { get; set; }

        public decimal Amount { get; set; }

        public string BankName { get; set; }
    }
}
