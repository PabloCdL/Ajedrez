using System;

namespace JuegoAjedrez.AjedrezConsola
{
    class Program
    {
        static void Main(string[] args)
        {
          
            AuthService auth = new AuthService();

            Usuario usuarioLogueado = auth.IniciarSesion();

         
            if (usuarioLogueado == null)
            {
                return;
            }

            bool ejecutandoMenu = true;
            while (ejecutandoMenu)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("========================================");
                Console.WriteLine($"   MENÚ PRINCIPAL - JUGADOR: {usuarioLogueado.Nombre.ToUpper()} ");
                Console.WriteLine("========================================");
                Console.ResetColor();
                Console.WriteLine("1. Iniciar Partida");
                Console.WriteLine("2. Ver Puntaje (Récord)");
                Console.WriteLine("3. Ver Reglas del Juego");
                Console.WriteLine("4. Salir");
                Console.Write("\nSeleccione una opción (1-4): ");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        
                        BuclePartidaAjedrez(usuarioLogueado);
                        break;

                    case "2":
                    
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("========================================");
                        Console.WriteLine("          REPORTE DE RENDIMIENTO        ");
                        Console.WriteLine("========================================");
                        Console.ResetColor();
                        Console.WriteLine($"Jugador activo: {usuarioLogueado.Nombre}");
                        Console.WriteLine($"Puntaje más alto registrado: {usuarioLogueado.Punteo} pts");
                        Console.WriteLine("\nPresione cualquier tecla para regresar al menú...");
                        Console.ReadKey();
                        break;

                    case "3":
                        
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("========================================");
                        Console.WriteLine("        REGLAS BÁSICAS DEL JUEGO        ");
                        Console.WriteLine("========================================");
                        Console.ResetColor();
                        Console.WriteLine("1. El bando Blanco ('B') siempre inicia la partida.");
                        Console.WriteLine("2. Formato de entrada: Fila,Columna a Fila,Columna (Ej: 6,4 a 4,4).");
                        Console.WriteLine("3. Cada pieza enemiga eliminada te sumará 10 puntos.");
                        Console.WriteLine("4. Capturar al Rey enemigo te dará 50 puntos adicionales (60 en total) y ganará la partida.");
                        Console.WriteLine("5. Para abandonar una partida y regresar al menú, escribe 'salir'.");
                        Console.WriteLine("\nPresione cualquier tecla para regresar al menú...");
                        Console.ReadKey();
                        break;

                    case "4":
                        Console.WriteLine("\n¡Gracias por jugar! Cerrando sistema...");
                        ejecutandoMenu = false;
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nOpción no válida. Intente de nuevo.");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                }
            }
        }


        static void BuclePartidaAjedrez(Usuario usuarioLogueado)
        {
            GameState juego = new GameState();
            bool intentoCapturaRey = false;

            while (true)
            {
                Console.Clear();
                DibujarTableroConsola(juego.Tablero);

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\nPUNTAJE OBTENIDO: {juego.PuntajePartida} pts");
                Console.ResetColor();

                Console.WriteLine($"Turno de las piezas: {juego.CurrentPlayer}");
                Console.WriteLine("Introduce el movimiento (Ejemplo sencillo: 7,2 6,1)");
                Console.Write("O escribe 'salir' para regresar al menú: ");

                string entrada = Console.ReadLine();
                if (entrada.ToLower().Trim() == "salir") break;

                try
                {
                     
                     
                    var numeros = System.Text.RegularExpressions.Regex.Matches(entrada, @"\d+");

                    if (numeros.Count != 4)
                    {
                        throw new Exception("Formato incorrecto");
                    }

                    int filaOrigen = int.Parse(numeros[0].Value);
                    int colOrigen = int.Parse(numeros[1].Value);
                    int filaDestino = int.Parse(numeros[2].Value);
                    int colDestino = int.Parse(numeros[3].Value);

                    Position desde = new Position(filaOrigen, colOrigen);
                    Position hasta = new Position(filaDestino, colDestino);

                     
                    string objetivo = juego.Tablero[hasta.Fila, hasta.Columna];
                    if (objetivo.ToLower().StartsWith("r") &&
                        ((juego.CurrentPlayer == Player.Blanco && objetivo.EndsWith("n")) ||
                         (juego.CurrentPlayer == Player.Negro && objetivo.EndsWith("B"))))
                    {
                        intentoCapturaRey = true;
                    }
                    else
                    {
                        intentoCapturaRey = false;
                    }

                     
                    bool movimientoExitoso = juego.RealizarMovimiento(desde, hasta);

                    if (!movimientoExitoso)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Movimiento inválido según las reglas. Presione una tecla...");
                        Console.ResetColor();
                        Console.ReadKey();
                    }
                    else
                    {
                         
                        if (intentoCapturaRey)
                        {
                            Console.Clear();
                            DibujarTableroConsola(juego.Tablero);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\n¡JAQUE MATE! El Rey ha sido capturado de forma legal. Victoria total.");
                            Console.ResetColor();
                            Console.ReadKey();
                            break;
                        }

                        continue;
                    }
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error de digitación. Intenta usar el formato simple: 7,2 6,1. Presiona una tecla...");
                    Console.ResetColor();
                    Console.ReadKey();
                }
            }

             
              
             
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("========================================");
            Console.WriteLine("          RESUMEN DE LA PARTIDA         ");
            Console.WriteLine("========================================");
            Console.ResetColor();
            Console.WriteLine($"Jugador: {usuarioLogueado.Nombre}");
            Console.WriteLine($"Puntos obtenidos: {juego.PuntajePartida} pts");
            Console.WriteLine($"Récord histórico anterior: {usuarioLogueado.Punteo} pts");
            Console.WriteLine("----------------------------------------");

            if (juego.PuntajePartida > usuarioLogueado.Punteo)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("¡NUEVO RÉCORD HISTÓRICO LOGRADO!");
                usuarioLogueado.Punteo = juego.PuntajePartida;
                Console.WriteLine($"Tu nuevo puntaje más alto es: {usuarioLogueado.Punteo} pts");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Buen juego, pero no lograste superar tu récord actual.");
            }

            Console.ResetColor();
            Console.WriteLine("\nPresione cualquier tecla para regresar al Menú Principal...");
            Console.ReadKey();
        }

         
        static void DibujarTableroConsola(string[,] tablero)
        {
            Console.WriteLine("   0    1    2    3    4    5    6    7  (Columnas)");
            Console.WriteLine(" ┌────┬────┬────┬────┬────┬────┬────┬────┐");
            for (int i = 0; i < 8; i++)
            {
                Console.Write(i + "│");
                for (int j = 0; j < 8; j++)
                {
                     
                    if ((i + j) % 2 == 0) Console.BackgroundColor = ConsoleColor.DarkGray;
                    else Console.BackgroundColor = ConsoleColor.Black;

                    Console.Write($" {tablero[i, j]} ");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write("│");
                }
                Console.WriteLine();
                if (i < 7) Console.WriteLine(" ├────┼────┼────┼────┼────┼────┼────┼────┤");
            }
            Console.WriteLine(" └────┴────┴────┴────┴────┴────┴────┴────┘");
        }
    }
}