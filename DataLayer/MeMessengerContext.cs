using DataLayer.Entities;
using DataLayer.Mapping;
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class MeMessengerContext : DbContext
    {
        public MeMessengerContext(DbContextOptions<MeMessengerContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<UserChat> UserChats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assembly = typeof(AccountMapping).Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}