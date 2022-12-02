using System.ComponentModel.DataAnnotations;

namespace NuitInfo2022.Models.Entities
{
    public class ChatMessageModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid SenderId { get; set; }
        public ApplicationUser Sender { get; set; }

        [Required]
        public Guid RecipientId { get; set; }
        public ApplicationUser Recipient { get; set; }

        [Required]
        public string CypheredMessage { get; set; }
        
        [Required]
        public DateTime SentAt { get; set; }
    }
}
