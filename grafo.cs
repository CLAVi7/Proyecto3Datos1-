using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Proyecto3Datos1_
{
    // Representa un nodo en el grafo (puede ser un aeropuerto o portavión)
    public class Nodo
    {
        public Point Ubicacion { get; set; }
        public List<Nodo> Adyacentes { get; set; } = new List<Nodo>();

        public Nodo(Point ubicacion)
        {
            Ubicacion = ubicacion;
        }
    }

    // Representa el grafo con listas de adyacencia
    public class Grafo
    {
        private List<Nodo> nodos = new List<Nodo>();
        private int distanciaMaxima; // Distancia máxima para conectar nodos

        public Grafo(int distanciaMaxima)
        {
            this.distanciaMaxima = distanciaMaxima;
        }

        // Agrega un nodo al grafo
        public void AgregarNodo(Point ubicacion)
        {
            nodos.Add(new Nodo(ubicacion));
        }

        // Genera conexiones entre nodos que están dentro de la distancia máxima
        public void GenerarConexiones()
        {
            for (int i = 0; i < nodos.Count; i++)
            {
                for (int j = i + 1; j < nodos.Count; j++)
                {
                    double distancia = CalcularDistancia(nodos[i].Ubicacion, nodos[j].Ubicacion);
                    if (distancia <= distanciaMaxima)
                    {
                        nodos[i].Adyacentes.Add(nodos[j]);
                        nodos[j].Adyacentes.Add(nodos[i]);
                    }
                }
            }
        }

        // Calcula la distancia entre dos puntos
        private double CalcularDistancia(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        // Muestra el grafo para verificar conexiones
        // Muestra el grafo en el TextBox para verificar conexiones
        // Muestra el grafo para verificar conexiones
        public void MostrarGrafo(string rutaArchivo)
        {
            // Crea o sobrescribe el archivo de texto
            using (StreamWriter writer = new StreamWriter(rutaArchivo))
            {
                foreach (var nodo in nodos)
                {
                    // Muestra la ubicación del nodo
                    writer.Write($"Nodo: {nodo.Ubicacion} - Conexiones: ");

                    // Muestra los nodos adyacentes
                    foreach (var adyacente in nodo.Adyacentes)
                    {
                        writer.Write($"{adyacente.Ubicacion}, ");
                    }

                    writer.WriteLine(); // Salto de línea después de cada nodo
                }
            }
        }


    }
}
