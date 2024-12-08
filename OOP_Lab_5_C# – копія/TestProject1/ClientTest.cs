using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace lab_1.Tests
{
    [TestClass]
    public class ClientTests
    {
        [TestMethod]
        [DataRow("John")]
        [DataRow("Kara")]
        [DataRow("Joe")]
        public void SetName_ValidValue_SetsName(string name)
        {
            // Arrange
            var client = new Client();

            // Act
            client.Name = name;

            // Assert
            Assert.AreEqual(name, client.Name);
        }

        [TestMethod]
        [DataRow("Jo")]
        [DataRow("K1")]
        [DataRow("1")]
        [ExpectedException(typeof(Exception), "Некоректний ввід імені")]
        public void SetName_InvalidValue_ThrowsException(string name)
        {
            // Arrange
            var client = new Client();

            // Act
            client.Name = name; // Недопустима довжина імені
        }

        [TestMethod]
        [DataRow("Ovich")]
        [DataRow("Ovna")]
        [DataRow("Kovalenko")]
        public void SetSurname_ValidValue_SetsSurname(string surname)
        {
            // Arrange
            var client = new Client();

            // Act
            client.Surname = surname;

            // Assert
            Assert.AreEqual(surname, client.Surname);
        }

        [TestMethod]
        [DataRow("Ov")]
        [DataRow("O1")]
        [DataRow("1")]
        [ExpectedException(typeof(Exception), "Некоректний ввід прізвища")]
        public void SetSurname_InvalidValue_ThrowsException(string surname)
        {
            // Arrange
            var client = new Client();

            // Act
            client.Surname = surname; // Недопустима довжина прізвища
        }

        [TestMethod]
        [DataRow(Gender.Man)]
        [DataRow(Gender.Woman)]
        [DataRow(Gender.Other)]
        public void SetStatus_ValidValue_SetsStatus(Gender gender)
        {
            // Arrange
            var client = new Client();

            // Act
            client.Gender = gender;

            // Assert
            Assert.AreEqual(gender, client.Gender);
        }

        [TestMethod]
        [DataRow("Kyiv")]
        [DataRow("Kharkiv")]
        [DataRow("New-York")]
        public void SetCity_ValidValue_SetsCity(string city)
        {
            // Arrange
            var client = new Client();

            // Act
            client.City = city;

            // Assert
            Assert.AreEqual(city, client.City);
        }

        [TestMethod]
        [DataRow("Ky1")]
        [DataRow("1")]
        [DataRow(" ")]
        [ExpectedException(typeof(Exception), "Некоректний ввід міста")]
        public void SetCity_InvalidValue_ThrowsException(string city)
        {
            // Arrange
            var client = new Client();

            // Act
            client.City = city; // Місто повинно містити більше одного символу
        }

        [TestMethod]
        [DataRow("Shevchenka")]
        [DataRow("Street")]
        [DataRow("Main-Street")]
        public void SetStreet_ValidValue_SetsStreet(string street)
        {
            // Arrange
            var client = new Client();

            // Act
            client.Street = street;

            // Assert
            Assert.AreEqual(street, client.Street);
        }

        [TestMethod]
        [DataRow("Shevchenka1")]
        [DataRow(" ")]
        [DataRow("")]
        [ExpectedException(typeof(Exception), "Некоректний ввід вулиці")]
        public void SetStreet_InvalidValue_ThrowsException(string street)
        {
            // Arrange
            var client = new Client();

            // Act
            client.Street = street; // Порожнє значення для вулиці
        }

        [TestMethod]
        [DataRow("11")]
        [DataRow("25A")]
        [DataRow("14-B")]
        public void SetHousenumber_ValidValue_SetsHousenumber(string houseNumber)
        {
            // Arrange
            var client = new Client();

            // Act
            client.Housenumber = houseNumber;

            // Assert
            Assert.AreEqual(houseNumber, client.Housenumber);
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("14-B///32fd")]
        [ExpectedException(typeof(Exception), "Некоректний ввід номеру будинку")]
        public void SetHousenumber_InvalidValue_ThrowsException(string houseNumber)
        {
            // Arrange
            var client = new Client();

            // Act
            client.Housenumber = houseNumber; // Номер будинку не може починатися з 0
        }

        [TestMethod]
        [DataRow(Status.Active)]
        [DataRow(Status.Inactive)]
        public void SetStatus_ValidValue_SetsStatus(Status status)
        {
            // Arrange
            var client = new Client();

            // Act
            client.Status = status;

            // Assert
            Assert.AreEqual(status, client.Status);
        }

        [TestMethod]
        [DataRow(2001, 11, 11)]
        [DataRow(1990, 12, 12)]
        [DataRow(2010, 01, 01)]
        public void SetBirthDate_ValidValue_SetsBirthDate(int year, int month, int day)
        {
            // Arrange
            var client = new Client();
            var birthDate = new DateOnly(year, month, day);

            // Act
            client.BirthDate = birthDate;

            // Assert
            Assert.AreEqual(birthDate, client.BirthDate);
        }

        [TestMethod]
        [DataRow(2022, 11, 11)]
        [DataRow(0001, 12, 31)]
        [DataRow(1900, 12, 12)]
        [DataRow(2045, 01, 01)]
        [ExpectedException(typeof(Exception), "Некоректний ввід дати народження")]
        public void SetBirthDate_InvalidValue_ThrowsException(int year, int month, int day)
        {
            // Arrange
            var client = new Client();
            var birthDate = new DateOnly(year, month, day);

            // Act
            client.BirthDate = birthDate; // Некоректний ввід дати народження
        }

        [TestMethod]
        [DataRow("Andrew", "Garfield")]
        [DataRow("Test", "Testko")]
        [DataRow("Joe", "Moe")]
        public void GetFullname_ValidValue_GetsFullname(string name, string surname)
        {
            // Arrange
            var client = new Client();

            // Act
            client.Name = name;
            client.Surname = surname;

            // Assert
            Assert.AreEqual($"{name} {surname}", client.FullName);
        }

        [TestMethod]
        [DataRow("New-York", "Main-street", "11A")]
        [DataRow("Kyiv", "Pchilky", "4")]
        public void GetAddress_ValidValue_GetsAddress(string city, string street, string houseNumber)
        {
            // Arrange
            var client = new Client();

            // Act
            client.City = city;
            client.Street = street;
            client.Housenumber = houseNumber;

            // Assert
            Assert.AreEqual($"{city}, {street}, {houseNumber}", client.getAddress());
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(4)]
        [DataRow(10)]
        public void ConstructorWithoutParameters_IncreasesCounter(int counter)
        {
            // Arrange
            int initialCounter = Client.Counter;

            // Act
            for (int i = 0; i <= counter; i++)
            {
                new Client();
            }

            // Assert
            Assert.AreEqual(initialCounter + counter + 1, Client.Counter);
        }

        [TestMethod]
        [DataRow("Andrew", "Garfield", Gender.Man)]
        [DataRow("Test", "Testko", Gender.Woman)]
        [DataRow("Joe", "Moe", Gender.Other)]
        public void ConstructorWithParameters_InitializesProperties(string name, string surname, Gender gender)
        {
            // Act
            var client = new Client(name, surname, gender);

            // Assert
            Assert.AreEqual(name, client.Name);
            Assert.AreEqual(surname, client.Surname);
            Assert.AreEqual(gender, client.Gender);
        }

        [TestMethod]
        [DataRow("John", "Kharkovich", Gender.Man, "kyiv", "Khreschatyk", "11A", Status.Active, 2000, 12, 12)]
        [DataRow("Jane", "Smith", Gender.Woman, "New-York", "Main-Street", "4-B", Status.Inactive, 1990, 01, 01)]
        [DataRow("Marta", "Koch", Gender.Other, "Dnipro", "Kosach-Kvitka", "11/2", Status.Active, 2002, 08, 03)]
        public void ConstructorWithAllParameters_InitializesAllProperties(string name, string surname, Gender gender, string city, string street, string houseNumber, Status status, int year, int month, int day)
        {
            // Arrange
            var birthDate = new DateOnly(year, month, day);

            // Act
            var client = new Client(name, surname, gender, city, street, houseNumber, status, birthDate);

            // Assert
            Assert.AreEqual(name, client.Name);
            Assert.AreEqual(surname, client.Surname);
            Assert.AreEqual(gender, client.Gender);
            Assert.AreEqual(city, client.City);
            Assert.AreEqual(street, client.Street);
            Assert.AreEqual(houseNumber, client.Housenumber);
            Assert.AreEqual(status, client.Status);
            Assert.AreEqual(birthDate, client.BirthDate);
        }

        [TestMethod]
        [DataRow("Andrew Garfield Man")]
        [DataRow("Test Testko Woman")]
        [DataRow("Joe Moe Other")]
        public void Parse_3values_ValidInput_ReturnsClient(string s)
        {
            // Act
            string[] input = s.Split(" ");
            var client = Client.Parse(s);

            // Assert
            Assert.AreEqual(input[0], client.Name);
            Assert.AreEqual(input[1], client.Surname);
            Assert.AreEqual(Client.toGender(input[2]), client.Gender);
        }

        [TestMethod]
        [DataRow("John Kharkovich Man kyiv Khreschatyk 11A Active 12/12/2000")]
        [DataRow("Jane Smith Woman New-York Main-Street 4-B Inactive 01/01/1990")]
        [DataRow("Marta Koch Other Dnipro Kosach-Kvitka 11/2 Active 03/08/2003")]
        public void Parse_8values_ValidInput_ReturnsClient(string s)
        {
            // Act
            string[] input = s.Split(" ");
            var client = Client.Parse(s);

            // Assert
            Assert.AreEqual(input[0], client.Name);
            Assert.AreEqual(input[1], client.Surname);
            Assert.AreEqual(Client.toGender(input[2]), client.Gender);
            Assert.AreEqual(input[3], client.City);
            Assert.AreEqual(input[4], client.Street);
            Assert.AreEqual(input[5], client.Housenumber);
            Assert.AreEqual(Client.toStatus(input[6]), client.Status);
            Assert.AreEqual(Client.toDate(input[7]), client.BirthDate);
        }

        [TestMethod]
        [DataRow("Andrew Garfield")]
        [DataRow("Test")]
        [DataRow("")]
        [ExpectedException(typeof(Exception), "Некоректна кіл-ть даних")]
        public void Parse_InvalidInput_ThrowsException(string s)
        {
            // Act
            Client.Parse(s);
        }

        [TestMethod]
        [DataRow("John Kharkovich Man kyiv Khreschatyk 11A Active 12/12/2000")]
        [DataRow("Jane Smith Woman New-York Main-Street 4-B Inactive 01/01/1990")]
        [DataRow("Marta Koch Other Dnipro Kosach-Kvitka 11/2 Active 03/08/2003")]
        public void TryParse_ValidInput_ReturnsTrueAndClient(string s)
        {
            // Act
            string[] input = s.Split(" ");
            bool result = Client.TryParse(s, out Client client);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNotNull(client);
            Assert.AreEqual(input[0], client.Name);
            Assert.AreEqual(input[1], client.Surname);
            Assert.AreEqual(Client.toGender(input[2]), client.Gender);
            Assert.AreEqual(input[3], client.City);
            Assert.AreEqual(input[4], client.Street);
            Assert.AreEqual(input[5], client.Housenumber);
            Assert.AreEqual(Client.toStatus(input[6]), client.Status);
            Assert.AreEqual(Client.toDate(input[7]), client.BirthDate);
        }

        [TestMethod]
        [DataRow("Andrew Garfield")]
        [DataRow("Test")]
        [DataRow("")]
        public void TryParse_InvalidInput_ReturnsFalseAndNull(string s)
        {
            // Act
            bool result = Client.TryParse(s, out Client client);

            // Assert
            Assert.IsFalse(result);
            Assert.IsNull(client);
        }

        [TestMethod]
        [DataRow((float)11.0)]
        [DataRow((float)20.5)]
        [DataRow((float)1.0)]
        public void AddBonus_ValidInput_IncreasesBonusAndBonusCounter(float price)
        {
            // Arrange
            var client = new Client();
            float initialBonus = client.Bonus;
            float initialBonusCounter = Client.BonusCounter;

            // Act
            client.addBonus(price);

            // Assert
            Assert.AreEqual(initialBonus + price, client.Bonus);
            Assert.AreEqual(initialBonusCounter + price, Client.BonusCounter);
        }

        [TestMethod]
        [DataRow((float)100, (float)1000, (float)90, (float)100, (float)900)]
        [DataRow((float)200, (float)1000, (float)180, (float)200, (float)800)]
        [DataRow((float)100, (float)10, (float)99, (float)10, (float)0)]
        public void GetDiscount_AppliesCorrectDiscount(float price, float bonus, float expectPrice, float expectBonusSpend, float expectBonusRemain)
        {
            // Arrange
            var client = new Client();
            client.addBonus(bonus);

            // Act
            var result = client.getDiscount(price);

            // Assert
            Assert.AreEqual(expectPrice, result[0], "Сума зі знижкою");
            Assert.AreEqual(expectBonusSpend, result[1], "Витрачені бонуси");
            Assert.AreEqual(expectBonusRemain, result[2], "Залишок бонусів");
        }

        [TestMethod]
        [DataRow(100.0f, (float)5, (float)1000, (float)95, (float)50, (float)950)]
        [DataRow((float)200, (float)20, (float)1000, (float)160, (float)400, (float)600)]
        [DataRow((float)100, (float)2 ,(float)1000, (float)98, (float)20, (float)980)]
        public void GetDiscount_WithCustomDiscount_AppliesCorrectDiscount(float price, float maxDisc ,float bonus, float expectPrice, float expectBonusSpend, float expectBonusRemain)
        {
            // Arrange
            var client = new Client();
            client.addBonus(bonus);

            // Act
            var result = client.getDiscount(price, maxDisc);

            // Assert
            Assert.AreEqual(expectPrice, result[0], "Сума зі знижкою");
            Assert.AreEqual(expectBonusSpend, result[1], "Витрачені бонуси");
            Assert.AreEqual(expectBonusRemain, result[2], "Залишок бонусів");
        }

        [TestMethod]
        [DataRow((float)100, (float)1000, "true", "1000", "0")]
        [DataRow((float)10, (float)1000, "true", "100", "900")]
        public void BuyByBonus_SufficientBonus_ReturnsTrue(float price, float bonus, string expectResult, string expectBonusSpend, string expectBonusRemain)
        {
            // Arrange
            var client = new Client();
            client.addBonus(bonus);

            // Act
            var result = client.buyByBonus(price);

            // Assert
            Assert.AreEqual(expectResult, result[0]);
            Assert.AreEqual(expectBonusSpend, result[1], "Витрачені бонуси");
            Assert.AreEqual(expectBonusRemain, result[2], "Залишок бонусів");
        }

        [TestMethod]
        [DataRow((float)100, (float)900, "false", "0", "900")]
        [DataRow((float)2000, (float)2000, "false", "0", "2000")]
        public void BuyByBonus_InsufficientBonus_ReturnsFalse(float price, float bonus, string expectResult, string expectBonusSpend, string expectBonusRemain)
        {
            // Arrange
            var client = new Client();
            client.addBonus(bonus);

            // Act
            var result = client.buyByBonus(price);

            // Assert
            Assert.AreEqual(expectResult, result[0]);
            Assert.AreEqual(expectBonusSpend, result[1], "Витрачені бонуси");
            Assert.AreEqual(expectBonusRemain, result[2], "Залишок бонусів");
        }

        [TestMethod]
        [DataRow((float)100, (float)1000, (double)0, (double)1000, (double)0)]
        [DataRow((float)200, (float)1000, (double)100, (double)1000, (double)0)]
        [DataRow((float)10, (float)70, (double)3, (double)70, (double)0)]
        public void ShowMaxDiscount_UsingStaticMethod_ReturnsCorrectValues(float price, float bonus, double expectPrice, double expectBonusSpend, double expectBonusRemain)
        {
            // Act
            var result = Client.showMaxDiscount(price, bonus);

            // Assert
            Assert.AreEqual(expectPrice, result[0], "Сума зі знижкою");
            Assert.AreEqual(expectBonusSpend, result[1], "Витрачені бонуси");
            Assert.AreEqual(expectBonusRemain, result[2], "Залишок бонусів");
        }

        [TestMethod]
        [DataRow("John Kharkovich Man kyiv Khreschatyk 11A Active 12/12/2000")]
        [DataRow("Jane Smith Woman New-York Main-Street 4-B Inactive 01/01/1990")]
        [DataRow("Marta Koch Other Dnipro Kosach-Kvitka 11/2 Active 03/08/2003")]
        public void ToString_GetsToString(string s)
        {
            // Arrange


            // Act
            var client = Client.Parse(s);

            // Assert
            Assert.AreEqual(s, client.ToString());

        }
    }
}
