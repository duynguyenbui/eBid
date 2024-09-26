namespace eBid.Identity.API.Models.AccountViewModels
{
    public record SignUpViewModel
    {
        [Required] public string Username { get; set; }
        [Required] [EmailAddress] public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required] public string CardNumber { get; set; }

        [Required] public string SecurityNumber { get; set; }

        [Required]
        [RegularExpression(@"(0[1-9]|1[0-2])\/[0-9]{2}", ErrorMessage = "Expiration should match a valid MM/YY value")]
        public string Expiration { get; set; }

        [Required] public string CardHolderName { get; set; }

        public int CardType { get; set; }

        [Required] public string Street { get; set; }

        [Required] public string City { get; set; }

        [Required] public string State { get; set; }

        [Required] public string PhoneNumber { get; set; }

        [Required] public string Country { get; set; }

        [Required] public string ZipCode { get; set; }

        [Required] public string Name { get; set; }

        [Required] public string LastName { get; set; }
    }
}