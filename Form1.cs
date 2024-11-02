using System;
using System.Drawing;
using System.Windows.Forms;


namespace Proyecto3Datos1_
{
    public partial class Form1 : Form
    {
        private int[,] mapa;
        private int tamaño = 20; // Tamaño de la matriz
        private List<Point> aeropuertos = new List<Point>();
        private List<Point> portaviones = new List<Point>();
        private Grafo grafo;
        
       

        public Form1()
        {
            InitializeComponent();
            int distanciaMaxima = 10;
            mapa = new int[tamaño, tamaño];
            grafo = new Grafo(distanciaMaxima);
           
            this.ClientSize = new Size(1020, 620);
            InicializarMatriz();
            GenerarPuntosImportantes();
            DibujarMatriz();
            
        }

        private void InicializarMatriz()
        {
            mapa = new int[tamaño, tamaño];
            Random random = new Random();
            // Ajusta estos valores para cambiar el tamaño de la zona de tierra
            int tierraInicioX = tamaño / 4 - 1;
            int tierraFinX = 3 * tamaño / 4 + 1;
            int tierraInicioY = tamaño / 4 - 1;
            int tierraFinY = 3 * tamaño / 4 + 1;

            for (int i = 0; i < tamaño; i++)
            {
                for (int j = 0; j < tamaño; j++)
                {
                    // Generar una zona central de tierra con forma algo irregular
                    if (i >= tierraInicioX && i <= tierraFinX && j >= tierraInicioY && j <= tierraFinY)
                    {
                        // Agregar aleatoriedad para que no sea un cuadrado perfecto
                        if (random.Next(0, 6) > 0)  // 3 de cada 4 casillas serán tierra
                        {
                            mapa[i, j] = 1; // Tierra
                        }
                        else
                        {
                            mapa[i, j] = 0; // Agua ocasional para borde irregular
                        }
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
            int numAeropuertos = 5;
            int numPortaviones = 5;
            int distanciaMinima = 6; // Distancia mínima entre puntos importantes

            List<Point> puntosImportantes = new List<Point>(); // Lista para almacenar todos los puntos importantes

            // Generar ubicaciones de aeropuertos en tierra
            for (int i = 0; i < numAeropuertos; i++)
            {
                Point ubicacion;
                do
                {
                    int x = random.Next(0, tamaño);
                    int y = random.Next(0, tamaño);
                    ubicacion = new Point(x, y);
                }
                // Verificar que el punto está en tierra, no repetido y cumple la distancia mínima con todos los puntos importantes
                while (mapa[ubicacion.X, ubicacion.Y] != 1 || EstaADistancia(ubicacion, puntosImportantes, distanciaMinima));

                aeropuertos.Add(ubicacion);
                puntosImportantes.Add(ubicacion); // Agregar a la lista de puntos importantes
                grafo.AgregarNodo(ubicacion);
            }

            // Generar ubicaciones de portaviones en el agua
            for (int i = 0; i < numPortaviones; i++)
            {
                Point ubicacion;
                do
                {
                    int x = random.Next(0, tamaño);
                    int y = random.Next(0, tamaño);
                    ubicacion = new Point(x, y);
                }
                // Verificar que el punto está en agua, no repetido y cumple la distancia mínima con todos los puntos importantes
                while (mapa[ubicacion.X, ubicacion.Y] != 0 || EstaADistancia(ubicacion, puntosImportantes, distanciaMinima));

                portaviones.Add(ubicacion);
                puntosImportantes.Add(ubicacion); // Agregar a la lista de puntos importantes
                grafo.AgregarNodo(ubicacion);
                
            }

            string rutaArchivo = @"C:\adyacencia\ListaAdyacencia.txt"; // Puedes cambiar la ruta según necesites
            grafo.GenerarConexiones(); // Genera conexiones entre nodos
            grafo.MostrarGrafo(rutaArchivo);

        }

        // Función para verificar la distancia mínima entre un punto y todos los puntos en una lista
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
