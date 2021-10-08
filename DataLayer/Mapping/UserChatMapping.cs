using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Mapping
{
    public class UserChatMapping : IEntityTypeConfiguration<UserChat>
    {
        public void Configure(EntityTypeBuilder<UserChat> builder)
        {
            builder.ToTable("UserChats");
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Account)
                .WithMany(x => x.UserChats)
                .HasForeignKey(x => x.AccountId);

            builder.HasOne(x => x.Chat)
                .WithMany(x => x.UserChats)
                .HasForeignKey(x => x.ChatId);
        }
    }
}