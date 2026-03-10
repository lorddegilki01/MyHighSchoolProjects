namespace MiniE_TicaretUygulaması.Models;

public sealed class Product
{
    public int UrunId { get; set; }
    public string UrunAdi { get; set; } = string.Empty;
    public int KategoriId { get; set; }
    public string KategoriAdi { get; set; } = string.Empty;
    public decimal Fiyat { get; set; }
    public int Stok { get; set; }
    public DateTime EklenmeTarihi { get; set; }
}
