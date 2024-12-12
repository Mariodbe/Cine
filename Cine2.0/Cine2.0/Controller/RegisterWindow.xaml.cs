using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace CineApp
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent(); // Inicializa los elementos definidos en XAML.
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim(); // Recupera el texto del cuadro de usuario.
            string password = PasswordBox.Password.Trim(); // Recupera la contraseña.

            // Validación para el nombre de usuario.
            if (!IsValidUsername(username))
            {
                MessageBox.Show("El nombre de usuario debe tener al menos 3 letras y solo caracteres alfabéticos (incluyendo ñ y acentos).");
                return;
            }

            // Validación para la contraseña.
            if (password.Length < 4)
            {
                MessageBox.Show("La contraseña debe tener al menos 4 caracteres.");
                return;
            }

            // Intentar registrar al usuario en la base de datos.
            if (Database.RegisterUser(username, password))
            {
                MessageBox.Show("Usuario registrado exitosamente");
                this.Close(); // Cierra la ventana actual.
            }
            else
            {
                MessageBox.Show("Error al registrar el usuario");
            }
        }

        private bool IsValidUsername(string username)
        {
            // Expresión regular actualizada para admitir caracteres alfabéticos Unicode, incluyendo ñ y acentos.
            return Regex.IsMatch(username, @"^[\p{L}]{3,}$");
        }
    }
}
