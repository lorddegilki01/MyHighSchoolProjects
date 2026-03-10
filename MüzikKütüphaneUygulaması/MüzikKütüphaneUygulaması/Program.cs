using MüzikKütüphaneUygulaması.Data;
using MüzikKütüphaneUygulaması.UI;

namespace MüzikKütüphaneUygulaması;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        try
        {
            DatabaseInitializer.Initialize();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Veritabanı başlatılırken hata oluştu:\n\n{ex.Message}\n\nLütfen App.config dosyasındaki MySQL ayarlarını kontrol edin.",
                "Başlatma Hatası",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return;
        }

        Application.Run(new LoginForm());
    }
}
