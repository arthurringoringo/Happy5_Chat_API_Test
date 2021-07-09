using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Happy5ChatTest.Models
{
    public class MessageDTO
    {

        public string message { get; set; }
    }

    public class ActiveConversationDTO
    {
        public ActiveConversationDTO()
        {
            lastMessage = new LastMessage(); 
        }
        public Guid groupId { get; set; }
        public string receiver { get; set; }
        public int unreadMessages { get; set; }

        public LastMessage lastMessage { get; set; }
    }

    public class LastMessage
    {
        public string messageSender { get; set; }
        public string message { get; set; }
        public string timesent { get; set; }
    }

    public class RegistrationDTO
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

    }
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid userId { get; set; }
        [Required]
        public string userName { get; set; }
        [Required]
        public string password { get; set; }
    }
    public class Group
    { 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid groupId { get; set; }
        public string users { get; set; }
        public ICollection<Message> messages { get; set; }
    }
    public class Message
    { 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid chatId{ get; set; }
        [ForeignKey("groupId")]
        public Guid groupId { get; set; }
        [Required]
        public Guid senderId{ get; set; }
        [Required]
        public Guid recieverId { get; set; }
        [MaxLength(1500)]
        public string message{ get; set; }
        public bool seen { get; set; }
        public DateTime timeSent { get; set; }


        public Group Group{ get; set; }

    }
}
