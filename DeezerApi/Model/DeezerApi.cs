using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace DeezerApi.Model
{
	public static class DeezerApi
	{
		private static HttpClient client = new HttpClient();

		//Getting song info
		public static Song GetInfoSong(string trackId, Artist artist, Album album)
		{
			string url = "https://api.deezer.com/track/" + trackId;
			var json = "";

			Song song = new Song();
			Album al = album;
			Artist ar = artist;

			using (WebClient wc = new WebClient())
			{
				json = wc.DownloadString(url);
			}

			try
			{
				if (json == "{\"error\":{\"type\":\"Exception\",\"message\":\"Quota limit exceeded\",\"code\":4}}")
				{
					Thread.Sleep(5000);
					GetInfoSong(trackId, ar, al);
				}

				dynamic data = JObject.Parse(json);
				dynamic data2;

				//Song
				song.Id = data.id;
				song.Title = data.title;
				song.Disk_Number = data.disk_number;
				song.ReleaseDate = data.release_data;
				song.Duration = data.duration;
				song.Track_Position = data.track_position;

				//Album
				if (album.Name == null)
				{
					JObject albumJson = data.album;
					data2 = albumJson;
					album.Name = data2.title;
					album.CoverUrl = data2.cover_xl;
					album.ReleaseDate = data2.release_data;
					album.Id = data2.id;
				}

				//Artist
				if (artist.Name == null)
				{
					JObject artistJson = data.artist;
					data2 = artistJson;
					artist.Name = data2.name;
					artist.Id = data2.id;
					artist.PictureUrl = data2.picture_xl;
				}

				song.Artist = artist;
				song.Album = album;
				album.AddSong(song);

				return song;

			}
			catch(Exception)
			{
				try
				{
					Thread.Sleep(200);
					return GetInfoSong(trackId, ar, al);
				}
				catch (Exception)
				{
					return null;
				}
			}


		}

		public static Song GetInfoSong(string trackId, Artist artist)
		{
			return GetInfoSong(trackId, artist, new Album());
		}

		public static Song GetInfoSong(string trackId)
		{
			return GetInfoSong(trackId, new Artist(), new Album());
		}

		public static Song GetInfoSong(string trackId, Album album)
		{
			return GetInfoSong(trackId, new Artist(), album);
		}



		//Get songs in a playlist
		public static List<Song> GetPlaylistSongs(string playlistid)
		{
			string url = "https://api.deezer.com/playlist/" + playlistid;
			var json = "";
			List<Song> songInfo = new List<Song>();

			using (WebClient wc = new WebClient())
			{
				json = wc.DownloadString(url);
			}


			//Data van songs ophalen via deezer api calls
			try
			{
				dynamic data = JObject.Parse(json);
				dynamic data2 = data.tracks;

				if (json == "{\"error\":{\"type\":\"Exception\",\"message\":\"Quota limit exceeded\",\"code\":4}}")
				{
					Thread.Sleep(5000);
					GetPlaylistSongs(playlistid);
				}

				var trackInfo = JObject.Parse(json)
				.Descendants()
				.Where(x => x is JObject)
				.Where(x => x["id"] != null && x["title"] != null && x["duration"] != null)
				.Select(x => new { ID = (int)x["id"]})
				.ToList();

				foreach (var info in trackInfo)
				{
					songInfo.Add(GetInfoSong(info.ID.ToString()));
				}

				return songInfo;

			}
			catch (Exception)
			{
				return null;
			}
		}

		public static List<Song> GetAlbumSongs(string albumID)
		{
			string url = "https://api.deezer.com/album/" + albumID;
			var json = "";
			List<Song> songInfo = new List<Song>();
			Album al = new Album();

			using (WebClient wc = new WebClient())
			{
				json = wc.DownloadString(url);
			}


			//Data van songs ophalen via deezer api calls
			try
			{

				if (json == "{\"error\":{\"type\":\"Exception\",\"message\":\"Quota limit exceeded\",\"code\":4}}")
				{
					Thread.Sleep(5000);
					using (WebClient wc = new WebClient())
					{
						json = wc.DownloadString(url);
					}
				}


				var trackInfo = JObject.Parse(json)
					.Descendants()
					.Where(x => x is JObject)
					.Where(x => x["id"] != null && x["title"] != null && x["duration"] != null)
					.Select(x => new { ID = (int)x["id"]})
					.ToList();


				foreach (var info in trackInfo)
				{
					songInfo.Add(GetInfoSong(info.ID.ToString(),al));
				}


				return songInfo;

			}
			catch (Exception)
			{
				return null;
			}
		}

		public static List<Song> GetArtistSongs(string artistID)
		{
			string url = "https://api.deezer.com/artist/" + artistID + "/top?limit=2000";
			var json = "";
			List<Song> songInfo = new List<Song>();
			Artist artist = new Artist();

			using (WebClient wc = new WebClient())
			{
				json = wc.DownloadString(url);
			}


			//Data van songs ophalen via deezer api calls
			try
			{
				dynamic data = JObject.Parse(json);
				dynamic data2 = data.tracks;

				var trackInfo = JObject.Parse(json)
				.Descendants()
				.Where(x => x is JObject)
				.Where(x => x["id"] != null && x["title"] != null && x["duration"] != null)
				.Select(x => new { ID = (int)x["id"]})
				.ToList();


				foreach (var info in trackInfo)
				{
					songInfo.Add(GetInfoSong(info.ID.ToString(),artist));
				}


				return songInfo;

			}
			catch (Exception)
			{
				return null;
			}
		}

		public static String GetArtistName(string artistID)
		{
			string url = "https://api.deezer.com/artist/" + artistID;
			var json = "";

			using (WebClient wc = new WebClient())
			{
				json = wc.DownloadString(url);
			}


			//Data van songs ophalen via deezer api calls
			try
			{

				if (json == "{\"error\":{\"type\":\"Exception\",\"message\":\"Quota limit exceeded\",\"code\":4}}")
				{
					Thread.Sleep(5000);
					using (WebClient wc = new WebClient())
					{
						json = wc.DownloadString(url);
					}
				}

				dynamic data = JObject.Parse(json);
				string artistName;


				artistName = data.name;

				return artistName;

			}
			catch (Exception)
			{
				return "Error occured can't find the artist";
			}
		}

		public static String GetAlbumName(string albumID)
		{
			string url = "https://api.deezer.com/album/" + albumID;
			var json = "";
			string album;

			using (WebClient wc = new WebClient())
			{
				json = wc.DownloadString(url);
			}


			//Data van songs ophalen via deezer api calls
			try
			{

				if (json == "{\"error\":{\"type\":\"Exception\",\"message\":\"Quota limit exceeded\",\"code\":4}}")
				{
					Thread.Sleep(5000);
					using (WebClient wc = new WebClient())
					{
						json = wc.DownloadString(url);
					}
				}

				if (json == "{\"error\":{\"type\":\"DataException\",\"message\":\"no data\",\"code\":800}}")
				{
					album = "Not found";
				}
				else
				{
					dynamic data = JObject.Parse(json);
					album = data.title;

				}

				return album;

			}
			catch (Exception)
			{
				return "Error occured can't find the album";
			}
		}


		public static Artist GetArtist(string artistID)
		{
			string url = "https://api.deezer.com/artist/" + artistID;
			var json = "";
			Artist artist = new Artist();

			using (WebClient wc = new WebClient())
			{
				json = wc.DownloadString(url);
			}


			//Data van songs ophalen via deezer api calls
			try
			{

				if (json == "{\"error\":{\"type\":\"Exception\",\"message\":\"Quota limit exceeded\",\"code\":4}}")
				{
					Thread.Sleep(5000);
					using (WebClient wc = new WebClient())
					{
						json = wc.DownloadString(url);
					}
				}

				dynamic data = JObject.Parse(json);

				artist.Name = data.name;
				artist.Albums = data.nb_album;

				return artist;

			}
			catch (Exception)
			{
				return null;
			}
		}

		public static Album GetAlbum(string albumID)
		{
			string url = "https://api.deezer.com/album/" + albumID;
			var json = "";
			Album album = new Album();

			using (WebClient wc = new WebClient())
			{
				json = wc.DownloadString(url);
			}


			//Data van songs ophalen via deezer api calls
			try
			{

				if (json == "{\"error\":{\"type\":\"Exception\",\"message\":\"Quota limit exceeded\",\"code\":4}}")
				{
					Thread.Sleep(5000);
					using (WebClient wc = new WebClient())
					{
						json = wc.DownloadString(url);
					}
				}

				if (json == "{\"error\":{\"type\":\"DataException\",\"message\":\"no data\",\"code\":800}}")
				{
					album.Name = "Not found";
				}
				else
				{
					dynamic data = JObject.Parse(json);
					album.Name = data.title;
					album.Genre = data.genre_id;
					album.Songs = GetAlbumSongs(albumID);

				}

				return album;

			}
			catch (Exception)
			{
				return null;
			}
		}


		public static string GetPlayListName(string playlistID)
		{
			string url = "https://api.deezer.com/playlist/" + playlistID;
			var json = "";

			using (WebClient wc = new WebClient())
			{
				json = wc.DownloadString(url);
			}


			//Data van songs ophalen via deezer api calls
			try
			{

				if (json == "{\"error\":{\"type\":\"Exception\",\"message\":\"Quota limit exceeded\",\"code\":4}}")
				{
					Thread.Sleep(5000);
					using (WebClient wc = new WebClient())
					{
						json = wc.DownloadString(url);
					}
				}

				dynamic data = JObject.Parse(json);
				return data.title;

			}
			catch (Exception)
			{
				return "Error occured can't find the name";
			}
		}

	}
}
