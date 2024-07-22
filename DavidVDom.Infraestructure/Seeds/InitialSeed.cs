using Azure;
using DavidVDom.Domain.Abstractions;
using DavidVDom.Domain.Entities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace DavidVDom.Infraestructure.Seeds
{
    public class InitialSeed
    {
        public static async Task Seed(DavidVDomDbContext context, IUnitOfWork unitOfWork, ILogger logger)
        {
            var banks = await GetBanksAsync("https://api.opendata.esett.com/EXP06/Banks", logger);

            if (banks != null)
            {
                await context.Banks.AddRangeAsync(banks);
                await unitOfWork.SaveChangesAsync();
            }
        }

        private static async Task<List<Bank>> GetBanksAsync(string url, ILogger logger)
        {
            List<Bank> banks = [];

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    var response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        dynamic jsonArray = JsonConvert.DeserializeObject(json);

                        if (jsonArray != null)
                        {
                            foreach (dynamic jsonObject in jsonArray)
                            {
                                // Deserialize each JSON object into a Bank object using the chosen method
                                var bank = DeserializeBank(jsonObject.ToString()); // Pass JSON object as a string
                                banks.Add(bank);
                            }
                        }
                    }
                    else
                    {
                        string errorMessage = "Endpoint response not successful";
                        logger.LogError(errorMessage);
                        throw new InvalidOperationException(errorMessage);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "It has not been possible to carry out the initial seed: ");
                    throw;
                }
            }

            return banks;
        }

        public static Bank DeserializeBank(string json)
        {
            dynamic jsonObject = JsonConvert.DeserializeObject(json); // Use Newtonsoft.Json for dynamic parsing

            string Name = jsonObject.ContainsKey("name") ? jsonObject.name : null; // Handle missing properties
            string BIC = jsonObject.ContainsKey("bic") ? jsonObject.bic : null;
            string Country = jsonObject.ContainsKey("country") ? jsonObject.country : null;

            var bank = new Bank(Name, BIC, Country);

            return bank;
        }
    }
}
