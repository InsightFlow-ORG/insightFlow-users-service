using System;
using System.Threading.Tasks;
using Bogus;
using userService.src.dtos;
using userService.src.services;

namespace userService.src.data
{
    /// <summary>
    /// In-memory data seeder for populating initial user data.
    /// </summary>
    public static class InMemorySeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var userService = services.GetService(typeof(IUserService)) as IUserService;
            if (userService == null) return;

            
            if (userService.GetAllUsers().Count > 0) return;

            var faker = new Faker("es");

            for (int i = 0; i < 10; i++)
            {
                var name = faker.Name.FirstName();
                var lastName = faker.Name.LastName();
                var email = $"{name.ToLower()}.{lastName.ToLower()}@insightflow.com";
                var userName = $"{name.ToLower()}.{lastName.ToLower()}";

                if (userService.EmailExists(email) || userService.UserNameExists(userName))
                    continue;

                userService.CreateUser(new CreateUserRequest
                {
                    Name = name,
                    LastName = lastName,
                    Email = email,
                    UserName = userName,
                    DateOfBirth = DateOnly.FromDateTime(faker.Date.Past(30, DateTime.Today.AddYears(-18))),
                    Address = faker.Address.FullAddress(),
                    PhoneNumber = faker.Phone.PhoneNumber(),
                    Password = "Passw0rd2!",
                });
            }

    
            var adminEmail = "admin@insightflow.com";
            if (!userService.EmailExists(adminEmail))
            {
                var admin = userService.CreateUser(new CreateUserRequest
                {
                    Name = "Admin",
                    LastName = "User",
                    Email = adminEmail,
                    UserName = "admin",
                    DateOfBirth = new DateOnly(1990, 1, 1),
                    Address = "Admin Address",
                    PhoneNumber = "000000000",
                    Password = "Admin1234!"
                });
                
                
                admin.Role = "Admin";
            }

            await Task.CompletedTask;
        }
    }
}