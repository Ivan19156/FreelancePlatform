using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using FreelancePlatform.Models;

namespace FreelancePlatform.Services
{
    public class ChatService
    {
        private readonly FreelancePlatformDbContext _context;
        public ChatService(FreelancePlatformDbContext context) { _context = context; }

        public bool SendMessage(Chat message)
        {
            if (message == null) return false;
            _context.Chats.Add(message);
            _context.SaveChanges();
            return true;
        }

        public List<Chat> GetChatHistory(int chatId)
        {
            var chatHistory = _context.Chats.Where(c => c.Id == chatId).ToList();
            return chatHistory;
        }
    }
}
