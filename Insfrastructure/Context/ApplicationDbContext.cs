using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insfrastructure.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Customer>()
                .HasIndex(c => new { c.Firstname, c.Lastname, c.DateOfBirth })
                .IsUnique();

            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.Email)
                .IsUnique();

            modelBuilder.Entity<Customer>().HasData(
            new Customer
            {
                Id = 1,
                Firstname = "Ali",
                Lastname = "Taami",
                DateOfBirth = new DateTime(2002, 10, 11),
                PhoneNumber = "9301327634", // Valid phone number
                Email = "alitaami81@gmail.com", // Valid email
                BankAccountNumber = "5859831001081461" // Valid 16-digit bank account number
            },
             new Customer
             {
                 Id = 2,
                 Firstname = "Ata",
                 Lastname = "Taami",
                 DateOfBirth = new DateTime(2012, 10, 11),
                 PhoneNumber = "9301327624", // Valid phone number
                 Email = "atataami91@gmail.com", // Valid email
                 BankAccountNumber = "5859831001081462" // Valid 16-digit bank account number
             }

             // Add more customer data as needed
             );

            base.OnModelCreating(modelBuilder);
        }
    }
}