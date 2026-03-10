namespace MüzikKütüphaneUygulaması.Models;

public sealed class Song
{
    public int SarkiId { get; set; }
    public string SarkiAdi { get; set; }
    public int SanatciId { get; set; }
    public string SanatciAdi { get; set; }
    public int TurId { get; set; }
    public string TurAdi { get; set; }
    public string Album { get; set; }
    public int Yil { get; set; }
    public int SureSaniye { get; set; }
    public DateTime EklenmeTarihi { get; set; }
}
