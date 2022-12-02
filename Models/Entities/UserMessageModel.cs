using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace NuitInfo2022.Models.Entities
{
    public class UserMessageModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public ApplicationUser User { get; set; }

        [Required]
        [MaxLength(500)]
        public string Message { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }
    }
}
