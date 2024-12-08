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
        private static int counter; // Лічильник
        private static float bonusCounter;

        public static int Counter 
        {
            get { return counter; }
            set { counter = value; }
        } // Властивість для лічильника

        public static float BonusCounter { get { return bonusCounter; } } // Властивість для лічильника кіл-ті бонусів

        // Конструктоор без параметрів
        public Client()
        {
            Counter++;
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

        public static Client Parse(string s) 
        {
            string[] input = s.Split(" ");
            int count = input.Length;

            if (input.Length == 1) 
            {
                if (input[0] != "") 
                {
                    count = 1;
                }
            }
            
            switch (count) 
            {
                case 0:
                    return new Client();

                case 3:
                    return new Client(input[0], input[1], toGender(input[2]));

                case 8:
                    return new(input[0], input[1], toGender(input[2]), input[3], input[4], input[5], toStatus(input[6]), toDate(input[7]));

                default:
                    throw new Exception("Некоректна кіл-ть даних");
            }
        }

        public static bool TryParse(string s, out Client obj)
        {
            try
            {
                obj = Parse(s);
                return true;
            }
            catch
            {
                obj = null;
                return false;
            }
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
            set { gender = value; }
        }

        public string City
        {
            get { return city; }
            set
            {
                Regex regex = new Regex(@"^[a-zA-Zа-яА-ЯёЁїЇіІєЄґҐ]+(-[a-zA-Zа-яА-ЯёЁїЇіІєЄґҐ]+)*$");
                if (regex.IsMatch(value)) { this.city = value; }
                else { throw new Exception("\nНекоректний ввід міста"); }
            }
        }
        
        public string Street
        {
            get { return street; }
            set
            {
                Regex regex = new Regex(@"^[a-zA-Zа-яА-ЯёЁїЇіІєЄґҐ]+(-[a-zA-Zа-яА-ЯёЁїЇіІєЄґҐ]+)*$");
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
            get { return $"{Name} {Surname}"; }
        }

        public string getAddress()
        {
            return $"{City}, {Street}, {Housenumber}";
        }

        public void addBonus(float price)
        {
            this.Bonus += price;
            bonusCounter += price;
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
                bonusCounter -= price;
            }
            else 
            {
                result[0] = price - (this.Bonus / 10); //Сума зі знижкою
                result[1] = this.Bonus; // Витрачені бонуси
                result[2] = 0; // Залишок бонусів
                this.Bonus = 0;
                bonusCounter -= this.Bonus;
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
                result[0] = (float)Math.Round(price - maxDisc, 2); //Сума зі знижкою
                result[1] = (float)Math.Round(maxDisc * 10); // Витрачені бонуси
                result[2] = (float)Math.Round(bonus); // Залишок бонусів
                this.Bonus = bonus;
                bonusCounter -= maxDisc * 10;
            }
            else
            {
                result[0] = (float)Math.Round(price - (this.Bonus / 10)); //Сума зі знижкою
                result[1] = (float)Math.Round(this.Bonus); // Витрачені бонуси
                result[2] = 0; // Залишок бонусів
                this.Bonus = 0;
                bonusCounter -= this.Bonus;
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
                bonusCounter -= price * 10;
                result[1] = $"{price * 10}";
                result[2] = $"{this.Bonus}";
            }
            else 
            {
                result[0] = "false";
                result[1] = "0";
                result[2] = this.Bonus.ToString();
            }
            
            return result;
        }

        public float[] showMaxDiscount(float price)
        {
            float[] result = new float[3];
            result = getDiscount(price);
            addBonus(result[1]);
            return result;
        }

        public static double[] showMaxDiscount(float price, float bonus)
        {
            double maxDiscount = bonus / 10;

            double actualDiscount = Math.Min(maxDiscount, price);

            double discountedPrice = price - actualDiscount;

            double usedBonusPoints = actualDiscount * 10;

            double remainingBonusPoints = bonus - usedBonusPoints;
            if (remainingBonusPoints < 0)
                remainingBonusPoints = 0;

            return new double[] { discountedPrice, usedBonusPoints, remainingBonusPoints };
        }

        public static DateOnly toDate(string input)
        {
            DateOnly date;

            if (DateOnly.TryParseExact(input, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                return date;
            }
            else
            {
                throw new Exception("Некоректний ввід дати");
            }
        }

        public static Status toStatus(string input)
        {
            Status status;
            if (!string.IsNullOrWhiteSpace(input))
            {
                if (input == "1")
                {
                    return status = Status.Active;
                }
                else if (input == "2")
                {
                    return status = Status.Inactive;
                }
                else
                {
                    input = input.ToLower();
                    input = char.ToUpper(input[0]) + input.Substring(1);
                    if (input == "Active")
                    {
                        return status = Status.Active;
                    }
                    else if (input == "Inactive")
                    {
                        return status = Status.Inactive;
                    }
                    else throw new Exception("Некоректний ввід статусу");
                }
            }
            else
            {
                throw new Exception("Некоректний ввід статусу");
            }
        }

        public static Gender toGender(string input)
        {
            Gender gender;
            if (!string.IsNullOrWhiteSpace(input))
            {
                switch (input)
                {
                    case "1":
                        return gender = Gender.Man;

                    case "2":
                        return gender = Gender.Woman;

                    case "3":
                        return gender = Gender.Other;

                    default:
                        input = input.ToLower();
                        input = char.ToUpper(input[0]) + input.Substring(1);
                        if (input == "Man")
                        {
                            return gender = Gender.Man;
                        }
                        else if (input == "Woman")
                        {
                            return gender = Gender.Woman;
                        }
                        else if (input == "Other")
                        {
                            return gender = Gender.Other;
                        }
                        else
                        {
                            throw new Exception("Некоректний ввід гендеру");
                        }
                }
            }
            else
            {
                throw new Exception("Некоректний ввід гендеру");
            }
        }

        public override string ToString()
        {
            return $"{Name} {Surname} {Gender} {City} {Street} {Housenumber} {Status} {BirthDate.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)}";
        }
    };
}