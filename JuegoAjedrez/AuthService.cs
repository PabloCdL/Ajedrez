using System;
using System.Collections.Generic;
using System.Text;

namespace JuegoAjedrez.AjedrezConsola
{
    public class AuthService
    {
        private List<Usuario> listaUsuarios = new List<Usuario>
        {
            new Usuario {Nombre="Majo", Password="Majito_06", Punteo = 0},
            new Usuario {Nombre="Julian", Password="Juliancito_09", Punteo = 0},
            new Usuario {Nombre="Cristopher", Password="Cris_010", Punteo = 0},
            new Usuario {Nombre="Invitado", Password="Jugador_1234", Punteo = 0},
        };

    
        public Usuario IniciarSesion()
        {
            int intentos = 3;

            while (intentos > 0)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("========================================");
                Console.WriteLine("       SISTEMA DE ACCESO - AJEDREZ      ");
                Console.WriteLine("========================================\n");
                Console.ResetColor();

                Console.Write("Ingrese su usuario: ");
                string nombreInput = Console.ReadLine();

                Usuario usuarioBuscado = listaUsuarios.Find(us => us.Nombre.Equals(nombreInput, StringComparison.OrdinalIgnoreCase));

                if (usuarioBuscado != null)
                {
                    if (ValidarContrasena(usuarioBuscado, ref intentos))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n¡Acceso permitido! Bienvenido " + usuarioBuscado.Nombre);
                        Console.ResetColor();
                        Console.WriteLine("Presione una tecla para ir al menú principal...");
                        Console.ReadKey();
                        return usuarioBuscado; 
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Usuario no encontrado.");
                    intentos--;
                    Console.WriteLine($"Intentos restantes: {intentos}");
                    Console.ResetColor();
                    Console.ReadKey();
                }
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nAcceso denegado. Demasiados intentos fallidos.");
            Console.ResetColor();
            return null;
        }

        private bool ValidarContrasena(Usuario usuario, ref int intentos)
        {
            while (intentos > 0)
            {
                Console.Write("Ingrese la contraseña: ");
                string pass = LeerContrasenaOculta();
                Console.WriteLine();

                if (pass == usuario.Password)
                {
                    return true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Contraseña incorrecta...");
                    intentos--;
                    Console.WriteLine("Intentos restantes: " + intentos);
                    Console.ResetColor();
                    Console.ReadKey();
                    Console.Clear();
                    
                    Console.WriteLine($"Usuario seleccionado: {usuario.Nombre}");
                }
            }
            return false;
        }
         
        private string LeerContrasenaOculta()
        {
            string pass = "";
            ConsoleKeyInfo tecla;
            do
            {
                tecla = Console.ReadKey(intercept: true);
                switch (tecla.Key)
                {
                    case ConsoleKey.Backspace:
                        if (pass.Length > 0)
                        {
                            Console.Write("\b \b");
                            pass = pass.Remove(pass.Length - 1);
                        }
                        break;
                    case ConsoleKey.Enter:
                        break;
                    default:
                        if (!char.IsControl(tecla.KeyChar))
                        {
                            Console.Write("*");
                            pass += tecla.KeyChar;
                        }
                        break;
                }
            } while (tecla.Key != ConsoleKey.Enter);
            return pass;
        }
    }
}
