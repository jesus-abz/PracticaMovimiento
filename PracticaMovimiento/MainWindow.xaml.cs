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

        void actualizar()
        {
            while (true)
            {
                Dispatcher.Invoke(
                () =>
                {
                    var tiempoActual = stopwatch.Elapsed;
                    var deltaTime = tiempoActual - tiempoAnterior;

                    if (estadoJuego == EstadoJuego.GameOver)
                    {

                    }
                    else if (estadoJuego == EstadoJuego.GamePlay)
                    {

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
                    double topWaluigiActual = Canvas.GetTop(imgWaluigi);

                    Canvas.SetTop(imgWaluigi, topWaluigiActual - 15);
                }
                if (e.Key == Key.Down)
                {
                    double topWaluigiActual = Canvas.GetTop(imgWaluigi);

                    Canvas.SetTop(imgWaluigi, topWaluigiActual + 15);
                }
                if (e.Key == Key.Left)
                {
                    double topWaluigiActual = Canvas.GetLeft(imgWaluigi);

                    Canvas.SetLeft(imgWaluigi, topWaluigiActual - 15);
                }
                if (e.Key == Key.Right)
                {
                    double topWaluigiActual = Canvas.GetLeft(imgWaluigi);

                    Canvas.SetLeft(imgWaluigi, topWaluigiActual + 15);
                }
            }
            
        }
    }
}
