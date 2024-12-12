using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_A
{
    public class Worker : IPerson
    {
        private int id;
        private string firstName;
        private string lastName;
        private int age;
        private Specialization specialization;

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public Specialization Specialization { get; set; }

        public void Promote(Specialization newSpecialization) 
        {
            throw new NotImplementedException();
        }

        public string GetFullName()
        {
            throw new NotImplementedException();
        }

        public Worker(int id, string firstName, string lastName, int age, Specialization specialization)
        {
            throw new NotImplementedException();
        }
    }
}
