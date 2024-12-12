using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

namespace CineApp.View
{
    public partial class TicketCategorySelectionWindow : Window
    {
        private Movie _selectedMovie;
        private string _selectedTime;
        private int _selectedSeatsCount;
        private HashSet<string> _selectedSeats;

        // Constructor que recibe los parámetros de la película, la hora y los asientos seleccionados
        public TicketCategorySelectionWindow(Movie selectedMovie, string selectedTime, int selectedSeatsCount, HashSet<string> selectedSeats)
        {
            InitializeComponent();
            _selectedMovie = selectedMovie;
            _selectedTime = selectedTime;
            _selectedSeatsCount = selectedSeatsCount;
            _selectedSeats = selectedSeats;
        }

        private (decimal totalPrice, int totalTickets) CalculateTotalPriceAndTickets()
        {
            decimal totalPrice = 0;
            int totalTickets = 0;

            // Calcular el precio total y la cantidad de tickets por tipo de entrada
            totalPrice += CalculateTicketPrice(GeneralTicketsComboBox, ref totalTickets);
            totalPrice += CalculateTicketPrice(Under13TicketsComboBox, ref totalTickets);
            totalPrice += CalculateTicketPrice(Over65TicketsComboBox, ref totalTickets);

            return (totalPrice, totalTickets);
        }

        private decimal CalculateTicketPrice(ComboBox comboBox, ref int totalTickets)
        {
            decimal price = 0;
            // Verificar que hay un item seleccionado en el ComboBox
            if (comboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                price = decimal.Parse(selectedItem.Tag.ToString());
                totalTickets += int.Parse(selectedItem.Content.ToString());
            }
            return price;
        }

        private void ComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var (totalPrice, totalTickets) = CalculateTotalPriceAndTickets();

            TotalPriceTextBlock.Text = $"Precio total: {totalPrice}€";

            if (totalTickets != _selectedSeatsCount)
            {
                MessageBox.Show($"Debes seleccionar {_selectedSeatsCount} entradas, que corresponde al número de asientos seleccionados.");
            }
        }

        private void ConfirmSelection_Click(object sender, RoutedEventArgs e)
        {
            var (totalPrice, totalTickets) = CalculateTotalPriceAndTickets();

            if (totalTickets != _selectedSeatsCount)
            {
                MessageBox.Show($"Debes seleccionar {_selectedSeatsCount} entradas, que corresponde al número de asientos seleccionados.");
                return;
            }

            // Crear la ventana de pago con los asientos seleccionados
            PaymentWindow paymentWindow = new PaymentWindow(totalPrice, _selectedMovie, _selectedTime, _selectedSeatsCount, _selectedSeats);
            paymentWindow.Show();
            this.Close();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Volver a SeatSelectionWindow pasando los datos
            SeatSelectionWindow seatSelectionWindow = new SeatSelectionWindow(_selectedMovie, _selectedTime, _selectedSeatsCount, _selectedSeats);
            seatSelectionWindow.Show();
            this.Close();
        }
    }
}
