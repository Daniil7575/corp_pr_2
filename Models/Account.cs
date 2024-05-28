using System.ComponentModel.DataAnnotations;

namespace BankApplication.Models
{
 
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public string Number { get; set; }
        public DateTime OpenDate { get; set; }
        public int OwnerId { get; set; }
        public decimal Balance { get; set; }

        public Client Client { get; set; }

    }

    public class AccountDto
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public DateTime OpenDate { get; set; }
        public int OwnerId { get; set; }
        public decimal Balance { get; set; }
        public string ClientFullName { get; set; }
    }

    public class CreateAccountDto
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public DateTime OpenDate { get; set; }
        public int OwnerId { get; set; }
        public decimal Balance { get; set; }
    }
}
