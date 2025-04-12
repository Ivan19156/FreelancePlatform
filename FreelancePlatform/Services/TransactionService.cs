using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreelancePlatform.Models;

namespace FreelancePlatform.Services
{
    public class TransactionService
    {
        private readonly FreelancePlatformDbContext _context;
        public TransactionService(FreelancePlatformDbContext context)
        {
            _context = context;
        }

        public bool CreateTransaction(Transaction transaction)
        {
            if (transaction.Amount < 0)
            {
                return false;
            }
            var sender = _context.Users.FirstOrDefault(u => u.Id == transaction.SenderId);
            var receiver = _context.Users.FirstOrDefault(u => u.Id == transaction.ReceiverId);

            if (sender == null || receiver == null)
            {
                return false;
            }
            _context.Transactions.Add(transaction);

            sender.Balance -= transaction.Amount;
            receiver.Balance += transaction.Amount;

            _context.SaveChanges();

            return true;
        }


        public bool DepositFunds(int userId, decimal amount)
        {
            if (amount <= 0)
            {
                return false;
            }
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return false;
            }
            user.Balance += amount;
            var transaction = new Transaction
            {
                SenderId = null,
                Amount = amount,
                Date = DateTime.Now,
            };
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
            return true;
        }

        public bool WithdrawFunds(int userId, decimal amount)
        {
            if (amount <= 0)
            {
                return false;
            }

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return false;
            }
            if (user.Balance < amount)
            { return false; }

            user.Balance -= amount;

            var transaction = new Transaction
            {
                SenderId = userId,
                ReceiverId = null, // немає конкретного отримувача
                Amount = amount,
                Date = DateTime.Now,
                Description = "Виведення коштів"
            };

            _context.Transactions.Add(transaction);
            _context.SaveChanges();
            return true;
        }



        public List<Transaction> GetTransactionsHistory(int userId)
        {
            var transactions = _context.Transactions
                .Where(t => t.SenderId == userId || t.ReceiverId == userId)
                .OrderByDescending(t => t.Date)
                .ToList();

            return transactions;
        }



    }
}
