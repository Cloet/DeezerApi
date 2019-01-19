using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeezerApi.Model
{
	public static class DeezerApi
	{
		private static HttpClient client = new HttpClient();


		public static ArrayList getPlaylistSongsInfo(string playlistid)
		{
			string url = "https://api.deezer.com/playlist/" + playlistid;
			var json = "";
			ArrayList songInfo = new ArrayList();

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
					using (WebClient wc = new WebClient())
					{
						json = wc.DownloadString(url);
					}
				}

				var trackInfo = JObject.Parse(json)
				.Descendants()
				.Where(x => x is JObject)
				.Where(x => x["id"] != null && x["title"] != null && x["duration"] != null)
				.Select(x => new { ID = (int)x["id"], Title = (string)x["title"], Time = (string)x["duration"] })
				.ToList();


				foreach (var info in trackInfo)
				{
					songInfo.Add(getInfoSong(info.ID.ToString(), null));

				}


				return songInfo;

			}
			catch (Exception e)
			{
				Logger.Log(e.ToString());
				return null;
			}
		}

		public static ArrayList getAlbumSongs(string albumID)
		{
			string url = "https://api.deezer.com/album/" + albumID;
			var json = "";
			string[] albumtype = new string[2];
			ArrayList songInfo = new ArrayList();

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
					albumtype[1] = "";
					albumtype[0] = "album";
				}
				else
				{
					dynamic data = JObject.Parse(json);
					albumtype[0] = data.record_type;
					try
					{
						dynamic data2 = data.artist;
						albumtype[1] = data2.name;
					}
					catch (Exception) { albumtype[1] = ""; }
				}

				var trackInfo = JObject.Parse(json)
					.Descendants()
					.Where(x => x is JObject)
					.Where(x => x["id"] != null && x["title"] != null && x["duration"] != null)
					.Select(x => new { ID = (int)x["id"], Title = (string)x["title"], Time = (string)x["duration"] })
					.ToList();


				foreach (var info in trackInfo)
				{
					songInfo.Add(getInfoSong(info.ID.ToString(), albumtype));
				}


				return songInfo;

			}
			catch (Exception e)
			{
				return null;
			}
		}

		private static string[] getAlbumType(string albumID)
		{
			string url = "https://api.deezer.com/album/" + albumID;
			var json = "";
			string[] albumtype = new string[2];

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
					albumtype[1] = "";
					albumtype[0] = "album";
				}
				else
				{
					dynamic data = JObject.Parse(json);
					albumtype[0] = data.record_type;
					try
					{
						dynamic data2 = data.artist;
						albumtype[1] = data2.name;
					}
					catch (Exception) { albumtype[1] = ""; }
				}

				return albumtype;

			}
			catch (Exception e)
			{
				return null;
			}
		}

		public static ArrayList getArtistSongs(string artistID)
		{
			string url = "https://api.deezer.com/artist/" + artistID + "/top?limit=2000";
			var json = "";
			ArrayList songInfo = new ArrayList();

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
				.Select(x => new { ID = (int)x["id"], Title = (string)x["title"], Time = (string)x["duration"] })
				.ToList();


				foreach (var info in trackInfo)
				{
					songInfo.Add(getInfoSong(info.ID.ToString(), null));

				}


				return songInfo;

			}
			catch (Exception e)
			{
				return null;
			}
		}

		public static string getInfoSong(string trackID, string[] albumtypeAr)
		{
			string url = "https://api.deezer.com/track/" + trackID;
			var json = "";
			string title, album, artist, track;

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
				dynamic data2;
				string albumid;
				string folderstruct;
				string[] albumtype;




				title = data.title;
				track = data.track_position;

				JObject albumJson = data.album;
				data2 = albumJson;
				album = data2.title;
				albumid = data2.id;

				JObject artistJson = data.artist;
				data2 = artistJson;
				artist = data2.name;

				if (albumtypeAr != null)
				{
					albumtype = albumtypeAr;
				}
				else
				{
					albumtype = getAlbumType(albumid);
				}



				if (albumtype[0] == "album")
				{
					album = album + " (Album)";
				}
				if (albumtype[0] == "single")
				{
					album += " (Single)";
				}
				if (albumtype[0] == "")
				{
					album += " (Album)";
				}

				if (albumtype[1] == "Various Artists")
				{
					artist = "Various Artists";
				}

				Thread.Sleep(200);
				return folderstruct;

			}
			catch (Exception e)
			{
				return null;
			}

		}

		public static string getArtist(string artistID)
		{
			string url = "https://api.deezer.com/artist/" + artistID;
			var json = "";
			ArrayList songInfo = new ArrayList();

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
			catch (Exception e)
			{
				return "Error occured can't find the artist";
			}
		}

		public static string getAlbum(string albumID)
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
			catch (Exception e)
			{
				return "Error occured can't find the album";
			}
		}

		public static string getPlayListName(string playlistID)
		{
			string url = "https://api.deezer.com/playlist/" + playlistID;
			var json = "";
			string playlistname;

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
				playlistname = data.title;

				return playlistname;

			}
			catch (Exception e)
			{
				return "Error occured can't find the name";
			}
		}

	}
}
