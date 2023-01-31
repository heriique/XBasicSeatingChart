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
	public partial class DesignPageRight : ContentView
	{
        readonly CommonVM c = Application.Current.Resources["commonVM"] as CommonVM;
        public DesignPageRight ()
		{
			InitializeComponent ();
            c.NumActiveDesks = NumberOfActiveCells();
            System.Diagnostics.Debug.WriteLine("Init: " + c.NumActiveDesks);
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

        List<DesignPageGridLabel> cells = new List<DesignPageGridLabel>();


        #region RowCount Property

        /// <summary>
        /// Adds the specified number of Rows to RowDefinitions. 
        /// Default Height is Star
        /// </summary>
        public static readonly BindableProperty RowCountProperty =
            BindableProperty.Create(
                "RowCount", typeof(int), typeof(DesignPageRight),
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
            //System.Diagnostics.Debug.WriteLine("DPR: rowcountchanged");
            if (!(obj is DesignPageRight) || (int)newValue < 0)
                return;

            DesignPageRight dpr = (DesignPageRight)obj;
            Grid grid = dpr.designPageRightGrid;
            int change = (int)newValue - (int)oldValue;
            int columns = GetColumnCount(obj);
            if (change > 0)
            {
                for (int i = 0; i < change; i++)
                {
                    grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Star });
                    for (int j = 0; j < columns; j++)
                    {
                        DesignPageGridLabel cell = new DesignPageGridLabel(j, (int)oldValue + i);
                        //cell.Text = cell.Column + ", " + cell.Row;
                        dpr.cells.Add(cell);
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

                dpr.cells.RemoveRange(columns * (int)newValue, change * columns);
            }
            dpr.c.NumActiveDesks = dpr.NumberOfActiveCells();
        }

        #endregion

        #region ColumnCount Property

        /// <summary>
        /// Adds the specified number of Columns to ColumnDefinitions. 
        /// Default Width is Star
        /// </summary>
        public static readonly BindableProperty ColumnCountProperty =
            BindableProperty.Create(
                "ColumnCount", typeof(int), typeof(DesignPageRight),
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
            if (!(obj is DesignPageRight) || (int)newValue < 0)
                return;

            DesignPageRight dpr = (DesignPageRight)obj;
            Grid grid = dpr.designPageRightGrid;
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
                        DesignPageGridLabel cell = new DesignPageGridLabel((int)oldValue + i, j);
                        //cell.Text = cell.Column + ", " + cell.Row;
                        dpr.cells.Insert((int)newValue * j + (int)oldValue + i, cell);
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
                        dpr.cells.RemoveAt((int)newValue * j + (int)newValue + i);
                        
                    }
                }
            }
            dpr.c.NumActiveDesks = dpr.NumberOfActiveCells();
        }

        #endregion
    }
}