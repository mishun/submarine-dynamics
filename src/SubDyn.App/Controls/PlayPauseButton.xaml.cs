using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using SubDyn.Presentation;

namespace SubDyn.App.Controls
{
    public partial class PlayPauseButton : Button
    {
        private IPlayablePausable dataProvider = null;
        private BitmapImage playImage = new BitmapImage();
        private BitmapImage pauseImage = new BitmapImage();

        public PlayPauseButton()
        {
            InitializeComponent();

            playImage.BeginInit();
            playImage.UriSource = new Uri("pack://application:,,,/SubDyn.App;component/Resources/ButtonPlay16.png");
            playImage.EndInit();

            pauseImage.BeginInit();
            pauseImage.UriSource = new Uri("pack://application:,,,/SubDyn.App;component/Resources/ButtonPause16.png");
            pauseImage.EndInit();

            update();
        }

        public IPlayablePausable DataProvider
        {
            set
            {
                var handler = new Microsoft.FSharp.Control.FSharpHandler<PlayablePausableState>(OnStateChanged);
                if(dataProvider != null)
                    dataProvider.StateUpdated -= handler;
                dataProvider = value;
                if(dataProvider != null)
                    dataProvider.StateUpdated += handler;
                update();
            }
        }

        private void OnStateChanged(object sender, PlayablePausableState s)
        {
            if(!Dispatcher.CheckAccess())
                Dispatcher.BeginInvoke((Action)(() => update()));
            else
                update();
        }

        private void OnClicked(object sender, RoutedEventArgs e)
        {
            if(dataProvider != null)
                dataProvider.Trigger();
        }

        private void update()
        {
            if(dataProvider != null)
            {
                switch(dataProvider.State)
                {
                case PlayablePausableState.Initial:
                case PlayablePausableState.Paused:
                    image.Source = playImage;
                    break;

                case PlayablePausableState.Running:
                    image.Source = pauseImage;
                    break;
                }

                this.IsEnabled = true;
            }
            else
            {
                image.Source = playImage;
                this.IsEnabled = false;
            }
        }
    }
}
