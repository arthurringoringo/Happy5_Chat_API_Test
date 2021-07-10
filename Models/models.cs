using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Happy5ChatTest.Models
{
    //Get list of users Data TransferObjects
    public class UsersListDTO
    {
        public string username { get; set; }
    }
    //Sending Message Data Transfer object
    public class SendMessageDTO
    {

        public string message { get; set; }
    }
    // Data Transfer object for listing active convesation
    public class ActiveConversationDTO
    {
        //Each conversation contains Latest message with message Data Object
        public ActiveConversationDTO()
        {
            latestMessage = new MessageDTO();
        }
        public Guid groupId { get; set; }
        public string receiver { get; set; }
        public int unreadMessages { get; set; }

        public MessageDTO latestMessage { get; set; }
    }
    //Data Transfer Object for specific Conversation
    public class ConversationDTO
    {
        public ConversationDTO()
        {
            messages = new List<MessageDTO>();
        }
        public Guid conversationId  { get; set; }
        public string reciever { get; set; }
        public List<MessageDTO> messages{ get; set; }
    }
    //Data Transfer object for messages
    public class MessageDTO
    {
        public string messageSender { get; set; }
        public string message { get; set; }
        public string timesent { get; set; }
    }
    //Data Transfer Object for Registration
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
