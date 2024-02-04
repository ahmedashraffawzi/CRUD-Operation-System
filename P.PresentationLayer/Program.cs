using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using P.BusinessLogicLayer.Interfaces;
using P.BusinessLogicLayer.Repositories;
using P.DataAccessLayer.Contexts;
using P.DataAccessLayer.Models;
using P.PresentationLayer.MappingProfiles;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace P.PresentationLayer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var Builder = WebApplication.CreateBuilder(args);
            #region Configur Service
            Builder.Services.AddControllersWithViews();
            Builder.Services.AddDbContext<MvcAppDbcontext>(Options =>
            {
                Options.UseSqlServer(Builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            Builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            Builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            Builder.Services.AddAutoMapper(M => M.AddProfile(new EmployeeProfile()));
            Builder.Services.AddAutoMapper(M => M.AddProfile(new DepartmentProfile()));
            Builder.Services.AddAutoMapper(M => M.AddProfile(new UserProfile()));
            Builder.Services.AddAutoMapper(M => M.AddProfile(new RoleProfile()));
            Builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Builder.Services.AddIdentity<ApplicationUser, IdentityRole>(Options =>
            {
                Options.Password.RequireNonAlphanumeric = true;
                Options.Password.RequireDigit = true;
                Options.Password.RequireLowercase = true;
                Options.Password.RequireUppercase = true;
            })

                    .AddEntityFrameworkStores<MvcAppDbcontext>()
                    .AddDefaultTokenProviders();
                    
            Builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.LogoutPath = "Account/Login";
                        options.AccessDeniedPath = "Home/Error";


                    });

            #endregion
           
            var app = Builder.Build();
            #region Configur Http Request
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");
            });
            #endregion
            app.Run();
        }


    }
} 
