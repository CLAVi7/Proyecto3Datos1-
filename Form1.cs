using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Proyecto3Datos1_
{
    public partial class Form1 : Form
    {
        private int[,] mapa;
        private int tama�o = 20; // Tama�o de la matriz
        private Color[,] coloresOriginales;
        private List<Point> aeropuertos = new List<Point>();
        private List<Point> portaviones = new List<Point>();
        private Grafo grafo;
        private BateriaAntiaerea bateria;
        private Panel[,] paneles;
        private DateTime tiempoInicioPresion;
        private bool disparando = false;
        
        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            int distanciaMaxima = 10;
            mapa = new int[tama�o, tama�o];
            grafo = new Grafo(distanciaMaxima);
            paneles = new Panel[tama�o, tama�o];
            coloresOriginales = new Color[tama�o, tama�o];
            

            this.ClientSize = new Size(1020, 620);

            // Inicializar la matriz visual de paneles
            InicializarPaneles();

            // Inicializar el mapa y puntos importantes
            InicializarMatriz();
            GenerarPuntosImportantes();

            // Inicializar bater�a antia�rea en el centro de la �ltima fila
            bateria = new BateriaAntiaerea(paneles, tama�o / 2, tama�o - 1);

            // Dibujar los elementos iniciales en la matriz
            DibujarMatriz();
            
        }
        
        private void Form1_KeyDown(object? sender, KeyEventArgs e)
        {
            
            if (e.KeyCode == Keys.Left)
            {
                bateria.MoverIzquierda();
                
            }
            else if (e.KeyCode == Keys.Right)
            {
                bateria.MoverDerecha();
                
            }
            else if (e.KeyCode == Keys.Space && !disparando)
            {
                tiempoInicioPresion = DateTime.Now;
                disparando = true;
            }
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (e.KeyCode == Keys.Space && disparando)
            {
                disparando = false;
                TimeSpan duracionPresion = DateTime.Now - tiempoInicioPresion;

                // Calcular la velocidad en funci�n del tiempo de presi�n (por ejemplo, entre 50 ms y 200 ms)
                int velocidadBala = Math.Max(50, 200 - (int)duracionPresion.TotalMilliseconds);

                // Crear la bala en la posici�n de la bater�a y con la velocidad calculada
                Bala nuevaBala = new Bala(paneles, coloresOriginales, bateria.x, bateria.y - 1, velocidadBala);
            }
        }



        private void InicializarPaneles()
        {
            int tama�oCelda = 30;

            for (int i = 0; i < tama�o; i++)
            {
                for (int j = 0; j < tama�o; j++)
                {
                    Panel panel = new Panel
                    {
                        Size = new Size(tama�oCelda, tama�oCelda),
                        Location = new Point(j * tama�oCelda, i * tama�oCelda),
                        BackColor = Color.Blue // Color inicial para agua
                    };

                    paneles[i, j] = panel;
                    coloresOriginales[i, j] = panel.BackColor; // Guardar el color original
                    this.Controls.Add(panel);
                }
            }
        }

        private void InicializarMatriz()
        {
            Random random = new Random();
            int tierraInicioX = tama�o / 4 - 1;
            int tierraFinX = 3 * tama�o / 4 + 1;
            int tierraInicioY = tama�o / 4 - 1;
            int tierraFinY = 3 * tama�o / 4 + 1;

            for (int i = 0; i < tama�o; i++)
            {
                for (int j = 0; j < tama�o; j++)
                {
                    if (i >= tierraInicioX && i <= tierraFinX && j >= tierraInicioY && j <= tierraFinY && random.Next(0, 6) > 0)
                    {
                        mapa[i, j] = 1; // Tierra
                    }
                    else
                    {
                        mapa[i, j] = 0; // Agua
                    }
                }
            }
        }

        private void GenerarPuntosImportantes()
        {
            Random random = new Random();
            int numAeropuertos = 5;
            int numPortaviones = 5;
            int distanciaMinima = 6;
            List<Point> puntosImportantes = new List<Point>();

            for (int i = 0; i < numAeropuertos; i++)
            {
                Point ubicacion;
                do
                {
                    ubicacion = new Point(random.Next(0, tama�o), random.Next(0, tama�o));
                } while (mapa[ubicacion.X, ubicacion.Y] != 1 || EstaADistancia(ubicacion, puntosImportantes, distanciaMinima));

                aeropuertos.Add(ubicacion);
                puntosImportantes.Add(ubicacion);
                grafo.AgregarNodo(ubicacion);
            }

            for (int i = 0; i < numPortaviones; i++)
            {
                Point ubicacion;
                do
                {
                    ubicacion = new Point(random.Next(0, tama�o-2), random.Next(0, tama�o));
                } while (mapa[ubicacion.X, ubicacion.Y] != 0 || EstaADistancia(ubicacion, puntosImportantes, distanciaMinima));

                portaviones.Add(ubicacion);
                puntosImportantes.Add(ubicacion);
                grafo.AgregarNodo(ubicacion);
            }

            string rutaArchivo = @"C:\adyacencia\ListaAdyacencia.txt";
            grafo.GenerarConexiones();
            grafo.MostrarGrafo(rutaArchivo);
            validacion(@"C:\adyacencia\ValidacionPuntosImportantes.txt");
        }

        private bool EstaADistancia(Point punto, List<Point> lista, int distanciaMinima)
        {
            foreach (Point p in lista)
            {
                int distancia = Math.Abs(punto.X - p.X) + Math.Abs(punto.Y - p.Y);
                if (distancia < distanciaMinima)
                {
                    return true;
                }
            }
            return false;
        }

        private void DibujarMatriz()
        {
            for (int i = 0; i < tama�o-1; i++)
            {
                for (int j = 0; j < tama�o; j++)
                {
                    if (mapa[i, j] == 0)
                    {
                        paneles[i, j].BackColor = Color.Blue;
                        coloresOriginales[i, j] = Color.Blue; // Guardar color original
                    }
                    else
                    {
                        paneles[i, j].BackColor = Color.Green;
                        coloresOriginales[i, j] = Color.Green; // Guardar color original
                    }
                }
            }

            foreach (Point p in aeropuertos)
            {
                paneles[p.X, p.Y].BackColor = Color.Gray;
                coloresOriginales[p.X, p.Y] = Color.Gray; // Guardar color original
            }

            foreach (Point p in portaviones)
            {
                paneles[p.X, p.Y].BackColor = Color.Black;
                coloresOriginales[p.X, p.Y] = Color.Black; // Guardar color original
            }
        }

        public void validacion(string rutaArchivo)
        {
            using (StreamWriter writer = new StreamWriter(rutaArchivo))
            {
                writer.WriteLine("Validaci�n de Aeropuertos:");
                foreach (var aeropuerto in aeropuertos)
                {
                    writer.WriteLine($"Aeropuerto: {aeropuerto} - Ubicaci�n en Matriz: {mapa[aeropuerto.X, aeropuerto.Y]}");
                }

                writer.WriteLine("Validaci�n de Portaviones:");
                foreach (var portavion in portaviones)
                {
                    writer.WriteLine($"Portavi�n: {portavion} - Ubicaci�n en Matriz: {mapa[portavion.X, portavion.Y]}");
                }
            }
        }
    }
}

