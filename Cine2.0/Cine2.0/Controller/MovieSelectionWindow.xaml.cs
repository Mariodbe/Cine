using System.Collections.Generic;
using System.Windows;

namespace CineApp.View
{
    public partial class MovieSelectionWindow : Window
    {
        private string _username; // Variable para almacenar el nombre de usuario

        // Constructor que recibe el nombre de usuario
        public MovieSelectionWindow(string username)
        {
            InitializeComponent();
            _username = username; // Guardamos el nombre de usuario
            LoadMovies();
        }

        // Método para cargar las películas en el ListBox
        private void LoadMovies()
        {
            List<Movie> movies = new List<Movie>
            {
                new Movie { Title = "El Joker 2", ImagePath = "C:/Users/pablo/Pictures/Cine/eljoker2.jpg" },
                new Movie { Title = "Gladiador 2", ImagePath = "C:/Users/pablo/Pictures/Cine/gladiator.jpg" },
                new Movie { Title = "Robot Salvaje", ImagePath = "C:/Users/pablo/Pictures/Cine/robot-salvaje.jpg" }
            };

            MoviesListBox.ItemsSource = movies; // Asignar la lista de películas al ListBox
        }

        // Evento que maneja el cambio de selección en el ListBox
        private void MoviesListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (MoviesListBox.SelectedItem is Movie selectedMovie)
            {
                // Abre la ventana de selección de tickets pasando la película seleccionada y el nombre de usuario
                var ticketWindow = new TicketSelectionWindow(selectedMovie, _username);
                ticketWindow.Show();

                // Cierra la ventana actual de selección de película
                this.Close();
            }
        }
    }
}
