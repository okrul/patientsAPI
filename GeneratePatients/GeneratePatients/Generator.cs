using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GeneratePatients
{
    internal class Generator
    {
        private static readonly string[] genders = { "male", "female", "unknown" };
        private static readonly string[] surnames = { "Иванов", "Петров", "Сидоров", "Кузнецов", "Смирнов" };
        private static readonly string[] maleNames = { "Иван", "Петр", "Александр", "Виктор", "Владимир" };
        private static readonly string[] femaleNames = { "Ольга", "Мария", "Татьяна", "Елизавета", "Екатерина" };
        private static readonly string apiURL = "https://localhost:7075/Patient/PostPatient";
        private static readonly HttpClient httpClient = new HttpClient();
        private static readonly Random random = new Random();

        public async Task GenerateAndSendPatientsAsync(int num)
        {
            List<Patient> pList = new List<Patient>();
            for (int i = 0; i < num; i++)
            {
                pList.Add(GeneratePatient());
            }
            await SendPatientsToWebServiceAsync(pList);
        }

        protected async Task SendPatientsToWebServiceAsync(List<Patient> pList)
        {
            foreach (var patient in pList)
            {
                await httpClient.PostAsJsonAsync(apiURL, patient);
            }
        }

        protected Patient GeneratePatient()
        {
            var patient = new Patient();
            var gender = genders[random.Next(genders.Length)];
            patient.Gender = gender;
            patient.Active = random.Next(2) == 0;
            patient.BirthDate = DateTime.Today.AddDays(random.Next(30));
            patient.Name = new Name
            {
                Family = surnames[random.Next(surnames.Length)] + (gender == "female" ? "а" : ""),
                Use = "official",
                Given = new[] {
                    gender == "female" ? femaleNames[random.Next(femaleNames.Length)] : maleNames[random.Next(maleNames.Length)],
                    maleNames[random.Next(maleNames.Length)] + (gender == "female" ? "овна" : "ович")
                }
            };
            return patient;
        }
    }
}
