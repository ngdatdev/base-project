namespace BaseApiReference.Abstractions.IdGenerator;

/// <summary>
/// IGeneratorID is used to generate Id.
/// </summary>
public interface IGeneratorIdHandler
{
    /// <summary>
    /// NextId is used to generate Id.
    /// </summary>
    /// <returns></returns>
    long NextId();

    /// <summary>
    /// DecodeId is used to decode the Id generated.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    DecodeIdModel DecodeId(long id);
}
