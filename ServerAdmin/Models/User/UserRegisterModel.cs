using Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Security;

namespace ServerAdmin.Models
{
    public class UserCompleteModel
    {
        [Required]
        [StringLength(100)]
        public string NickName { get; set; }

        [Required]
        public DateTime BirthDay { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MembershipPassword]
        public string Password { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        public override string ToString()
        {
            return $"Nick: {NickName}, Email: {Email} , and name {FirstName} {LastName}";
        }
        public ApiUser ToEntity()
        {
            ApiUser entity = new ApiUser
            {
                NickName = this.NickName,
                BirthDay = this.BirthDay,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Password = this.Password,
                Email = this.Email,
                FavoriteMovies = new List<string>()
            };
            return entity;
        }

    }
}