using app.BLL.Interface;
using app.BLL.Repository;
using app.DAL.Context;
using app.DAL.model;
using app.Pl.Helper;
using app.Pl.MapperProfile;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace app.Pl
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();
			builder.Services.AddDbContext<CompanyContext>(options =>
			options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection")));
            //builder.Services.AddTransient<EmailSetting>();
            builder.Services.Configure<EmailSettings>(
				builder.Configuration.GetSection("EmailSettings"));

            builder.Services.AddScoped<IDepartmentRepository,DepartmentRepository>();
			//builder.Services.AddScoped<IEmployeeReopsitory,EmployeeReopsitory>();
            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile(new EmployeeProfile());
                cfg.AddProfile(new DepartmentProfile());
            });
			builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();

			#region Identity Module

			//builder.Services.AddScoped<UserManager<User>>();
			builder.Services.AddIdentity<User, IdentityRole>(opt =>
			{
				opt.Password.RequireNonAlphanumeric = true;
				opt.Password.RequireDigit = true;
				opt.Password.RequireLowercase = true;
				opt.Password.RequireUppercase = true;
				//P@ssw0rd
			})

				.AddEntityFrameworkStores<CompanyContext>()
				.AddDefaultTokenProviders();
				
			builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(opt =>
				{
					opt.LoginPath = "Account/Login";
					opt.AccessDeniedPath = "Home/Error";
				});
			#endregion

            var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();
			app.UseAuthorization();
			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Account}/{action=Login}/{id?}");

			app.Run();
		}
	}
}
