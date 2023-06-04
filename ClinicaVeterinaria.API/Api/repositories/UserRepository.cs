using ClinicaVeterinaria.API.Api.db;
using ClinicaVeterinaria.API.Api.model;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.API.Api.repositories
{
    public class UserRepository
    {
        private readonly ClinicaDBContext Context;

        public UserRepository(ClinicaDBContext context)
        {
            Context = context;
            context.Database.Migrate();
        }

        public UserRepository() { }

        public virtual async Task<List<User>> FindAll()
        {
            var users = await Context.Users.ToListAsync();
            return users.FindAll(u => u.Active == true) ?? new();
        }

        public virtual async Task<User?> FindById(Guid id)
        {
            return await Context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public virtual async Task<User?> FindByEmail(string email)
        {
            return await Context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public virtual async Task<User?> FindByPhone(string phone)
        {
            return await Context.Users.FirstOrDefaultAsync(u => u.Phone == phone);
        }

        public virtual async Task<User> Create(User user)
        {
            Context.Users.Add(user);
            await Context.SaveChangesAsync();

            return user;
        }

        public virtual async Task<User?> UpdatePassword(string email, string newPassword)
        {
            var found = Context.Users.FirstOrDefault(u => u.Email == email);
            if (found != null)
            {
                found.Password = newPassword;
                Context.Users.Update(found);
                await Context.SaveChangesAsync();

                return found;
            }
            return null;
        }

        public virtual async Task<User?> SwitchActivity(string email)
        {
            var foundUser = Context.Users.FirstOrDefault(u => u.Email == email);
            if (foundUser != null)
            {
                foundUser.Active = !foundUser.Active;
                Context.Users.Update(foundUser);
                await Context.SaveChangesAsync();

                return foundUser;
            }
            return null;
        }
    }
}
