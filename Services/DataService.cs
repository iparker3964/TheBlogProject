using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheBlogProject.Data;
using TheBlogProject.Enums;
using TheBlogProject.Models;

namespace TheBlogProject.Services
{
    public class DataService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<BlogUser> _userManager;
        public DataService(ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<BlogUser> userManager)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task ManageDataAsync()
        {
            //Create the DB fro the migrations
            await _dbContext.Database.MigrateAsync();
            //Task 1:Seed few Roles into the system.
            await SeedRolesAsync();

            //Task 2:Seed a few users into the system.
            await SeedUsersAsync();
        }

        private async Task SeedRolesAsync()
        {
            //If there are roles in the system do nothing
            if (_dbContext.Roles.Any())
            {
                return;
            }
            else
            {
                foreach (var role in Enum.GetNames(typeof(BlogRole)))
                {
                    //I need to use the role manager to create roles
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
        private async Task SeedUsersAsync()
        {
            //If there are users in the system do nothing
            if (_dbContext.Users.Any())
            {
                return;
            }
            else
            {
                //Step 1: Creates a new instance of blog user
                var adminUser = new BlogUser()
                {
                    Email = "iparker3964@gmail.com",
                    UserName = "iparker3964@gmail.com",
                    FirstName = "Isaiah",
                    LastName = "Parker",
                    PhoneNumber = "(800)555-1212",
                    EmailConfirmed = true
                    
                };

                //Step 2: Use the User manager to create a new user that is defined by adminUser
                await _userManager.CreateAsync(adminUser, "Abc@123");

                //Step 3: Add new user to administation role
                await _userManager.AddToRoleAsync(adminUser, BlogRole.Admin.ToString());

                //Step 1 Repeat: create the moderator user
                var modUser = new BlogUser()
                {
                    Email = "cparker@theGreat.com",
                    UserName = "cparker@theGreat.com",
                    FirstName = "Chad",
                    LastName = "Parker",
                    PhoneNumber = "(800)555-1213",
                    EmailConfirmed = true

                };

                //Step 2: Use the User manager to create a new user that is defined by modUser
                await _userManager.CreateAsync(modUser, "Abc@123");

                //Step 3: Add new user to moderator role
                await _userManager.AddToRoleAsync(modUser, BlogRole.Moderator.ToString());
            }
        }
    }
}
