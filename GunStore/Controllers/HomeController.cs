using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;

namespace ComicBookStore.Controllers
{
    public class Song
    {
        public string Name { get; set; }
        public string ArtistsName { get; set; }
        public string AlbumName { get; set; }
    }

    public class HomeController : Controller
    {
        private readonly Random m_oRandom = new Random();

        // GET: Home
        public ActionResult Home()
        {
            return View(GetiTunesRandomSong("guns"));
        }

        private Song GetiTunesRandomSong(string a_sSearchString)
        {
            string l_sJsonString;

            using (var l_oWebClient = new WebClient())
            {
                l_sJsonString = l_oWebClient.DownloadString(@"http://itunes.apple.com/search?term=" + a_sSearchString + "&media=music");
            }

            var jsonObj = JObject.Parse(l_sJsonString);
            var l_oCount = jsonObj["resultCount"].Value<int>();

            int l_iSongIndex = m_oRandom.Next(l_oCount);
            var l_oSelectedSong = ((JArray)jsonObj["results"])[l_iSongIndex];

            var l_oTrackName = l_oSelectedSong["trackName"].Value<string>();
            var l_oArtistName = l_oSelectedSong["artistName"].Value<string>();
            var l_oAlbumName = l_oSelectedSong["collectionName"].Value<string>();

            return new Song() { AlbumName = l_oAlbumName, Name = l_oTrackName, ArtistsName = l_oArtistName };
        }
    }
}