using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XBasicSeatingChart
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrePlacePageRight : ContentView
    {
        CommonVM c = Application.Current.Resources["commonVM"] as CommonVM;
        public PrePlacePageRight()
        {
            InitializeComponent();
        }

        private int NumberOfCells(BindableObject obj)
        {
            return GetColumnCount(obj) * GetRowCount(obj);
        }

        public int NumberOfActiveCells()
        {
            int count = 0;
            foreach(GridLabel item in cells)
            {
                if (item.GetDesk().Active)
                    count++;
            }
            return count;
        }

        /// <summary>
        /// Set all desks as empty, if they are enabled.
        /// </summary>
        /*public void ClearAllDesks()
        {
            foreach(PrePlacePageGridLabel item in cells)
            {
                if (item.GetDesk().Active)
                    item.Text = c.Available;
            }
        }*/

        List<PrePlacePageGridLabel> cells = new List<PrePlacePageGridLabel>();

        /*public void SetEnabledCell(int column, int row)
        {
            cells[GetColumnCount(this) * row + column].Enabled = true;
        }*/

        /*public void SetDisabledCell(int column, int row)
        {
            cells[GetColumnCount(this) * row + column].Enabled = false;
        }*/

        #region RowCount Property

        /// <summary>
        /// Adds the specified number of Rows to RowDefinitions. 
        /// Default Height is Star
        /// </summary>
        public static readonly BindableProperty RowCountProperty =
            BindableProperty.Create(
                "RowCount", typeof(int), typeof(PrePlacePageRight),
                propertyChanged: RowCountChanged);

        // Get
        public static int GetRowCount(BindableObject obj)
        {
            return (int)obj.GetValue(RowCountProperty);
        }

        // Set
        public static void SetRowCount(BindableObject obj, int value)
        {
            obj.SetValue(RowCountProperty, value);
        }

        // Change Event - Adds the Rows
        public static void RowCountChanged(BindableObject obj, object oldValue, object newValue)
        {
            if (!(obj is PrePlacePageRight) || (int)newValue < 0)
                return;

            PrePlacePageRight pppr = (PrePlacePageRight)obj;
            Grid grid = pppr.prePlacePageRightGrid;
            int change = (int)newValue - (int)oldValue;
            int columns = GetColumnCount(obj);
            if (change > 0)
            {
                for (int i = 0; i < change; i++)
                {
                    grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Star });
                    for (int j = 0; j < columns; j++)
                    {
                        PrePlacePageGridLabel cell = new PrePlacePageGridLabel(j, (int)oldValue + i);
                        //cell.BindingContext = pppr.c.Classroom.Desks[j, i];
                        //cell.BindingContext = pppr.c.Classroom.Desks[j][i];
                        //cell.SetBinding(GridLabel.TextProperty, "DeskName");
                        //cell.Text = cell.Column + ", " + cell.Row;
                        pppr.cells.Add(cell);
                        grid.Children.Add(cell, j, (int)oldValue + i);
                    }
                }
            } else if (change < 0)
            {
                change = Math.Abs(change);
                for (int i = 0; i < change; i++)
                {
                    grid.RowDefinitions.RemoveAt(grid.RowDefinitions.Count - 1);
                    /*for (int j = 0; j < columns; j++)
                    {
                        grid.Children.Remove(dpr.cells[columns * (int)newValue + j]);
                    }*/
                }
                var children = grid.Children.ToList();
                foreach (var child in children.Where(child => Grid.GetRow(child) >= (int)newValue))
                    grid.Children.Remove(child);

                pppr.cells.RemoveRange(columns * (int)newValue, change * columns);
            }
            pppr.c.NumActiveDesks = pppr.NumberOfActiveCells();
        }

        #endregion

        #region ColumnCount Property

        /// <summary>
        /// Adds the specified number of Columns to ColumnDefinitions. 
        /// Default Width is Star
        /// </summary>
        public static readonly BindableProperty ColumnCountProperty =
            BindableProperty.Create(
                "ColumnCount", typeof(int), typeof(PrePlacePageRight),
                 propertyChanged: ColumnCountChanged);

        // Get
        public static int GetColumnCount(BindableObject obj)
        {
            return (int)obj.GetValue(ColumnCountProperty);
        }

        // Set
        public static void SetColumnCount(BindableObject obj, int value)
        {
            obj.SetValue(ColumnCountProperty, value);
        }

        // Change Event - Add the Columns
        public static void ColumnCountChanged(
            BindableObject obj, object oldValue, object newValue)
        {
            if (!(obj is PrePlacePageRight) || (int)newValue < 0)
                return;

            PrePlacePageRight pppr = (PrePlacePageRight)obj;
            Grid grid = pppr.prePlacePageRightGrid;
            int change = (int)newValue - (int)oldValue;
            int rows = GetRowCount(obj);
            if (change > 0)
            {
                for (int i = 0; i < change; i++)
                    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
                for (int j = 0; j < rows; j++)
                {
                    for (int i = 0; i < change; i++)
                    {
                        PrePlacePageGridLabel cell = new PrePlacePageGridLabel((int)oldValue + i, j);
                        //cell.Text = cell.Column + ", " + cell.Row;
                        pppr.cells.Insert((int)newValue * j + (int)oldValue + i, cell);
                        // skew rest of label texts 
                        grid.Children.Add(cell, (int)oldValue + i, j);
                    }
                }
            }
            else if (change < 0)
            {
                change = Math.Abs(change);
                
                var children = grid.Children.ToList();
                foreach (var child in children.Where(child => Grid.GetColumn(child) >= (int)newValue))
                    grid.Children.Remove(child);
                for (int i = 0; i < change; i++)
                {
                    grid.ColumnDefinitions.RemoveAt(grid.ColumnDefinitions.Count - 1);
                }

                for (int j = 0; j < rows; j++)
                {
                    for (int i = 0; i < change; i++)
                    {
                        pppr.cells.RemoveAt((int)newValue * j + (int)newValue + i);
                        
                    }
                }
            }
            pppr.c.NumActiveDesks = pppr.NumberOfActiveCells();
        }

        #endregion
    }
}