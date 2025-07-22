using System;
using System.ComponentModel.DataAnnotations;

namespace QuizProject.API.Dtos
{
    public class AdminCreateUserDto
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        public string Role { get; set; }

        /// <summary>
        /// Kullanıcının doğum tarihi (opsiyonel)
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Kullanıcının telefon numarası (opsiyonel)
        /// </summary>
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Profil fotoğrafı base64 formatında
        /// </summary>
        public string ProfileImageBase64 { get; set; }

        /// <summary>
        /// E-posta doğrulama durumu (Admin tarafından otomatik true)
        /// </summary>
        public bool EmailConfirmed { get; set; } = true;
    }
}
