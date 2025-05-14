using API.Data.Repositories;
using API.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Tests.RepositoryTests;

public class WifiReviewRepositoryTests: BaseRepositoryTests<WifiReview, WifiReviewRepository>
{
    private const string CollectionName = "Reviews";
    
    public WifiReviewRepositoryTests() : base(CollectionName) {}

    public override Task AddAsync_ShouldInsertEntityIntoCollection(int num)
    {
        throw new NotImplementedException();
    }

    public override Task GetByIdAsync_ShouldReturnCorrectEntity_WhenIdExists(WifiReview entity)
    {
        throw new NotImplementedException();
    }

    protected override WifiReviewRepository GetRepository(TestDbContext context)
    {
        var services = new ServiceCollection();
        services.AddSingleton(context.Database);
        services.AddScoped<WifiReviewRepository>();
        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider.GetRequiredService<WifiReviewRepository>();
    }
}