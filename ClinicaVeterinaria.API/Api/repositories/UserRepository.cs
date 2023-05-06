using ClinicaVeterinaria.API.Api.db;
using ClinicaVeterinaria.API.Api.model;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.API.Api.repositories
{
    public class UserRepository
    {
        private readonly IDbContextFactory<ClinicaDBContext> ContextFactory;

        public UserRepository() { }

        public UserRepository(IDbContextFactory<ClinicaDBContext> contextFactory)
        {
            ContextFactory = contextFactory;
        }

        public virtual async Task<List<User>> FindAll()
        {
            using ClinicaDBContext context = ContextFactory.CreateDbContext();
            var users = await context.Users.ToListAsync();
            return users ?? new();
        }

        public virtual async Task<User?> FindById(Guid id)
        {
            using ClinicaDBContext context = ContextFactory.CreateDbContext();
            return await context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public virtual async Task<User?> FindByEmail(string email)
        {
            using ClinicaDBContext context = ContextFactory.CreateDbContext();
            return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public virtual async Task<User?> FindByPhone(string phone)
        {
            using ClinicaDBContext context = ContextFactory.CreateDbContext();
            return await context.Users.FirstOrDefaultAsync(u => u.Phone == phone);
        }

        public virtual async Task<User> Create(User user)
        {
            using ClinicaDBContext context = ContextFactory.CreateDbContext();
            context.Users.Add(user);
            await context.SaveChangesAsync();

            return user;
        }

        public virtual async Task<User?> UpdatePassword(string email, string newPassword)
        {
            using ClinicaDBContext context = ContextFactory.CreateDbContext();
            var found = context.Users.FirstOrDefault(u => u.Email == email);
            if (found != null)
            {
                found.Password = newPassword;
                context.Users.Update(found);
                await context.SaveChangesAsync();

                return found;
            }
            return null;
        }

        public virtual async Task<User?> Delete(string email)
        {
            using ClinicaDBContext context = ContextFactory.CreateDbContext();
            var foundUser = context.Users.FirstOrDefault(u => u.Email == email);
            if (foundUser != null)
            {
                context.Users.Remove(foundUser);
                await context.SaveChangesAsync();

                return foundUser;
            }
            return null;
        }
    }
}
