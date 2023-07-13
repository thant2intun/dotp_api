namespace DOTP_BE.Interfaces
{
    public interface IRefreshTokenGenerator
    {
        string GenerateToken(string username);
    }
}
