using System.ComponentModel.DataAnnotations;

namespace SecurityApp.Models
{
    public class LoginViewModel
    {
        [Required]
        [MaxLength(128)]
        public string Username { get; set; }

        [Required]
        [MaxLength(256)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}