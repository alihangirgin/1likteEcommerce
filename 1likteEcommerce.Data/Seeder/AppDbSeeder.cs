using _1likteEcommerce.Core.Constants;
using _1likteEcommerce.Core.Models;
using _1likteEcommerce.Data.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace _1likteEcommerce.Data.Seeder
{
    public class AppDbSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (!context.Users.Any())
            {
                context.Users.Add(new User
                {
                    Id = TestUserConstants.UnitTestUserId,
                    UserName = TestUserConstants.UnitTestUserName,
                    Email = TestUserConstants.UnitTestEmail,
                    NormalizedUserName = TestUserConstants.UnitTestUserName.ToUpper(),
                    NormalizedEmail = TestUserConstants.UnitTestEmail.ToUpper(),
                    EmailConfirmed = true,
                    PasswordHash = new PasswordHasher<User>().HashPassword(null, TestUserConstants.UnitTestPassword),
                    UserBasket = new Basket() { BasketItems = new List<BasketItem>(), CreatedAt = DateTime.Now }
                });
                await context.SaveChangesAsync();
            }
            if (!context.Categories.Any())
            {
                context.Categories.Add(new Category
                {
                    CreatedAt = DateTime.Now,
                    Description = TestUserConstants.UnitTestDescription,
                    Title = TestUserConstants.UnitTestTitle,
                    Products = new List<Product>(),
                    Id = TestUserConstants.UnitTestCategoryId
                });
                await context.SaveChangesAsync();
            }

            if (!context.Products.Any())
            {
                context.Products.Add(new Product
                {
                    CategoryId = TestUserConstants.UnitTestCategoryId,
                    BasketItems = new List<BasketItem>(),
                    CreatedAt = DateTime.Now,
                    Description = TestUserConstants.UnitTestDescription,
                    Title = TestUserConstants.UnitTestTitle,
                    Price = 100,
                    Id = TestUserConstants.UnitTestProductId
                });
                await context.SaveChangesAsync();
            }
        }
    }
}
