using Microsoft.AspNetCore.Identity;

namespace TaskManagementSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<TaskModel>? Todos { get; set; }
    }
}
