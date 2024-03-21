using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliomatic_MAUI_App.Models
{
    public class CurrentUser
    {       
        public static Guid Id { get; private set; }
        public static string Email { get; private set; }
        public static string FirstName { get; set; }
        public static string LastName { get; set; }
        public static string MiddleName { get; set; }
        public static string Author { get => $"{LastName} {FirstName}"; }
        public static string FullName { get => $"{LastName} {FirstName} {MiddleName}"; }

        public CurrentUser(Guid id, string email, string firstName, string lastName, string middleName)
        {           
            Id = id;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
        }
    }
}
