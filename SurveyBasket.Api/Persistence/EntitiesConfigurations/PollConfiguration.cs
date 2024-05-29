﻿//using System;
//using Microsoft.EntityFrameworkCore.Migrations;
//using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

//namespace SurveyBasket.Api.Persistence.EntitiesConfigurations
//{
//    public class PollConfiguration : IEntityTypeConfiguration<Poll>
//    {
//        public void Configure(EntityTypeBuilder<Poll> builder)
//        {
//            builder.HasIndex(x => x.Title).IsUnique();

//            builder.Property(x => x.Title).HasMaxLength(100);
//            builder.Property(x => x.Summary).HasMaxLength(1500);
//        }
//    }
//}

//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using SurveyBasket.Api.Entities;

//namespace SurveyBasket.Api.Persistence.EntitiesConfigurations
//{
//    public class PollConfiguration : IEntityTypeConfiguration<Poll>
//    {
//        public void Configure(EntityTypeBuilder<Poll> builder)
//        {
//            builder.HasIndex(x => x.Title).IsUnique();

//            builder.Property(x => x.Title).HasMaxLength(100);
//            builder.Property(x => x.Summary).HasMaxLength(1500);

//            var dateOnlyConverter = new DateOnlyConverter();

//            builder.Property(x => x.StartsAt)
//                .HasConversion(dateOnlyConverter)
//                .HasColumnType("date");

//            builder.Property(x => x.EndsAt)
//                .HasConversion(dateOnlyConverter)
//                .HasColumnType("date");
//        }
//    }
//}


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SurveyBasket.Api.Entities;

namespace SurveyBasket.Api.Persistence.EntitiesConfigurations
{
    public class PollConfiguration : IEntityTypeConfiguration<Poll>
    {
        public void Configure(EntityTypeBuilder<Poll> builder)
        {
            builder.HasIndex(x => x.Title).IsUnique();

            builder.Property(x => x.Title).HasMaxLength(100);
            builder.Property(x => x.Summary).HasMaxLength(1500);

            var dateOnlyConverter = new DateOnlyConverter();

            builder.Property(x => x.StartsAt)
                .HasConversion(dateOnlyConverter)
                .HasColumnType("date");

            builder.Property(x => x.EndsAt)
                .HasConversion(dateOnlyConverter)
                .HasColumnType("date");
        }
    }
}
