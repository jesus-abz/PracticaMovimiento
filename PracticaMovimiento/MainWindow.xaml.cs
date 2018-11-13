using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//Librerias de Multiprocesamiento
using System.Threading;
using System.Diagnostics;

namespace PracticaMovimiento
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Stopwatch stopwatch;
        TimeSpan tiempoAnterior;

        enum EstadoJuego { GamePlay, GameOver};
        EstadoJuego estadoJuego = EstadoJuego.GamePlay;

        enum Direccion { Arriba, Abajo, Izquierda, Derecha, Ninguna };
        Direccion direccionJugador = Direccion.Ninguna;

        double velocidadWaluigi = 60;

        public MainWindow()
        {
            InitializeComponent();
            miCanvas.Focus();

            stopwatch = new Stopwatch();
            stopwatch.Start();
            tiempoAnterior = stopwatch.Elapsed;

            // 1/ Establecer Instrucciones 
            ThreadStart threadStart = new ThreadStart(actualizar);
            // 2/ Inicializar el Thread
            Thread threadMoverDeconfirm = new Thread(threadStart);
            // 3/ Ejecutar el Thread
            threadMoverDeconfirm.Start();
        }

        void moverJugador(TimeSpan deltaTime)
        {
            double topWaluigiActual = Canvas.GetTop(imgWaluigi);
            double leftWaluigiActual = Canvas.GetLeft(imgWaluigi);

            switch (direccionJugador)
            {
                case Direccion.Arriba:
                    if (topWaluigiActual - (velocidadWaluigi * deltaTime.TotalSeconds) >= 0)
                    {
                        Canvas.SetTop(imgWaluigi, topWaluigiActual - (velocidadWaluigi * deltaTime.TotalSeconds));
                    }
                    break;
                case Direccion.Abajo:
                    double nuevaPosicionY = topWaluigiActual + (velocidadWaluigi * deltaTime.TotalSeconds);
                    if (nuevaPosicionY + imgWaluigi.Width <= 450)
                    {
                        Canvas.SetTop(imgWaluigi, topWaluigiActual + (velocidadWaluigi * deltaTime.TotalSeconds));
                    }
                    break;
                case Direccion.Izquierda:
                    if (leftWaluigiActual - (velocidadWaluigi * deltaTime.TotalSeconds) >= 0)
                    {
                        Canvas.SetLeft(imgWaluigi, leftWaluigiActual - (velocidadWaluigi * deltaTime.TotalSeconds));
                    }
                    break;
                case Direccion.Derecha:
                    double nuevaPosicionX = leftWaluigiActual + (velocidadWaluigi * deltaTime.TotalSeconds);
                    if (nuevaPosicionX + imgWaluigi.Width <= 800)
                    {
                        Canvas.SetLeft(imgWaluigi, leftWaluigiActual + (velocidadWaluigi * deltaTime.TotalSeconds));
                    }
                    break;
                case Direccion.Ninguna:
                    break;
                default:
                    break;
            }
        }

        void actualizar()
        {
            while (true)
            {
                Dispatcher.Invoke(
                () =>
                {
                    var tiempoActual = stopwatch.Elapsed;
                    var deltaTime = tiempoActual - tiempoAnterior;

                    //velocidadWaluigi += 10 * deltaTime.TotalSeconds;

                    if (estadoJuego == EstadoJuego.GameOver)
                    {

                    }
                    else if (estadoJuego == EstadoJuego.GamePlay)
                    {
                        moverJugador(deltaTime);

                        double leftATactual = Canvas.GetLeft(imgAT);
                        Canvas.SetLeft(imgAT, leftATactual - (20 * deltaTime.TotalSeconds));

                        if (Canvas.GetLeft(imgAT) <= -100)
                        {
                            Canvas.SetLeft(imgAT, 800);
                        }

                        //Interseccion en X
                        double xAT = Canvas.GetLeft(imgAT);
                        double xWaluigi = Canvas.GetLeft(imgWaluigi);
                        if (xWaluigi + imgWaluigi.Width >= xAT && xWaluigi <= xAT + imgAT.Width)
                        {
                            lblInterseccionX.Text = "SI HAY INTERSECCION EN X";
                        }
                        else
                        {
                            lblInterseccionX.Text = "No hay Intersección en X";
                        }

                        //Interseccion en Y
                        double yAT = Canvas.GetTop(imgAT);
                        double yWaluigi = Canvas.GetTop(imgWaluigi);
                        if (yWaluigi + imgWaluigi.Height >= yAT && yWaluigi <= yAT + imgAT.Height)
                        {
                            lblInterseccionY.Text = "SI HAY INTERSECCION EN Y";
                        }
                        else
                        {
                            lblInterseccionY.Text = "No hay Intersección en Y";
                        }

                        //Colisión
                        if (xWaluigi + imgWaluigi.Width >= xAT && xWaluigi <= xAT + imgAT.Width && yWaluigi + imgWaluigi.Height >= yAT && yWaluigi <= yAT + imgAT.Height)
                        {
                            lblColision.Text = "SI HAY COLISIÓN";
                            estadoJuego = EstadoJuego.GameOver;
                            miCanvas.Visibility = Visibility.Collapsed;
                            canvasGameOver.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            lblColision.Text = "No hay colisión";
                        }

                    }

                tiempoAnterior = tiempoActual;
                    
                }
                );
            }
        }

        private void miCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            if(estadoJuego == EstadoJuego.GamePlay)
            {
                if (e.Key == Key.Up)
                {
                    direccionJugador = Direccion.Arriba;
                }
                if (e.Key == Key.Down)
                {
                    direccionJugador = Direccion.Abajo;
                }
                if (e.Key == Key.Left)
                {
                    direccionJugador = Direccion.Izquierda;
                }
                if (e.Key == Key.Right)
                {
                    direccionJugador = Direccion.Derecha;
                }
            }
            
        }

        private void miCanvas_KeyUp(object sender, KeyEventArgs e)
        {
            if(estadoJuego == EstadoJuego.GamePlay)
            {
                if(e.Key == Key.Up && direccionJugador == Direccion.Arriba)
                {
                    direccionJugador = Direccion.Ninguna;
                }
                if (e.Key == Key.Down && direccionJugador == Direccion.Abajo)
                {
                    direccionJugador = Direccion.Ninguna;
                }
                if (e.Key == Key.Left && direccionJugador == Direccion.Izquierda)
                {
                    direccionJugador = Direccion.Ninguna;
                }
                if (e.Key == Key.Right && direccionJugador == Direccion.Derecha)
                {
                    direccionJugador = Direccion.Ninguna;
                }
            }
        }
    }
}
