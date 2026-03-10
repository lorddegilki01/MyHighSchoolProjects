namespace MiniE_TicaretUygulaması.Models;

public sealed class OrderSummary
{
    public int SiparisId { get; set; }
    public int KullaniciId { get; set; }
    public string KullaniciAdi { get; set; } = string.Empty;
    public DateTime Tarih { get; set; }
    public decimal ToplamTutar { get; set; }
    public string Durum { get; set; } = string.Empty;
}
