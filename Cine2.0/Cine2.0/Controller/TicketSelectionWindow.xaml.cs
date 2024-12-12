using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;

namespace CineApp.View
{
    public partial class TicketSelectionWindow : Window
    {
        private Movie _selectedMovie;
        private string _username; // Variable para almacenar el nombre de usuario
        private Button _selectedTimeButton; // Variable para almacenar el botón de hora seleccionado
        private int _selectedSeatsCount = 1; // Contador de asientos seleccionados (puedes ajustarlo según sea necesario)

        // Constructor que recibe la película seleccionada y el nombre de usuario
        public TicketSelectionWindow(Movie selectedMovie, string username)
        {
            InitializeComponent();
            _selectedMovie = selectedMovie; // Asignar la película seleccionada
            _username = username; // Asignar el nombre de usuario
            DisplaySelectedMovie();
        }

        // Método para mostrar la película seleccionada
        private void DisplaySelectedMovie()
        {
            SelectedMovieTitle.Text = _selectedMovie.Title; // Mostrar el título de la película
        }

        // Evento para seleccionar la hora de la función
        private void SelectTime_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                // Cambiar el color del botón de hora seleccionado
                if (_selectedTimeButton != null)
                {
                    _selectedTimeButton.Background = Brushes.DarkSlateBlue; // Color de los botones no seleccionados
                }

                // Asignar el botón seleccionado
                _selectedTimeButton = button;
                _selectedTimeButton.Background = Brushes.CornflowerBlue; // Color del botón seleccionado
            }
        }

        // Evento para comprar las entradas
        private void BuyTickets_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedTimeButton == null)
            {
                MessageBox.Show("Por favor, selecciona una hora para la función.");
                return;
            }

            // Obtener la cantidad de entradas seleccionadas desde el ComboBox
            ComboBoxItem selectedItem = (ComboBoxItem)TicketQuantityComboBox.SelectedItem;
            if (selectedItem != null)
            {
                _selectedSeatsCount = int.Parse(selectedItem.Content.ToString());
            }

            // Crear un HashSet vacío para los asientos seleccionados (no hay asientos seleccionados aún)
            HashSet<string> selectedSeats = new HashSet<string>();

            // Pasar a la ventana de selección de asientos, pasando los parámetros necesarios
            SeatSelectionWindow seatSelectionWindow = new SeatSelectionWindow(_selectedMovie, _selectedTimeButton.Content.ToString(), _selectedSeatsCount, selectedSeats);
            seatSelectionWindow.Show();

            // Cerrar la ventana actual de selección de tickets
            this.Close();
        }

        // Evento para volver a la ventana anterior
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Regresar a la ventana de selección de películas, pasando el nombre de usuario
            MovieSelectionWindow movieSelectionWindow = new MovieSelectionWindow(_username);
            movieSelectionWindow.Show();

            // Cerrar la ventana actual de selección de tickets
            this.Close();
        }
    }
}
