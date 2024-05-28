using System.ComponentModel.DataAnnotations;

namespace BankApplication.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Address { get; set; }

        public string ClientFullName { get; set; }

        public string Sex { get; set; }

        public DateTime BirthDate { get; set; }

        public bool IsDebtor { get; set; }

        public ICollection<Account> Accounts { get; set; }
    }

    public class CreateClientDTO
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Address { get; set; }

        public string ClientFullName { get; set; }

        public string Sex { get; set; }

        public DateTime BirthDate { get; set; }

        public bool IsDebtor { get; set; }
    }
}
