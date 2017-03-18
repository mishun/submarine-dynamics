using System;
using System.IO;
using System.Windows;

namespace SubDyn.App
{
    public partial class ManoeuvreEditor
    {
        private SimulatorManoeuvre man = null;

        private ManoeuvreEditor()
        {
            InitializeComponent();
        }

        public ManoeuvreEditor(MainWindow mainWindow, SimulatorManoeuvre man)
            : this()
        {
            Owner = mainWindow;
            GetManoeuvreParameters(man);
        }

        public SimulatorManoeuvre Result
        {
            get { return man; }
        }

        private void GetManoeuvreParameters(SimulatorManoeuvre man)
        {
            if(man == null)
                return;

            manoeuvreName.Text = (man.Name != string.Empty ? man.Name : "Введите название маневра");
            manoeuvreType.SelectedIndex = (int)man.Type;
        }

        private SimulatorManoeuvre SetManoeuvreParameters()
        {
            try
            {
                var newManoeuvre = new SimulatorManoeuvre();

                newManoeuvre.Name = (manoeuvreName.Text == "Введите название маневра" ? string.Empty : manoeuvreName.Text);
                newManoeuvre.Type = (SimulatorManoeuvreType)manoeuvreType.SelectedIndex;

                return newManoeuvre;
            }
            catch(Exception)
            {
                MessageBox.Show("Проверьте правильность параметров маневра.", "Ошибка");
                return null;
            }
        }

        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            if(man != null)
                DialogResult = true;    
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void openManoeuvre_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = @"Параметры маневра (*.manoeuvre)|*.manoeuvre";
            openFileDialog.InitialDirectory =
                File.Exists((string)manoeuvreFileName.ToolTip) ? 
                Path.GetFullPath((string) manoeuvreFileName.ToolTip) : 
                Directory.GetCurrentDirectory();

            if(System.Windows.Forms.DialogResult.OK != openFileDialog.ShowDialog())
                return;

            try
            {
                GetManoeuvreParameters(SimulatorManoeuvre.LoadFromFile(openFileDialog.FileName));
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Неправильный формат входных данных.", "Ошибка");
            }
            catch (Exception ex)
            {
                MessageBox.Show("При загрузке параметров маневра возникла ошибка: " + ex.Message, "Ошибка");
            }
        }

        private void saveManoeuvre_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.Filter = @"Параметры маневра (*.manoeuvre)|*.manoeuvre";
            saveFileDialog.InitialDirectory =
                File.Exists((string)manoeuvreFileName.ToolTip) ?
                    Path.GetFullPath((string)manoeuvreFileName.ToolTip) : Directory.GetCurrentDirectory();

            if(System.Windows.Forms.DialogResult.OK != saveFileDialog.ShowDialog())
                return;

            try
            {
                SetManoeuvreParameters().SaveToFile(saveFileDialog.FileName);
            }
            catch(InvalidOperationException)
            {
                MessageBox.Show("Неправильный формат выходных данных.", "Ошибка");
            }
            catch(Exception ex)
            {
                MessageBox.Show("При сохранении параметров маневра возникла ошибка: " + ex.Message, "Ошибка");
            }
        }

        private void manoeuvreName_GotFocus(object sender, RoutedEventArgs e)
        {
            if(manoeuvreName.Text == "Введите название маневра")
                manoeuvreName.Text = string.Empty;
        }

        private void manoeuvreName_LostFocus(object sender, RoutedEventArgs e)
        {
            if(manoeuvreName.Text == string.Empty)
                manoeuvreName.Text = "Введите название маневра";
        }
    }
}
