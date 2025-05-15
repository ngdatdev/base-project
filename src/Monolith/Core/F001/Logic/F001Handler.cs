using System;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using BaseApiReference.Abstractions.Caching;
using BaseApiReference.Abstractions.IdGenerator;
using BaseApiReference.Enum;
using Common.Features;
using F001.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using PenomyAPI.App.Common.Caching;

namespace F001.Logic;

public class F001Handler : IHandler<F001Request, F001Response>
{
    private readonly IGeneratorIdHandler _generatorIdHandler;
    private readonly ICacheHandler _cacheHandler;

    public F001Handler(IGeneratorIdHandler generatorIdHandler, ICacheHandler cacheHandler)
    {
        _generatorIdHandler = generatorIdHandler;
        _cacheHandler = cacheHandler;
    }

    public async Task<F001Response> HandlerAsync(
        F001Request request,
        CancellationToken cancellationToken
    )
    {
        var accountStatus = AccountStatus.GetAll();

        var helloWorld = request.Name;
        var generatorId = _generatorIdHandler.NextId();

        var result = await _cacheHandler.GetAsync<string>(helloWorld, cancellationToken);

        if (result == AppCacheModel<string>.NotFound)
        {
            Thread.Sleep(5000);
            await _cacheHandler.SetAsync(
                key: helloWorld,
                value: JsonConvert.SerializeObject(
                    value: generatorId,
                    formatting: Newtonsoft.Json.Formatting.None,
                    settings: new() { NullValueHandling = NullValueHandling.Ignore }
                ),
                new() { AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(seconds: 60) },
                cancellationToken
            );
        }

        return await Task.FromResult(
            new F001Response()
            {
                Data = new ResponseBody() { FullName = helloWorld, GeneratorId = generatorId },
                StatusCode = 200,
                Message = "Success"
            }
        );
    }
}
