using System.ComponentModel.DataAnnotations;
using BankApplication.Data;

namespace BankApplication.Models
{
    public class PhysClientInfo
    {
        [Key]
        public int id { get; set; }

        public int client_id { get; set; }

        public bool is_bank_employee { get; set; }

        public string photo { get; set; }
    }
}
