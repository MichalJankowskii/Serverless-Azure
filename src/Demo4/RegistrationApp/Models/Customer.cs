namespace RegistrationApp.Models
{
    using Microsoft.WindowsAzure.Storage.Table;

    public class Customer : TableEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public int BirthYear { get; set; }
    }
}
