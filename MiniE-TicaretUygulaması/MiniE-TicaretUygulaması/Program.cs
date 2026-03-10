using System;
using System.Windows.Forms;
using MiniE_TicaretUygulaması.Data;
using MiniE_TicaretUygulaması.UI;

namespace MiniE_TicaretUygulaması;

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
                $"Veritabanı başlatılırken hata oluştu:\n\n{ex.Message}\n\nLütfen App.config dosyasındaki MySQL bağlantı ayarlarını kontrol edin.",
                "Başlatma Hatası",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return;
        }

        Application.Run(new LoginForm());
    }
}


