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
    internal class AppChatMessageMap : IEntityTypeConfiguration<AppChatMessage>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<AppChatMessage> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();
            builder.Property(u => u.ChatId).HasColumnType("varchar(50)");
            builder.Property(u => u.Role).IsRequired().HasColumnType("varchar(50)");
            builder.Property(u => u.Content).IsRequired().HasColumnType("text").IsUnicode();
            builder.Property(u=>u.CreateTime).IsRequired().HasColumnType("timestamp");
        }
    }
}
