namespace ProjectTravelin.Services.EmailServices
{
    public interface IEmailService
    {
        Task SendBookingApprovedEmailAsync(
            string toEmail,
            string nameSurname,
            string tourName,
            string bookingDate);
    }
}