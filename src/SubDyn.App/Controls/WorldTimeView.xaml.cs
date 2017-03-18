using System;
using System.Windows;
using System.Windows.Controls;
using SubDyn.Presentation;

namespace SubDyn.App.Controls
{
    public partial class WorldTimeView : Grid
    {
        private IWorldDataProvider dataProvider = null;

        public WorldTimeView()
        {
            InitializeComponent();
            update();
        }

        public IWorldDataProvider DataProvider
        {
            set
            {
                var handle = new Microsoft.FSharp.Control.FSharpHandler<double>(OnWorldUpdate);
                if(dataProvider != null)
                    dataProvider.Updated -= handle;
                dataProvider = value;
                if(dataProvider != null)
                    dataProvider.Updated += handle;
                update();
            }
        }

        private void OnWorldUpdate(object sender, double time)
        {
            if(!Dispatcher.CheckAccess())
                Dispatcher.BeginInvoke((Action)(() => update()));
            else
                update();
        }

        private void update()
        {
            if(dataProvider == null)
                timeBlock.Text = "00:00:00.00";
            else
            {
                double time = dataProvider.Time;
                double whole = System.Math.Truncate(time);
                timeBlock.Text = TimeSpan.FromSeconds(whole).ToString() + string.Format(".{0:00}", System.Math.Truncate((time - whole) * 100.0));
            }
        }
    }
}
