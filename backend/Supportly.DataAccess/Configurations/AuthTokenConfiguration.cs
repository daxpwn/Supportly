using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Supportly.DataAccess.Configurations
{
    public class AuthTokenConfiguration : IEntityTypeConfiguration<AuthToken>
    {
        public void Configure(EntityTypeBuilder<AuthToken> builder)
        {
            builder.Property(x => x.TokenId).HasMaxLength(100).IsRequired();
            builder.HasIndex(x => x.TokenId).IsUnique()
                   .IncludeProperties(x => x.InvalidatedAt);

            builder.HasOne(x => x.User)
                   .WithMany()
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.JwtToken)
                   .WithOne(x => x.RefreshToken)
                   .HasForeignKey<AuthToken>(x => x.BaseTokenId)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
