using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using SubDyn.Data;
using SubDyn.Math;

namespace SubDyn.App
{
    public partial class BodyEditor
    {
        private ModelData model = null;

        private BodyEditor()
        {
            InitializeComponent();
        }

        public BodyEditor(MainWindow mainWindow, ModelData model)
            : this()
        {
            Owner = mainWindow;
            this.model = model;
            GetBodyParameters(model);
        }

        public ModelData Result
        {
            get { return model; }
        }

        private void GetBodyParameters(ModelData model)
        {
            if(model == null)
                return;

            bodyName.Text = (model.Name != string.Empty ? model.Name : "Введите название корпуса");

            bodyLength.Text = model.Length.ToString("0.0000");
            bodyWidth.Text = model.Beam.ToString("0.0000");
            bodyHeight.Text = model.Height.ToString("0.0000");
            bodyVolume.Text = model.Volume.ToString("0.0000");

            if(model.AddedMasses != null)
            {
                Action<TextBox, double, string> copy = (box, k, name) =>
                    {
                        box.Text = k.ToString("0.0000");
                        box.ToolTip = name + " = " + k;
                    };

                var m = model.AddedMasses;

                copy(k11, m.KK.XX, "k11");
                copy(k12, m.KK.XY, "k12");
                copy(k13, m.KK.XZ, "k13");
                copy(k21, m.KK.YX, "k21");
                copy(k22, m.KK.YY, "k22");
                copy(k23, m.KK.YZ, "k23");
                copy(k31, m.KK.ZX, "k31");
                copy(k32, m.KK.ZY, "k32");
                copy(k33, m.KK.ZZ, "k33");

                copy(k44, m.WW.XX, "k44");
                copy(k45, m.WW.XY, "k45");
                copy(k46, m.WW.XZ, "k46");
                copy(k54, m.WW.YX, "k54");
                copy(k55, m.WW.YY, "k55");
                copy(k56, m.WW.YZ, "k56");
                copy(k64, m.WW.ZX, "k64");
                copy(k65, m.WW.ZY, "k65");
                copy(k66, m.WW.ZZ, "k66");

                copy(k14, m.KW.XX, "k14");
                copy(k15, m.KW.XY, "k15");
                copy(k16, m.KW.XZ, "k16");
                copy(k24, m.KW.YX, "k24");
                copy(k25, m.KW.YY, "k25");
                copy(k26, m.KW.YZ, "k26");
                copy(k34, m.KW.ZX, "k34");
                copy(k35, m.KW.ZY, "k35");
                copy(k36, m.KW.ZZ, "k36");

                copy(k41, m.WK.XX, "k41");
                copy(k42, m.WK.XY, "k42");
                copy(k43, m.WK.XZ, "k43");
                copy(k51, m.WK.YX, "k51");
                copy(k52, m.WK.YY, "k52");
                copy(k53, m.WK.YZ, "k53");
                copy(k61, m.WK.ZX, "k61");
                copy(k62, m.WK.ZY, "k62");
                copy(k63, m.WK.ZZ, "k63");
            }
        }

        /*private ModelData SetBodyParameters()
        {
            try
            {
                var newBody = new ModelData
                    {
                        Name = bodyName.Text == "Введите название корпуса" ? string.Empty : bodyName.Text,
                        Length = Convert.ToDouble(bodyLength.Text),
                        Beam = Convert.ToDouble(bodyWidth.Text),
                        Height = Convert.ToDouble(bodyHeight.Text),
                        Volume = Convert.ToDouble(bodyVolume.Text)
                    };

                {
                    Func<TextBox, double> f = (box) => Convert.ToDouble(box.Text);
                    newBody.AddedMasses = new AddedMasses.AddedMasses(
                        new Matrix3(f(k11), f(k12), f(k13), f(k21), f(k22), f(k23), f(k31), f(k32), f(k33)),
                        new Matrix3(f(k14), f(k15), f(k16), f(k24), f(k25), f(k26), f(k34), f(k35), f(k36)),
                        new Matrix3(f(k41), f(k42), f(k43), f(k51), f(k52), f(k53), f(k61), f(k62), f(k63)),
                        new Matrix3(f(k44), f(k45), f(k46), f(k54), f(k55), f(k56), f(k64), f(k65), f(k66))
                    );
                }

                return newBody;
            }
            catch(Exception)
            {
                MessageBox.Show("Проверьте правильность параметров корпуса.", "Ошибка");
                return null;
            }
        }*/

        private void openBody_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new System.Windows.Forms.OpenFileDialog
                {
                    Filter = @"Параметры корпуса (*.body)|*.body",
                    InitialDirectory = Directory.GetCurrentDirectory()
                };

            if(System.Windows.Forms.DialogResult.OK != openFileDialog.ShowDialog())
                return;

            try
            {
                model = ModelData.LoadFromFile(openFileDialog.FileName);
                model.AddedMasses = SubDyn.AddedMasses.equivalentEllipsoid(model.Volume, model.Length, model.Beam, model.Height);
                GetBodyParameters(model);
            }
            catch(InvalidOperationException)
            {
                MessageBox.Show("Неправильный формат входных данных.", "Ошибка");
            }
            catch(Exception ex)
            {
                MessageBox.Show("При загрузке параметров корпуса возникла ошибка: " + ex.Message, "Ошибка");
            }
        }

        private void saveBody_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new System.Windows.Forms.SaveFileDialog
                {
                    Filter = @"Параметры корпуса (*.body)|*.body",
                    InitialDirectory = Directory.GetCurrentDirectory()
                };

            if(System.Windows.Forms.DialogResult.OK != saveFileDialog.ShowDialog())
                return;

            try
            {
                if(model != null)
                    model.SaveToFile(saveFileDialog.FileName);
            }
            catch(InvalidOperationException)
            {
                MessageBox.Show("Неправильный формат входных данных.", "Ошибка");
            }
            catch(Exception ex)
            {
                MessageBox.Show("При сохранении параметров корпуса возникла ошибка: " + ex.Message, "Ошибка");
            }
        }

        private void bodyName_GotFocus(object sender, RoutedEventArgs e)
        {
            if(bodyName.Text == "Введите название корпуса")
                bodyName.Text = string.Empty;
        }

        private void bodyName_LostFocus(object sender, RoutedEventArgs e)
        {
            if(bodyName.Text == string.Empty)
                bodyName.Text = "Введите название корпуса";
        }

        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            //result = SetBodyParameters();
            if(model != null)
                DialogResult = true;
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void calculateAddedMasses_Click(object sender, RoutedEventArgs e)
        {
            if(addedMassesCalculationMethod.SelectedIndex == 0)
            {
                //var model = SetBodyParameters();
                if(model != null)
                {
                    model.AddedMasses = SubDyn.AddedMasses.equivalentEllipsoid(model.Volume, model.Length, model.Beam, model.Height);
                    GetBodyParameters(model);
                }
            }
            else
                throw new NotImplementedException("calculateAddedMasses_Click");
        }
    }
}
