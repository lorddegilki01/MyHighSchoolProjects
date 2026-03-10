using MüzikKütüphaneUygulaması.Models;

namespace MüzikKütüphaneUygulaması.UI;

public partial class MainForm
{
    private void BtnClearFilter_Click(object sender, EventArgs e)
    {
        _txtSearch.Clear();
        _cmbGenreFilter.SelectedIndex = 0;
        _cmbSort.SelectedIndex = 0;
        LoadSongs();
    }

    private void BtnAddToPlaylist_Click(object sender, EventArgs e)
    {
        if (_dgvSongs.CurrentRow?.DataBoundItem is not Song song)
        {
            MessageBox.Show("Listeye eklemek için şarkı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_playlistItems.Any(x => x.SarkiId == song.SarkiId))
        {
            MessageBox.Show("Bu şarkı listede zaten var.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        _playlistItems.Add(new PlaylistItem
        {
            SarkiId = song.SarkiId,
            SarkiAdi = song.SarkiAdi,
            SanatciAdi = song.SanatciAdi,
            TurAdi = song.TurAdi,
            Yil = song.Yil
        });

        RefreshPlaylistGrid();
    }

    private void BtnRemoveFromPlaylist_Click(object sender, EventArgs e)
    {
        if (_dgvPlaylist.CurrentRow?.DataBoundItem is not PlaylistItem selected)
        {
            MessageBox.Show("Çıkarmak için listeden şarkı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var existing = _playlistItems.FirstOrDefault(x => x.SarkiId == selected.SarkiId);
        if (existing != null)
        {
            _playlistItems.Remove(existing);
            RefreshPlaylistGrid();
        }
    }

    private void BtnSavePlaylist_Click(object sender, EventArgs e)
    {
        if (_playlistItems.Count == 0)
        {
            MessageBox.Show("Önce listeye en az bir şarkı ekleyin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            _playlistService.SavePlaylist(_currentUser.KullaniciId, _txtPlaylistName.Text, _playlistItems);
            _playlistItems.Clear();
            _txtPlaylistName.Clear();
            RefreshPlaylistGrid();
            LoadSavedPlaylists();
            MessageBox.Show("Çalma listesi kaydedildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Liste kaydedilemedi:\n\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void DgvAdminSongs_SelectionChanged(object sender, EventArgs e)
    {
        if (_dgvAdminSongs.CurrentRow?.DataBoundItem is not Song song)
        {
            return;
        }

        _txtSongName.Text = song.SarkiAdi;
        _txtSongAlbum.Text = song.Album;
        _nudSongYear.Value = Math.Min(_nudSongYear.Maximum, Math.Max(_nudSongYear.Minimum, song.Yil));
        _nudSongDuration.Value = Math.Min(_nudSongDuration.Maximum, Math.Max(_nudSongDuration.Minimum, song.SureSaniye));

        SelectComboById(_cmbSongArtist, song.SanatciId);
        SelectComboById(_cmbSongGenre, song.TurId);
    }

    private void DgvAdminArtists_SelectionChanged(object sender, EventArgs e)
    {
        if (_dgvAdminArtists.CurrentRow?.DataBoundItem is Artist artist)
        {
            _txtArtistName.Text = artist.SanatciAdi;
        }
    }

    private void DgvAdminGenres_SelectionChanged(object sender, EventArgs e)
    {
        if (_dgvAdminGenres.CurrentRow?.DataBoundItem is Genre genre)
        {
            _txtGenreName.Text = genre.TurAdi;
        }
    }

    private void BtnAdminAddSong_Click(object sender, EventArgs e)
    {
        try
        {
            var song = BuildSongFromForm(0);
            _libraryService.AddSong(song);
            LoadAdminSongs();
            LoadSongs();
            MessageBox.Show("Şarkı eklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Şarkı eklenemedi:\n\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnAdminUpdateSong_Click(object sender, EventArgs e)
    {
        if (_dgvAdminSongs.CurrentRow?.DataBoundItem is not Song selected)
        {
            MessageBox.Show("Güncellemek için şarkı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var song = BuildSongFromForm(selected.SarkiId);
            _libraryService.UpdateSong(song);
            LoadAdminSongs();
            LoadSongs();
            MessageBox.Show("Şarkı güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Şarkı güncellenemedi:\n\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnAdminDeleteSong_Click(object sender, EventArgs e)
    {
        if (_dgvAdminSongs.CurrentRow?.DataBoundItem is not Song selected)
        {
            MessageBox.Show("Silmek için şarkı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var confirm = MessageBox.Show("Seçili şarkı silinsin mi?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes)
        {
            return;
        }

        try
        {
            _libraryService.DeleteSong(selected.SarkiId);
            LoadAdminSongs();
            LoadSongs();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Şarkı silinemedi:\n\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnAdminAddArtist_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_txtArtistName.Text))
        {
            MessageBox.Show("Sanatçı adı boş olamaz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            _libraryService.AddArtist(_txtArtistName.Text);
            _txtArtistName.Clear();
            LoadGenresAndCombos();
            LoadAdminArtists();
            LoadAdminSongs();
            LoadSongs();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Sanatçı eklenemedi:\n\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnAdminUpdateArtist_Click(object sender, EventArgs e)
    {
        if (_dgvAdminArtists.CurrentRow?.DataBoundItem is not Artist selected)
        {
            MessageBox.Show("Güncellemek için sanatçı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            _libraryService.UpdateArtist(selected.SanatciId, _txtArtistName.Text);
            LoadGenresAndCombos();
            LoadAdminArtists();
            LoadAdminSongs();
            LoadSongs();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Sanatçı güncellenemedi:\n\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnAdminDeleteArtist_Click(object sender, EventArgs e)
    {
        if (_dgvAdminArtists.CurrentRow?.DataBoundItem is not Artist selected)
        {
            MessageBox.Show("Silmek için sanatçı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            _libraryService.DeleteArtist(selected.SanatciId);
            LoadGenresAndCombos();
            LoadAdminArtists();
            LoadAdminSongs();
            LoadSongs();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Sanatçı silinemedi:\n\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnAdminAddGenre_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_txtGenreName.Text))
        {
            MessageBox.Show("Tür adı boş olamaz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            _libraryService.AddGenre(_txtGenreName.Text);
            _txtGenreName.Clear();
            LoadGenresAndCombos();
            LoadAdminGenres();
            LoadAdminSongs();
            LoadSongs();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Tür eklenemedi:\n\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnAdminUpdateGenre_Click(object sender, EventArgs e)
    {
        if (_dgvAdminGenres.CurrentRow?.DataBoundItem is not Genre selected)
        {
            MessageBox.Show("Güncellemek için tür seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            _libraryService.UpdateGenre(selected.TurId, _txtGenreName.Text);
            LoadGenresAndCombos();
            LoadAdminGenres();
            LoadAdminSongs();
            LoadSongs();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Tür güncellenemedi:\n\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnAdminDeleteGenre_Click(object sender, EventArgs e)
    {
        if (_dgvAdminGenres.CurrentRow?.DataBoundItem is not Genre selected)
        {
            MessageBox.Show("Silmek için tür seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            _libraryService.DeleteGenre(selected.TurId);
            LoadGenresAndCombos();
            LoadAdminGenres();
            LoadAdminSongs();
            LoadSongs();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Tür silinemedi:\n\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnAdminDeleteUser_Click(object sender, EventArgs e)
    {
        if (_dgvAdminUsers.CurrentRow?.DataBoundItem is not UserSummary user)
        {
            MessageBox.Show("Silmek için kullanıcı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (user.KullaniciId == _currentUser.KullaniciId)
        {
            MessageBox.Show("Aktif kullanıcı hesabı silinemez.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var confirm = MessageBox.Show("Seçili kullanıcı silinsin mi?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes)
        {
            return;
        }

        try
        {
            _userService.DeleteUser(user.KullaniciId);
            LoadUsers();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Kullanıcı silinemedi:\n\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private Song BuildSongFromForm(int songId)
    {
        if (string.IsNullOrWhiteSpace(_txtSongName.Text))
        {
            throw new InvalidOperationException("Şarkı adı boş olamaz.");
        }

        var artist = _cmbSongArtist.SelectedItem as ComboItem;
        var genre = _cmbSongGenre.SelectedItem as ComboItem;

        if (artist?.Id == null || genre?.Id == null)
        {
            throw new InvalidOperationException("Sanatçı ve tür seçimi zorunludur.");
        }

        return new Song
        {
            SarkiId = songId,
            SarkiAdi = _txtSongName.Text.Trim(),
            SanatciId = artist.Id.Value,
            TurId = genre.Id.Value,
            Album = _txtSongAlbum.Text.Trim(),
            Yil = Convert.ToInt32(_nudSongYear.Value),
            SureSaniye = Convert.ToInt32(_nudSongDuration.Value)
        };
    }

    private static void SelectComboById(ComboBox combo, int id)
    {
        for (var i = 0; i < combo.Items.Count; i++)
        {
            if (combo.Items[i] is ComboItem item && item.Id == id)
            {
                combo.SelectedIndex = i;
                return;
            }
        }
    }
}

