﻿// <auto-generated />
using System;
using ExchangeRate.Aggregator.Shared.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ExchangeRate.Aggregator.Shared.Infrastructure.Migrations
{
    [DbContext(typeof(ExchangeRateDbContext))]
    partial class ExchangeRateDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ExchangeRate.Aggregator.Shared.Abstractions.Entities.Bank", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ApiSettings")
                        .HasColumnType("jsonb")
                        .HasColumnName("api_settings");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_bank");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_bank_name");

                    b.ToTable("bank");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ApiSettings = "{\"LatestRatesUrl\":\"http://api.exchangeratesapi.io/v1/latest?access_key=413d96c4d38020d4cbf67e45d5cca487\"}",
                            Description = "Bank A...",
                            Name = "Bank A"
                        });
                });

            modelBuilder.Entity("ExchangeRate.Aggregator.Shared.Abstractions.Entities.Currency", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("code");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_currency");

                    b.HasIndex("Code")
                        .IsUnique()
                        .HasDatabaseName("ix_currency_code");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_currency_name");

                    b.ToTable("currency");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Code = "USD",
                            Description = "Unites States Dollar",
                            Name = "Unites States Dollar"
                        },
                        new
                        {
                            Id = 2,
                            Code = "EUR",
                            Description = "Euro",
                            Name = "Euro"
                        });
                });

            modelBuilder.Entity("ExchangeRate.Aggregator.Shared.Abstractions.Entities.Rate", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<int>("BankId")
                        .HasColumnType("integer")
                        .HasColumnName("bank_id");

                    b.Property<int>("BaseCurrencyId")
                        .HasColumnType("integer")
                        .HasColumnName("base_currency_id");

                    b.Property<int>("CurrencyId")
                        .HasColumnType("integer")
                        .HasColumnName("currency_id");

                    b.Property<DateTimeOffset>("DateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_time");

                    b.Property<decimal>("Value")
                        .HasColumnType("numeric")
                        .HasColumnName("value");

                    b.HasKey("Id")
                        .HasName("pk_rate");

                    b.HasIndex("BankId")
                        .HasDatabaseName("ix_rate_bank_id");

                    b.HasIndex("BaseCurrencyId")
                        .HasDatabaseName("ix_rate_base_currency_id");

                    b.HasIndex("CurrencyId")
                        .HasDatabaseName("ix_rate_currency_id");

                    b.HasIndex("DateTime", "CurrencyId", "BaseCurrencyId", "BankId")
                        .IsUnique()
                        .HasDatabaseName("ix_rate_date_time_currency_id_base_currency_id_bank_id");

                    b.ToTable("rate");
                });

            modelBuilder.Entity("ExchangeRate.Aggregator.Shared.Abstractions.Entities.Rate", b =>
                {
                    b.HasOne("ExchangeRate.Aggregator.Shared.Abstractions.Entities.Bank", "Bank")
                        .WithMany("Rates")
                        .HasForeignKey("BankId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_rate_bank_bank_id");

                    b.HasOne("ExchangeRate.Aggregator.Shared.Abstractions.Entities.Currency", "BaseCurrency")
                        .WithMany("BaseRates")
                        .HasForeignKey("BaseCurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_rate_currency_base_currency_id");

                    b.HasOne("ExchangeRate.Aggregator.Shared.Abstractions.Entities.Currency", "Currency")
                        .WithMany("Rates")
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_rate_currency_currency_id");

                    b.Navigation("Bank");

                    b.Navigation("BaseCurrency");

                    b.Navigation("Currency");
                });

            modelBuilder.Entity("ExchangeRate.Aggregator.Shared.Abstractions.Entities.Bank", b =>
                {
                    b.Navigation("Rates");
                });

            modelBuilder.Entity("ExchangeRate.Aggregator.Shared.Abstractions.Entities.Currency", b =>
                {
                    b.Navigation("BaseRates");

                    b.Navigation("Rates");
                });
#pragma warning restore 612, 618
        }
    }
}
