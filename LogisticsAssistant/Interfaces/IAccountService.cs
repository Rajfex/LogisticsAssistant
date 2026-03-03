namespace LogisticsAssistant.Interfaces
{
    public interface IAccountService
    {
        Task<bool> RegisterAsync(string name, string password);
        Task<bool> LoginAsync(string name, string password);
        Task LogoutAsync();
    }
}
