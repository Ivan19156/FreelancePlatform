using FreelancePlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancePlatform.Services
{
    public class FeedbackService
    {
        private readonly FreelancePlatformDbContext _context;
        public FeedbackService(FreelancePlatformDbContext context)
        {
            _context = context;
        }
        public bool LeaveFeedback(int senderId, int recipientId, int rating, string comment)
        {
            try
            {
                if (rating < 1 || rating > 5)
                {

                    return false;
                }

                var sender = _context.Users.FirstOrDefault(u => u.Id == senderId);
                var recipient = _context.Users.FirstOrDefault(u => u.Id == recipientId);

                if (sender == null || recipient == null)
                {

                    return false;
                }

                var feedback = new Feedback
                {
                    SenderId = senderId,
                    RecipientId = recipientId,
                    Ratings = rating,
                    Comment = comment,
                    Date = DateTime.Now
                };

                _context.Feedbacks.Add(feedback);
                _context.SaveChanges();


                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public List<Feedback> GetFeedbackForUser(int userId)
        {
            return _context.Feedbacks
                .Where(f => f.RecipientId == userId)
                .OrderByDescending(f => f.Date)
                .ToList();
        }

        public double GetAverageRating(int userId)
        {
            var ratings = _context.Feedbacks
                .Where(f => f.RecipientId == userId)
                .Select(f => f.Ratings);

            return ratings.Any() ? ratings.Average() : 0;
        }

    }
}
