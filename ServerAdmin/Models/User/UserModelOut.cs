using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerAdmin.Models
{
    public class UserRetModel
    {
        public string NickName { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public UserRetModel(ApiUser userToShow)
        {
            this.NickName = userToShow.NickName;
            this.Email = userToShow.Email;
            this.FirstName = userToShow.FirstName;
        }

        public override string ToString()
        {
            return $"Nick: {NickName}, Email: {Email} , and name {FirstName}";
        }
    }
}