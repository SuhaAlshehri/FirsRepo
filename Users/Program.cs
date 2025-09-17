//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Builder;

//using System;
//using Users.Data;
//using Users.Models;


//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<DBusers>();
//builder.Services.AddControllers();
//// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

//builder.Services.AddOpenApi();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//builder.Services.AddDbContext<DBusers>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//var app = builder.Build();
////builder.Services.AddEndpointsApiExplorer();
////builder.Services.AddSwaggerGen();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//    app.UseSwagger();
//    app.UseSwaggerUI(options => { options.SwaggerEndpoint("","openAi"); });
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System;
using Users.Data;
using Users.Models;
using Users.Extentions;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<DBusers>();

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<DBusers>()
    .AddDefaultTokenProviders();


builder.Services.AddCustemJWTAuth(builder.Configuration);

builder.Services.AddCors(options =>

options.AddDefaultPolicy(
    policy =>
    {
        policy.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader();
    }
    ));

builder.Services.AddDbContext<DBusers>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
     