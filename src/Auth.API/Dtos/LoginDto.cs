using System.ComponentModel.DataAnnotations;

namespace Auth.API.Dtos
{
    public class LoginDto
    {
        [EmailAddress]
        public string Email { get; set; }
        
        
        public string UserName { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}