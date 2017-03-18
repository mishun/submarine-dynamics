using System;
using System.Windows;
using SubDyn.Presentation;
using SubDyn.Data;

namespace SubDyn.App
{
    public partial class MainWindow
    {
        private ModelData model = null;
        private SimulatorManoeuvre manoeuvre = null;
        private Presentation.Simulator simulator = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void simulatorUpdate()
        {
            if(model != null && manoeuvre != null)
            {
                simulator = new Presentation.Simulator(model, manoeuvre);
                topView.DataProvider = simulator.ModelProvider;
                timeView.DataProvider = simulator.WorldProvider;
                playPauseButton.DataProvider = simulator;
                timeScaleBox.DataProvider = simulator;
            }
            else
            {
                simulator = null;
                topView.DataProvider = null;
                timeView.DataProvider = null;
                playPauseButton.DataProvider = null;
                timeScaleBox.DataProvider = null;
            }
        }

        private void bodyEditor_Click(object sender, RoutedEventArgs e)
        {
            var bodyEditor = new BodyEditor(this, model);
            if(true == bodyEditor.ShowDialog())
            {
                model = bodyEditor.Result;
                simulatorUpdate();
            }
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void stopButton_click(object sender, RoutedEventArgs e)
        {
            simulatorUpdate();
        }

        private void buttonResultsTrajectory_Click(object sender, RoutedEventArgs e)
        {
        }

        private void buttonManoeuvre_Click(object sender, RoutedEventArgs e)
        {
            var manoeuvreEditor = new ManoeuvreEditor(this, manoeuvre);
            if(true == manoeuvreEditor.ShowDialog())
            {
                manoeuvre = manoeuvreEditor.Result;
                simulatorUpdate();
            }
        }

        private void settings_Click(object sender, RoutedEventArgs e)
        {
        }

        private void buttonSaveResults_Click(object sender, RoutedEventArgs e)
        {
            /*using(var saveFileDialog = new System.Windows.Forms.SaveFileDialog())
            {
                saveFileDialog.Filter = @"Результаты расчета (*.results)|*.results";
                saveFileDialog.InitialDirectory = Directory.GetCurrentDirectory();

                if(System.Windows.Forms.DialogResult.OK == saveFileDialog.ShowDialog())
                {
                    try
                    {
                        _model.SaveSolutionToFile(saveFileDialog.FileName);
                    }
                    catch(InvalidOperationException)
                    {
                        MessageBox.Show("Неправильный формат выходных данных.", "Ошибка");
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("При сохранении результатов расчета возникла ошибка: " + ex.Message, "Ошибка");
                    }
                }
            }*/
        }
    }
}
