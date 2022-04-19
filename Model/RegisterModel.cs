﻿using System.ComponentModel.DataAnnotations;

namespace RecruitmentPortalBE.Model
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        //[Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

       // [Required(ErrorMessage = "Staff Id is required")]
        public string EmployeeId { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

    }
}
