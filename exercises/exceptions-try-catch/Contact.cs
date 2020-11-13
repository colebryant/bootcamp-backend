using System;

namespace exceptions_try_catch
{
    public class Contact
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string FullName { get{return $"{FirstName} + {LastName}";} }
    }
}
