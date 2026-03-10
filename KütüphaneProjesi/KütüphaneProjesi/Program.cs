using KütüphaneProjesi.UI;
using KütüphaneProjesi.Data;

namespace KütüphaneProjesi
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            ApplicationConfiguration.Initialize();

            try
            {
                VeritabaniBaslatici.Baslat();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Veritabanı başlatılırken hata oluştu:\n\n" + ex.Message,
                    "Başlatma Hatası",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            Application.Run(new LoginForm());
        }
    }
}



