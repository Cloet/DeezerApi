using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeezerApi.Model
{
	public class Artist
	{

		private int _id;
		private String _name;
		private int _albums;
		private String _pictureUrl;

		public int Id
		{
			get { return _id; }
			set { _id = value; }
		}

		public int Albums
		{
			get { return _albums; }
			set { _albums = value; }
		}

		public String PictureUrl
		{
			get { return _pictureUrl; }
			set { _pictureUrl = value.Replace("//","/"); }
		}

		public String Name
		{
			get { return _name; }
			set { _name = value; }
		}

		public Artist()
		{
		}

	}
}
