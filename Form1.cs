using System;
using System.Drawing;
using System.Windows.Forms;

namespace Proyecto3Datos1_
{
    public partial class Form1 : Form
    {
        private int[,] mapa;
        private int tamaño = 20; // Tamaño reducido de la matriz
        private List<Point> aeropuertos = new List<Point>();
        private List<Point> portaviones = new List<Point>();

        public Form1()
        {
            InitializeComponent();

            // Establece el tamaño del formulario para ver la matriz completa
            this.ClientSize = new Size(620, 620); // Ajusta el tamaño del formulario

            InicializarMatriz();
            GenerarPuntosImportantes();
            DibujarMatriz();
        }

        private void InicializarMatriz()
        {
            mapa = new int[tamaño, tamaño];
            Random random = new Random();
            int tierraAncho = tamaño / 2; // Ancho de la zona de tierra

            // Generar una zona central de tierra con forma irregular
            for (int i = 0; i < tamaño; i++)
            {
                for (int j = 0; j < tamaño; j++)
                {
                    // Generar tierra aleatoria en una forma irregular
                    if (i >= tamaño / 4 && i <= 3 * tamaño / 4 && j >= tamaño / 4 && j <= 3 * tamaño / 4)
                    {
                        // Usar ruido aleatorio para decidir si es tierra (1) o agua (0)
                        mapa[i, j] = random.Next(0, 4) == 0 ? 0 : 1; // 1/4 de probabilidad de ser agua
                    }
                    else
                    {
                        mapa[i, j] = 0; // Agua en los bordes
                    }
                }
            }
        }



        private void GenerarPuntosImportantes()
        {
            Random random = new Random();
            int numAeropuertos = 5; // Número de aeropuertos
            int numPortaviones = 5; // Número de portaviones
            int distanciaMinima = 6;

            // Generar ubicaciones de aeropuertos (en tierra)
            for (int i = 0; i < numAeropuertos; i++)
            {
                Point ubicacion;
                do
                {
                    int x = random.Next(0, tamaño);
                    int y = random.Next(0, tamaño);
                    ubicacion = new Point(x, y);
                }
                while (mapa[ubicacion.X, ubicacion.Y] != 1 ||
                       aeropuertos.Contains(ubicacion) ||
                       EstaADistancia(ubicacion, aeropuertos, distanciaMinima)); // Solo en tierra y sin repetir

                aeropuertos.Add(ubicacion);
            }

            // Generar ubicaciones de portaviones (en mar)
            for (int i = 0; i < numPortaviones; i++)
            {
                Point ubicacion;
                do
                {
                    int x = random.Next(0, tamaño);
                    int y = random.Next(0, tamaño);
                    ubicacion = new Point(x, y);
                }
                while (mapa[ubicacion.X, ubicacion.Y] != 0 ||
                       portaviones.Contains(ubicacion) ||
                       EstaADistancia(ubicacion, portaviones, distanciaMinima) ||
                       EstaADistancia(ubicacion, aeropuertos, distanciaMinima)); // Solo en mar y sin repetir

                portaviones.Add(ubicacion);
            }
        }

        // Método para verificar si hay elementos adyacentes en una lista
        private bool EstaADistancia(Point punto, List<Point> lista, int distanciaMinima)
        {
            foreach (Point p in lista)
            {
                // Calcular la distancia Manhattan
                int distancia = Math.Abs(punto.X - p.X) + Math.Abs(punto.Y - p.Y);
                if (distancia < distanciaMinima)
                {
                    return true; // Hay un punto dentro de la distancia mínima
                }
            }
            return false; // No hay puntos dentro de la distancia mínima
        }


        private void DibujarMatriz()
        {
            int tamañoCelda = 30;
            Panel[,] paneles = new Panel[tamaño, tamaño];

            // Crear la matriz visual de paneles
            for (int i = 0; i < tamaño; i++)
            {
                for (int j = 0; j < tamaño; j++)
                {
                    Panel panel = new Panel
                    {
                        Size = new Size(tamañoCelda, tamañoCelda),
                        Location = new Point(j * tamañoCelda, i * tamañoCelda),
                        BackColor = mapa[i, j] == 0 ? Color.Blue : Color.Green
                    };

                    paneles[i, j] = panel;
                    this.Controls.Add(panel);
                }
            }

            // Colorear aeropuertos en gris
            foreach (Point p in aeropuertos)
            {
                paneles[p.X, p.Y].BackColor = Color.Gray; // Color de aeropuerto
            }

            // Colorear portaviones en azul oscuro
            foreach (Point p in portaviones)
            {
                paneles[p.X, p.Y].BackColor = Color.Black; // Color de portavión
            }
        }



    }
}
