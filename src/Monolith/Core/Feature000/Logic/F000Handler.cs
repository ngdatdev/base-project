using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using BaseApiReference.Abstractions.IdGenerator;
using BaseApiReference.Abstractions.Tokens;
using BaseApiReference.Entities;
using Common.Constants;
using Common.Features;
using Feature000.Data;
using Microsoft.AspNetCore.Identity;
using PenomyAPI.App.Common.Caching;

namespace Feature000.Logic;

/// <summary>
/// F000 Handler
/// </summary>
public sealed class F000Handler : IHandler<F000Request, F000Response>
{
    private readonly IGeneratorIdHandler _generatorIdHandler;
    private readonly ICacheHandler _cacheHandler;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IRefreshTokenHandler _refreshTokenHandler;
    private readonly IAccessTokenHandler _accessTokenHandler;
    private readonly IF000Repository _f002Repository;

    public F000Handler(
        IGeneratorIdHandler generatorIdHandler,
        ICacheHandler cacheHandler,
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IRefreshTokenHandler refreshTokenHandler,
        IAccessTokenHandler accessTokenHandler,
        IF000Repository f002Repository
    )
    {
        _generatorIdHandler = generatorIdHandler;
        _cacheHandler = cacheHandler;
        _userManager = userManager;
        _signInManager = signInManager;
        _refreshTokenHandler = refreshTokenHandler;
        _accessTokenHandler = accessTokenHandler;
        _f002Repository = f002Repository;
    }

    public async Task<F000Response> HandlerAsync(
        F000Request request,
        CancellationToken cancellationToken
    )
    {
        // Find user by username.
        var foundUser = await _userManager.FindByNameAsync(request.Username);

        // Responds if user does not exist.
        if (Equals(objA: foundUser, objB: default))
        {
            return new() { StatusCode = AppCodes.USER_NOT_FOUND_CODE };
        }

        // Check email confirmation.
        var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(foundUser);

        // Responds if email is not confirmed.
        if (!isEmailConfirmed)
        {
            return new() { StatusCode = 411 };
        }

        // Check password and handle lockout on failure.
        var signInResult = await _signInManager.CheckPasswordSignInAsync(
            user: foundUser,
            password: request.Password,
            lockoutOnFailure: true
        );

        // Get user roles.
        var foundUserRoles = await _userManager.GetRolesAsync(user: foundUser);

        // Init list of user claims.
        List<Claim> userClaims =
        [
            new(type: JwtRegisteredClaimNames.Jti, value: Guid.NewGuid().ToString()),
            new(type: JwtRegisteredClaimNames.Sub, value: foundUser.Id.ToString()),
            new(type: "role", value: foundUserRoles[default])
        ];

        // Generate new access token.
        var newAccessToken = _accessTokenHandler.GenerateSigningToken(claims: userClaims);

        return await Task.FromResult(
            new F000Response()
            {
                Data = new ResponseBody()
                {
                    AccessToken = newAccessToken,
                    RefreshToken = "123456",
                    User = new()
                    {
                        Email = foundUser.Email,
                        AvatarUrl = foundUser.Avatar,
                        FullName = foundUser.FullName
                    }
                },
                StatusCode = AppCodes.SUCCESS_CODE,
                Message = "Operation success."
            }
        );
    }
}
