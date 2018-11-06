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

        public MainWindow()
        {
            InitializeComponent();
            miCanvas.Focus();

            stopwatch = new Stopwatch();
            stopwatch.Start();
            tiempoAnterior = stopwatch.Elapsed;

            // 1/ Establecer Instrucciones 
            ThreadStart threadStart = new ThreadStart(moverDeconfirm);
            // 2/ Inicializar el Thread
            Thread threadMoverDeconfirm = new Thread(threadStart);
            // 3/ Ejecutar el Thread
            threadMoverDeconfirm.Start();
        }

        void moverDeconfirm()
        {
            while (true)
            {
                Dispatcher.Invoke(
                () =>
                {
                    var tiempoActual = stopwatch.Elapsed;
                    var deltaTime = tiempoActual - tiempoAnterior;

                    double leftATactual = Canvas.GetLeft(imgAT);
                    Canvas.SetLeft(imgAT, leftATactual - (200 * deltaTime.TotalSeconds) );

                    if(Canvas.GetLeft(imgAT) <= -100)
                    {
                        Canvas.SetLeft(imgAT, 800);
                    }
                    tiempoAnterior = tiempoActual;
                }
                );
            }
        }

        private void miCanvas_KeyDown(object sender, KeyEventArgs e)
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
