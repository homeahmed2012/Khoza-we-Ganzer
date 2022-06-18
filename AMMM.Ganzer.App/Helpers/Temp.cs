using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AMMM.Ganzer.App.Helpers
{
    public enum Governate
    {
        [Display(Name = "القاهرة")]
        Cairo = 0,
        
        [Display(Name = "اخري")]
        Other = 1
    }

    public enum District
    {
        [Display(Name = "مصر الجديدة")]
        MasrElgadida = 0,

        [Display(Name = "العباسية")]
        Abbasya = 1,
        
        [Display(Name = "اخري")]
        Other = 30
    }

    public enum FitnessLevel
    {
        [Display(Name = "منخفض")]
        low = 0,
        
        [Display(Name = "متوسط")]
        mid = 1,
        
        [Display(Name = "عالي")]
        high = 2,
    }

    public enum RideType
    {
        [Display(Name = "رايد")]
        NoramlRide = 1,

        [Display(Name = "رايد طويل")]
        LongNormalRide = 2,

        [Display(Name = "تمرينة")]
        Exercise = 3,

        [Display(Name = "تمرينة قوية")]
        StrongExercise = 4,
        
        [Display(Name = "رايد مهم")]
        ImportantRide = 5,

        [Display(Name = "سفرية")]
        Trip = 6,

        [Display(Name = "سفرية عظيمة")]
        GreatTrip = 7
    }

    public enum Intensity
    {
        [Display(Name = "خفيفة")]
        Light = 6,

        [Display(Name = "متوسطة")]
        Moderate = 6,

        [Display(Name = "شديدة")]
        Intense = 6
    }

    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            string displayName;
            displayName = enumValue.GetType()
                .GetMember(enumValue.ToString())
                .FirstOrDefault()
                .GetCustomAttribute<DisplayAttribute>()?
                .GetName();
            if (String.IsNullOrEmpty(displayName))
            {
                displayName = enumValue.ToString();
            }
            return displayName;
        }
    }

}
