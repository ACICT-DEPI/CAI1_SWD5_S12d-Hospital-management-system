﻿using Hospital.Models;
using Hospital.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Utilities
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly HospitalDbContext _context;

        public DbInitializer(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, HospitalDbContext context)
        {
            _userManager = userManager ;
            _roleManager = roleManager ;
            _context = context ;
        }

        public void Initialize()
        {
            try
            {
                // check if any pending migrations exist , if not Migrate.
                if (_context.Database.GetPendingMigrations().Any())
                {
                    _context.Database.Migrate();
                }
            }
            catch (Exception )
            {
                throw;
            }


            // if roles don't exist . make it
            if (!_roleManager.RoleExistsAsync(WebSiteRoles.WebSite_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(WebSiteRoles.WebSite_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(WebSiteRoles.Website_Patient)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(WebSiteRoles.Website_Doctor)).GetAwaiter().GetResult();

                // Default Admin user creation.
                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "Belal",
                    Email = "belal@abc.com"
                }, "belal@123").GetAwaiter().GetResult();

                // check on the DB if this user exist assign him as admin.
                var Appuser = _context.ApplicationUsers.FirstOrDefault(x=>x.Email== "belal@abc.com");
                if (Appuser!=null)
                {
                    _userManager.AddToRoleAsync(Appuser, WebSiteRoles.WebSite_Admin).GetAwaiter().GetResult();
                }
            }




        }


    }
}