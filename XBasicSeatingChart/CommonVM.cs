using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;

namespace XBasicSeatingChart
{
    enum CurrentPage
    {
        StartPage, StudentPage, DesignPage, PrePlacePage, PlacePage
    }

    internal class CommonVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Available = "Available";
        public string NoDesk = "No Desk";

        #region CurrentPage
        CurrentPage _currentPage = CurrentPage.StartPage;
        public CurrentPage CurrentPage
        {
            get { return _currentPage; }
            set
            {
                if (_currentPage != value)
                {
                    _currentPage = value;
                    Mainpage.SwapPage();
                }
            }
        }
        #endregion

        #region Color themes
        // Color Themes:                       Ukraine,           Light,           Dark
        readonly Color[] leftColors  =       { Color.Yellow,      Color.LightGray, Color.DimGray};
        readonly Color[] rightColors =       { Color.DeepSkyBlue, Color.White,     Color.Black};
        readonly Color[] buttonColorsLeft =  { Color.Gold,        Color.White,     Color.Gray };
        readonly Color[] buttonColorsRight = { Color.DodgerBlue,  Color.LightGray, Color.DimGray };
        readonly Color[] fontColors  =       { Color.Black,       Color.Black,     Color.White};
        readonly Color[] entryColors =       { Color.White,       Color.White,     Color.Black};
        readonly Color[] placeholderColors = { Color.Gray,        Color.Gray,      Color.Gray};
        readonly string[] themes =           { "Ukraine", "Light", "Dark" };
        public SolidColorBrush LeftColor{ get => new SolidColorBrush( leftColors[_theme]); }
        public SolidColorBrush RightColor{ get => new SolidColorBrush(rightColors[_theme]); }
        public SolidColorBrush ButtonColorLeft { get => new SolidColorBrush(buttonColorsLeft[_theme]); }
        public SolidColorBrush ButtonColorRight { get => new SolidColorBrush(buttonColorsRight[_theme]); }
        public Color EntryColor { get => entryColors[_theme]; }
        public Color PlaceholderColor { get => placeholderColors[_theme]; }
        public Color FontColor { get => fontColors[_theme]; }  
        public Color StepperColor { get => buttonColorsLeft[_theme]; }
        public string ThemeName { get => themes[_theme]; }
        private int _theme;
        public int Theme {
            get => _theme;
            set { 
                _theme = value % 3;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LeftColor)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RightColor)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ButtonColorLeft)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ButtonColorRight)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EntryColor)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PlaceholderColor)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FontColor)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StepperColor)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ThemeName)));          
            } 
        }
        #endregion

        #region InputName
        string _inputName;
        /// <summary>
        /// The name being typed in StudentPageLeft.
        /// </summary>
        public String InputName
        { 
            get => _inputName;
            set
            {
                _inputName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(InputName)));
                AddName.ChangeCanExecute();
            }
        }
        #endregion

        /// <summary>
        /// The list of names which the app will place.
        /// </summary>
        public ObservableCollection<string> Names { get; set; } = new ObservableCollection<string>();

        #region SelectedName
        private string _selectedName;
        /// <summary>
        /// The name in <cref>Names</cref> that is currently selected by the user.
        /// <c>null</c> represents no value selected.
        /// </summary>
        public string SelectedName
        {
            get { return _selectedName; }
            set
            {
                _selectedName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof (SelectedName)));
                RemoveName.ChangeCanExecute();
            }
        }
        #endregion

        /// <summary>
        /// Names that have yet to be placed. Together, <cref>UnplacedNames</cref>, <cref>PrePlacedNames</cref>
        /// and <cref>PlacedNames</cref> should contain all the names in <cref>Names</cref>, but not necessarily
        /// in the same order.
        /// </summary>
        public ObservableCollection<string> UnplacedNames { get; set; } = new ObservableCollection<string>();

        private void OnUnplacedNamesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UnplacedNames.Count)));
        }

        #region SelectedUnplacedName
        private string _selectedUnplacedName;
        public string SelectedUnplacedName
        {
            get { return _selectedUnplacedName; }
            set
            {
                _selectedUnplacedName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedUnplacedName)));
            }
        }
        #endregion

        public ObservableCollection<string> PrePlacedNames { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> PlacedNames { get; set; } = new ObservableCollection<string>();

        private Collection<string> _stateUnplacedNames = new Collection<string>();
        private Collection<string> _statePlacedNames = new Collection<string>();
        private string[,] _stateDeskNames;
        private int?[,] _stateDeskIndices;

        public Classroom Classroom;

        #region Rows
        private int _rows = 7;
        public int Rows 
        { 
            get { return _rows; } 
            set
            {
                if (_rows != value)
                {
                    _rows = value;
                    Classroom.Resize(_columns, value);
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Rows)));
                    
                }
                
            }
        }
        #endregion

        #region Columns
        private int _columns = 8;
        public int Columns
        {
            get { return _columns; }
            set
            {
                if (_columns != value)
                {
                    _columns = value;
                    Classroom.Resize(value, _rows);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Columns)));
                    
                }
            }
        }
        #endregion

        #region NumActiveDesks
        private int _numActiveDesks = 0;
        public int NumActiveDesks
        {
            get { return _numActiveDesks; }
            set
            { 
                _numActiveDesks = value; 
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NumActiveDesks)));
                GoToPrePlacePage.ChangeCanExecute();
            }
        }
        #endregion

        public void SwapActive(int column, int row)
        {
            if (GetActiveDesk(column, row))
                SetInactiveDesk(column, row);
            else
                SetActiveDesk(column, row);
        }

        public bool GetActiveDesk(int column, int row)
        {
            return Classroom.DeskAt(column, row).Active;
        }

        public void SetActiveDesk(int column, int row)
        {
            if (!GetActiveDesk(column, row))
            {
                Classroom.DeskAt(column, row).Active = true;
                //DesignPageRight.SetEnabledCell(column, row);
                //PrePlacePageRight.SetEnabledCell(column, row);
                NumActiveDesks++;
            } else
            {
                throw new InvalidOperationException("Trying to set an active desk to active.");
            }
        }

        public void SetInactiveDesk(int column, int row)
        {
            if (GetActiveDesk(column, row))
            {
                Classroom.DeskAt(column, row).Active = false;
                //DesignPageRight.SetDisabledCell(column, row);
                //PrePlacePageRight.SetDisabledCell(column, row);
                NumActiveDesks--;
            } else
            {
                throw new InvalidOperationException("Trying to set an inactive desk to inactive.");
            }
        }

        public static  MainPage Mainpage { get; set; } 
        public static PrePlacePageRight PrePlacePageRight { get; set; }
        public static DesignPageRight DesignPageRight { get; set; }
        public static PlacePageRight PlacePageRight { get; set; }

        #region Command declarations
        public Command GoToStudentPage { get; }
        public Command GoBackToStartPage { get; }
        public Command GoToDesignPage { get; }
        public Command GoBackToStudentPage { get; }
        public Command GoToPrePlacePage { get; }
        public Command GoBackToDesignPage { get; }
        public Command GoToPlacePage { get; }
        public Command GoBackToPrePlacePage { get; }
        public Command AddName { get; }
        public Command RemoveName { get; }
        public Command ColorScheme { get; }
        public Command Randomize { get; }
        #endregion

        /// <summary>
        /// Adds the current <cref>SelectedUnplacedName</cref> to <cref>PrePlacedNames</cref>
        /// and the <c>Desks[col, row]</c> and removes it from <cref>UnplacedNames</cref>.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        public void PrePlaceAddName(int column, int row)
        {
            if (column < 0 || column >= Columns)
                throw new ArgumentOutOfRangeException(nameof(column));
            if (row < 0 || row >= Rows)
                throw new ArgumentOutOfRangeException(nameof(row));
            if (string.IsNullOrWhiteSpace(SelectedUnplacedName))
                return;
                //throw new InvalidOperationException(nameof(SelectedUnplacedName) + "is empty!");
            Desk d = Classroom.DeskAt(column, row);
            if (!d.Active)
                throw new InvalidOperationException("Attempting to place a student at an inactive desk.");
            if (!d.IsEmpty())
                throw new InvalidOperationException("Attempting to place a student at a non-empty desk.");

            d.SetName(SelectedUnplacedName, Names.IndexOf(SelectedUnplacedName));

            PrePlacedNames.Add(SelectedUnplacedName);
            UnplacedNames.Remove(SelectedUnplacedName);
            //PlaceAddName(column, row);

            //string rval = SelectedUnplacedName;
            SelectedUnplacedName = null;
            //return rval;
        }
        /// <summary>
        /// Removes the name at <cref>col</cref>, <cref>row</cref> from <cref>PrePlacedNames</cref> and
        /// from <cref>Desks</cref> and adds it to <cref>UnplacedNames</cref> and <cref>SelectedUnplacedName</cref>.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        public void PrePlaceRemoveName(int column, int row)
        {
            if (column < 0 || column >= Columns)
                throw new ArgumentOutOfRangeException(nameof(column));
            if (row < 0 || row >= Rows)
                throw new ArgumentOutOfRangeException(nameof(row));
            Desk d = Classroom.DeskAt(column, row);
            if (!d.Active)
                throw new InvalidOperationException("Attempting to remove a student from an inactive desk.");
            if (d.IsEmpty())
                throw new InvalidOperationException("Attempting to remove a student from an empty desk.");
            string name = d.DeskName;
            Classroom.DeskAt(column, row).SetName(string.Empty, null);
            PrePlacedNames.Remove(name);
            UnplacedNames.Add(name);
        }

        public void PlaceNames()
        {
            SaveState();
            Classroom.PlaceNames(UnplacedNames);
            foreach (var name in UnplacedNames)
            {
                PlacedNames.Add(name);
                //UnplacedNames.Remove(name);
            }
            UnplacedNames.Clear();
        }

        public void SaveState()
        {
            _stateUnplacedNames.Clear();
            _statePlacedNames.Clear();
            _stateDeskNames = new string[Classroom.Columns, Classroom.Rows];
            _stateDeskIndices = new int?[Classroom.Columns, Classroom.Rows];
            foreach (var name in UnplacedNames)
                _stateUnplacedNames.Add(name);
            foreach (var name in PlacedNames)
                _statePlacedNames.Add(name);
            for (int i = 0; i < Classroom.Columns; i++)
                for (int j = 0; j < Classroom.Rows; j++)
                {
                    _stateDeskNames[i, j] = Classroom.Desks[i, j].DeskName;
                    _stateDeskIndices[i, j] = Classroom.Desks[i, j].index;
                }
        }

        public void ResetState()
        {
            UnplacedNames.Clear();
            PlacedNames.Clear();
            foreach (var name in _stateUnplacedNames)
                UnplacedNames.Add(name);
            foreach (var name in _statePlacedNames)
                PlacedNames.Add(name);
            for (int i = 0; i < Classroom.Columns; i++)
                for (int j = 0; j < Classroom.Rows; j++)
                {
                    Classroom.Desks[i, j].DeskName = _stateDeskNames[i, j];
                    Classroom.Desks[i, j].index = _stateDeskIndices[i, j];
                }

        }

        /// <summary>
        /// A view model used by the entire application.
        /// </summary>
        public CommonVM()
        {
            Theme = 0;

            //Names = new ObservableCollection<string>();
            Names.CollectionChanged += (s, e) => GoToPrePlacePage.ChangeCanExecute();;
            UnplacedNames.CollectionChanged += OnUnplacedNamesCollectionChanged;

            #region Command definitions
            GoToStudentPage = new Command(() =>
            {
                CurrentPage = CurrentPage.StudentPage;
            });

            GoBackToStartPage = new Command(() =>
            {
                CurrentPage = CurrentPage.StartPage;
            });

            GoToDesignPage = new Command(() =>
            {
                CurrentPage = CurrentPage.DesignPage;
                //GoToPrePlacePage.ChangeCanExecute();
            });

            GoBackToStudentPage = new Command(() =>
            {
                CurrentPage = CurrentPage.StudentPage;
            });

            GoToPrePlacePage = new Command(() =>
            {
                CurrentPage = CurrentPage.PrePlacePage;
                System.Diagnostics.Debug.WriteLine("test TEST");

                // Consider only doing this the first time. However, later changes to Names must be handled.
                UnplacedNames = new ObservableCollection<string>(Names);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UnplacedNames)));
                PrePlacedNames = new ObservableCollection<string>();
                //PrePlacePageRight.ClearAllDesks();
                Classroom.SetAllActiveDesksAsEmpty();
            },() => Names.Count <= NumActiveDesks);

            GoBackToDesignPage = new Command(() =>
            {
                CurrentPage = CurrentPage.DesignPage;
            });

            GoToPlacePage = new Command(() =>
            {
                CurrentPage = CurrentPage.PlacePage;
            });

            GoBackToPrePlacePage = new Command(() =>
            {
                CurrentPage = CurrentPage.PrePlacePage;
            });

            ColorScheme = new Command(() =>
            {
                Theme++;
            });

            AddName = new Command(() =>
            {
                Names.Add(InputName);
                InputName = String.Empty;
                SelectedName = String.Empty;
            },
            () => !string.IsNullOrEmpty(InputName) && !Names.Contains(InputName));

            RemoveName = new Command(() =>
            {
                if (!string.IsNullOrEmpty(_selectedName))
                {
                    Names.Remove(SelectedName);
                    SelectedName = string.Empty;
                }

            },
            () => !string.IsNullOrEmpty(_selectedName));

            Randomize = new Command(() =>
            {
                if (UnplacedNames.Count > 0)
                {
                    PlaceNames();
                } else
                {
                    ResetState();
                }
            });
            #endregion

            Classroom = new Classroom(Columns, Rows);
        }
    }
}
