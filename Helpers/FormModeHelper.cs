namespace DOTP_BE.Helpers
{
    public class FormModeHelper
    {
        public static readonly Dictionary<string, string> formModeIndexMap = new Dictionary<string, string>
        {
            { "ExtendOperatorLicense", "လုပ်ငန်းလိုင်စင်သက်တမ်းတိုးခြင်း" }, //v1
            { "ExtendCarLicense", "ယာဥ်လိုင်စင်သက်တမ်းတိုး" }, //v1
            { "AddNewCar", "ယာဥ်အင်အားတိုးခြင်း" }, //v1
            { "Decrease Car", "ယာဥ်အင်အားလျှော့ခြင်း" }, //v1
            { "ChangeLicenseOwnerAddress", "လိုင်စင်ရလုပ်ငန်းပိုင်ရှင်နေရပ်လိပ်စာပြောင်းလဲခြင်း" },
            { "ChangeVehicleOwnerAddress", "ယာဥ်ပိုင်ရှင်နေရပ်လိပ်စာပြောင်းလဲခြင်း" },
            { "ChangeVehicleOwnerName", "ယာဥ်ပိုင်ရှင်အမည်ပြောင်းလဲခြင်း" },
            { "ChangeVehicleType", "ယာဥ်အမျိုးအစားပြောင်းလဲခြင်း" }
        };
    }
}
