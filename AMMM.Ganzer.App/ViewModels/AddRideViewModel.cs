using AMMM.Ganzer.App.Helpers;
using System.ComponentModel.DataAnnotations;

namespace AMMM.Ganzer.App.ViewModels
{
    public class AddRideViewModel
    {

        [Required(ErrorMessage = "اجباري")]
        [Display(Name = "اسم الرايد")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "اجباري")]
        [Display(Name = "نوع الرايد")]
        public RideType RideType { get; set; }

        [Required(ErrorMessage = "اجباري")]
        [Display(Name = "التاريخ")]
        public string? RideDate { get; set; }

        [Required(ErrorMessage = "اجباري")]
        [Display(Name = "التفاصيل")]
        [MaxLength(300)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "اجباري")]
        [Display(Name = "الوقت")]
        public string? RideTime { get; set; }

        [Required(ErrorMessage = "اجباري")]
        [Display(Name = "المسافة")]
        public int Distance { get; set; }

        [Required(ErrorMessage = "اجباري")]
        [Display(Name = "نقطة التجمع")]
        public string? GatheringPoint { get; set; }

        [Required(ErrorMessage = "اجباري")]
        [Display(Name = "الوجهة")]
        public string? Distenation { get; set; }

        [Required(ErrorMessage = "اجباري")]
        [Display(Name = "درجة الحرارة")]
        public int Temperature { get; set; }

        [Display(Name = "اتجاه الرياح")]
        public string? WindDirection { get; set; }

        [Display(Name = "شدة الرياح")]
        public Intensity? WindIntensity { get; set; }

        [Display(Name = "الامطار")]
        public Intensity? Rain { get; set; }

    }
}
