using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace lab_1
{

    public class Client
    {
        private string name;
        private string surname;
        private Gender gender;
        private string city;
        private string street;
        private string housenumber;
        //private string address;
        //private float bonus;
        private Status status;
        private DateOnly birthDate;

        public float Bonus { get; private set; } = 0; // Автовластивість + private set

        //public bool checkName(string name) 
        //{
        //    Regex regex = new Regex(@"^[a-zA-Z]{3,20}$");
        //    if (regex.IsMatch(name)) { return true; }
        //    else { return false; }
        //}

        //public void SetName(string name, string surname) 
        //{
        //    //Regex regex = new Regex(@"^[a-zA-Z]{3,20}$");
        //    if (checkName(name) && checkName(surname))
        //    {
        //        this.Name = $"{name} {surname}";
        //    }
        //    else { throw new Exception("\nНекоректний ввід"); }
        //}

        public string Name {
            get { return name; }

            set 
            {
                Regex regex = new Regex(@"^[a-zA-Z]{3,20}$");
                if (regex.IsMatch(value)) { this.name = value; }
                else { throw new Exception("\nНекоректний ввід"); }
            }
        }

        public string Surname
        {
            get { return surname; }
            set
            {
                Regex regex = new Regex(@"^[a-zA-Z]{3,20}$");
                if (regex.IsMatch(value)) { this.surname = value; }
                else { throw new Exception("\nНекоректний ввід"); }
            }
        }

        //public void SetGender(string input)
        //{
        //    if (!string.IsNullOrWhiteSpace(input))
        //    {
        //        switch (input)
        //        {
        //            case "1":
        //                this.Gender = Gender.Man;
        //                break;
        //            case "2":
        //                this.Gender = Gender.Woman;
        //                break;
        //            case "3":
        //                this.Gender = Gender.Other;
        //                break;
        //            default:
        //                input = input.ToLower();
        //                input = char.ToUpper(input[0]) + input.Substring(1);
        //                if (input == "Man")
        //                {
        //                    this.Gender = Gender.Man;
        //                }
        //                else if (input == "Woman")
        //                {
        //                    this.Gender = Gender.Woman;
        //                }
        //                else if (input == "Other")
        //                {
        //                    this.Gender = Gender.Other;
        //                }
        //                else
        //                {
        //                    throw new Exception("Некоректний ввід");
        //                }
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        throw new Exception("Некоректний ввід");
        //    }
        //}

        public Gender Gender
        {
            get { return gender; }

            set { this.gender = value; }
        }

        public string City
        {
            get { return city; }
            set
            {
                Regex regex = new Regex(@"^[a-zA-Z]{1,}$");
                if (regex.IsMatch(value)) { this.city = value; }
                else { throw new Exception("\nНекоректний ввід"); }
            }
        }
        
        public string Street
        {
            get { return street; }
            set
            {
                Regex regex = new Regex(@"^[a-zA-Z]{1,}$");
                if (regex.IsMatch(value)) { this.street = value; }
                else { throw new Exception("\nНекоректний ввід"); }
            }
        }

        public string Housenumber
        {
            get { return housenumber; }
            set
            {
                Regex regex = new Regex(@"^[1-9]\d*([A-Za-z]|\/[1-9]\d*|-[A-Za-z])?$");
                if (regex.IsMatch(value)) { this.housenumber = value; }
                else { throw new Exception("\nНекоректний ввід"); }
            }
        }

        //public bool checkHousenumber(string input) 
        //{
        //    Regex regex = new Regex(@"^[1-9]\d*([A-Za-z]|\/[1-9]\d*|-[A-Za-z])?$");
        //    if (regex.IsMatch(input)) { return true; }
        //    else { return false; }
        //}

        //public void SetAddress(string input) 
        //{
        //    string[] input_1 = input.Split(',');
        //    if (input_1.Length == 3)
        //    {
        //        string city, street, housenumber;
        //        city = input_1[0].Trim();
        //        street = input_1[1].Trim();
        //        housenumber = input_1[2].Trim();

        //        if (checkCityStreet(city) || checkCityStreet(street) || checkHousenumber(housenumber))
        //        {
        //            Address = input;
        //        } else { throw new Exception("Некоректний ввід"); }
        //    } else { throw new Exception("Некоректний ввід"); }
        //}



        //public void SetStatus(string input)
        //{
        //    if (!string.IsNullOrWhiteSpace(input))
        //    {
        //        if (input == "1")
        //        {
        //            Status = Status.Active;
        //        }
        //        else if (input == "2")
        //        {
        //            Status = Status.Inactive;
        //        }
        //        else
        //        {
        //            input = input.ToLower();
        //            input = char.ToUpper(input[0]) + input.Substring(1);
        //            if (input == "Active")
        //            {
        //                Status = Status.Active;
        //            }
        //            else if (input == "Inactive")
        //            {
        //                Status = Status.Inactive;
        //            }
        //            else throw new Exception("Некоректний ввід");
        //        }
        //    }
        //    else
        //    {
        //        throw new Exception("Некоректний ввід");
        //    }
            
        //}

        public Status Status
        {
            get { return status; }
            set { status = value; }
        }

        //public void SetDate(string input)
        //{
        //    DateOnly dateOfBirth;

        //    if (DateOnly.TryParseExact(input, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOfBirth))
        //    {

        //        DateOnly today = DateOnly.FromDateTime(DateTime.Now);

        //        int age = today.Year - dateOfBirth.Year;

        //        if (today < dateOfBirth.AddYears(age))
        //        {
        //            age--;
        //        }

        //        if (age >= 14 && age <= 122)
        //        {
        //            BirthDate = dateOfBirth;
        //        }
        //        else
        //        {
        //            throw new Exception("Некоректний ввід");
        //        }
        //    }
        //    else
        //    {
        //        throw new Exception("Некоректний ввід");
        //    }
        //}

        public DateOnly BirthDate
        {
            get { return birthDate; }
            set {
                DateOnly today = DateOnly.FromDateTime(DateTime.Now);

                int age = today.Year - value.Year;

                if (today < value.AddYears(age))
                {
                    age--;
                }

                if (age >= 14 && age <= 122)
                {
                    birthDate = value;
                }
                else
                {
                    throw new Exception("Некоректний ввід");
                }
            }
        }

        public string FullName
        {
            get { return $"{Name}  {Surname}"; }
        }//!

        public void addBonus(float price)
        {
            this.Bonus += price;
        }

        public float[] getDiscount(float price) //max discount 10%
        {
            float bonus;
            float[] result = new float[3];
            bonus = this.Bonus - price;
            if (bonus >= 0)
            {
                result[0] = price - (price / 10); //Сума зі знижкою
                result[1] = price; // Витрачені бонуси
                result[2] = bonus; // Залишок бонусів
                this.Bonus = bonus;
            }
            else 
            {
                result[0] = price - (this.Bonus / 10); //Сума зі знижкою
                result[1] = this.Bonus; // Витрачені бонуси
                result[2] = 0; // Залишок бонусів
                this.Bonus = 0;
            }
            return result;
        }

        public string[] buyByBonus (float price)
        {
            string[] result = new string[3];
            if ((this.Bonus / 10) >= price)
            {
                this.Bonus -= price * 10;
                result[0] = "true";
            }
            else 
            {
                result[0] = "false";
            }

            result[1] = $"{price * 10}";
            result[2] = $"{this.Bonus}";
            
            return result;
        }

        public float[] showMaxDiscount(float price) 
        {
            float[] result = new float[3];
            result = getDiscount(price);
            addBonus(result[1]);
            return result;
        }

        public string getAddress()
        {
            return $"{City}, {Street}, {Housenumber}";
            //set
            //{
            //    if (checkCityStreet(this.City) && checkCityStreet(this.Street) && checkHousenumber(this.HouseNumber))
            //    {
            //        address = $"{City}, {Street}, {HouseNumber}";
            //    }
            //    else { throw new Exception("Некоректний ввід"); }
            //}
        }
    };
}