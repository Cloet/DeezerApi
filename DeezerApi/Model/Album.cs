using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeezerApi.Model
{
	public class Album
	{

		private List<Song> _songs = new List<Song>();
		private String _name;
		private int _genreId;
		private String _releaseDate;
		private String _coverUrl;
		private int _id;

		public int Id
		{
			get { return _id; }
			set { _id = value; }
		}

		public String CoverUrl
		{
			get { return _coverUrl; }
			set { _coverUrl = value.Replace("//","/"); }
		}

		public List<Song> Songs
		{
			get { return _songs; }
			set { _songs = value; }
		}

		public String Name
		{
			get { return _name; }
			set { _name = value; }
		}

		public int Genre
		{
			get { return _genreId; }
			set { _genreId = value; }
		}

		public String ReleaseDate
		{
			get { return _releaseDate; }
			set { _releaseDate = value; }
		}

		public Album()
		{
		}

		public void AddSong(Song s)
		{
			_songs.Add(s);
		}


	}
}
