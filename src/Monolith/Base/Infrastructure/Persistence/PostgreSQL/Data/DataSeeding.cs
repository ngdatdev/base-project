using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BaseApiReference.Abstractions.IdGenerator;
using BaseApiReference.Entities;
using Infrastructure.IdGenerator;
using Infrastructure.Persistence.PostgreSQL.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence.PostgreSQL.Data;

/// <summary>
/// Data Seeding
/// </summary>
public class DataSeeding
{
    private readonly IGeneratorIdHandler _generatorIdHandler;

    public DataSeeding(IGeneratorIdHandler generatorIdHandler)
    {
        _generatorIdHandler = generatorIdHandler;
    }

    public async Task<bool> SeedAsync(
        AppDbContext context,
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IConfigurationManager configuration,
        CancellationToken cancellationToken
    )
    {
        var roles = context.Set<Role>();

        // Is tables empty.
        var isTableEmpty = await IsTableEmptyAsync(
            roles: roles,
            cancellationToken: cancellationToken
        );

        if (!isTableEmpty)
        {
            return true;
        }

        // Init list of role.
        var newRoles = InitNewRoles();

        // Init admin.
        var admin = InitAdmin();

        #region Transaction
        var executedTransactionResult = false;

        await context
            .Database.CreateExecutionStrategy()
            .ExecuteAsync(operation: async () =>
            {
                await using var dbTransaction = await context.Database.BeginTransactionAsync(
                    cancellationToken: cancellationToken
                );

                try
                {
                    foreach (var newRole in newRoles)
                    {
                        await roleManager.CreateAsync(role: newRole);
                    }

                    await userManager.CreateAsync(
                        user: admin,
                        password: configuration
                            .GetRequiredSection("Admin")
                            .GetRequiredSection("Password")
                            .Value
                    );

                    await userManager.AddToRoleAsync(user: admin, role: "admin");

                    var emailConfirmationToken =
                        await userManager.GenerateEmailConfirmationTokenAsync(user: admin);

                    await userManager.ConfirmEmailAsync(user: admin, token: emailConfirmationToken);

                    await context.SaveChangesAsync(cancellationToken: cancellationToken);

                    await dbTransaction.CommitAsync(cancellationToken: cancellationToken);

                    executedTransactionResult = true;
                }
                catch
                {
                    await dbTransaction.RollbackAsync(cancellationToken: cancellationToken);
                }
            });
        #endregion

        return executedTransactionResult;
    }

    private static async Task<bool> IsTableEmptyAsync(
        DbSet<Role> roles,
        CancellationToken cancellationToken
    )
    {
        // Is roles table empty.
        var isTableNotEmpty = await roles.AnyAsync(cancellationToken: cancellationToken);

        if (isTableNotEmpty)
        {
            return false;
        }

        // ...

        return true;
    }

    private List<Role> InitNewRoles()
    {
        Dictionary<long, string> newRoleNames = [];

        long userRole = _generatorIdHandler.NextId();
        long adminRole = _generatorIdHandler.NextId();

        newRoleNames.Add(key: userRole, value: "user");

        newRoleNames.Add(key: adminRole, value: "admin");

        List<Role> newRoles = [];

        foreach (var newRoleName in newRoleNames)
        {
            Role newRole = new() { Id = newRoleName.Key, Name = newRoleName.Value, };

            newRoles.Add(item: newRole);
        }

        return newRoles;
    }

    private User InitAdmin()
    {
        long adminId = _generatorIdHandler.NextId();
        User admin =
            new()
            {
                Id = adminId,
                UserName = "admin",
                Email = "nvdatdz8b@gmail.com",
                FullName = "Nguyen Van Dat",
                Avatar = "https://i.pravatar.cc/150?u=" + adminId.ToString(),
            };

        return admin;
    }
}
