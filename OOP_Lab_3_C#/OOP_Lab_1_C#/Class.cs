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
        private Status status;
        private DateOnly birthDate;

        public float Bonus { get; private set; } = 0; // Автовластивість + private set

        // Конструктоор без параметрів
        public Client()
        {
            //Name = "Unknown";
            //Surname = "Unknown";
            //Gender = Gender.Other;
            //City = "Unknown";
            //Street = "Unknown";
            //Housenumber = "1";
            //Status = Status.Inactive;
            //BirthDate = new DateOnly(2001, 01, 01);
        }

        // Конструктор з параметрами(ім'я, призвище та гендер)
        public Client(string name, string surname, Gender gender) : this()
        {
            this.Name = name;
            this.Surname = surname;
            this.Gender = gender;
        }

        // Конструктор з параметрами(всі поля)
        public Client(string name, string surname, Gender gender, string city, string street, string housenumber, Status status, DateOnly birthDate) : this(name, surname, gender)
        {
            this.City = city;
            this.Street = street;
            this.Housenumber = housenumber;
            this.Status = status;
            this.BirthDate = birthDate;
        }

        public string Name {
            get { return name; }

            set 
            {
                Regex regex = new Regex(@"^[a-zA-Zа-яА-ЯёЁїЇіІєЄґҐ]{3,20}$");
                if (regex.IsMatch(value)) { this.name = value; }
                else { throw new Exception("\nНекоректний ввід імені"); }
            }
        }

        public string Surname
        {
            get { return surname; }
            set
            {
                Regex regex = new Regex(@"^[a-zA-Zа-яА-ЯёЁїЇіІєЄґҐ]{3,20}$");
                if (regex.IsMatch(value)) { this.surname = value; }
                else { throw new Exception("\nНекоректний ввід прізвища"); }
            }
        }

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
                Regex regex = new Regex(@"^[a-zA-Zа-яА-ЯёЁїЇіІєЄґҐ]{1,}$");
                if (regex.IsMatch(value)) { this.city = value; }
                else { throw new Exception("\nНекоректний ввід міста"); }
            }
        }
        
        public string Street
        {
            get { return street; }
            set
            {
                Regex regex = new Regex(@"^[a-zA-Zа-яА-ЯёЁїЇіІєЄґҐ]{1,}$");
                if (regex.IsMatch(value)) { this.street = value; }
                else { throw new Exception("\nНекоректний ввід вулиці"); }
            }
        }

        public string Housenumber
        {
            get { return housenumber; }
            set
            {
                Regex regex = new Regex(@"^[1-9]\d*([a-zA-Zа-яА-ЯёЁїЇіІєЄґҐ]|\/[1-9]\d*|-[a-zA-Zа-яА-ЯёЁїЇіІєЄґҐ])?$");
                if (regex.IsMatch(value)) { this.housenumber = value; }
                else { throw new Exception("\nНекоректний ввід номеру будинку"); }
            }
        }

        public Status Status
        {
            get { return status; }
            set { status = value; }
        }

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
                    throw new Exception("Некоректний ввід дати народження");
                }
            }
        }

        public string FullName
        {
            get { return $"{Name}  {Surname}"; }
        }

        public void addBonus(float price)
        {
            this.Bonus += price;
        }
        
        //max discount 10%
        public float[] getDiscount(float price) 
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
        
        //max discount any%
        public float[] getDiscount(float price, float newMaxDiscount) 
        {
            float maxDisc, bonus;
            float[] result = new float[3];
            maxDisc = price * (newMaxDiscount/100);
            bonus = this.Bonus - maxDisc * 10;
            if (bonus >= 0)
            {
                result[0] = price - maxDisc; //Сума зі знижкою
                result[1] = maxDisc * 10; // Витрачені бонуси
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
        }
    };
}