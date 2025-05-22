using System.Text;
using API.Data.Repositories;
using API.Data.Repositories.Interfaces;
using API.Helpers;
using API.Services.Database;
using API.Services.Interfaces.Auth;
using API.Services.Interfaces.Map;
using API.Services.Interfaces.User;
using API.Services.Interfaces.Wifi;
using API.Services.Map;
using API.Services.User;
using API.Services.Wifi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<MongoDbService>();
builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var service = sp.GetRequiredService<MongoDbService>();
    return service.GetDatabase();
});
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IWifiRepository, WifiRepository>();
builder.Services.AddScoped<IWifiReviewRepository, WifiReviewRepository>();
builder.Services.AddScoped<IWifiSearchService, WifiSearchService>();
builder.Services.AddScoped<IUserFavoriteService, UserFavoriteService>();
builder.Services.AddScoped<IUserAuthService, UserAuthService>();
builder.Services.AddScoped<IWifiPasswordSharingService, WifiPasswordSharingService>();
builder.Services.AddScoped<IUserReviewService, UserReviewService>();
builder.Services.AddScoped<IMapService, MapService>();
builder.Services.AddScoped<IPasswordHelper, PasswordHelper>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
    );
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; //should be true if pushed to production
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["JwtConfig:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Key"]!)),
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
    };
});

var app = builder.Build();

app.UseCors("AllowFrontend");

if (app.Environment.IsDevelopment())
{
    // app.UseSwagger();
    // app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();