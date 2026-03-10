namespace MüzikKütüphaneUygulaması.Models;

public sealed class PlaylistItem
{
    public int SarkiId { get; set; }
    public string SarkiAdi { get; set; }
    public string SanatciAdi { get; set; }
    public string TurAdi { get; set; }
    public int Yil { get; set; }
}
