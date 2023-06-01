using System.ComponentModel.DataAnnotations;

namespace DaySixMVC.Models
{
    public class RegisterModel
    {
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPasword { get; set; }
    }
}
