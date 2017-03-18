using System;
using System.Windows.Controls;
using SubDyn.Presentation;

namespace SubDyn.App.Controls
{
    public partial class TopViewContainer : Border
    {
        public TopViewContainer()
        {
            InitializeComponent();
        }

        public IModelDataProvider DataProvider
        {
            set { topView.DataProvider = value; }
        }
    }
}
