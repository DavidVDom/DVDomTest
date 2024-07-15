using DavidVDom.Domain.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace DavidVDom.Domain.Entities
{
    public class Bank : Entity<Bank>
    {
        public Bank() { }

        public Bank(string? name, string? bIC, string? country)
        {
            Id = Guid.NewGuid();
            Name = name;
            BIC = bIC;
            Country = country;
        }

        [MaxLength(250)]
        public string? Name { get; set; }

        [MaxLength(11)]
        public string? BIC { get; set; }

        [MaxLength(7)]
        public string? Country { get; set; }

        public static Bank CreateNewBank(string name, string bic, string country)
        {
            return new Bank(name, bic, country);
        }
    }
}
