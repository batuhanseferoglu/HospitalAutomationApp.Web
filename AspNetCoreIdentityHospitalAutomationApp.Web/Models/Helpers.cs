namespace AspNetCoreIdentityHospitalAutomationApp.Web.Models
{
    public static class Helpers
    {
        public static object ToDBNullOrDefault(this object obj)
        {
            return obj ?? DBNull.Value;
        }
    }
}
