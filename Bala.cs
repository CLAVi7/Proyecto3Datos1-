using System.Drawing;
using System.Windows.Forms;

namespace Proyecto3Datos1_
{
    public class Bala
    {
        private Panel[,] paneles;
        private Color[,] coloresOriginales;
        public int X { get; private set; }
        public int Y { get; private set; }
        private int velocidad;
        private System.Windows.Forms.Timer timer; // Especifica el espacio de nombres completo

        public Bala(Panel[,] paneles, Color[,] coloresOriginales, int xInicial, int yInicial, int velocidad)
        {
            this.paneles = paneles;
            this.coloresOriginales = coloresOriginales;
            X = xInicial;
            Y = yInicial;
            this.velocidad = velocidad;

            // Configurar el timer para mover la bala en cada tick
            timer = new System.Windows.Forms.Timer(); // Usa el espacio de nombres completo aquí también
            timer.Interval = velocidad;
            timer.Tick += (sender, e) => Mover();
            timer.Start();

            ActualizarPosicion();
        }

        private void Mover()
        {
            // Limpiar la posición actual
            LimpiarPosicionActual();

            // Mover hacia arriba
            Y--;

            // Verificar si la bala ha salido de la matriz
            if (Y < 0)
            {
                timer.Stop();
                timer.Dispose();
                return;
            }

            // Actualizar la nueva posición
            ActualizarPosicion();
        }

        private void ActualizarPosicion()
        {
            if (Y >= 0)
            {
                paneles[Y, X].BackColor = Color.Yellow; // Representa la bala en amarillo
            }
        }

        private void LimpiarPosicionActual()
        {
            // Verificar que la posición es válida antes de restaurar el color
            if (Y >= 0 && Y < paneles.GetLength(0) && X >= 0 && X < paneles.GetLength(1))
            {
                // Restaurar el color original desde la matriz de colores originales
                paneles[Y, X].BackColor = coloresOriginales[Y, X];
            }
        }
    }
}

