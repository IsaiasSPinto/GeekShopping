﻿using GeekShopping.IdentityServer.Configuration;
using GeekShopping.IdentityServer.Model;
using GeekShopping.IdentityServer.Model.Context;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace GeekShopping.IdentityServer.Initialazer;

public class DbInitializer : IDbInitializer
{
    private readonly MySqlContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    public DbInitializer(RoleManager<IdentityRole> roleManager, MySqlContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public void Initialize()
    {
        if (_roleManager.FindByNameAsync(IdentityConfiguration.Admin).Result != null) return;

        _roleManager.CreateAsync(new IdentityRole(IdentityConfiguration.Admin)).GetAwaiter().GetResult();
        _roleManager.CreateAsync(new IdentityRole(IdentityConfiguration.Client)).GetAwaiter().GetResult();

        ApplicationUser admin = new ApplicationUser()
        {
            UserName = "admin",
            Email = "admin@admin.com",
            EmailConfirmed = true,
            FirstName = "Admin",
            LastName = "Admin"
        };

        _userManager.CreateAsync(admin, "123456@Admin").GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(admin, IdentityConfiguration.Admin).GetAwaiter().GetResult();

        var adminClaims = _userManager.AddClaimsAsync(admin, new Claim[]
        {
            new Claim(JwtClaimTypes.Name, $"{admin.FirstName} {admin.LastName}"),
            new Claim(JwtClaimTypes.GivenName, $"{admin.FirstName}"),
            new Claim(JwtClaimTypes.FamilyName, $"{admin.LastName}"),
            new Claim(JwtClaimTypes.Role, IdentityConfiguration.Admin),
        }).GetAwaiter().GetResult();


        ApplicationUser client = new ApplicationUser()
        {
            UserName = "client",
            Email = "client@client.com",
            EmailConfirmed = true,
            FirstName = "client",
            LastName = "client"
        };

        _userManager.CreateAsync(client, "123456@Client").GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(client, IdentityConfiguration.Client).GetAwaiter().GetResult();

        var clientClaims = _userManager.AddClaimsAsync(client, new Claim[]
        {
            new Claim(JwtClaimTypes.Name, $"{client.FirstName} {client.LastName}"),
            new Claim(JwtClaimTypes.GivenName, $"{client.FirstName}"),
            new Claim(JwtClaimTypes.FamilyName, $"{client.LastName}"),
            new Claim(JwtClaimTypes.Role, IdentityConfiguration.Client),
        }).GetAwaiter().GetResult();
    }
}
