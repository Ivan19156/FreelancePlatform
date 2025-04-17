using System;
using Microsoft.EntityFrameworkCore;
using FreelancePlatform.Models;
using FreelancePlatform.Services;

namespace FreelancePlatform
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new DbContextOptionsBuilder<FreelancePlatformDbContext>()
                .UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=FreelancePlatformDB;Integrated Security=True;")
                .Options;

            using (var context = new FreelancePlatformDbContext(options))
            {
                var chatService = new ChatService(context);
                var feedbackService = new FeedbackService(context);
                var projectService = new ProjectService(context);
                var requestService = new RequestService(context);
                var transactionService = new TransactionService(context);
                var userService = new UserService(context);

                bool exit = false;
                while (!exit)
                {
                    Console.WriteLine("\nВиберіть дію:");
                    Console.WriteLine("1 - Зареєструвати користувача");
                    Console.WriteLine("2 - Створити проєкт");
                    Console.WriteLine("3 - Подати заявку на проєкт");
                    Console.WriteLine("4 - Виконати транзакцію");
                    Console.WriteLine("5 - Залишити відгук");
                    Console.WriteLine("0 - Вийти");

                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            Console.Write("Введіть ім'я: ");
                            string name = Console.ReadLine();
                            Console.Write("Введіть email: ");
                            string email = Console.ReadLine();
                            Console.Write("Введіть пароль: ");
                            string password = Console.ReadLine();
                            Console.Write("Введіть роль (freelancer/customer): ");
                            string role = Console.ReadLine();
                            userService.RegisterUser(new User { Name = name, Email = email, Password = password, Role = role, Balance = 100m });
                            Console.WriteLine("Користувач зареєстрований!");
                            break;

                        case "2":
                            Console.Write("Введіть назву проєкту: ");
                            string projectName = Console.ReadLine();
                            Console.Write("Введіть опис: ");
                            string description = Console.ReadLine();
                            Console.Write("Введіть бюджет: ");
                            decimal budget = Convert.ToDecimal(Console.ReadLine());
                            Console.Write("Введіть ID замовника: ");
                            int customerId = Convert.ToInt32(Console.ReadLine());
                            projectService.CreateProject(new Project { Name = projectName, Description = description, Budget = budget, Status = "Open", CustomerId = customerId });
                            Console.WriteLine("Проєкт створено!");
                            break;

                        case "3":
                            Console.Write("Введіть ID фрілансера: ");
                            int freelancerId = Convert.ToInt32(Console.ReadLine());
                            Console.Write("Введіть ID проєкту: ");
                            int projectId = Convert.ToInt32(Console.ReadLine());
                            Console.Write("Введіть запропоновану ціну: ");
                            decimal offeredPrice = Convert.ToDecimal(Console.ReadLine());
                            requestService.SubmitRequest(new Request { FreelancerId = freelancerId, ProjectId = projectId, OfferedPrice = offeredPrice, StartDate = DateTime.Now });
                            Console.WriteLine("Заявка подана!");
                            break;

                        case "4":
                            Console.Write("Введіть ID відправника: ");
                            int senderId = Convert.ToInt32(Console.ReadLine());
                            Console.Write("Введіть ID отримувача: ");
                            int receiverId = Convert.ToInt32(Console.ReadLine());
                            Console.Write("Введіть суму: ");
                            decimal amount = Convert.ToDecimal(Console.ReadLine());
                            transactionService.CreateTransaction(new Transaction { SenderId = senderId, ReceiverId = receiverId, Amount = amount, Date = DateTime.Now, Description = "Оплата за послугу" });
                            Console.WriteLine("Транзакція виконана!");
                            break;

                        case "5":
                            Console.Write("Введіть ID відправника: ");
                            int feedbackSenderId = Convert.ToInt32(Console.ReadLine());
                            Console.Write("Введіть ID отримувача: ");
                            int feedbackRecipientId = Convert.ToInt32(Console.ReadLine());
                            Console.Write("Введіть рейтинг (1-5): ");
                            int rating = Convert.ToInt32(Console.ReadLine());
                            Console.Write("Введіть коментар: ");
                            string comment = Console.ReadLine();
                            feedbackService.LeaveFeedback(feedbackSenderId, feedbackRecipientId, rating, comment);
                            Console.WriteLine("Відгук залишено!");
                            break;

                        case "0":
                            exit = true;
                            Console.WriteLine("Вихід...");
                            break;

                        default:
                            Console.WriteLine("Невірний вибір, спробуйте знову.");
                            break;
                    }
                }
            }
        }
    }
}