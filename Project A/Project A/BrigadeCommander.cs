using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_A
{
    public class BrigadeCommander: IPerson
    {
        private string firstName;
        private string lastName;
        private int age;

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        public string GetFullName() 
        {
            throw new NotImplementedException();
        }

        public BrigadeCommander(string firstName, string lastName, int age) 
        {
            throw new NotImplementedException();
        }
    }
}
