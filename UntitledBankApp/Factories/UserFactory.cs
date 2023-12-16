using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UntitledBankApp.Factories
{
    public class UserFactory
    {
        public User CreateUser(Role role, string fullName, string username, string password, string passwordVerified, string email, string emailVerified, string address, string telephonenumber)
        {
            switch (role)
            {
                case Role.Admin:
                    return new Admin(fullName, username, password, passwordVerified, email, emailVerified, address, telephonenumber);
                case Role.Client:
                    return new Client(fullName, username, password, passwordVerified, email, emailVerified, address, telephonenumber);
                default:
                    throw new ArgumentException("Invalid role");
            }
        }
    }
}

