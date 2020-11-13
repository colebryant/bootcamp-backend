using System;
using System.Collections.Generic;

namespace classes
{

    public class Employee
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
    }
    public class Company
    {
        public Company(string name, DateTime dateFounded) {
            Name = name;
            CreatedOn = dateFounded;
            EmployeeList = new List<Employee>();
        }
        // Some readonly properties (let's talk about gets, baby)
        public string Name { get; }
        public DateTime CreatedOn { get; }

        // Create a public property for holding a list of current employees

        public List<Employee> EmployeeList { get; set; }

        /*
            Create a constructor method that accepts two arguments:
                1. The name of the company
                2. The date it was created

            The constructor will set the value of the public properties
        */

        public void ListEmployees() {
            foreach(Employee person in EmployeeList) {
                Console.WriteLine($"{person.FirstName} {person.LastName}");
            }
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            // Create an instance of a company. Name it whatever you like.
            Company DunderMifflin = new Company("MegaCorp", new DateTime(2018, 12, 1));
            // Create three employees
            Employee Michael = new Employee(){
                FirstName = "Michael",
                LastName = "Scott",
                Title = "Regional Manager",
                StartDate = new DateTime(2018, 12, 1)
            };
            Employee Dwight = new Employee(){
                FirstName = "Dwight",
                LastName = "Schrute",
                Title = "Assistant to the Regional Manager",
                StartDate = new DateTime(2018, 12, 2)
            };
            Employee Pam = new Employee(){
                FirstName = "Pam",
                LastName = "Beasley",
                Title = "Receptionist",
                StartDate = new DateTime(2018, 12, 3)
            };

            // Assign the employees to the company
            List<Employee> Employees = new List<Employee>();
            Employees.Add(Michael);
            Employees.Add(Dwight);
            Employees.Add(Pam);

            DunderMifflin.EmployeeList = Employees;

            /*
                Iterate the company's employee list and generate the
                simple report shown above
            */

            DunderMifflin.ListEmployees();
        }
    }
}
