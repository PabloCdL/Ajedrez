using JuegoAjedrez.AjedrezConsola;
using System;
using System.Collections.Generic;
using System.Text;

namespace JuegoAjedrez.AjedrezConsola
{
    public class GameState
    {
         
        public string[,] Tablero { get; private set; } = new string[8, 8];
        public Player CurrentPlayer { get; private set; }
        public int PuntajePartida { get; private set; } = 0;   

         
        public GameState()
        {
            CurrentPlayer = Player.Blanco;   
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
             
            if (desde.Fila < 0 || desde.Fila > 7 || desde.Columna < 0 || desde.Columna > 7 ||
                hasta.Fila < 0 || hasta.Fila > 7 || hasta.Columna < 0 || hasta.Columna > 7) return false;

            string pieza = Tablero[desde.Fila, desde.Columna];
            string piezaDestino = Tablero[hasta.Fila, hasta.Columna];

             
            if (pieza == "  ") return false;
            if (desde.Fila == hasta.Fila && desde.Columna == hasta.Columna) return false;

             
            if (CurrentPlayer == Player.Blanco && pieza.EndsWith("n")) return false;
            if (CurrentPlayer == Player.Negro && pieza.EndsWith("B")) return false;

             
            if (piezaDestino != "  ")
            {
                if ((pieza.EndsWith("B") && piezaDestino.EndsWith("B")) ||
                    (pieza.EndsWith("n") && piezaDestino.EndsWith("n"))) return false;
            }

             
            string tipoPieza = pieza.ToLower();
            bool movimientoValido = false;

            int difFila = hasta.Fila - desde.Fila;
            int difCol = hasta.Columna - desde.Columna;
            int absDifFila = Math.Abs(difFila);
            int absDifCol = Math.Abs(difCol);

            switch (tipoPieza[0])
            {
                case 'p':  
                    if (pieza == "PB")  
                    {
                        if (difCol == 0 && piezaDestino == "  ")
                        {
                            if (desde.Fila == 6 && (difFila == -1 || difFila == -2))
                            {
                                if (difFila == -2 && Tablero[5, desde.Columna] != "  ") movimientoValido = false;
                                else movimientoValido = true;
                            }
                            else if (difFila == -1) movimientoValido = true;
                        }
                        else if (absDifCol == 1 && difFila == -1 && piezaDestino != "  ") movimientoValido = true;
                    }
                    else  
                    {
                        if (difCol == 0 && piezaDestino == "  ")
                        {
                            if (desde.Fila == 1 && (difFila == 1 || difFila == 2))
                            {
                                if (difFila == 2 && Tablero[2, desde.Columna] != "  ") movimientoValido = false;
                                else movimientoValido = true;
                            }
                            else if (difFila == 1) movimientoValido = true;
                        }
                        else if (absDifCol == 1 && difFila == 1 && piezaDestino != "  ") movimientoValido = true;
                    }
                    break;

                case 't':  
                    if (desde.Fila == hasta.Fila || desde.Columna == hasta.Columna)
                        movimientoValido = ValidarCaminoLibreRecto(desde, hasta);
                    break;

                case 'a':  
                    if (absDifFila == absDifCol)
                        movimientoValido = ValidarCaminoLibreDiagonal(desde, hasta);
                    break;

                case 'd':  
                    if (desde.Fila == hasta.Fila || desde.Columna == hasta.Columna)
                        movimientoValido = ValidarCaminoLibreRecto(desde, hasta);
                    else if (absDifFila == absDifCol)
                        movimientoValido = ValidarCaminoLibreDiagonal(desde, hasta);
                    break;

                case 'r':  
                    if (absDifFila <= 1 && absDifCol <= 1) movimientoValido = true;
                    break;

                case 'c':  
                    if ((absDifFila == 2 && absDifCol == 1) || (absDifFila == 1 && absDifCol == 2))
                        movimientoValido = true;  
                    break;
            }
             
            if (!movimientoValido) return false;

             
            if (piezaDestino != "  ")
            {
                PuntajePartida += 10;  

                if (piezaDestino.ToLower().StartsWith("r"))
                {
                    PuntajePartida += 50;
                }
            }

             
            Tablero[hasta.Fila, hasta.Columna] = pieza;
            Tablero[desde.Fila, desde.Columna] = "  ";


            if (CurrentPlayer == Player.Blanco)
                CurrentPlayer = Player.Negro;
            else
                CurrentPlayer = Player.Blanco;

            return true;
        }

         
        private bool ValidarCaminoLibreRecto(Position desde, Position hasta)
        {
            if (desde.Fila == hasta.Fila)
            {
                int paso;
                if (desde.Columna < hasta.Columna)
                {
                    paso = 1;
                }
                else
                {
                    paso = -1;
                }

                for (int c = desde.Columna + paso; c != hasta.Columna; c += paso)
                {
                    if (Tablero[desde.Fila, c].Trim() != "") return false;
                }
            }
            else  
            {
                int paso;   
                if(desde.Fila < hasta.Fila )
                {
                    paso = 1;
                }
                else
                {
                    paso = -1;
                }
                 
                for (int f = desde.Fila + paso; f != hasta.Fila; f += paso)
                {
                    if (Tablero[f, desde.Columna].Trim() != "") return false;
                }
            }
            return true;
        }

        private bool ValidarCaminoLibreDiagonal(Position desde, Position hasta)
        {
            int pasoFila;
            if(desde.Fila < hasta.Fila)
            {
                pasoFila = 1;
            }else
            {
                pasoFila= -1;
            }

            int pasoCol;   
            if( desde.Columna < hasta.Columna)
            {
                pasoCol = 1;
            }
            else
            {
                pasoCol = -1;
            } 
            
            int f = desde.Fila + pasoFila;
            int c = desde.Columna + pasoCol;
              
            while (f != hasta.Fila && c != hasta.Columna)
            {
                if (Tablero[f, c].Trim() != "")
                {
                    return false;  
                }
                f += pasoFila;
                c += pasoCol;
            }
            return true;  
        }
    }  
}
