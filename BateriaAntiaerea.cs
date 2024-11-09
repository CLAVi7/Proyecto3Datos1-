namespace Proyecto3Datos1_
{
    public class BateriaAntiaerea
    {
        private Panel[,] paneles;
        private int x, y; // Posici�n actual de la bater�a en la matriz

        public BateriaAntiaerea(Panel[,] paneles, int xInicial, int yInicial)
        {
            this.paneles = paneles;
            this.x = xInicial;
            this.y = yInicial;
            ActualizarPosicion();
        }

        public void MoverIzquierda()
        {
            if (x > 0) // Asegurarse de que no se salga del l�mite izquierdo
            {
                LimpiarPosicionActual();
                x--;
                ActualizarPosicion();
            }
        }

        public void MoverDerecha()
        {
            if (x < paneles.GetLength(1) - 1) // Asegurarse de que no se salga del l�mite derecho
            {
                LimpiarPosicionActual();
                x++;
                ActualizarPosicion();
            }
        }

        private void ActualizarPosicion()
        {
            paneles[y, x].BackColor = Color.Red; // Actualizar la celda actual para que sea roja (indica la bater�a)
        }

        private void LimpiarPosicionActual()
        {
            paneles[y, x].BackColor = Color.Blue; // Restaurar el color de fondo anterior (ej., agua)
        }
    }


}


