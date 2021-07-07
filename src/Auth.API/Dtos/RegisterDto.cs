using System.ComponentModel.DataAnnotations;

namespace Auth.API.Dtos
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [EmailAddress]
        [Compare(nameof(Email), ErrorMessage = "Emails are not matching")]
        public string ConfirmedEmail { get; set; }
        
        [Required]
        public string UserName { get; set; }
        
        [Phone]
        public string Phone { get; set; }  
        
        [Required]
        public string Firstname { get; set; }  
        [Required]
        public string Lastname { get; set; }

        [Required]
        [MinLength(8)] 
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,}", ErrorMessage = "Passwords must be at least 8 characters long and contain at least an upper case letter, lower case letter, digit and a symbol")]
        public string Password { get; set; }
        
        [Compare(nameof(Password), ErrorMessage = "Passwords are not matching")]
        public string ConfirmedPassword { get; set; }
    }
}