using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeezerApi.Model
{
	public class Song
	{
		private String _releaseDate;
		private int _duration;
		private int _track_position;
		private int _disk_number;
		private String _title;
		private Artist _artist;
		private Album _album;

		public String ReleaseDate
		{
			get { return _releaseDate; }
			set { _releaseDate = value; }
		}

		public int Duration
		{
			get { return _duration; }
			set { _duration = value; }
		}

		public int Track_Position
		{
			get { return _track_position; }
			set { _track_position = value; }
		}

		public int Disk_Number
		{
			get { return _disk_number; }
			set { _disk_number = value; }
		}

		public String Title
		{
			get { return _title; }
			set { _title = value; }
		}

		public Artist Artist
		{
			get { return _artist; }
			set { _artist = value; }
		}

		public Album Album
		{
			get { return _album; }
			set { _album = value; }
		}

		public Song()
		{
		}
	}
}
