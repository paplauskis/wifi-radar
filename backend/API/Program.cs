using API.Data.Repositories;
using API.Data.Repositories.Interfaces;
using API.Services.Database;
using API.Services.Interfaces.User;
using API.Services.Interfaces.Wifi;
using API.Services.User;
using API.Services.Wifi;
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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
    );
});

var app = builder.Build();

app.UseCors("AllowFrontend");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.UseSwagger();
    // app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();