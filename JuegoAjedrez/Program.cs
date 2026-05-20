using System;

namespace JuegoAjedrez
{
    class Program
    {
        static void Main(string[] args)
        {
            GameState juego = new GameState();

            while (true)
            {
                Console.Clear();
                DibujarTableroConsola(juego.Tablero);

                Console.WriteLine($"\nTurno de las piezas: {juego.CurrentPlayer}");
                Console.WriteLine("Introduce el movimiento (Formato: fila_origen,col_origen a fila_destino,col_destino)");
                Console.WriteLine("Ejemplo: 6,4 a 4,4 (Mueve peón adelante) o escribe 'salir':");

                string entrada = Console.ReadLine();
                if (entrada.ToLower() == "salir") break;

                try
                {
                    // Procesar la entrada de texto
                    string[] partes = entrada.Split(" a ");
                    string[] origen = partes[0].Split(',');
                    string[] destino = partes[1].Split(',');

                    Position desde = new Position(int.Parse(origen[0]), int.Parse(origen[1]));
                    Position hasta = new Position(int.Parse(destino[0]), int.Parse(destino[1]));

                    if (!juego.RealizarMovimiento(desde, hasta))
                    {
                        Console.WriteLine("Movimiento inválido (revisa el turno o la casilla elegida). Presiona una tecla...");
                        Console.ReadKey();
                    }
                }
                catch
                {
                    Console.WriteLine("Error de formato. Usa exactamente: F,C a F,C. Presiona una tecla...");
                    Console.ReadKey();
                }
            }
        }

        // Lee la matriz del juego y la dibuja usando colores nativos de la consola
        static void DibujarTableroConsola(string[,] tablero)
        {
            Console.WriteLine("   0    1    2    3    4    5    6    7  (Columnas)");
            Console.WriteLine(" ┌────┬────┬────┬────┬────┬────┬────┬────┐");
            for (int i = 0; i < 8; i++)
            {
                Console.Write(i + "│"); // Índice de fila
                for (int j = 0; j < 8; j++)
                {
                    // Simular el color de las casillas claras y oscuras
                    if ((i + j) % 2 == 0) Console.BackgroundColor = ConsoleColor.DarkGray;
                    else Console.BackgroundColor = ConsoleColor.Black;

                    Console.Write($" {tablero[i, j]} ");
                    Console.BackgroundColor = ConsoleColor.Black; // Resetear
                    Console.Write("│");
                }
                Console.WriteLine();
                if (i < 7) Console.WriteLine(" ├────┼────┼────┼────┼────┼────┼────┼────┤");
            }
            Console.WriteLine(" └────┴────┴────┴────┴────┴────┴────┴────┘");
        }
    }
}