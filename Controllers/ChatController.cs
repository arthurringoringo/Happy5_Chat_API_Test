using Happy5ChatTest.DataContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Happy5ChatTest.Models;
using Microsoft.EntityFrameworkCore;

namespace Happy5ChatTest.Controllers
{
    [Authorize]
    [ApiController]
    
    public class ChatController : ControllerBase
    {
        private readonly APIDbContext _context;

        public ChatController(APIDbContext Context)
        {
            _context = Context ?? throw new ArgumentNullException(nameof(Context));
        }

        [HttpPost("chat/send/{username}")]
        public IActionResult createChat(string username,[FromBody] MessageDTO message)
        {
            var senderId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var recieverId = _context.Users.Where(x => x.userName == username).FirstOrDefault();

            if (recieverId == null)
            {
                return NotFound("Reciever not found");
            }
            if (senderId.Equals(recieverId.userId))
            {
                return Ok("Sending to your own user is not allowed");
            }
            var groupId = _context.Groups.Where(x => x.users.Contains(senderId) && x.users.Contains(recieverId.userId.ToString())).FirstOrDefault();
            if (groupId == null)
            {
                Group temp = new Group();
                temp.users = senderId + "," + recieverId.userId.ToString();

                _context.Groups.Add(temp);
                _context.SaveChanges();
                _context.Entry(temp).State = EntityState.Detached;

                Message chat = new Message();
                chat.groupId = temp.groupId;
                chat.senderId = Guid.Parse(senderId);
                chat.recieverId = recieverId.userId;
                chat.message = message.message;
                chat.seen = false;
                chat.timeSent = DateTime.Now;
                try 
                {
                
                    _context.Messages.Add(chat);
                    _context.SaveChanges();
                    _context.Entry(chat).State = EntityState.Detached;

                }
                catch (Exception ex)
                {
                    return Ok($"Message Not Sent: {ex.Message}");
                }
            }
            else
            {
                try
                {
                    Message chat = new Message();
                    chat.groupId = groupId.groupId;
                    chat.senderId = Guid.Parse(senderId);
                    chat.recieverId = recieverId.userId;
                    chat.message = message.message;
                    chat.seen = false;
                    chat.timeSent = DateTime.Now;

                    _context.Messages.Add(chat);
                    _context.SaveChanges();
                    _context.Entry(chat).State = EntityState.Detached;
                }
                catch (Exception ex)
                {
                    return Ok($"Message Not Sent: {ex.Message}");
                }
            }
            return Ok("Message Sent");
        }
        [HttpGet("active/conversation/")]
        public IActionResult showConvo()
        {
            var senderId = User.FindFirstValue(ClaimTypes.NameIdentifier);
           
            var activeGroups = _context.Groups.Include(x => x.messages).Where(x => x.users.Contains(senderId));

            List<ActiveConversationDTO> activeConvo = new List<ActiveConversationDTO>();

            foreach (var group in activeGroups)
            {
                var reciverId = group.users.Split(",");
                if (reciverId[0] == senderId)
                {
                    reciverId[0] = reciverId[1];
                }
                ActiveConversationDTO temp = new ActiveConversationDTO();
                Message lastMessage = group.messages.OrderByDescending(x => x.timeSent).FirstOrDefault();
                temp.lastMessage.message = lastMessage.message;
                temp.lastMessage.timesent = lastMessage.timeSent.ToString("dddd, dd MMMM yyyy HH:mm");
                temp.lastMessage.messageSender = _context.Users.Where(x => x.userId == lastMessage.senderId).FirstOrDefault().userName;
                temp.groupId = group.groupId;
                temp.receiver = _context.Users.Where(x => x.userId.Equals(Guid.Parse(reciverId[0]))).FirstOrDefault().userName;
                temp.unreadMessages = group.messages.Where(x => x.seen == false).Count();
                
                activeConvo.Add(temp);

            }

            return Ok(activeConvo);
        }
    }
}
