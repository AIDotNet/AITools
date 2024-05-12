using AI.Chat.Copilot.Domain.Models;
using AI.Chat.Copilot.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.Chat.Copilot.Infrastructure.Maps
{
    internal class AppChatHistoriesMap : IEntityTypeConfiguration<AppChatHistories>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<AppChatHistories> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();
            builder.Property(u => u.ChatId).HasColumnType("integer");
            builder.Property(u => u.RoleName).IsRequired().HasColumnType("varchar(50)");
            builder.Property(u => u.Content).IsRequired().HasColumnType("text").IsUnicode();
            builder.Property(u=>u.CreateTime).IsRequired().HasColumnType("timestamp");
        }
    }
}
