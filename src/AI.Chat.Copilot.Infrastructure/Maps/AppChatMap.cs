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
    internal class AppChatMap : IEntityTypeConfiguration<AppChat>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<AppChat> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();
            builder.Property(u => u.Title).IsRequired().HasColumnType("varchar(50)").IsUnicode();
            builder.Property(u=>u.CreateTime).IsRequired().HasColumnType("timestamp");
            builder.Property(u => u.IsDeleted).HasColumnType("boolean");
            builder.Property(u => u.DeletedTime).IsRequired(false).HasColumnType("timestamp");
        }
    }
}
