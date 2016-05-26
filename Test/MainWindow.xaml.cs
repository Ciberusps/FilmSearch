using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using EasyHttp.Http;
using MaterialMenu;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public ObservableCollection<MovieGenre> movieGenres { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            movieGenres = new ObservableCollection<MovieGenre>
            {
                new MovieGenre {id=1750, name="Аниме", image="Images/Genres/Anime.jpg"},
                new MovieGenre {id=22, name="Биография", image="Images/Genres/Biography.jpg"},
                new MovieGenre {id=3, name="Боевик", image="Images/Genres/Boevik.jpg"},
                new MovieGenre {id=13, name="Вестерн", image="Images/Genres/Western.jpg"},
                new MovieGenre {id=19, name="Военный", image="Images/Genres/Military.jpg"},
                new MovieGenre {id=17, name="Детектив", image="Images/Genres/Detective.jpg"},
                new MovieGenre {id=8, name="Драма", image="Images/Genres/Drama.jpg"},
                new MovieGenre {id=23, name="История", image="Images/Genres/History.jpg"},
                new MovieGenre {id=6, name="Комедия", image="Images/Genres/Comedy.jpg"},
                new MovieGenre {id=16, name="Криминал", image="Images/Genres/Criminal.jpg"},
                new MovieGenre {id=7, name="Мелодрама", image="Images/Genres/Melodrama.jpg"},
                new MovieGenre {id=21, name="Музыка", image="Images/Genres/Music.jpg"},
                new MovieGenre {id=14, name="Мультфильм", image="Images/Genres/Cartoon.jpg"},
                new MovieGenre {id=9, name="Мюзикл", image="Images/Genres/Musicl.jpg"},
                new MovieGenre {id=10, name="Приключения", image="Images/Genres/Journey.jpg"},
                new MovieGenre {id=11, name="Семейный", image="Images/Genres/Family.jpg"},
                new MovieGenre {id=24, name="Спорт", image="Images/Genres/Sport.jpg"},
                new MovieGenre {id=4, name="Триллер", image="Images/Genres/Thriller.jpg"},
                new MovieGenre {id=1, name="Ужасы", image="Images/Genres/Horror.jpg"},
                new MovieGenre {id=2, name="Фантастика", image="Images/Genres/Fantastic.jpg"},
                new MovieGenre {id=18, name="Фильм-нуар", image="Images/Genres/Noure.jpg"},
                new MovieGenre {id=5, name="Фэнтези", image="Images/Genres/Fantasy.jpg"}
            };

            MoviesGenresList.ItemsSource = movieGenres;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Menu.Toggle();
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("You Clicked Packing!");
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            Menu.Hide();
        }

        /*private void DefaultClick(object sender, RoutedEventArgs e)
        {
            Menu.Theme = SideMenuTheme.Default;
        }

        private void PrimaryClick(object sender, RoutedEventArgs e)
        {
            Menu.Theme = SideMenuTheme.Primary;
        }

        private void SuccessClick(object sender, RoutedEventArgs e)
        {
            Menu.Theme = SideMenuTheme.Success;
        }

        private void WarningClick(object sender, RoutedEventArgs e)
        {
            Menu.Theme = SideMenuTheme.Warning;
        }

        private void DangerClick(object sender, RoutedEventArgs e)
        {
            Menu.Theme = SideMenuTheme.Danger;
        }*/



        /*private void ToggleClosingTypeClick(object sender, RoutedEventArgs e)
        {
            Menu.ClosingType = Menu.ClosingType == ClosingType.Auto
                ? ClosingType.Manual
                : ClosingType.Auto;
        }

        private SideMenu MapMenuToTheme(SideMenuTheme theme)
        {
            //this should not be necesray but colors are not changing correctly
            //when changing theme porperty... maybe its needed to implement INotifyPropertyChanged
            return new SideMenu
            {
                MenuWidth = Menu.MenuWidth,
                Theme = theme,
                Menu = Menu.Menu
            };
        }*/

        private void MoviesGenresList_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MovieGenre movieGenre = (MovieGenre)MoviesGenresList.SelectedItem;
            movieGenre.GetPopularFilmsInGenre();

            ContentTabControl.SelectedItem = GenreTab;

            GenreList.ItemsSource = movieGenre.genreMovies;
        }

        private void ShowGenreTab(object sender, RoutedEventArgs e)
        {
            ContentTabControl.SelectedItem = GenresTab;
        }

        private void GenreMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Movie movie = (Movie)GenreList.SelectedItem;
            MessageBox.Show(movie.nameRU + /*" " + movie.nameEN + " " + movie.country +*/ " " + movie.id);
        }
    }

    public class MovieGenre
    {
        public static string popularFilmsRequest =
            "http://api.kinopoisk.cf/getTop?type=kp_item_top_popular_films&genreID=";
        public static string topBestFilmsRequest =
            "http://api.kinopoisk.cf/getTop?type=kp_item_top_best_films&genreID=";

        public string image { get; set; }
        public string name { get; set; }
        public int id { get; set; }

        private Hashtable _JSONRoot;
        private ArrayList _JSONMovies;

        public ObservableCollection<Movie> genreMovies { get; set; }

        public void GetPopularFilmsInGenre()
        {
            /*HttpWebRequest popularFilmsHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(popularFilmsRequest + id);
//            popularFilmsHttpWebRequest.ContentType = @"text/html; charset=windows-1251";
            HttpWebResponse popularFilmsHttpWebResponse = (HttpWebResponse)popularFilmsHttpWebRequest.GetResponse();

            // Get the stream containing content returned by the server.
            Stream dataStream = popularFilmsHttpWebResponse.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();*/

            string responseFromServer;

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(popularFilmsRequest + id);
            using (HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse())
            {
                using (Stream resStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(resStream, Encoding.GetEncoding(response.CharacterSet));
                    responseFromServer = reader.ReadToEnd();
                }
            }

            if (responseFromServer == null) throw new ArgumentNullException(nameof(responseFromServer));

            MessageBox.Show(responseFromServer);

            JObject jRoot = JObject.Parse(responseFromServer); 
            JArray jMovies = jRoot["items"] as JArray;

            genreMovies = new ObservableCollection<Movie>();

            foreach (JObject movie in jMovies)
            {
                Movie currentMovie = new Movie();

                if (movie["id"] != null) currentMovie.id = int.Parse(movie["id"].ToString());
                if (movie["year"] != null) currentMovie.year = movie["year"].ToString();
                if (movie["filmLength"] != null) currentMovie.filmLength = movie["filmLength"].ToString();
                if (movie["country"] != null) currentMovie.country = movie["country"].ToString();
                if (movie["genre"] != null) currentMovie.genre = movie["genre"].ToString();
                if (movie["rating"] != null) currentMovie.rating = movie["rating"].ToString();
                if (movie["nameRU"] != null) currentMovie.nameRU = movie["nameRU"].ToString();
                if (movie["nameEN"] != null) currentMovie.nameEN = movie["nameEN"].ToString();
                if (movie["posterURL"] != null) currentMovie.posterURL = movie["posterURL"].ToString();
                if (movie["videoURL"] != null) currentMovie.videoURL = movie["videoURL"].ToString();

                genreMovies.Add(currentMovie);
            }


        }

        public void GetTopBestFilmsInGenre()
        {

        }
    }

    public class Movie
    {
        public static string filmBigImageRequest = "http://st.kp.yandex.net/images/film_big/";
        public static string filmImageRequest = "http://st.kp.yandex.net/images/film/";

        public int id;
        public string year;
        public string filmLength;
        public string country;
        public string genre;
        public string rating;
        public string nameRU;
        public string nameEN;
        public string posterURL;
        public string videoURL;
    }
}
