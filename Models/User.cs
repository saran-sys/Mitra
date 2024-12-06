//using System;
//namespace Mitra.Models
//{
//    public class User
//    {
//        public Guid Id { get; set; }
//        public string Name { get; set; }
//        public string Email { get; set; }
//        public string Password { get; set; }
//        public int Role { get; set; }
//    }
//}

using System;

namespace Mitra.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } // Hashed Password
        public int Role { get; set; } // Example: 1 = User, 0 = Admin
    }
}
