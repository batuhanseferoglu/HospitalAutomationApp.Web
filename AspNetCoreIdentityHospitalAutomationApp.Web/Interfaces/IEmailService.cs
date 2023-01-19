namespace AspNetCoreIdentityHospitalAutomationApp.Web.Interfaces
{
    public interface IEmailService
    {
        Task SendResetPasswordEmail(string resetPasswordEmailLink, string ToEmail);

        
    }
}
