using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DeezerApi.Model;

namespace DeezerApi.Example
{
	class Program
	{
		static void Main(string[] args)
		{
			Album test = new Album();

			test = Model.DeezerApi.GetAlbum("72419862");

			Console.WriteLine("Album " + test.Name + ":");
			foreach (var song in test.Songs )
			{
				Console.WriteLine("Song Title: " + song.Title + "- Artist name:" + song.Artist.Name + "- Duration:" + song.Duration + "- Album name:" +
				                  song.Album.Name + "- ReleaseDate:" + song.ReleaseDate);
			}

			Console.ReadLine();
			Console.Clear();

			List<Song> songs = new List<Song>();

			songs = Model.DeezerApi.GetPlaylistSongs("1306931615");

			Console.WriteLine("Playlist: " + Model.DeezerApi.GetPlayListName("1306931615"));
			foreach (Song s in songs)
			{
				Console.WriteLine("Title: " + s.Title + "- Artist:" + s.Artist.Name + "- Album:" + s.Album.Name);
			}

			Console.ReadLine();


		}
	}
}
