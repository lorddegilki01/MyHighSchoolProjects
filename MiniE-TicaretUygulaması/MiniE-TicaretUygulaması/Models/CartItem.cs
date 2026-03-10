namespace MiniE_TicaretUygulaması.Models;

public sealed class CartItem
{
    public int UrunId { get; set; }
    public string UrunAdi { get; set; } = string.Empty;
    public decimal BirimFiyat { get; set; }
    public int Adet { get; set; }
    public int MevcutStok { get; set; }
    public decimal AraToplam => BirimFiyat * Adet;
}
