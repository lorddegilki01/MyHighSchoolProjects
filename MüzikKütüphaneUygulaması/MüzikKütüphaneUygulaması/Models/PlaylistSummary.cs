namespace MüzikKütüphaneUygulaması.Models;

public sealed class PlaylistSummary
{
    public int ListeId { get; set; }
    public string ListeAdi { get; set; }
    public int SarkiSayisi { get; set; }
    public DateTime OlusturmaTarihi { get; set; }
}
