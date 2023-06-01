using Microsoft.AspNetCore.Identity;

namespace DaySixMVC.Models
{
    public class AppUser:IdentityUser
    {
        public string Name { get; set; }
    }
}
