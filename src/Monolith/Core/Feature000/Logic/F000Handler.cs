using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using BaseApiReference.Abstractions.Caching;
using BaseApiReference.Abstractions.IdGenerator;
using BaseApiReference.Abstractions.Tokens;
using BaseApiReference.Entities;
using Common.Applications.Repositories;
using Common.Constants;
using Common.Features;
using Feature000.Data;
using Microsoft.AspNetCore.Identity;

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
    private readonly IUnitOfWork _unitOfWork;

    public F000Handler(
        IGeneratorIdHandler generatorIdHandler,
        ICacheHandler cacheHandler,
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IRefreshTokenHandler refreshTokenHandler,
        IAccessTokenHandler accessTokenHandler,
        IF000Repository f002Repository,
        IUnitOfWork unitOfWork
    )
    {
        _generatorIdHandler = generatorIdHandler;
        _cacheHandler = cacheHandler;
        _userManager = userManager;
        _signInManager = signInManager;
        _refreshTokenHandler = refreshTokenHandler;
        _accessTokenHandler = accessTokenHandler;
        _f002Repository = f002Repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<F000Response> HandlerAsync(
        F000Request request,
        CancellationToken cancellationToken
    )
    {
        await _cacheHandler.GetAsync<string>(key: "test");

        _unitOfWork.Repository<User>().Count();

        var foundUser = await _userManager.FindByNameAsync(request.Username);

        if (Equals(objA: foundUser, objB: default))
        {
            return new() { StatusCode = AppCodes.USER_NOT_FOUND_CODE };
        }

        var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(foundUser);

        if (!isEmailConfirmed)
        {
            return new() { StatusCode = 411 };
        }

        var signInResult = await _signInManager.CheckPasswordSignInAsync(
            user: foundUser,
            password: request.Password,
            lockoutOnFailure: true
        );

        var foundUserRoles = await _userManager.GetRolesAsync(user: foundUser);

        List<Claim> userClaims =
        [
            new(type: JwtRegisteredClaimNames.Jti, value: Guid.NewGuid().ToString()),
            new(type: JwtRegisteredClaimNames.Sub, value: foundUser.Id.ToString()),
            new(type: "role", value: foundUserRoles[default])
        ];

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
