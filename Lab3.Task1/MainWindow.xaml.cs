using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab3.Task1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string ApiUrl = "https://api.privatbank.ua/p24api/exchange_rates?json&date=";
        private readonly HttpClient _httpClient;
        private ExchangeRates _exchangeRates;

        public MainWindow()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            LoadExchangeRatesAsync();
        }

        private async void LoadExchangeRatesAsync()
        {
            try
            {
                var currentDate = DateTime.Now.ToString("dd.MM.yyyy");
                var response = await _httpClient.GetAsync(ApiUrl + currentDate);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                _exchangeRates = JsonConvert.DeserializeObject<ExchangeRates>(content);
                PopulateCurrencyComboBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading exchange rates: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PopulateCurrencyComboBox()
        {
            var currencies = new List<string> { };
            foreach (var rate in _exchangeRates.ExchangeRate)
            {
                currencies.Add(rate.Currency);
            }
            fromCurrencyComboBox.ItemsSource = currencies;
            toCurrencyComboBox.ItemsSource = currencies;
        }

        private void ConvertCurrency()
        {
            try
            {
                var fromCurrency = fromCurrencyComboBox.SelectedItem.ToString();
                var toCurrency = toCurrencyComboBox.SelectedItem.ToString();
                var amount = double.Parse(amountTextBox.Text);
                var result = CalculateResult(fromCurrency, toCurrency, amount);
                resultTextBlock.Text = $"{Math.Round(amount, 2)} {toCurrency} = {Math.Round(result, 2)} {fromCurrency}";
            }
            catch (Exception)
            {
                resultTextBlock.Text = "Invalid input";
            }
        }

        private double CalculateResult(string fromCurrency, string toCurrency, double amount)
        {
            return amount * GetExchangeRate(toCurrency) / GetExchangeRate(fromCurrency);
        }

        private double GetExchangeRate(string currency)
        {
            foreach (var rate in _exchangeRates.ExchangeRate)
            {
                if (rate.Currency == currency)
                    return rate.PurchaseRateNB;
            }
            return 0;
        }

        private void CurrencySelectionChanged(object sender, RoutedEventArgs e)
        {
            if (fromCurrencyComboBox.SelectedItem != null && toCurrencyComboBox.SelectedItem != null && amountTextBox.Text != "")
            {
                ConvertCurrency();
            }
        }
    }

    public class ExchangeRates
    {
        public List<ExchangeRate> ExchangeRate { get; set; }
    }

    public class ExchangeRate
    {
        public string Currency { get; set; }
        public double PurchaseRateNB { get; set; }
    }
}