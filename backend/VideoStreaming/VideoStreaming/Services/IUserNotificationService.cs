namespace VideoStreaming.Services
{
    public interface IUserNotificationService
    {
        Task SendParticipantNotificationAsync(string email, string fullName, string ownerUserName, string roomTitle, string roomAccessCode);
        Task SendParticipantRemovingNotificationAsync(string email, string fullName, string ownerUserName, string roomTitle);
    }
}
