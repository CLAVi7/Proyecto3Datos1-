using System;
using System.Drawing;
using System.Windows.Forms;


namespace Proyecto3Datos1_
{
    public partial class Form1 : Form
    {
        private int[,] mapa;
        private int tama�o = 20; // Tama�o de la matriz
        private List<Point> aeropuertos = new List<Point>();
        private List<Point> portaviones = new List<Point>();
        private Grafo grafo;
        
       

        public Form1()
        {
            InitializeComponent();
            int distanciaMaxima = 10;
            mapa = new int[tama�o, tama�o];
            grafo = new Grafo(distanciaMaxima);
           
            this.ClientSize = new Size(1020, 620);
            InicializarMatriz();
            GenerarPuntosImportantes();
            DibujarMatriz();
            
        }

        private void InicializarMatriz()
        {
            mapa = new int[tama�o, tama�o];
            Random random = new Random();
            // Ajusta estos valores para cambiar el tama�o de la zona de tierra
            int tierraInicioX = tama�o / 4 - 1;
            int tierraFinX = 3 * tama�o / 4 + 1;
            int tierraInicioY = tama�o / 4 - 1;
            int tierraFinY = 3 * tama�o / 4 + 1;

            for (int i = 0; i < tama�o; i++)
            {
                for (int j = 0; j < tama�o; j++)
                {
                    // Generar una zona central de tierra con forma algo irregular
                    if (i >= tierraInicioX && i <= tierraFinX && j >= tierraInicioY && j <= tierraFinY)
                    {
                        // Agregar aleatoriedad para que no sea un cuadrado perfecto
                        if (random.Next(0, 6) > 0)  // 3 de cada 4 casillas ser�n tierra
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
            int distanciaMinima = 6; // Distancia m�nima entre puntos importantes

            List<Point> puntosImportantes = new List<Point>(); // Lista para almacenar todos los puntos importantes

            // Generar ubicaciones de aeropuertos en tierra
            for (int i = 0; i < numAeropuertos; i++)
            {
                Point ubicacion;
                do
                {
                    int x = random.Next(0, tama�o);
                    int y = random.Next(0, tama�o);
                    ubicacion = new Point(x, y);
                }
                // Verificar que el punto est� en tierra, no repetido y cumple la distancia m�nima con todos los puntos importantes
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
                    int x = random.Next(0, tama�o);
                    int y = random.Next(0, tama�o);
                    ubicacion = new Point(x, y);
                }
                // Verificar que el punto est� en agua, no repetido y cumple la distancia m�nima con todos los puntos importantes
                while (mapa[ubicacion.X, ubicacion.Y] != 0 || EstaADistancia(ubicacion, puntosImportantes, distanciaMinima));

                portaviones.Add(ubicacion);
                puntosImportantes.Add(ubicacion); // Agregar a la lista de puntos importantes
                grafo.AgregarNodo(ubicacion);
                
            }

            string rutaArchivo = @"C:\adyacencia\ListaAdyacencia.txt"; // Puedes cambiar la ruta seg�n necesites
            grafo.GenerarConexiones(); // Genera conexiones entre nodos
            grafo.MostrarGrafo(rutaArchivo);

        }

        // Funci�n para verificar la distancia m�nima entre un punto y todos los puntos en una lista
        private bool EstaADistancia(Point punto, List<Point> lista, int distanciaMinima)
        {
            foreach (Point p in lista)
            {
                // Calcular la distancia Manhattan
                int distancia = Math.Abs(punto.X - p.X) + Math.Abs(punto.Y - p.Y);
                if (distancia < distanciaMinima)
                {
                    return true; // Hay un punto dentro de la distancia m�nima
                }
            }
            return false; // No hay puntos dentro de la distancia m�nima
        }


        private void DibujarMatriz()
        {
            int tama�oCelda = 30;
            Panel[,] paneles = new Panel[tama�o, tama�o];

            // Crear la matriz visual de paneles
            for (int i = 0; i < tama�o; i++)
            {
                for (int j = 0; j < tama�o; j++)
                {
                    Panel panel = new Panel
                    {
                        Size = new Size(tama�oCelda, tama�oCelda),
                        Location = new Point(j * tama�oCelda, i * tama�oCelda),
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
                paneles[p.X, p.Y].BackColor = Color.Black; // Color de portavi�n
            }
        }



    }
}
