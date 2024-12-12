using Project_A;
using static System.Net.Mime.MediaTypeNames;

namespace Test
{
    [TestClass]
    public class CompanyTest
    {
        [TestMethod]
        [DataRow("2024-06-01", "CompanyOne")]
        [DataRow("2023-12-31", "Company-Two")]
        [DataRow("2020-02-29", "Company Three")]
        public void Constructor_ValidInput(string dateStr, string name)
        {
            DateTime founded = DateTime.Parse(dateStr);
            Company company = new(founded, name);

            Assert.AreEqual(founded, company.Founded);
            Assert.AreEqual(name, company.Name);
        }

        [TestMethod]
        [DataRow("2025-06-01", "CompanyOne")]
        [DataRow("2023-13-31", "Company-Two")]
        [DataRow("2020-02-44", "Company Three")]
        [ExpectedException(typeof(Exception), "Некоректний ввід дати")]
        public void Constructor_InvalidDateInput_ThrowsException(string dateStr, string name)
        {
            DateTime founded = DateTime.Parse(dateStr);
            Company company = new(founded, name);
        }

        [TestMethod]
        [DataRow("2024-06-01", "Company1")]
        [DataRow("2023-12-31", "Company!")]
        [DataRow("2020-02-20", "Co")]
        [DataRow("2020-02-20", "OneTwoThreeFourFiveSix")]
        [ExpectedException(typeof(Exception), "Некоректний ввід назви")]
        public void Constructor_InvalidNameInput_ThrowsException(string dateStr, string name)
        {
            DateTime founded = DateTime.Parse(dateStr);
            Company company = new(founded, name);
        }

        [TestMethod]
        [DataRow("Brigade", "Kyiv", "Andrii", "Koval", 30)]
        [DataRow("Brig", "Kharkiv", "Oleksandr", "Halushenko", 28)]
        [DataRow("Kvitka", "Odesa", "Serhiy", "Pachov", 53)]
        public void AddBrigade_ValidInput(string brigName, string location, string comFirstName, string comLastName, int comAge)
        {
            DateTime founded = DateTime.Parse("2024-06-01");

            Company company = new(founded, "Company");
            BrigadeCommander brigadeCommander = new(comFirstName, comLastName, comAge);
            Brigade brigade = new(brigName, brigadeCommander, location);

            company.AddBrigade(brigade);

            Assert.AreEqual(company.Brigades[0].Name, brigName);
            Assert.AreEqual(company.Brigades[0].BrigadeCommander.FirstName, comFirstName);
            Assert.AreEqual(company.Brigades[0].BrigadeCommander.LastName, comLastName);
            Assert.AreEqual(company.Brigades[0].BrigadeCommander.Age, comAge);
            Assert.AreEqual(company.Brigades[0].Location, location);
        }

        [TestMethod]
        [DataRow("Brigade", "Kyiv", "Andrii", "Koval", 30)]
        [DataRow("Brig", "Kharkiv", "Oleksandr", "Halushenko", 28)]
        [DataRow("Kvitka", "Odesa", "Serhiy", "Pachov", 53)]
        public void DeleteBrigade_BrigadeDeleted(string brigName, string location, string comFirstName, string comLastName, int comAge)
        {
            // Arrange
            DateTime founded = DateTime.Parse("2024-06-01");
            string companyName = "Company";
            Company company = new(founded, companyName);
            BrigadeCommander brigadeCommander = new(comFirstName, comLastName, comAge);
            Brigade brigade = new(brigName, brigadeCommander, location);

            company.AddBrigade(brigade);

            // Act
            company.DeleteBrigade(brigade);

            // Assert: перевірка, що бригади більше немає в списку
            Assert.IsFalse(company.Brigades.Contains(brigade), "Бригаду не було видалено");
        }

        [TestMethod]
        [DataRow("Brigade", "Kyiv", "Andrii", "Koval", 30)]
        [DataRow("Brig", "Kharkiv", "Oleksandr", "Halushenko", 28)]
        [DataRow("Kvitka", "Odesa", "Serhiy", "Pachov", 53)]
        public void PrintToDisplay_PrintsCorrectOutput(string brigName, string location, string comFirstName, string comLastName, int comAge)
        {
            // Arrange
            DateTime founded = DateTime.Parse("2024-06-01");
            string companyName = "Company";
            Company company = new(founded, companyName);
            BrigadeCommander brigadeCommander = new(comFirstName, comLastName, comAge);
            Brigade brigade = new(brigName, brigadeCommander, location);

            company.AddBrigade(brigade);

            using StringWriter sw = new StringWriter();
            Console.SetOut(sw);

            // Act
            company.PrintToDisplay();

            // Expected
            string brigNames = "";
            foreach (var brig in company.Brigades) { brigNames += brig.Name + " "; }
            string expectedOutput =
                $"Company: {companyName} | Founded: {founded} | Brigades: {brigNames}";

            // Assert
            string actualOutput = sw.ToString();
            Assert.AreEqual(expectedOutput, actualOutput);
        }

        [TestMethod]
        [DataRow("Brigade", 4, 3, 12)]
        [DataRow("Brig", 5, 6, 30)]
        [DataRow("Kvitka", 2, 4, 8)]
        public void GetTotalWorkers_ValidInput_GetsTotalWorkers(string brigName, int workersCount, int brigadesCount, int expected)
        {
            DateTime founded = DateTime.Parse("2024-06-01");
            Specialization spec = (Specialization)Enum.Parse(typeof(Specialization), "Drywaller");

            Company company = new(founded, "Company");
            BrigadeCommander brigadeCommander = new("Andrii", "Koval", 30);
            Brigade brigade = new(brigName, brigadeCommander, "Kyiv");

            for (int j = 1; j <= brigadesCount; j++)
            {
                for (int i = 1; i <= workersCount; i++)
                {
                    Worker worker = new(i, "Serhii", "Pachov", 40, spec);
                    brigade.AddWorker(worker);
                }
                company.AddBrigade(brigade);
            }

            int result = company.GetTotalWorkers();

            Assert.AreEqual(expected, result);
        }
    }

    [TestClass]
    public class BrigadeTest
    {
        [TestMethod]
        [DataRow("Brigade", "Kyiv", "Andrii", "Koval", 30)]
        [DataRow("Brig", "Kharkiv", "Oleksandr", "Halushenko", 28)]
        [DataRow("Kvitka", "Odesa", "Serhiy", "Pachov", 53)]
        public void Constructor_ValidInput(string brigName, string location, string comFirstName, string comLastName, int comAge)
        {
            BrigadeCommander brigadeCommander = new(comFirstName, comLastName, comAge);
            Brigade brigade = new(brigName, brigadeCommander, location);

            Assert.AreEqual(brigade.Name, brigName);
            Assert.AreEqual(brigade.BrigadeCommander.FirstName, comFirstName);
            Assert.AreEqual(brigade.BrigadeCommander.LastName, comLastName);
            Assert.AreEqual(brigade.BrigadeCommander.Age, comAge);
            Assert.AreEqual(brigade.Location, location);
        }

        [TestMethod]
        [DataRow("Br", "Kyiv", "Andrii", "Koval", 30)]
        [DataRow("BrigadeBrigadeBrigade", "Kharkiv", "Oleksandr", "Halushenko", 28)]
        [DataRow("Kvitka1", "Odesa", "Serhiy", "Pachov", 53)]
        [DataRow("Kvitka_", "Odesa", "Serhiy", "Pachov", 53)]
        [DataRow("Kvitka!", "Odesa", "Serhiy", "Pachov", 53)]
        [ExpectedException(typeof(Exception), "Некоректний ввід назви")]
        public void Constructor_InvalidNameInput_ThrowsException(string brigName, string location, string comFirstName, string comLastName, int comAge)
        {
            BrigadeCommander brigadeCommander = new(comFirstName, comLastName, comAge);
            Brigade brigade = new(brigName, brigadeCommander, location);
        }

        [TestMethod]
        [DataRow("Brigade", "Ky1v", "Andrii", "Koval", 30)]
        [DataRow("BrigadeBrigade", "Kha_rkiv", "Oleksandr", "Halushenko", 28)]
        [DataRow("Kvitka", "!desa", "Serhiy", "Pachov", 53)]
        [DataRow("KvitkaA", "O", "Serhiy", "Pachov", 53)]
        [DataRow("KvitkaAA", "Od", "Serhiy", "Pachov", 53)]
        [DataRow("KvitkaAA", "OdesaOdesaOdesaOdesaO", "Serhiy", "Pachov", 53)]
        [ExpectedException(typeof(Exception), "Некоректний ввід локації")]
        public void Constructor_InvalidLocationInput_ThrowsException(string brigName, string location, string comFirstName, string comLastName, int comAge)
        {
            BrigadeCommander brigadeCommander = new(comFirstName, comLastName, comAge);
            Brigade brigade = new(brigName, brigadeCommander, location);
        }

        [TestMethod]
        [DataRow(1, "Andrii", "Koval", 30, "Bricklayer")]
        [DataRow(2, "Oleksandr", "Halushenko", 28, "Drywaller")]
        [DataRow(3, "Serhiy", "Pachov", 53, "Insulator")]
        public void AddWorker_ValidInput(int id, string firstName, string lastName, int age, string specialization)
        {
            BrigadeCommander brigadeCommander = new("Andrii", "Koval", 30);
            Brigade brigade = new("Brigade", brigadeCommander, "Kyiv");

            Specialization spec = (Specialization)Enum.Parse(typeof(Specialization), specialization);

            Worker worker = new(id, firstName, lastName, age, spec);

            brigade.AddWorker(worker);

            Assert.AreEqual(worker.FirstName, firstName);
            Assert.AreEqual(worker.LastName, lastName);
            Assert.AreEqual(worker.Age, age);
            Assert.AreEqual(worker.Specialization, spec);
        }

        [TestMethod]
        [DataRow(1, "Andrii", "Koval", 30, "Bricklayer")]
        [DataRow(2, "Oleksandr", "Halushenko", 28, "Drywaller")]
        [DataRow(3, "Serhiy", "Pachov", 53, "Insulator")]
        public void RemoveWorker_ValidInput(int id, string firstName, string lastName, int age, string specialization)
        {
            BrigadeCommander brigadeCommander = new("Andrii", "Koval", 30);
            Brigade brigade = new("Brigade", brigadeCommander, "Kyiv");

            Specialization spec = (Specialization)Enum.Parse(typeof(Specialization), specialization);

            Worker worker = new(id, firstName, lastName, age, spec);

            brigade.AddWorker(worker);

            brigade.RemoveWorker(id);

            Assert.IsFalse(brigade.Workers.Contains(worker), "Працівника не було видалено");
        }

        [TestMethod]
        [DataRow(4, 1, 3)]
        [DataRow(3, 2, 1)]
        [DataRow(10, 7, 3)]
        public void GetWorkerCount_GetsWorkerCount(int toAdd, int toRemove, int expected)
        {
            BrigadeCommander brigadeCommander = new("Andrii", "Koval", 30);
            Brigade brigade = new("Brigade", brigadeCommander, "Kyiv");

            Specialization spec = (Specialization)Enum.Parse(typeof(Specialization), "Insulator");

            for (int i = 1; i <= toAdd; i++)
            {
                Worker worker = new(i, "Andrii", "Koval", 30, spec);
                brigade.AddWorker(worker);
            }

            for (int i = 1; i <= toRemove; i++)
            {
                brigade.RemoveWorker(i);
            }

            Assert.AreEqual(expected, brigade.Workers.Count);
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(14)]
        [DataRow(5)]
        public void GetWorkerCount_ValidInput_GetsWorkerCount(int workersCount)
        {
            DateTime founded = DateTime.Parse("2024-06-01");
            Specialization spec = (Specialization)Enum.Parse(typeof(Specialization), "Drywaller");

            BrigadeCommander brigadeCommander = new("Andrii", "Koval", 30);
            Brigade brigade = new("Brigade", brigadeCommander, "Kyiv");

            for (int i = 1; i <= workersCount; i++)
            {
                Worker worker = new(i, "Serhii", "Pachov", 40, spec);
                brigade.AddWorker(worker);
            }

            int result = brigade.GetWorkerCount();

            Assert.AreEqual(workersCount, result);
        }
    }

    [TestClass]
    public class WorkerTest
    {
        [TestMethod]
        [DataRow(1, "Andrii", "Koval", 30, "Insulator")]
        [DataRow(2, "Oleksandr", "Halushenko", 28, "Drywaller")]
        [DataRow(3, "Serhiy", "Pachov", 53, "Ironworker")]
        public void Constructor_ValidInput(int id, string firstName, string lastName, int age, string specialization)
        {
            Specialization spec = (Specialization)Enum.Parse(typeof(Specialization), specialization);

            Worker worker = new(id, firstName, lastName, age, spec);

            Assert.AreEqual(worker.Id, id);
            Assert.AreEqual(worker.FirstName, firstName);
            Assert.AreEqual(worker.LastName, lastName);
            Assert.AreEqual(worker.Age, age);
            Assert.AreEqual(worker.Specialization, spec);
        }

        [TestMethod]
        [DataRow(1, "An", "Koval", 30, "Bricklayer")]
        [DataRow(2, "Oleksandrrrrrrrrrrrrrrrrrrrr", "Halushenko", 28, "Drywaller")]
        [DataRow(3, "Serhi1", "Pachov", 53, "Insulator")]
        [DataRow(3, "Ser-hii", "Pachov", 53, "Insulator")]
        [DataRow(3, "Ser_hii", "Pachov", 53, "Insulator")]
        [DataRow(3, "Serhii!", "Pachov", 53, "Insulator")]
        [ExpectedException(typeof(Exception), "Некоректний ввід імені")]
        public void Constructor_InvalidFirstNameInput_ThrowsException(int id, string firstName, string lastName, int age, string specialization)
        {
            Specialization spec = (Specialization)Enum.Parse(typeof(Specialization), specialization);

            Worker worker = new(id, firstName, lastName, age, spec);
        }

        [TestMethod]
        [DataRow(1, "Andrii", "Ko", 30, "Bricklayer")]
        [DataRow(2, "Oleksandr", "Halus-henko", 28, "Drywaller")]
        [DataRow(3, "Serhii", "Pachov!", 53, "Insulator")]
        [DataRow(3, "Serhii", "Pachov1", 53, "Insulator")]
        [DataRow(3, "Serhii", "Pac_hov", 53, "Insulator")]
        [DataRow(3, "Serhii", "Pachooooooooooooooooooooooooov", 53, "Insulator")]
        [ExpectedException(typeof(Exception), "Некоректний ввід прізвища")]
        public void Constructor_InvalidlastNameInput_ThrowsException(int id, string firstName, string lastName, int age, string specialization)
        {
            Specialization spec = (Specialization)Enum.Parse(typeof(Specialization), specialization);

            Worker worker = new(id, firstName, lastName, age, spec);
        }

        [TestMethod]
        [DataRow(1, "Andrii", "Koval", 70, "Bricklayer")]
        [DataRow(2, "Oleksandr", "Halushenko", 1, "Drywaller")]
        [DataRow(3, "Serhii", "Pachov", 14, "Insulator")]
        [DataRow(3, "Serhii", "Pachov", -1, "Insulator")]
        [DataRow(3, "Serhii", "Pachov", 1000, "Insulator")]
        [DataRow(3, "Serhii", "Pachov", 0, "Insulator")]
        [ExpectedException(typeof(Exception), "Некоректний ввід віку")]
        public void Constructor_InvalidAgeInput_ThrowsException(int id, string firstName, string lastName, int age, string specialization)
        {
            Specialization spec = (Specialization)Enum.Parse(typeof(Specialization), specialization);

            Worker worker = new(id, firstName, lastName, age, spec);
        }

        [TestMethod]
        [DataRow(1, "Andrii", "Koval", 30, "Brick")]
        [DataRow(2, "Oleksandr", "Halushenko", 32, "Drywaller1")]
        [DataRow(3, "Serhii", "Pachov", 30, "Insul-ator")]
        [DataRow(3, "Serhii", "Pachov", 55, "Insul_ator")]
        [DataRow(3, "Serhii", "Pachov", 44, "I")]
        [DataRow(3, "Serhii", "Pachov", 40, "Insulatorrrrrrrrrrrrrrrrrrrrrrr")]
        [ExpectedException(typeof(Exception), "Некоректний ввід спеціалізації")]
        public void Constructor_InvalidSpecializationInput_ThrowsException(int id, string firstName, string lastName, int age, string specialization)
        {
            Specialization spec = (Specialization)Enum.Parse(typeof(Specialization), specialization);

            Worker worker = new(id, firstName, lastName, age, spec);
        }

        [TestMethod]
        [DataRow(0, "Andrii", "Koval", 70, "Brick")]
        [DataRow(-1, "Oleksandr", "Halushenko", 1, "Drywaller1")]
        [DataRow(1.1, "Serhii", "Pachov", 14, "Insul-ator")]
        [ExpectedException(typeof(Exception), "Некоректний ввід Id")]
        public void Constructor_InvalidIdInput_ThrowsException(int id, string firstName, string lastName, int age, string specialization)
        {
            Specialization spec = (Specialization)Enum.Parse(typeof(Specialization), specialization);

            Worker worker = new(id, firstName, lastName, age, spec);
        }

        [TestMethod]
        [DataRow("Bricklayer", "Glazier")]
        [DataRow("Drywaller", "Pipefitter")]
        [DataRow("Insulator", "Painter")]
        public void Promote_ValidInput_ChangesSpecialization(string specialization, string newSpecialization)
        {
            Specialization specCurrent = (Specialization)Enum.Parse(typeof(Specialization), specialization);
            Specialization specNew = (Specialization)Enum.Parse(typeof(Specialization), newSpecialization);

            Worker worker = new(1, "Serhii", "Pachov", 30, specCurrent);

            worker.Promote(specNew);

            Assert.AreEqual(worker.Specialization, specNew);
        }

        [TestMethod]
        [DataRow("Bricklayer", "Glaz ier")]
        [DataRow("Drywaller", "Pipefitter1")]
        [DataRow("Insulator", "Painter!")]
        [DataRow("Drywaller", "Pipef_itter")]
        [ExpectedException(typeof(Exception), "Некоректний ввід спеціалізації")]
        public void Promote_InvalidInput_ThrowsException(string specialization, string newSpecialization)
        {
            Specialization specCurrent = (Specialization)Enum.Parse(typeof(Specialization), specialization);
            Specialization specNew = (Specialization)Enum.Parse(typeof(Specialization), newSpecialization);

            Worker worker = new(1, "Serhii", "Pachov", 30, specCurrent);

            worker.Promote(specNew);
        }


        [TestMethod]
        [DataRow("Bricklayer", "Glazier", "Bricklayer Glazier")]
        [DataRow("Drywaller", "Pipefitter", "Drywaller Pipefitter")]
        [DataRow("Insulator", "Painter", "Insulator Painter")]
        public void GetFullName_ValidInput_GetsFullName(string firstName, string lastName, string expected)
        {
            Specialization spec = (Specialization)Enum.Parse(typeof(Specialization), "Glazier");

            Worker worker = new(1, firstName, lastName, 30, spec);

            string result = worker.GetFullName();

            Assert.AreEqual(result, expected);
        }


    }

    [TestClass]
    public class BrigadeCommanderTest
    {
        [TestMethod]
        [DataRow("Andrii", "Koval", 30)]
        [DataRow("Oleksandr", "Halushenko", 28)]
        [DataRow("Serhiy", "Pachov", 53)]
        public void Constructor_ValidInput(string firstName, string lastName, int age)
        {
            BrigadeCommander brigadeCommander = new(firstName, lastName, age);

            Assert.AreEqual(brigadeCommander.FirstName, firstName);
            Assert.AreEqual(brigadeCommander.LastName, lastName);
            Assert.AreEqual(brigadeCommander.Age, age);
        }

        [TestMethod]
        [DataRow("Andri1", "Koval", 30)]
        [DataRow("Olek_sandr", "Halushenko", 28)]
        [DataRow("Ser hiy", "Pachov", 53)]
        [DataRow("Serh!y", "Pachov", 53)]
        [DataRow("Serhiyyyyyyyyyyyyyyyyyyyyyyyyyyyyyy", "Pachov", 53)]
        [DataRow("Se", "Pachov", 53)]
        [ExpectedException(typeof(Exception), "Некоректний ввід імені")]
        public void Constructor_InvalidFirstNameInput_ThrowsException(string firstName, string lastName, int age)
        {
            BrigadeCommander brigadeCommander = new(firstName, lastName, age);
        }

        [TestMethod]
        [DataRow("Andrii", "Koval!", 30)]
        [DataRow("Oleksandr", "Halush4nko", 28)]
        [DataRow("Serhiy", "Pac_hov", 53)]
        [DataRow("Serh!y", "Pac hov", 53)]
        [DataRow("Serh!y", "Pa", 53)]
        [DataRow("Serh!y", "Pachovvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv", 53)]
        [ExpectedException(typeof(Exception), "Некоректний ввід прізвища")]
        public void Constructor_InvalidLastNameInput_ThrowsException(string firstName, string lastName, int age)
        {
            BrigadeCommander brigadeCommander = new(firstName, lastName, age);
        }

        [TestMethod]
        [DataRow("Andrii", "Koval", 17)]
        [DataRow("Oleksandr", "Halushenko", 75)]
        [DataRow("Serhiy", "Pachov", -1)]
        [ExpectedException(typeof(Exception), "Некоректний ввід віку")]
        public void Constructor_InvalidAgeInput_ThrowsException(string firstName, string lastName, int age)
        {
            BrigadeCommander brigadeCommander = new(firstName, lastName, age);
        }

        [TestMethod]
        [DataRow("Andrii", "Koval", 28, "Andrii Koval")]
        [DataRow("Oleksandr", "Halushenko", 45, "Oleksandr Halushenko")]
        [DataRow("Serhiy", "Pachov", 53, "Serhiy Pachov")]
        public void GetFullName_ValidInput_GetsFullName(string firstName, string lastName, int age, string expected)
        {
            BrigadeCommander brigadeCommander = new(firstName, lastName, age);

            string result = brigadeCommander.GetFullName();

            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        [DataRow("Andrii")]
        [DataRow("Oleksandr")]
        [DataRow("Serhiy")]
        public void SetFirstName_ValidInput_SetsFirstName(string firstName)
        {
            BrigadeCommander brigadeCommander = new("firstName", "lastName", 30);
            brigadeCommander.FirstName = firstName;

            Assert.AreEqual(brigadeCommander.FirstName, firstName);
        }

        [TestMethod]
        [DataRow("Andri1")]
        [DataRow("Olek_sandr")]
        [DataRow("Ser hiy")]
        [DataRow("Serh!y")]
        [DataRow("Serhiyyyyyyyyyyyyyyyyyyyyyyyyyyyyyy")]
        [DataRow("Se")]
        [ExpectedException(typeof(Exception), "Некоректний ввід імені")]
        public void SetFirstName_InvalidFirstNameInput_ThrowsException(string firstName)
        {
            BrigadeCommander brigadeCommander = new("firstName", "lastName", 30);
            brigadeCommander.FirstName = firstName;
        }

        [TestMethod]
        [DataRow("Andrii")]
        [DataRow("Oleksandr")]
        [DataRow("Serhiy")]
        public void SetLastName_ValidInput_SetsLastName(string lastName)
        {
            BrigadeCommander brigadeCommander = new("firstName", "lastName", 30);
            brigadeCommander.LastName = lastName;

            Assert.AreEqual(brigadeCommander.LastName, lastName);
        }

        [TestMethod]
        [DataRow("Andri1")]
        [DataRow("Olek_sandr")]
        [DataRow("Ser hiy")]
        [DataRow("Serh!y")]
        [DataRow("Serhiyyyyyyyyyyyyyyyyyyyyyyyyyyyyyy")]
        [DataRow("Se")]
        [ExpectedException(typeof(Exception), "Некоректний ввід прізвища")]
        public void SetLastName_InvalidLastNameInput_ThrowsException(string lastName)
        {
            BrigadeCommander brigadeCommander = new("firstName", "lastName", 30);
            brigadeCommander.LastName = lastName;
        }


        [TestMethod]
        [DataRow(30)]
        [DataRow(53)]
        [DataRow(60)]
        public void SetAge_ValidInput_SetsAge(int age)
        {
            BrigadeCommander brigadeCommander = new("firstName", "lastName", 30);
            brigadeCommander.Age = age;

            Assert.AreEqual(brigadeCommander.Age, age);
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        [DataRow(70)]
        [DataRow(17)]
        [ExpectedException(typeof(Exception), "Некоректний ввід віку")]
        public void SetAge_InvalidAgeInput_ThrowsException(int age)
        {
            BrigadeCommander brigadeCommander = new("firstName", "lastName", 30);
            brigadeCommander.Age = age;
        }
    }

}