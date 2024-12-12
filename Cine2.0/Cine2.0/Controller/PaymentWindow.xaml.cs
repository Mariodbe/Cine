using System;
using System.Collections.Generic;
using System.Windows;
using MySql.Data.MySqlClient;

namespace CineApp.View
{
    public partial class PaymentWindow : Window
    {
        private decimal _totalPrice;
        private Movie _selectedMovie;
        private string _selectedTime;
        private int _selectedSeatsCount;
        private HashSet<string> _selectedSeats; // Almacena los asientos seleccionados

        // Constructor que recibe los parámetros necesarios, incluyendo los asientos seleccionados
        public PaymentWindow(decimal totalPrice, Movie selectedMovie, string selectedTime, int selectedSeatsCount, HashSet<string> selectedSeats)
        {
            InitializeComponent();
            _totalPrice = totalPrice;
            _selectedMovie = selectedMovie;
            _selectedTime = selectedTime;
            _selectedSeatsCount = selectedSeatsCount;
            _selectedSeats = selectedSeats; // Asignar la colección de asientos seleccionados
            TotalPriceTextBlock.Text = $"Total a pagar: {_totalPrice}€";
        }

        // Evento de confirmación de pago
        private void ConfirmPayment_Click(object sender, RoutedEventArgs e)
        {
            string cardNumber = CardNumberTextBox.Text;
            string cvv = CVVTextBox.Text;
            string expiryDate = ExpiryDateTextBox.Text;

            if (IsValidCardDetails(cardNumber, cvv, expiryDate))
            {
                if (IsCardInDatabase(cardNumber, cvv, expiryDate))
                {
                    // Guardar los datos en la base de datos si el pago es exitoso
                    SaveReservationToDatabase();

                    // Mensaje de éxito
                    MessageBox.Show($"Pago realizado con éxito por un total de {_totalPrice}€", "Pago confirmado");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("La tarjeta no es válida o no está registrada en nuestra base de datos.", "Error de pago");
                }
            }
            else
            {
                MessageBox.Show("Los datos de la tarjeta son inválidos. Intenta nuevamente.", "Error de pago");
            }
        }

        // Método para guardar la reserva en la base de datos
        private void SaveReservationToDatabase()
        {
            if (_selectedSeats == null || _selectedSeats.Count == 0)
            {
                MessageBox.Show("No hay asientos seleccionados para guardar.", "Error");
                return;
            }

            // Verificar los valores de los asientos seleccionados antes de guardarlos
            foreach (var seat in _selectedSeats)
            {
                // Asegurarse de que el formato del asiento es correcto
                if (!IsValidSeatFormat(seat))
                {
                    MessageBox.Show($"El formato del asiento '{seat}' no es válido.", "Error");
                    return;
                }

                // Depuración: Verificar cada asiento seleccionado antes de la inserción
                MessageBox.Show($"Asiento seleccionado: {seat}");
            }

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
                    command.Parameters.AddWithValue("@cantidad_entradas", _selectedSeats.Count); // Usar el conteo de los asientos seleccionados

                    command.ExecuteNonQuery();

                    // Mensaje de éxito
                    MessageBox.Show("Reserva guardada exitosamente.", "Éxito");

                    // Actualizar los asientos a "ocupado"
                    foreach (var seat in _selectedSeats)
                    {
                        string updateSeatQuery = "UPDATE asientos SET estado = 1 WHERE pelicula_id = @peliculaId AND hora_funcion = @horaFuncion AND asiento = @asiento";
                        MySqlCommand updateCommand = new MySqlCommand(updateSeatQuery, connection);
                        updateCommand.Parameters.AddWithValue("@peliculaId", _selectedMovie.Id);
                        updateCommand.Parameters.AddWithValue("@horaFuncion", _selectedTime);
                        updateCommand.Parameters.AddWithValue("@asiento", seat);

                        updateCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al guardar la reserva: {ex.Message}", "Error");
                }
            }
        }

        // Método adicional para verificar que el formato del asiento es válido (ej. "1-1", "2-3")
        private bool IsValidSeatFormat(string seat)
        {
            string[] parts = seat.Split('-');
            if (parts.Length != 2) return false;

            // Verifica que ambos componentes sean números y que estén en un rango razonable
            if (int.TryParse(parts[0], out int row) && int.TryParse(parts[1], out int col))
            {
                return row > 0 && col > 0; // Aseguramos que el asiento tenga valores mayores a 0
            }

            return false;
        }

        // Método para verificar los detalles de la tarjeta
        private bool IsValidCardDetails(string cardNumber, string cvv, string expiryDate)
        {
            if (string.IsNullOrEmpty(cardNumber) || string.IsNullOrEmpty(cvv) || string.IsNullOrEmpty(expiryDate))
            {
                return false;
            }

            // Validación del número de tarjeta (16 dígitos)
            if (cardNumber.Length != 16 || !long.TryParse(cardNumber, out _))
            {
                MessageBox.Show("El número de tarjeta debe tener 16 dígitos.");
                return false;
            }

            // Validación del CVV (3 dígitos)
            if (cvv.Length != 3 || !int.TryParse(cvv, out _))
            {
                MessageBox.Show("El CVV debe tener 3 dígitos.");
                return false;
            }

            // Validación de la fecha de vencimiento (formato MM/AA)
            if (!DateTime.TryParseExact(expiryDate, "MM/yy", null, System.Globalization.DateTimeStyles.None, out DateTime expiry))
            {
                MessageBox.Show("La fecha de vencimiento debe estar en formato MM/AA.");
                return false;
            }

            if (expiry < DateTime.Now) // Verifica si la tarjeta está vencida
            {
                MessageBox.Show("La tarjeta está expirada.");
                return false;
            }

            return true;
        }

        // Método para verificar si la tarjeta está registrada en la base de datos
        private bool IsCardInDatabase(string cardNumber, string cvv, string expiryDate)
        {
            string connectionString = "Server=dam.clp4c2egnzkb.us-east-1.rds.amazonaws.com; Database=cine; User ID=admin;Password=MagicCut;";
            string query = "SELECT * FROM tarjetas WHERE numero_tarjeta = @cardNumber AND cvv = @cvv AND fecha_vencimiento = @expiryDate";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@cardNumber", cardNumber);
                    command.Parameters.AddWithValue("@cvv", cvv);
                    command.Parameters.AddWithValue("@expiryDate", expiryDate);

                    MySqlDataReader reader = command.ExecuteReader();

                    // Si se encuentra una tarjeta en la base de datos
                    return reader.HasRows;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error de conexión a la base de datos: {ex.Message}", "Error");
                    return false;
                }
            }
        }

        // Evento de cierre de ventana (sin pagar)
        private void CancelPayment_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Cierra la ventana sin realizar el pago
        }

        // Evento para volver atrás a la ventana de TicketCategorySelection
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Crear un HashSet vacío para los asientos seleccionados
            HashSet<string> selectedSeats = new HashSet<string>();

            // Redirigir a la ventana de selección de categoría de tickets, pasando los parámetros necesarios
            TicketCategorySelectionWindow ticketCategorySelectionWindow = new TicketCategorySelectionWindow(_selectedMovie, _selectedTime, _selectedSeatsCount, selectedSeats);
            ticketCategorySelectionWindow.Show();
            this.Close();  // Cierra la ventana de pago
        }
    }
}
