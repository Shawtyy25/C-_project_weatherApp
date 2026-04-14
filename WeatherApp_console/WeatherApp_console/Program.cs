using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WeatherApp_console
{
    public class Weather
    {
        [JsonPropertyName("description")] public string Description { get; set; }
    }

    public class Main
    {
        [JsonPropertyName("temp")] public float Temp { get; set; }
        [JsonPropertyName("feels_like")] public float FeelsLike { get; set; }
        [JsonPropertyName("temp_min")] public float TempMin { get; set; }
        [JsonPropertyName("temp_max")] public float TempMax { get; set; }
        [JsonPropertyName("pressure")] public int Pressure { get; set; }
        [JsonPropertyName("humidity")] public int Humidity { get; set; }
    }

    public class Wind
    {
        [JsonPropertyName("speed")] public float Speed { get; set; }
    }

    public class WeatherResponse
    {
        [JsonPropertyName("weather")] public Weather[] Weather { get; set; }
        [JsonPropertyName("main")] public Main Main { get; set; }
        [JsonPropertyName("wind")] public Wind Wind { get; set; }
        [JsonPropertyName("name")] public string Name { get; set; }
    }


    class Program
    {
        private const string ApiKey = "5c277bbcaaa9df27a6ced7c978b5f134";
        private static readonly HttpClient HttpClient = new HttpClient();

        static async Task Main(string[] args)
        {
            Console.Title = "Profi Időjárás Lekérdező";

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("========================================");
            Console.WriteLine("       TERMINÁL IDŐJÁRÁS ÁLLOMÁS        ");
            Console.WriteLine("========================================");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Írd be a város nevét a kereséshez!");
            Console.WriteLine("A kilépéshez írd be: exit\n");


            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Város neve: ");
                Console.ResetColor();

                string? cityName = Console.ReadLine()?.Trim();


                if (string.IsNullOrEmpty(cityName))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Írjon be valamit!");
                    Console.ResetColor();
                    continue;
                };

                if (cityName.ToLower() == "exit" || cityName.ToLower() == "kilépés")
                {
                    Console.WriteLine("Viszlát!");
                    break;
                }

                await FetchAndDisplayWeatherAsync(cityName);
                Console.WriteLine();
            }
        }

        private static async Task FetchAndDisplayWeatherAsync(string cityName)
        {
            string apiUrl =
                $"https://api.openweathermap.org/data/2.5/weather?q={cityName}&units=metric&appid={ApiKey}&lang=hu";

            try
            {
                HttpResponseMessage response = await HttpClient.GetAsync(apiUrl);

                if (!response.IsSuccessStatusCode)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(
                        $"[Hiba] Nem található adat a következőhöz: '{cityName}'. Kérlek, ellenőrizd a gépelést!");
                    Console.ResetColor();
                    return;
                }

                string responseBody = await response.Content.ReadAsStringAsync();
                var weatherData = JsonSerializer.Deserialize<WeatherResponse>(responseBody);

                if (weatherData != null)
                {
                    string desc = weatherData.Weather[0].Description;
                    desc = char.ToUpper(desc[0]) + desc.Substring(1);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n--- {weatherData.Name} Időjárása ---");

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"Állapot:      {desc}");
                    Console.WriteLine(
                        $"Hőmérséklet:  {Math.Round(weatherData.Main.Temp)} °C (Hőérzet: {Math.Round(weatherData.Main.FeelsLike)} °C)");
                    Console.WriteLine(
                        $"Min / Max:    {Math.Round(weatherData.Main.TempMin)} °C / {Math.Round(weatherData.Main.TempMax)} °C");
                    Console.WriteLine($"Páratartalom: {weatherData.Main.Humidity} %");
                    Console.WriteLine($"Légnyomás:    {weatherData.Main.Pressure} hPa");

                    double windKmH = Math.Round(weatherData.Wind.Speed * 3.6, 1);
                    Console.WriteLine($"Szélsebesség: {windKmH} km/h");

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("----------------------------------");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Kritikus Hiba] {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}