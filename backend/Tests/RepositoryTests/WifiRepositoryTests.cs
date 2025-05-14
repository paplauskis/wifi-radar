using API.Data.Repositories;
using API.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using Tests.Helpers;
using Xunit;

namespace Tests.RepositoryTests;

public class WifiRepositoryTests
{
    private const string CollectionName = "Favorite-WiFi-Spots";
    private readonly List<WifiNetwork> _testData = JsonHelper.GetPocoObjects<WifiNetwork>();
    
    public WifiRepositoryTests()
    {
    }
    
    [Fact]
    public async Task AddAsync_ShouldInsertEntityIntoCollection()
    {
        
    }

    private WifiRepository GetRepository(TestDbContext context)
    {
        var services = new ServiceCollection();
        services.AddSingleton(context.Database);
        services.AddScoped<WifiRepository>();
        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider.GetRequiredService<WifiRepository>();
    }
}