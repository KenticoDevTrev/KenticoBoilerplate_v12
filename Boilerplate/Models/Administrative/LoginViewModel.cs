using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Models.Administrative
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email Address Required")]
        [DisplayName("Email Address")]
        //[EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string UserName { get; set; }


        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }


        [DisplayName("Remember Me")]
        public bool StaySignedIn { get; set; }
    }
}