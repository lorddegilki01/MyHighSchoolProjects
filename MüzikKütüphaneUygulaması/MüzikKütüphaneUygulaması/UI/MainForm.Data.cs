using MüzikKütüphaneUygulaması.Models;

namespace MüzikKütüphaneUygulaması.UI;

public partial class MainForm
{
    private void LoadInitialData()
    {
        var errors = new List<string>();

        TryLoad("Türler", LoadGenresAndCombos, errors);
        TryLoad("Şarkılar", LoadSongs, errors);
        TryLoad("Çalma listesi", RefreshPlaylistGrid, errors);
        TryLoad("Kaydedilen listeler", LoadSavedPlaylists, errors);

        if (_currentUser.IsYonetici)
        {
            TryLoad("Yönetim - Şarkılar", LoadAdminSongs, errors);
            TryLoad("Yönetim - Sanatçılar", LoadAdminArtists, errors);
            TryLoad("Yönetim - Türler", LoadAdminGenres, errors);
            TryLoad("Yönetim - Kullanıcılar", LoadUsers, errors);
        }

        if (errors.Count > 0)
        {
            MessageBox.Show(
                "Veriler yüklenirken bazı hatalar oluştu:\n\n" + string.Join("\n\n", errors),
                "Yükleme Hatası",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
    }

    private static void TryLoad(string step, Action action, List<string> errors)
    {
        try
        {
            action();
        }
        catch (Exception ex)
        {
            errors.Add(step + ": " + ex.Message);
        }
    }

    private void LoadGenresAndCombos()
    {
        _genres = _libraryService.GetGenres().ToList();
        _artists = _libraryService.GetArtists().ToList();

        var genreItems = new List<ComboItem>
        {
            new ComboItem { Id = null, Key = "ALL", Text = "Tüm Türler" }
        };
        genreItems.AddRange(_genres.Select(x => new ComboItem { Id = x.TurId, Key = x.TurId.ToString(), Text = x.TurAdi }));

        _cmbGenreFilter.DataSource = null;
        _cmbGenreFilter.DataSource = genreItems;
        _cmbGenreFilter.SelectedIndex = 0;

        _cmbSort.DataSource = new List<ComboItem>
        {
            new ComboItem { Key = "AdArtan", Text = "Ada Göre (A-Z)" },
            new ComboItem { Key = "AdAzalan", Text = "Ada Göre (Z-A)" },
            new ComboItem { Key = "YilYeni", Text = "Yıla Göre (Yeni-Eski)" },
            new ComboItem { Key = "YilEski", Text = "Yıla Göre (Eski-Yeni)" }
        };
        _cmbSort.SelectedIndex = 0;

        var artistSource = _artists.Select(x => new ComboItem { Id = x.SanatciId, Key = x.SanatciId.ToString(), Text = x.SanatciAdi }).ToList();
        _cmbSongArtist.DataSource = artistSource;

        var genreSource = _genres.Select(x => new ComboItem { Id = x.TurId, Key = x.TurId.ToString(), Text = x.TurAdi }).ToList();
        _cmbSongGenre.DataSource = genreSource;
    }

    private void LoadSongs()
    {
        var selectedGenre = _cmbGenreFilter.SelectedItem as ComboItem;
        var sort = (_cmbSort.SelectedItem as ComboItem)?.Key ?? "AdArtan";

        var songs = _libraryService.GetSongs(_txtSearch.Text, selectedGenre?.Id, sort).ToList();
        _dgvSongs.DataSource = songs;

        if (_dgvSongs.Columns["SarkiId"] != null) _dgvSongs.Columns["SarkiId"].HeaderText = "ID";
        if (_dgvSongs.Columns["SarkiAdi"] != null) _dgvSongs.Columns["SarkiAdi"].HeaderText = "Şarkı";
        if (_dgvSongs.Columns["SanatciAdi"] != null) _dgvSongs.Columns["SanatciAdi"].HeaderText = "Sanatçı";
        if (_dgvSongs.Columns["TurAdi"] != null) _dgvSongs.Columns["TurAdi"].HeaderText = "Tür";
        if (_dgvSongs.Columns["SureSaniye"] != null) _dgvSongs.Columns["SureSaniye"].HeaderText = "Süre (sn)";
        if (_dgvSongs.Columns["EklenmeTarihi"] != null) _dgvSongs.Columns["EklenmeTarihi"].HeaderText = "Eklenme";

        HideInternalSongColumns(_dgvSongs);
    }

    private void LoadAdminSongs()
    {
        var songs = _libraryService.GetSongs(string.Empty, null, "AdArtan").ToList();
        _dgvAdminSongs.DataSource = songs;
        HideInternalSongColumns(_dgvAdminSongs);
    }

    private static void HideInternalSongColumns(DataGridView grid)
    {
        if (grid.Columns["SanatciId"] != null) grid.Columns["SanatciId"].Visible = false;
        if (grid.Columns["TurId"] != null) grid.Columns["TurId"].Visible = false;
    }

    private void LoadAdminArtists()
    {
        _dgvAdminArtists.DataSource = _libraryService.GetArtists().ToList();
    }

    private void LoadAdminGenres()
    {
        _dgvAdminGenres.DataSource = _libraryService.GetGenres().ToList();
    }

    private void LoadUsers()
    {
        _dgvAdminUsers.DataSource = _userService.GetAllUsers().ToList();
    }

    private void RefreshPlaylistGrid()
    {
        _dgvPlaylist.DataSource = null;
        _dgvPlaylist.DataSource = _playlistItems.ToList();

        if (_dgvPlaylist.Columns["SarkiId"] != null) _dgvPlaylist.Columns["SarkiId"].HeaderText = "ID";
        if (_dgvPlaylist.Columns["SarkiAdi"] != null) _dgvPlaylist.Columns["SarkiAdi"].HeaderText = "Şarkı";
        if (_dgvPlaylist.Columns["SanatciAdi"] != null) _dgvPlaylist.Columns["SanatciAdi"].HeaderText = "Sanatçı";
        if (_dgvPlaylist.Columns["TurAdi"] != null) _dgvPlaylist.Columns["TurAdi"].HeaderText = "Tür";

        _lblPlaylistCount.Text = $"Şarkı: {_playlistItems.Count}";
    }

    private void LoadSavedPlaylists()
    {
        _dgvSavedPlaylists.DataSource = _playlistService.GetPlaylists(_currentUser.KullaniciId).ToList();

        if (_dgvSavedPlaylists.Columns["ListeId"] != null) _dgvSavedPlaylists.Columns["ListeId"].HeaderText = "Liste ID";
        if (_dgvSavedPlaylists.Columns["ListeAdi"] != null) _dgvSavedPlaylists.Columns["ListeAdi"].HeaderText = "Liste Adı";
        if (_dgvSavedPlaylists.Columns["SarkiSayisi"] != null) _dgvSavedPlaylists.Columns["SarkiSayisi"].HeaderText = "Şarkı";
        if (_dgvSavedPlaylists.Columns["OlusturmaTarihi"] != null)
        {
            _dgvSavedPlaylists.Columns["OlusturmaTarihi"].HeaderText = "Oluşturma";
            _dgvSavedPlaylists.Columns["OlusturmaTarihi"].DefaultCellStyle.Format = "g";
        }
    }
}

