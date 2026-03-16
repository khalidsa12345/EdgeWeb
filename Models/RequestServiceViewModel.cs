using System.ComponentModel.DataAnnotations;

namespace EdgeWEB.Models
{
    public class RequestServiceViewModel
    {
        // Services 
        public bool StrategyPlanning { get; set; }
        public bool PMO { get; set; }
        public bool Governance { get; set; }
        public bool ISO { get; set; }
        public bool Digital { get; set; }
        public bool Financial { get; set; }
        public bool Sustainability { get; set; }
        public bool HR { get; set; }
        public bool BusinessProcess { get; set; }
        public bool AI { get; set; }
        public bool CyberSecurity { get; set; }

        // Contact Information
        [Required(ErrorMessage = "Please enter your full name")]
        [Display(Name = "Full Name")]
        public string PersonName { get; set; }

        [Required(ErrorMessage = "Please enter your email address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your mobile number")]
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        // Additional Information
        [Display(Name = "Add Note")]
        public string AddNote { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; }
    }
}