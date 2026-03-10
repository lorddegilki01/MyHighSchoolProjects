namespace MiniE_TicaretUygulaması.Models;

public sealed class AppUser
{
    public int KullaniciId { get; set; }
    public string Isim { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Rol { get; set; } = "kullanici";
    public DateTime KayitTarihi { get; set; }

    public bool IsAdmin => Rol.Equals("admin", StringComparison.OrdinalIgnoreCase);
}
