﻿using bejebeje.admin.Application.Common.Interfaces;
using bejebeje.admin.Infrastructure.Persistence;
using bejebeje.admin.WebUI;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using Respawn;
using Respawn.Graph;

namespace bejebeje.admin.Application.IntegrationTests;

[SetUpFixture]
public class Testing
{
    private static IConfigurationRoot _configuration = null!;
    private static IServiceScopeFactory _scopeFactory = null!;
    private static Respawner _respawner = null!;
    private static string? _currentUserId;

    [OneTimeSetUp]
    public async Task RunBeforeAnyTests()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .AddEnvironmentVariables();

        _configuration = builder.Build();

        var startup = new Startup(_configuration);

        var services = new ServiceCollection();

        services.AddSingleton(Mock.Of<IWebHostEnvironment>(w =>
            w.EnvironmentName == "Development" &&
            w.ApplicationName == "bejebeje.admin.WebUI"));

        services.AddLogging();

        startup.ConfigureServices(services);

        // replace service registration for icurrentuserservice
        // remove existing registration
        var currentUserServiceDescriptor = services.FirstOrDefault(d =>
            d.ServiceType == typeof(ICurrentUserService));

        if (currentUserServiceDescriptor != null)
        {
            services.Remove(currentUserServiceDescriptor);
        }

        // register testing version
        services.AddTransient(provider =>
            Mock.Of<ICurrentUserService>(s => s.UserId == _currentUserId));

        _scopeFactory = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>();
        
        _respawner = await Respawner.CreateAsync(_configuration["ConnectionString"], new RespawnerOptions
        {
            TablesToIgnore = new Table[] { "__EFMigrationsHistory" }
        });

        EnsureDatabase();
    }

    private static void EnsureDatabase()
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Database.Migrate();
    }

    public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        return await mediator.Send(request);
    }

    public static async Task ResetState()
    {
        await _respawner.ResetAsync(_configuration["ConnectionString"]);

        _currentUserId = null;
    }

    public static async Task<TEntity?> FindAsync<TEntity>(params object[] keyValues)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.FindAsync<TEntity>(keyValues);
    }

    public static async Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Add(entity);

        await context.SaveChangesAsync();
    }

    public static async Task<int> CountAsync<TEntity>() where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.Set<TEntity>().CountAsync();
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
    }
}
