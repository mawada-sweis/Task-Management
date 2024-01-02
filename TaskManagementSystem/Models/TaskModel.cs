using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using TaskManagementSystem.Data;

namespace TaskManagementSystem.Models
{
    public class TaskModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DueDate { get; set; }
        public string? Status { get; set; }

        // Foreign key property
        public string? UserId { get; set; }

        // Navigation property
        public ApplicationUser? User { get; set; }
    }

    public class TaskCreateViewModel
    {
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        public bool GenerateDescription { get; set; }

        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        public string Status { get; set; }
    }

}
