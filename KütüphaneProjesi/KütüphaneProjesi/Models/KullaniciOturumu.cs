namespace KütüphaneProjesi.Models;

public sealed class KullaniciOturumu
{
    public int Id { get; init; }
    public string Isim { get; init; } = string.Empty;
    public string Rol { get; init; } = "Kullanici";

    public bool YoneticiMi => string.Equals(Rol, "Yonetici", StringComparison.OrdinalIgnoreCase);
}
