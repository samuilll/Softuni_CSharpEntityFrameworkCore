﻿using System.Collections.Generic;

namespace P01_BillsPaymentSystem.Data.Models
{
    public class User
    {
        public User()
        {

        }
        public User(string firstName, string lastName, string email, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
        }

        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string  LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public virtual ICollection<PaymentMethod> PaymentMethods { get; set; }

        public override string ToString()
        {
            return $"{this.FirstName} {this.LastName}";
        }
    }
}
