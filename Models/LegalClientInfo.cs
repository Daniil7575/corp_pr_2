using System.ComponentModel.DataAnnotations;

namespace BankApplication.Models
{
    public class LegalClientInfo
    {
        [Key]
        public int id { get; set; }

        public int client_id { get; set; }

        public string name { get; set; }

        public string address { get; set; }

        public string director_fullname { get; set; }

        public string chief_accountant_fullname { get; set; }

        public string form_of_ownership { get; set; }
    }
}
