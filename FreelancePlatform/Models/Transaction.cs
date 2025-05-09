﻿namespace FreelancePlatform.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int? SenderId { get; set; }
        public User User { get; set; }
        public int? ReceiverId { get; set; }
       
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }

        public string Description { get; set; }

        public int UserId { get; set; }
    }
}

