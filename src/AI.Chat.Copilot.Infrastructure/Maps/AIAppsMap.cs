using AI.Chat.Copilot.Domain.Models;
using AI.Chat.Copilot.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.Chat.Copilot.Infrastructure.Maps
{
    internal class AIAppsMap : IEntityTypeConfiguration<AIApps>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<AIApps> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();
            builder.Property(u => u.Name).IsRequired().HasColumnType("varchar(50)").IsUnicode();
            builder.Property(u => u.Description).HasColumnType("varchar(200)").IsUnicode();
            builder.Property(u => u.Prompt).HasColumnType("text").IsUnicode();
            builder.Property(u => u.Temperature).HasColumnType("integer");
            builder.Property(u => u.MaxTokens).HasColumnType("integer");
            builder.Property(u => u.AIModelType).HasColumnType("integer").HasConversion<EnumToNumberConverter<AIModelType,int>>();
            builder.Property(u => u.ModelId).HasColumnType("varchar(50)");
            builder.Property(u => u.Secret).HasColumnType("varchar(200)");
            builder.Property(u=>u.Endpoint).HasColumnType("varchar(50)");
            builder.Property(u=>u.CreateTime).IsRequired().HasColumnType("timestamp");
            builder.Property(u => u.IsDeleted).HasColumnType("boolean");
            builder.Property(u => u.DeletedTime).IsRequired(false).HasColumnType("timestamp");
        }
    }
}
