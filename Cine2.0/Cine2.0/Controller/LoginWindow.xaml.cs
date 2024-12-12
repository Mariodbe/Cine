using System;
using System.Windows;
using CineApp;

namespace CineApp.View
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            // Validar las credenciales usando la clase Database
            if (Database.ValidateLogin(username, password))
            {
                // Si las credenciales son correctas, abrir la ventana de selección de películas
                MovieSelectionWindow movieSelectionWindow = new MovieSelectionWindow(username);
                movieSelectionWindow.Show();

                // Cerrar la ventana de inicio de sesión
                this.Close();
            }
            else
            {
                // Si las credenciales no son correctas, mostrar un mensaje de error
                MessageBox.Show("Nombre de usuario o contraseña incorrectos", "Error de inicio de sesión", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            // Llamada a la ventana de registro, para que el usuario pueda crear una nueva cuenta
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.Show();
            this.Close();
        }
    }
}
