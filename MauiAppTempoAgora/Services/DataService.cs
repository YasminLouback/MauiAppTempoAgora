using MauiAppTempoAgora.Models;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MauiAppTempoAgora.Services
{
    public class DataService
    {
        // HttpClient estático para evitar problemas de consumo de recursos
        private static readonly HttpClient client = new HttpClient();

        public static async Task<Tempo?> GetPrevisao(string cidade)
        {
            Tempo? t = null;

            string chave = "6135072afe7f6cec1537d5cb08a5a1a2"; 
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={cidade}&units=metric&appid={chave}";

            try
            {
                HttpResponseMessage resp = await client.GetAsync(url);

                // Cidade não encontrada
                if (resp.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new Exception("Cidade não encontrada.");
                }

                // Outros erros da API
                if (!resp.IsSuccessStatusCode)
                {
                    throw new Exception("Erro ao acessar o serviço de clima.");
                }

                string json = await resp.Content.ReadAsStringAsync();
                var rascunho = JObject.Parse(json);

                // Converte horários Unix para LocalDateTime
                DateTime sunrise = DateTimeOffset.FromUnixTimeSeconds((long)rascunho["sys"]["sunrise"]).LocalDateTime;
                DateTime sunset = DateTimeOffset.FromUnixTimeSeconds((long)rascunho["sys"]["sunset"]).LocalDateTime;

                // Preenche o objeto Tempo
                t = new Tempo()
                {
                    lat = (double)rascunho["coord"]["lat"],
                    lon = (double)rascunho["coord"]["lon"],
                    main = (string)rascunho["weather"][0]["main"],
                    description = (string)rascunho["weather"][0]["description"],
                    temp_min = (double)rascunho["main"]["temp_min"],
                    temp_max = (double)rascunho["main"]["temp_max"],
                    speed = (double)rascunho["wind"]["speed"],
                    visibility = (int)rascunho["visibility"],
                    sunrise = sunrise.ToString("HH:mm"),
                    sunset = sunset.ToString("HH:mm")
                };
            }
            catch (HttpRequestException)
            {
                // Sem conexão com a internet
                throw new Exception("Sem conexão com a internet.");
            }

            return t;
        }
    }
}