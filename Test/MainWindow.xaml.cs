using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Security.Permissions;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Newtonsoft.Json.Linq;
using xZune.Vlc;
using Vlc = xZune.Vlc.Vlc;

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

            MoviePlayer.instance = new MoviePlayer(MoviePlayerMediaElement, MoviePlayerPreviewImage, MoviePlayerProgress, MoviePlayerVolume, MoviePlayerCanvas, MoviePlayerPlayButton);

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
        /* private void ProgressBar_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
         {
             var value = (float)(e.GetPosition(ProgressBar).X / ProgressBar.ActualWidth);
             ProgressBar.Value = value;
         }*/

        private void MoviesGenresList_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MovieGenre movieGenre = (MovieGenre)MoviesGenresList.SelectedItem;
            movieGenre.GetMoviesGenre();

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
            movie.GetMovie();

            /* MovieDescription.DataContext = movie;
             MovieDescription.Content = movie;*/

            MovieDescriptionImage.Source = new BitmapImage(new Uri(movie.bigImage));


            MovieDescriptionNameRU.Text = movie.nameRU;
            MovieDescriptionNameEN.Text = movie.nameEN;

            if (movie.year == null) MovieDescriptionYear.Text = "неизвестно"; else MovieDescriptionYear.Text = movie.year;
            if (movie.country == null) MovieDescriptionCountry.Text = "неизвестно"; else MovieDescriptionCountry.Text = movie.country;
            if (movie.slogan == null) MovieDescriptionSlogan.Text = "неизвестно"; else MovieDescriptionSlogan.Text = movie.slogan;

            if (movie.genre == null) MovieDescriptionGenre.Text = "неизвестно"; else MovieDescriptionGenre.Text = movie.genre;
            if (movie.budgetData.budget == null) MovieDescriptionBudget.Text = "неизвестно"; else MovieDescriptionBudget.Text = movie.budgetData.budget;
            if (movie.budgetData.grossRU == null) MovieDescriptionGrossRU.Text = "неизвестно"; else MovieDescriptionGrossRU.Text = movie.budgetData.grossRU;
            if (movie.budgetData.grossUSA == null) MovieDescriptionGenreGrossUSA.Text = "неизвестно"; else MovieDescriptionGenreGrossUSA.Text = movie.budgetData.grossUSA;
            if (movie.budgetData.grossWorld == null) MovieDescriptionGrossWorld.Text = "неизвестно"; else MovieDescriptionGrossWorld.Text = movie.budgetData.grossWorld;
            if (movie.rentData.premiereWorld == null) MovieDescriptionPremiereWorld.Text = "неизвестно"; else MovieDescriptionPremiereWorld.Text = movie.rentData.premiereWorld;
            if (movie.rentData.premiereRU == null) MovieDescriptionPremiereRU.Text = "неизвестно"; else MovieDescriptionPremiereRU.Text = movie.rentData.premiereRU;
            if (movie.description == null) MovieDescriptionDescription.Text = "неизвестно"; else MovieDescriptionDescription.Text = movie.description;
            if (movie.videoURL != null)
            {
                MoviePlayerMediaElement.Source = new Uri(movie.videoURL);
                MoviePlayerPreviewImage.Source = new BitmapImage(new Uri(movie.gallery[1].ToString()));
            }


            //            MovieDescriptionRating.Text = movie.rating;
            ContentTabControl.SelectedItem = FilmDescriptionTab;

            /*MyBitmapImage.Source = new BitmapImage(new Uri(movie.bigImage));
            NameRU.Text = movie.nameRU;
            Genre.Text = movie.genre;
            Rating.Text = movie.rating;*/
            //            MessageBox.Show(movie.NameRU + /*" " + movie.nameEN + " " + movie.country +*/ " " + movie.id + "" + movie.bigImage);
        }

        private void MovieDescriptionTrailer_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            MoviePlayer.instance.ChangeState();
        }

        private void MoviePreviewImage_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            MoviePlayer.instance.Play();
        }


        public class MoviePlayer
        {
            public static MoviePlayer instance;

            public MediaElement mediaElement;
            public Image previewImage;
            public ProgressBar movieProgressBar;
            public Slider volumeSlider;
            public Canvas canvas;
            public Button playButton;
            public bool isPlaying = false;
            public bool isPlayed = false;

            public MoviePlayer(MediaElement player, Image previewImage, ProgressBar movieProgressBar, Slider volumeSlider, Canvas canvas, Button playButton)
            {
                this.mediaElement = player;
                this.previewImage = previewImage;
                this.movieProgressBar = movieProgressBar;
                this.volumeSlider = volumeSlider;
                this.canvas = canvas;
                this.playButton = playButton;

                mediaElement.Loaded += OnMovieLoaded;
            }

            public void Play()
            {
                if (!isPlayed)
                {
                    isPlayed = true;
                    previewImage.Visibility = Visibility.Collapsed;
                }

                isPlaying = true;
                mediaElement.Play();
            }

            public void Pause()
            {
                isPlaying = false;
                mediaElement.Pause();
            }

            public void ChangeState()
            {
                if (isPlaying)
                {
                    Pause();
                }
                else
                {
                    Play();
                }
            }

            public void OnMovieLoaded(Object Object, RoutedEventArgs routedEventArgs)
            {
                /*Canvas.SetLeft(playButton, mediaElement.ActualWidth);
                Canvas.SetTop(playButton, mediaElement.ActualHeight);*/
            }
        }
    }


    public class MovieGenre
    {
        public static string POPULAR_MOVIE_REQUEST =
            "http://api.kinopoisk.cf/getTop?type=kp_item_top_popular_films&genreID=";
        public static string TOP_BEST_MOVIE_REQUEST =
            "http://api.kinopoisk.cf/getTop?type=kp_item_top_best_films&genreID=";

        public string image { get; set; }
        public string name { get; set; }
        public int id { get; set; }

        private Hashtable _JSONRoot;
        private ArrayList _JSONMovies;

        public ObservableCollection<Movie> genreMovies { get; set; }

        public void GetMoviesGenre()
        {
            string responseFromServer = Caching.CacheRequest(AppData.movieRequest == AppData.MovieRequest.TopBestMovies
                ? TOP_BEST_MOVIE_REQUEST + id
                : POPULAR_MOVIE_REQUEST + id);

            //MessageBox.Show(responseFromServer);

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

                currentMovie.bigImage = Movie.FILM_BIG_IMAGE_REQUEST + currentMovie.id + ".jpg";
                currentMovie.smallImage = Movie.FILM_IMAGE_REQUEST + currentMovie.id + ".jpg";

                genreMovies.Add(currentMovie);
            }
        }
    }

    public class Movie
    {
        public const string FILM_BIG_IMAGE_REQUEST = "http://st.kp.yandex.net/images/film_big/";
        public const string FILM_IMAGE_REQUEST = "http://st.kp.yandex.net/images/film/";
        public const string FILM_REQUEST = "http://api.kinopoisk.cf/getFilm?filmID=";
        public const string FILM_GALLERY_PREVIEW = "http://st.kp.yandex.net/images/";

        //Minimal properties
        public int id { get; set; }
        public string year { get; set; }
        public string filmLength { get; set; }
        public string country { get; set; }
        public string genre { get; set; }
        public string rating { get; set; }
        public string nameRU { get; set; }
        public string nameEN { get; set; }
        public string posterURL { get; set; }
        public string videoURL { get; set; }

        //All properties
        public string webURL { get; set; }
        public string slogan { get; set; }
        public string description { get; set; }
        public string ratingMPAA { get; set; }
        public string ratingAgeLimits { get; set; }

        //rentData
        public RentData rentData;

        public class RentData
        {
            public string premiereRU { get; set; }
            public string distributors { get; set; }
            public string premiereWorld { get; set; }
            public string premiereWorldCountry { get; set; }
            public string premiereDVD { get; set; }
            public string premiereBluRay { get; set; }
            public string distributorRelease { get; set; }
        }

        public BudgetData budgetData;

        public class BudgetData
        {
            public string grossRU { get; set; }
            public string grossUSA { get; set; }
            public string grossWorld { get; set; }
            public string budget { get; set; }
        }

        public ArrayList gallery;
        public List<Creator> creators;

        public class Creator
        {
            public string id { get; set; }
            public string nameRU { get; set; }
            public string nameEN { get; set; }
            public string posterURL { get; set; }
            public string professionText { get; set; }
            public string professionKey { get; set; }
        }

        public News topNewsByFilm;

        public class News
        {
            public string id { get; set; }
            public string newsDate { get; set; }
            public string newsImagePreviewURL { get; set; }
            public string newsTitle { get; set; }
            public string newsDescription { get; set; }
        }

        public ArrayList triviaData;

        //Custom properties
        public string bigImage { get; set; }
        public string smallImage { get; set; }

        public void GetMovie()
        {
            string responseFromServer = Caching.CacheRequest(FILM_REQUEST + id);

            //            MessageBox.Show(responseFromServer);

            JObject jRoot = JObject.Parse(responseFromServer);
            if (jRoot["webURL"] != null) webURL = jRoot["webURL"].ToString();
            if (jRoot["slogan"] != null) slogan = jRoot["slogan"].ToString();
            if (jRoot["description"] != null) description = jRoot["description"].ToString();
            if (jRoot["ratingMPAA"] != null) ratingMPAA = jRoot["ratingMPAA"].ToString();
            if (jRoot["ratingAgeLimits"] != null) ratingAgeLimits = jRoot["ratingAgeLimits"].ToString();

            JObject jRentData;
            if (jRoot["rentData"] != null)
            {
                jRentData = jRoot["rentData"] as JObject;
                rentData = new RentData();
                if (jRentData["premiereRU"] != null) rentData.premiereRU = jRentData["premiereRU"].ToString();
                if (jRentData["distributors"] != null) rentData.distributors = jRentData["distributors"].ToString();
                if (jRentData["premiereWorld"] != null) rentData.premiereWorld = jRentData["premiereWorld"].ToString();
                if (jRentData["premiereWorldCountry"] != null) rentData.premiereWorldCountry = jRentData["premiereWorldCountry"].ToString();
                if (jRentData["premiereDVD"] != null) rentData.premiereDVD = jRentData["premiereDVD"].ToString();
                if (jRentData["premiereBluRay"] != null) rentData.premiereBluRay = jRentData["premiereBluRay"].ToString();
                if (jRentData["distributorRelease"] != null) rentData.distributorRelease = jRentData["distributorRelease"].ToString();
            }

            JObject jBudgetData;
            if (jRoot["budgetData"] != null)
            {
                jBudgetData = jRoot["budgetData"] as JObject;
                budgetData = new BudgetData();
                if (jBudgetData["grossRU"] != null) budgetData.grossRU = jBudgetData["grossRU"].ToString();
                if (jBudgetData["grossUSA"] != null) budgetData.grossUSA = jBudgetData["grossUSA"].ToString();
                if (jBudgetData["grossWorld"] != null) budgetData.grossWorld = jBudgetData["grossWorld"].ToString();
                if (jBudgetData["budget"] != null) budgetData.budget = jBudgetData["budget"].ToString();
            }

            JObject jNews;
            if (jRoot["topNewsByFilm"] != null)
            {
                jNews = jRoot["topNewsByFilm"] as JObject;
                topNewsByFilm = new News();
                if (jNews["id"] != null) topNewsByFilm.id = jNews["id"].ToString();
                if (jNews["newsDate"] != null) topNewsByFilm.newsDate = jNews["newsDate"].ToString();
                if (jNews["newsImagePreviewURL"] != null) topNewsByFilm.newsImagePreviewURL = jNews["newsImagePreviewURL"].ToString();
                if (jNews["newsTitle"] != null) topNewsByFilm.newsTitle = jNews["newsTitle"].ToString();
                if (jNews["newsDescription"] != null) topNewsByFilm.newsDescription = jNews["newsDescription"].ToString();
            }

            JArray jGallery;
            if (jRoot["gallery"] != null)
            {
                jGallery = jRoot["gallery"] as JArray;
                gallery = new ArrayList();
                for (int i = 0; i < jGallery.Count; i++)
                {
                    if (jGallery[i]["preview"] != null)
                        gallery.Add(FILM_GALLERY_PREVIEW + jGallery[i]["preview"]);
                }
            }

            JArray jTriviaData;
            if (jRoot["triviaData"] != null)
            {
                jTriviaData = jRoot["triviaData"] as JArray;
                triviaData = new ArrayList();
                for (int i = 0; i < jTriviaData.Count; i++)
                {
                    triviaData.Add(jTriviaData[i]);
                }
            }
        }
    }

    public class Caching
    {
        public static string CacheRequest(string resource)
        {
            string responseFromServer;

            HttpRequestCachePolicy requestPolicy =
                new HttpRequestCachePolicy(HttpCacheAgeControl.MaxAge,
                TimeSpan.FromDays(3));

            HttpWebRequest webRequest =
                (HttpWebRequest)WebRequest.Create(resource);
            webRequest.CachePolicy = requestPolicy;

            using (HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse())
            {
                using (Stream resStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(resStream, Encoding.GetEncoding(response.CharacterSet));
                    responseFromServer = reader.ReadToEnd();

                    reader.Close();
                    resStream.Close();
                    response.Close();
                }
            }

            if (responseFromServer == null) throw new ArgumentNullException(nameof(responseFromServer));

            return responseFromServer;
        }
    }

    public class AppData
    {
        public enum MovieRequest
        {
            TopBestMovies,
            PopularMovies
        }

        public static MovieRequest movieRequest = MovieRequest.TopBestMovies;
    }

}
