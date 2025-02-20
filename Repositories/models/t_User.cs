using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Repositories.models
{
    public class t_User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int c_UserId { get; set; }
        [StringLength(100)]
        [Required(ErrorMessage = "Username is required.")]
        public string c_UserName { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string c_Email { get; set; }
        
        [StringLength(100)]
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,100}$",
        ErrorMessage = "Password must be at least 6 characters long, contain at least 1 uppercase letter, 1 lowercase letter, 1 number, and 1 special character.")]
        public string c_Password { get; set; }

        [StringLength(10)]
        public string? c_Gender { get; set; }
        [StringLength(4000)]

        public string? c_Image { get; set; }
        public IFormFile? ProfilePicture { get; set; }
    }
}