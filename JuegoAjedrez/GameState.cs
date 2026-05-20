using JuegoAjedrez.AjedrezConsola;
using System;
using System.Collections.Generic;
using System.Text;

namespace JuegoAjedrez
{
    public class GameState
    {
        // Matriz bidimensional de 8x8 que hereda la lógica del tablero del video
        public string[,] Tablero { get; private set; } = new string[8, 8];
        public Player CurrentPlayer { get; private set; }

        public GameState()
        {
            CurrentPlayer = Player.Blanco; // Inician las blancas
            InicializarTablero();
        }

        private void InicializarTablero()
        {
             
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    Tablero[i, j] = "  ";

            
            Tablero[0, 0] = "tn"; Tablero[0, 1] = "cn"; Tablero[0, 2] = "an"; Tablero[0, 3] = "dn";
            Tablero[0, 4] = "rn"; Tablero[0, 5] = "an"; Tablero[0, 6] = "cn"; Tablero[0, 7] = "tn";
            for (int j = 0; j < 8; j++) Tablero[1, j] = "pn";

             
            for (int j = 0; j < 8; j++) Tablero[6, j] = "PB";
            Tablero[7, 0] = "TB"; Tablero[7, 1] = "CB"; Tablero[7, 2] = "AB"; Tablero[7, 3] = "DB";
            Tablero[7, 4] = "RB"; Tablero[7, 5] = "AB"; Tablero[7, 6] = "CB"; Tablero[7, 7] = "TB";
        }

        public bool RealizarMovimiento(Position desde, Position hasta)
        {
            string pieza = Tablero[desde.Fila, desde.Columna];
            string piezaDestino = Tablero[hasta.Fila, hasta.Columna];

          
            if (pieza == "  ") return false; 
            if (desde.Fila == hasta.Fila && desde.Columna == hasta.Columna) return false; // No se movió

            // Validar turnos
            if (CurrentPlayer == Player.Blanco && pieza.EndsWith("n")) return false;
            if (CurrentPlayer == Player.Negro && pieza.EndsWith("B")) return false;

            // 2. REGLA DE CAPTURA (¿Se pueden comer?)
            if (piezaDestino != "  ")
            {
                // Si la pieza del destino es del mismo equipo (mismo sufijo 'B' o 'n'), es inválido
                if ((pieza.EndsWith("B") && piezaDestino.EndsWith("B")) ||
                    (pieza.EndsWith("n") && piezaDestino.EndsWith("n")))
                {
                    return false; // No puedes comer de tu propio equipo
                }
            }

            // 3. REGLAS DE MOVIMIENTO REALES POR PIEZA

            // --- LÓGICA DEL PEÓN BLANCO ---
            if (pieza == "PB")
            {
                int avanceFila = desde.Fila - hasta.Fila;
                int avanceCol = hasta.Columna - desde.Columna;

                // Caso A: Movimiento recto hacia adelante (Solo si la casilla destino está VACÍA)
                if (avanceCol == 0 && piezaDestino == "  ")
                {
                    if (desde.Fila == 6 && (avanceFila == 1 || avanceFila == 2)) { /* Válido */ }
                    else if (desde.Fila != 6 && avanceFila == 1) { /* Válido */ }
                    else return false;
                }
                // Caso B: Movimiento diagonal para COMER (Solo si hay una pieza enemiga)
                else if (Math.Abs(avanceCol) == 1 && avanceFila == 1 && piezaDestino != "  ")
                {
                    // Es válido porque ya validamos arriba que el destino es enemigo
                }
                else
                {
                    return false; // Cualquier otro movimiento de peón es ilegal
                }
            }

            // --- LÓGICA DE LA TORRE (Blanca o Negra) ---
            if (pieza == "TB" || pieza == "tn")
            {
                // La torre solo se mueve si la fila se mantiene igual O la columna se mantiene igual
                if (desde.Fila != hasta.Fila && desde.Columna != hasta.Columna)
                    return false;

                // Validar que no haya piezas estorbando en el camino (Fila)
                if (desde.Fila == hasta.Fila)
                {
                    int paso = desde.Columna < hasta.Columna ? 1 : -1;
                    for (int c = desde.Columna + paso; c != hasta.Columna; c += paso)
                        if (Tablero[desde.Fila, c] != "  ") return false; // Camino bloqueado
                }
                // Validar que no haya piezas estorbando en el camino (Columna)
                else if (desde.Columna == hasta.Columna)
                {
                    int paso = desde.Fila < hasta.Fila ? 1 : -1;
                    for (int f = desde.Fila + paso; f != hasta.Fila; f += paso)
                        if (Tablero[f, desde.Columna] != "  ") return false; // Camino bloqueado
                }
            }

            // 4. EJECUCIÓN DEL MOVIMIENTO EN LA MATRIZ
            // Al sobreescribir 'pieza' sobre 'piezaDestino', la pieza enemiga "muere" automáticamente de la memoria
            Tablero[hasta.Fila, hasta.Columna] = pieza;
            Tablero[desde.Fila, desde.Columna] = "  ";

            // Cambiar de turno
            CurrentPlayer = CurrentPlayer == Player.Blanco ? Player.Negro : Player.Blanco;
            return true;
        }
    }
}
