namespace MüzikKütüphaneUygulaması.Models;

public sealed class UserSummary
{
    public int KullaniciId { get; set; }
    public string Isim { get; set; }
    public string Email { get; set; }
    public string Rol { get; set; }
    public DateTime KayitTarihi { get; set; }
}
