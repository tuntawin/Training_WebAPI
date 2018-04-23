using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Model
{
    public class ApplicationDbContext : IdentityDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //IConfigurationRoot configuration = new ConfigurationBuilder()
            //.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            //.AddJsonFile("appsettings.json")
            //.Build();


            string defaultConnection = @"Data Source=TEERAPONG\SQLSERVER2014; Initial Catalog = calendarionic; Persist Security Info = True; User ID = oceaweb; Password =oceaweb";




            optionsBuilder.UseSqlServer(defaultConnection);            
        }
    }
}
