using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using SubDyn.Presentation;

namespace SubDyn.App.Controls
{
    public partial class TimeScaleBox : ComboBox
    {
        private IHavingTimeScale dataProvider = null;
        private bool inside = false;

        public TimeScaleBox()
        {
            InitializeComponent();
            read();
        }

        public IHavingTimeScale DataProvider
        {
            set
            {
                var handler = new Microsoft.FSharp.Control.FSharpHandler<double>(OnTimeScaleChanged);
                if(dataProvider != null)
                    dataProvider.TimeScaleChanged -= handler;
                dataProvider = value;
                if(dataProvider != null)
                    dataProvider.TimeScaleChanged += handler;
                write();
                read();
            }
        }

        private void OnTimeScaleChanged(object sender, double timeScale)
        {
            if(!Dispatcher.CheckAccess())
                Dispatcher.BeginInvoke((Action)(() => { read(); }));
            else
                read();
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            write();
        }

        private void write()
        {
            if(inside || dataProvider == null)
                return;

            var item = SelectedItem as ComboBoxItem;
            if(item != null)
            {
                double timeScale = double.Parse(item.Tag.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture);
                dataProvider.TimeScale = timeScale;
            }
            else
                dataProvider.TimeScale = 1.0;
        }

        private void read()
        {
            if(dataProvider == null)
                IsEnabled = false;
            else
            {
                inside = true;
                try
                {
                    foreach(ComboBoxItem item in Items)
                    {
                        double itemScale = double.Parse(item.Tag.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture);
                        if(itemScale == dataProvider.TimeScale)
                        {
                            SelectedItem = item;
                            return;
                        }
                    }

                    SelectedItem = null;
                    Text = dataProvider.TimeScale.ToString(CultureInfo.InvariantCulture);
                }
                finally
                {
                    IsEnabled = true;
                    inside = false;
                }
            }
        }
    }
}
