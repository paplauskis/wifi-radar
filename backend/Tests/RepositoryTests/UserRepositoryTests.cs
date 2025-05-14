using API.Data.Repositories;
using API.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Tests.RepositoryTests;

public class UserRepositoryTests : BaseRepositoryTests<User, UserRepository>
{
    private const string CollectionName = "Users";
    
    public UserRepositoryTests() : base(CollectionName) {}


    public override Task AddAsync_ShouldInsertEntityIntoCollection(int num)
    {
        throw new NotImplementedException();
    }

    public override Task GetByIdAsync_ShouldReturnCorrectEntity_WhenIdExists(User entity)
    {
        throw new NotImplementedException();
    }

    protected override UserRepository GetRepository(TestDbContext context)
    {
        var services = new ServiceCollection();
        services.AddSingleton(context.Database);
        services.AddScoped<UserRepository>();
        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider.GetRequiredService<UserRepository>();
    }
}