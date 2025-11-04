using Microsoft.AspNetCore.Identity;

namespace cinemaTask.Models
{
    public class ApplicationUser :IdentityUser
    {
        public String Name { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;

        internal string? Adapt<T>()
        {
            throw new NotImplementedException();
        }
    }
}
