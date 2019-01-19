# DeezerApi
.Net deezerapi wrapper, written in c#.

## Done
- Basic functionality works
- Get album, song and artist info
- Get all songs in an album
- Get all songs in a playlist
- Get all songs from a certain artist

## TODO
- Error handling
- Clean up code
- Better album, song and artist implementation
- Genres
- I'll probably rewrite 90% of it. It's old code but it works for now.

## Usage
```C#
//Example
List<Songs> songs = new List<Song>();
songs = DeezerApi.GetPlaylistSongs("id");
```

```C#
//Options
//Getting song info, optional paramters to add song info to a certain artist and/or album
public static Song GetInfoSong(string trackId, Artist artist, Album album);
public static Song GetInfoSong(string trackId, Artist artist);
public static Song GetInfoSong(string trackId);
public static Song GetInfoSong(string trackId, Album album);

//Get songs in a playlist
public static List<Song> GetPlaylistSongs(string playlistid);

//Get Songs in an album
public static List<Song> GetAlbumSongs(string albumID);

//Get songs from a certain artist
public static List<Song> GetArtistSongs(string artistID);

//Get the name of an artist
public static String GetArtistName(string artistID);

//Get the name of an album
public static String GetAlbumName(string albumID);
	
//Returns an artist
public static Artist GetArtist(string artistID);

//Returns an album
public static Album GetAlbum(string albumID);

//Returns the name of a playlist
public static string GetPlayListName(string playlistID);
```

## Dependencies
- Json.Net
