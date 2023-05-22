namespace PlantsAPI.Services
{
    public interface IUserContext
    {
        string GetMe();
        bool HasAuthorization(Guid userId);
    }
}
