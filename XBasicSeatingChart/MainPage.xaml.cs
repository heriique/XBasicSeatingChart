using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XBasicSeatingChart
{
    public partial class MainPage : ContentPage
    {
        private double width = 0;
        private double height = 0;
        private ContentView _currentPageLeft, _currentPageRight;

        private StudentPageLeft _studentPageLeft = new StudentPageLeft();
        private StudentPageRight _studentPageRight = new StudentPageRight();
        private DesignPageLeft _designPageLeft = new DesignPageLeft();
        private DesignPageRight _designPageRight = new DesignPageRight();
        private PrePlacePageLeft _prePlacePageLeft = new PrePlacePageLeft();
        private PrePlacePageRight _prePlacePageRight = new PrePlacePageRight();
        private PlacePageLeft _placePageLeft = new PlacePageLeft();
        private PlacePageRight _placePageRight = new PlacePageRight();
        public MainPage()
        {
            InitializeComponent();
            CommonVM.Mainpage = this;
            CommonVM.PrePlacePageRight = _prePlacePageRight;
            CommonVM.DesignPageRight = _designPageRight;
            CommonVM.PlacePageRight = _placePageRight;
            _currentPageLeft = startPageLeft;
            _currentPageRight = startPageRight;
        }
        
        public void SwapPage()
        {
            //ContentView cwl = null, cwr = null;
            rotatingGrid.Children.Clear();

            CommonVM c = Application.Current.Resources["commonVM"] as CommonVM;

            if (c.CurrentPage == CurrentPage.StartPage)
            {
                _currentPageLeft = startPageLeft;
                _currentPageRight = startPageRight;
            } else if (c.CurrentPage == CurrentPage.StudentPage)
            {
                _currentPageLeft = _studentPageLeft;
                _currentPageRight = _studentPageRight;
            } else if (c.CurrentPage == CurrentPage.DesignPage)
            {
                _currentPageLeft = _designPageLeft;
                _currentPageRight = _designPageRight;
            } else if (c.CurrentPage == CurrentPage.PrePlacePage)
            {
                _currentPageLeft = _prePlacePageLeft;
                _currentPageRight = _prePlacePageRight;
            } else if (c.CurrentPage == CurrentPage.PlacePage)
            {
                _currentPageLeft = _placePageLeft;
                _currentPageRight = _placePageRight;
            }

            if (_currentPageLeft != null && _currentPageRight != null)
            {
                if (width > height)
                {
                    rotatingGrid.Children.Add(_currentPageRight, 1, 0);
                    rotatingGrid.Children.Add(_currentPageLeft, 0, 0);
                }
                else
                {
                    rotatingGrid.Children.Add(_currentPageRight, 0, 0);
                    rotatingGrid.Children.Add(_currentPageLeft, 0, 1);
                }
            }
        }
        
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (width != this.width || height != this.height)
            {
                this.width = width;
                this.height = height;
                if (width > height)
                {
                    rotatingGrid.RowDefinitions.Clear();
                    rotatingGrid.ColumnDefinitions.Clear();
                    rotatingGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    rotatingGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    rotatingGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.6, GridUnitType.Star) });
                    rotatingGrid.Children.Remove(_currentPageRight);
                    rotatingGrid.Children.Remove(_currentPageLeft);
                    rotatingGrid.Children.Add(_currentPageLeft, 0, 0);
                    rotatingGrid.Children.Add(_currentPageRight, 1, 0);
                }
                else
                {
                    rotatingGrid.RowDefinitions.Clear();
                    rotatingGrid.ColumnDefinitions.Clear();
                    rotatingGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    rotatingGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1.6, GridUnitType.Star) });
                    rotatingGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    rotatingGrid.Children.Remove(_currentPageRight);
                    rotatingGrid.Children.Remove(_currentPageLeft);
                    rotatingGrid.Children.Add(_currentPageRight, 0, 0);
                    rotatingGrid.Children.Add(_currentPageLeft, 0, 1);
                }
            }
        }
    }
}
