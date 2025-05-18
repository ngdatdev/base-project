namespace Feature000.Logic;

/// <summary>
/// F000 Repository
/// </summary>
public interface IF000Repository
{
    /// <summary>
    /// This method is used to check user found by username
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public bool IsUserFoundByUsername(string username);
}
