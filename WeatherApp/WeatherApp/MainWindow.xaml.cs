using System.Net.Http;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WeatherApp
{
    public partial class MainWindow : Window
    {
        private const string ApiKey = "5c277bbcaaa9df27a6ced7c978b5f134";
        private readonly HttpClient _httpClient = new HttpClient();

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void GetWeather_Click(object sender, RoutedEventArgs e)
        {
            string cityName = CityTextBox.Text.Trim();
            if (string.IsNullOrEmpty(cityName))
            {
                MessageBox.Show("Kérlek, írj be egy városnevet!", "Figyelmeztetés", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var btn = sender as Button;
            btn?.IsEnabled = false;

            await FetchWeatherDataAsync(cityName);

            btn?.IsEnabled = true;
        }

        private async Task FetchWeatherDataAsync(string cityName)
        {
            string apiUrl = $"https://api.openweathermap.org/data/2.5/weather?q={cityName}&units=metric&appid={ApiKey}&lang=hu";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var weatherData = JsonSerializer.Deserialize<WeatherResponse>(responseBody);

                if (weatherData != null)
                {
                    CityText.Text = weatherData.Name;
                    
                    string desc = weatherData.Weather[0].Description;
                    DescText.Text = char.ToUpper(desc[0]) + desc.Substring(1);
                    
                    TempText.Text = $"{Math.Round(weatherData.Main.Temp)}";
                    
                    MinMaxText.Text = $"Min: {Math.Round(weatherData.Main.TempMin)} °C | Max: {Math.Round(weatherData.Main.TempMax)} °C";
                    FeelsLikeText.Text = $"{Math.Round(weatherData.Main.FeelsLike)} °C";
                    HumidityText.Text = $"{weatherData.Main.Humidity} %";
                    
                    double windKmH = Math.Round(weatherData.Wind.Speed * 3.6, 1);
                    WindText.Text = $"{windKmH} km/h";
                    
                    PressureText.Text = $"{weatherData.Main.Pressure} hPa";
                    
                    string iconCode = weatherData.Weather[0].Icon;
                    string iconUrl = $"https://openweathermap.org/img/wn/{iconCode}@4x.png";
                    
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(iconUrl, UriKind.Absolute);
                    bitmap.EndInit();
                    WeatherIcon.Source = bitmap;
                }
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("Nem található ilyen város, vagy nincs internetkapcsolat.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Váratlan hiba történt: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}