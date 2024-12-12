using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MySql.Data.MySqlClient; // Asegúrate de tener la referencia de MySQL añadida

namespace CineApp.View
{
    public partial class SeatSelectionWindow : Window
    {
        private Movie _selectedMovie;
        private string _selectedTime;
        private int _maxSelectableSeats;
        private HashSet<string> _selectedSeats; // Para almacenar los asientos seleccionados

        // Constructor que recibe la película, la hora, el número máximo de asientos y los asientos seleccionados
        public SeatSelectionWindow(Movie selectedMovie, string selectedTime, int selectedSeatsCount, HashSet<string> selectedSeats)
        {
            InitializeComponent();
            _selectedMovie = selectedMovie; // Asignamos la película seleccionada
            _selectedTime = selectedTime;  // Asignamos la hora seleccionada
            _maxSelectableSeats = selectedSeatsCount;  // Asignamos el número máximo de asientos seleccionables
            _selectedSeats = selectedSeats ?? new HashSet<string>(); // Inicializamos o recibimos los asientos seleccionados

            CreateSeatButtons(); // Crear los botones de los asientos
        }

        // Método para crear los botones de los asientos
        private void CreateSeatButtons()
        {
            int rows = 8; // Número de filas
            int leftCols = 2;  // Lateral izquierdo (columnas 1 y 2)
            int centerCols = 5; // Centro (columnas 3 a 7)
            int rightCols = 4; // Lateral derecho (columnas 8 a 11)

            // Limpiar cualquier definición de filas y columnas previas
            SeatsGrid.RowDefinitions.Clear();
            SeatsGrid.ColumnDefinitions.Clear();
            SeatsGrid.Children.Clear();

            // Definir el número de filas
            for (int i = 0; i < rows; i++)
            {
                SeatsGrid.RowDefinitions.Add(new RowDefinition());
            }

            // Definir las columnas para las secciones de asientos
            SeatsGrid.ColumnDefinitions.Add(new ColumnDefinition()); // Lateral izquierdo (col 1)
            SeatsGrid.ColumnDefinitions.Add(new ColumnDefinition()); // Lateral izquierdo (col 2)

            // Columnas del centro (3 a 7)
            for (int i = 0; i < centerCols; i++)
            {
                SeatsGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            // Columnas del lado derecho (8 a 11)
            for (int i = 0; i < rightCols; i++)
            {
                SeatsGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            // Consultar los asientos ocupados desde la base de datos
            var occupiedSeats = GetOccupiedSeats(_selectedMovie.Title, _selectedTime);

            // Crear los botones para cada asiento
            for (int i = 0; i < rows; i++)
            {
                // Llenamos los asientos de las columnas
                for (int j = 0; j < leftCols; j++) // Lado izquierdo
                {
                    CreateSeatButton(i, j, occupiedSeats);
                }

                for (int j = leftCols; j < leftCols + centerCols; j++) // Centro
                {
                    CreateSeatButton(i, j, occupiedSeats);
                }

                for (int j = leftCols + centerCols; j < leftCols + centerCols + rightCols; j++) // Lado derecho
                {
                    CreateSeatButton(i, j, occupiedSeats);
                }
            }
        }

        // Método para crear un botón de asiento
        private void CreateSeatButton(int row, int col, List<string> occupiedSeats)
        {
            Button seatButton = new Button
            {
                Margin = new Thickness(2),
                Tag = $"{row+1}-{col+1}", // Guardamos la posición del asiento
                Content = $"{row + 1}-{col + 1}", // Mostramos la numeración correcta en los botones
                Background = Brushes.SkyBlue, // Color por defecto
                BorderBrush = Brushes.DarkSlateGray, // Borde gris oscuro
                BorderThickness = new Thickness(1),
                FontWeight = FontWeights.Bold,
                FontSize = 12,
                Padding = new Thickness(5),
                Width = 40, // Ajustar tamaño de los botones para que se asemejen más a una butaca
                Height = 40
            };

            // Crear un Border para aplicar CornerRadius
            Border border = new Border
            {
                Background = seatButton.Background,
                BorderBrush = seatButton.BorderBrush,
                BorderThickness = seatButton.BorderThickness,
                CornerRadius = new CornerRadius(8), // Bordes redondeados
                Child = seatButton // Colocamos el botón dentro del Border
            };

            // Marcar los asientos ocupados
            if (occupiedSeats.Contains($"{row + 1}-{col + 1}")) // Comprobamos si el asiento está ocupado
            {
                border.Background = Brushes.Red; // Rojo si está ocupado
                seatButton.IsEnabled = false; // Deshabilitar el asiento ocupado
            }
            else
            {
                seatButton.Click += SeatButton_Click; // Asignar evento de clic
            }

            // Agregar el Border (que contiene el botón) a la cuadrícula de asientos
            Grid.SetRow(border, row);
            Grid.SetColumn(border, col);
            SeatsGrid.Children.Add(border); // Agregar el Border a la cuadrícula
        }

        // Método para obtener los asientos ocupados desde la base de datos
        private List<string> GetOccupiedSeats(string movieTitle, string selectedTime)
        {
            List<string> occupiedSeats = new List<string>();
            string connectionString = "Server=dam.clp4c2egnzkb.us-east-1.rds.amazonaws.com; Database=cine; User ID=admin;Password=MagicCut;";
            string query = "SELECT asientos FROM reservas WHERE pelicula = @pelicula AND hora_funcion = @hora_funcion";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@pelicula", movieTitle);
                    command.Parameters.AddWithValue("@hora_funcion", selectedTime);

                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var seats = reader.GetString("asientos").Split(',');
                        occupiedSeats.AddRange(seats); // Agregar todos los asientos ocupados
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al consultar asientos ocupados: {ex.Message}", "Error");
                }
            }

            return occupiedSeats;
        }

        // Evento para manejar la selección de asientos
        private void SeatButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.IsEnabled)
            {
                string seatCoordinates = button.Tag.ToString();

                if (_selectedSeats.Contains(seatCoordinates)) // Si el asiento ya está seleccionado
                {
                    button.Background = Brushes.SkyBlue; // Volver a azul claro si se deselecciona
                    _selectedSeats.Remove(seatCoordinates); // Eliminar de la lista de seleccionados
                }
                else if (_selectedSeats.Count < _maxSelectableSeats) // Si no hemos alcanzado el máximo de selección
                {
                    button.Background = Brushes.ForestGreen; // Cambiar a verde si está seleccionado
                    _selectedSeats.Add(seatCoordinates); // Agregar a la lista de seleccionados
                }
                else
                {
                    MessageBox.Show($"Solo puedes seleccionar {_maxSelectableSeats} asientos.");
                }
            }
        }

        // Evento para el botón "Confirmar selección"
        // Evento para confirmar la selección de asientos y pasar a la siguiente ventana
        private void ConfirmSelection_Click(object sender, RoutedEventArgs e)
        {
            // Si no se han seleccionado el número máximo de asientos, mostramos un mensaje
            if (_selectedSeats.Count != _maxSelectableSeats)
            {
                MessageBox.Show($"Debes seleccionar {_maxSelectableSeats} asientos.");
                return;
            }

            // Pasa los datos a la siguiente ventana (TicketCategorySelectionWindow o PaymentWindow)
            TicketCategorySelectionWindow ticketCategorySelectionWindow = new TicketCategorySelectionWindow(_selectedMovie, _selectedTime, _maxSelectableSeats, _selectedSeats);
            ticketCategorySelectionWindow.Show();
            this.Close(); // Cierra la ventana actual
        }


        // Método para guardar la selección de asientos en la base de datos
        private void SaveReservationToDatabase()
        {
            string connectionString = "Server=dam.clp4c2egnzkb.us-east-1.rds.amazonaws.com; Database=cine; User ID=admin;Password=MagicCut;";
            string query = "INSERT INTO reservas (pelicula, hora_funcion, asientos, cantidad_entradas) VALUES (@pelicula, @hora_funcion, @asientos, @cantidad_entradas)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@pelicula", _selectedMovie.Title);
                    command.Parameters.AddWithValue("@hora_funcion", _selectedTime);
                    command.Parameters.AddWithValue("@asientos", string.Join(",", _selectedSeats)); // Convertir asientos seleccionados a cadena
                    command.Parameters.AddWithValue("@cantidad_entradas", _selectedSeats.Count);

                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al guardar la reserva: {ex.Message}", "Error");
                }
            }
        }

        // Evento para el botón "Volver"
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            TicketSelectionWindow ticketSelectionWindow = new TicketSelectionWindow(_selectedMovie, "UsernamePlaceholder");
            ticketSelectionWindow.Show();
            this.Close();
        }
    }
}
