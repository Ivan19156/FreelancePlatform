using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreelancePlatform.Models;

namespace FreelancePlatform
{
    public class FreelancePlatformDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Chat> Chats { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string conn = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
            optionsBuilder.UseSqlServer(conn);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Project: Customer та Executor
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Customer)
                .WithMany(u => u.OwnedProjects)
                .HasForeignKey(p => p.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Project>()
                .HasOne(p => p.Executor)
                .WithMany(u => u.ExecutedProjects)
                .HasForeignKey(p => p.ExecutorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Request: Freelancer
            modelBuilder.Entity<Request>()
                .HasOne(r => r.Freelancer)
                .WithMany(u => u.Requests)
                .HasForeignKey(r => r.FreelancerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Request: Project
            modelBuilder.Entity<Request>()
                .HasOne(r => r.Project)
                .WithMany(p => p.Requests)
                .HasForeignKey(r => r.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Transaction: User
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Feedback: Sender
            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Sender)
                .WithMany(u => u.SentFeedbacks)
                .HasForeignKey(f => f.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Feedback: Recipient
            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Recipient)
                .WithMany(u => u.ReceivedFeedbacks)
                .HasForeignKey(f => f.RecipientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Chat: Sender
            modelBuilder.Entity<Chat>()
                .HasOne(c => c.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(c => c.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Chat: Recipient
            modelBuilder.Entity<Chat>()
                .HasOne(c => c.Recipient)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(c => c.RecipientId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
    }

