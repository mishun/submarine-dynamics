using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SubDyn.App
{
    public partial class SplitButton : UserControl
    {
        public event RoutedEventHandler Click;
        public event SelectionChangedEventHandler SelectionChanged;

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(SplitButton));

        public static readonly DependencyProperty ItemTemplateProperty =
                DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(SplitButton), new UIPropertyMetadata(null));

        public static readonly DependencyProperty IsExpandedProperty =
                DependencyProperty.Register("IsExpanded", typeof(bool), typeof(SplitButton));

        public static readonly DependencyProperty SelectedIndexProperty =
                DependencyProperty.Register("SelectedIndex", typeof(int), typeof(SplitButton));

        public SplitButton()
        {
            InitializeComponent();
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set
            {
                SetValue(IsExpandedProperty, value);
                if (!value)
                    collapsedAt = DateTime.Now;
                if (value)
                    ListBox.Focus();
            }
        }
        private DateTime collapsedAt = DateTime.MinValue;

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            if (DateTime.Now - collapsedAt <= TimeSpan.FromMilliseconds(200))
            {
                Expander.IsExpanded = false;
                IsExpanded = false;
                return;
            }
            IsExpanded = true;
        }

        private void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            IsExpanded = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Click != null)
                Click(this, e);
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ButtonContent.Content = ListBox.SelectedItem;

            if (SelectionChanged != null)
                SelectionChanged(this, e);
            IsExpanded = false;
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ButtonContent.Width = SplitGrid.ColumnDefinitions[0].ActualWidth;
        }
    }
}
