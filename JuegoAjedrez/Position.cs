using System;
using System.Collections.Generic;
using System.Text;

namespace JuegoAjedrez
{
    public class Position
    {
        public int Fila { get; set; }
        public int Columna { get; set; }

        public Position(int fila, int columna)
        {
            Fila = fila;
            Columna = columna;
        }
    }
}
