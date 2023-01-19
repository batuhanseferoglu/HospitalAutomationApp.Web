using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentityHospitalAutomationApp.Web.Localizations
{
    public class LocalizationIdentityErrorDescriber:IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName)
        {
            return new() {Code="DuplicateUserName", Description= $"{userName} daha önce başka bir kullanıcı tarafından alınmıştır!" };
        }

        public override IdentityError DuplicateEmail(string email)
        {
            return new() { Code = "DuplicateEmail", Description = $"{email} daha önce başka bir kullanıcı tarafından alınmıştır!" };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new() { Code = "PasswordTooShort", Description = "Şifre en az 8 karakterden oluşmalıdır!" };
        }

        public override IdentityError InvalidToken()
        {
            return new() { Code = "InvalidToken", Description = "Güvenlik nedeni ile bu bağlantının kullanım süresi dolmuştur. Lütfen tekrar deneyiniz!" };
        }
    }
}
