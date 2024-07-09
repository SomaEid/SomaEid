using Ikagai.Core;
using Ikagai.Email;
using Ikagai.Services.AddressService;
using Ikagai.Services.BaseService;
using Ikagai.Services.BloodAndDerivativesService;
using Ikagai.Services.BloodBankOrderService;
using Ikagai.Services.DashBoardServices;
using Ikagai.Services.DeliveryComapnyService;
using Ikagai.Services.DonationService;
using Ikagai.Services.DonorOrderService;
using Ikagai.Services.EmailService;
using Ikagai.Services.OrderService;
using Ikagai.Services.TestResultService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext with scoped lifetime
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity Services configured with scoped DbContext
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Cors Orgin 
builder.Services.AddCors();

// Email Configuration
builder.Services.AddSingleton(builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfigration>());

// Email Register
builder.Services.AddScoped<IEmailServices, EmailServices>();
builder.Services.AddScoped<IAddressServices, AddressServices>();
builder.Services.AddScoped<IBloodAndDerivativesService, BloodAndDerivativesService>();
builder.Services.AddScoped<IOrderServices, OrderServices>();
builder.Services.AddScoped<IBaseServices, BaseServices>();
builder.Services.AddScoped<IDonorOrderServices, DonorOrderServices>();
builder.Services.AddScoped<IBloodBankOrderServices, BloodBankOrderServices>();
builder.Services.AddScoped<IDeliveryOrdersService , DeliveryOrdersService>();
builder.Services.AddScoped<IDonationService, DonationServices>();
builder.Services.AddScoped<ITestResultServices, TestResultServices>();
builder.Services.AddScoped<IDashBoardService, DashBoardService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();

app.UseCors(x => x
.AllowAnyHeader()
.AllowAnyMethod()
.SetIsOriginAllowed(origin => true)
.AllowCredentials());

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
