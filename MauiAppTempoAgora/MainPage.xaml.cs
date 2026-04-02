using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            lbl_res.Text = ""; // Limpa resultados anteriores

            if (string.IsNullOrWhiteSpace(txt_cidade.Text))
            {
                await DisplayAlert("Aviso", "Preencha a cidade.", "OK");
                return;
            }

            try
            {
                Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);

                if (t != null)
                {
                    string dados_previsao =
                        $"Cidade: {txt_cidade.Text}\n" +
                        $"Latitude: {t.lat}\n" +
                        $"Longitude: {t.lon}\n" +
                        $"Clima: {t.main}\n" +
                        $"Descrição: {t.description}\n" +
                        $"Temp Máx: {t.temp_max}°C\n" +
                        $"Temp Min: {t.temp_min}°C\n" +
                        $"Velocidade do Vento: {t.speed} m/s\n" +
                        $"Visibilidade: {t.visibility} m\n" +
                        $"Nascer do Sol: {t.sunrise}\n" +
                        $"Por do Sol: {t.sunset}";

                    lbl_res.Text = dados_previsao;
                }
            }
            catch (Exception ex)
            {
                // Exibe alerta específico para erros retornados pelo DataService
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }
    }
}