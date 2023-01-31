using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace XBasicSeatingChart
{
    class Classroom
    {
        private int _rows, _columns;
        //public ObservableCollection<ObservableCollection<Desk>> Desks;
        public Desk[,] Desks;
        private int[,] _combos;

        public Classroom(int cols, int rows)
        {
            this._rows = rows;
            this._columns = cols;

            Desks = new Desk[cols, rows];
            //Desks = new ObservableCollection<ObservableCollection<Desk>>();
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    Desk d = new Desk(null, null);
                    Desks[i, j] = d;
                    d.Active = DefaultActiveDesk(i, j);
                    //Desks[i][j] = d;
                }
            }
        }

        public bool DefaultActiveDesk(int column, int row)
        {
            return row % 2 == 0 && column % 3 != 2;
        }

        public int Rows { get => _rows; }
        public int Columns { get => _columns; }


        /// <summary>
        /// Resizes the classroom. If downsizing, excess <c>Desk</c>s are lost.
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void Resize(int columns, int rows)
        {
            if (rows <= 0 || columns <= 0)
            {
                throw new ArgumentOutOfRangeException("Too low row/column count.");
            }
            if (rows == this._rows && columns == this._columns)
                return;
            Desk[,] temp = new Desk[columns, rows];
            //ObservableCollection<ObservableCollection<Desk>> temp = new ObservableCollection<ObservableCollection<Desk>>();
            int MinRows = Math.Min(rows, this._rows);
            int MinColumns = Math.Min(columns, this._columns);
            for (int i = 0; i < MinColumns; i++)
            {
                for (int j = 0; j < MinRows; j++)
                {
                    temp[i, j] = Desks[i, j];
                    //temp[i][j] = Desks[i][j];
                }
            }
            if (rows > this._rows)
            {
                for (int i = 0; i < MinColumns; i++)
                {
                    for (int j = MinRows; j < rows; j++)
                    {
                        temp[i, j] = new Desk(null, null);
                        //temp[i][j] = new Desk(null, null);
                        temp[i, j].Active = DefaultActiveDesk(i, j);
                    }
                }
            }
            if (columns > this._columns)
            {
                for (int i = MinColumns; i < columns; i++)
                {
                    for (int j = 0; j < MinRows; j++)
                    {
                        temp[i, j] = new Desk(null, null);
                        //temp[i][j] = new Desk(null, null);
                        temp[i, j].Active = DefaultActiveDesk(i, j);
                    }
                }
            }

            Desks = temp;
            this._rows = rows;
            this._columns = columns;
        }

        public void SetBlankCombos(int students)
        {
            _combos = new int[students, students];
            for (int i = 0; i < students; i++)
                for (int j = 0; j < students; j++)
                    _combos[i, j] = 0;
        }

        public int ActiveDesks()
        {
            int res = 0;
            for (int i = 0; i < _columns; i++)
            {
                for (int j = 0; j < _rows; j++)
                {
                    if (Desks[i, j].Active)
                    //if (Desks[i][j].IsActive())
                        res++;
                }

            }
            return res;
        }

        public int EmptyDesks()
        {
            int res = 0;
            for (int i = 0; i < _columns; i++)
            {
                for (int j = 0; j < _rows; j++)
                {
                    if (Desks[i, j].Active && Desks[i, j].IsEmpty())
                    //if (Desks[i][j].IsActive() && Desks[i][j].IsEmpty())
                        res++;
                }

            }
            return res;
        }

        public int RowOfDesk(int desk)
        {
            return desk / _columns;
        }

        public int ColOfDesk(int desk)
        {
            return desk % _columns;
        }

        public Desk DeskAt(int desk)
        {
            return Desks[desk % _columns, desk / _columns];
            //return Desks[desk % _columns][desk / _columns];
        }

        public Desk DeskAt(int col, int row)
        {
            return Desks[col, row];
            //return Desks[col][row];
        }

        public int DeskIndex(int col, int row)
        {
            return col + row * _columns;
        }

        public void SetAllActiveDesksAsEmpty()
        {
            for (int i = 0; i < _columns; i++)
            {
                for (int j = 0; j < _rows; j++)
                {
                    Desk d = Desks[i, j];
                    if (d.Active)
                        d.DeskName = String.Empty;
                }

            }
        }

        public void PlaceNames(Collection<string> names)
        {
            // Make a list of available desks
            var deskIndices = new List<int>();
            for (int i = 0; i < _columns; i++)
                for (int j = 0; j < _rows; j++)
                    if (Desks[i, j].Active && Desks[i, j].IsEmpty())
                    //if (Desks[i][j].IsActive() && Desks[i][j].IsEmpty())
                        //deskIndices.Add(i + j * cols);
                        deskIndices.Add(DeskIndex(i, j));
            //Shuffling desks
            MyShuffle.MyExtensions.Shuffle(deskIndices);

            int availableNames = names.Count;
            bool[] used = new bool[availableNames];
            if (_combos == null)
                _combos = new int[availableNames, availableNames];

            // Loop through the desks, but stop once we have placed enough names
            for (int i = 0; i < names.Count; i++)
            {
                // Col and row of the desk we will be analyzing
                int col = ColOfDesk(deskIndices[i]);
                int row = RowOfDesk(deskIndices[i]);
                //Debug.WriteLine("Starting work at desk " + deskIndices[i] + ", col " + col + ", row " + row);
                // loop through up to 8 neighbours
                // retrieve list of combos for that name/index
                // add lists together
                // After the loop, comboCount[x] will contain value y where:
                // x: assumed name to put at the desk
                // y: how many combos this would generate
                int[] comboCount = new int[names.Count];
                for (int j = -1; j < 2; j++)
                {
                    //Debug.WriteLine("j=" + j);
                    // Skip if col is -1 or above max
                    if (col + j < 0 || col + j >= _columns)
                        continue;
                    for (int k = -1; k < 2; k++)
                    {
                        //Debug.WriteLine("k=" + k);
                        // Skip if row is -1 or above max or if comparing to itself
                        if (row + k < 0 || row + k >= _rows || (j == 0 && k == 0))
                            continue;
                        if (Desks[col + j, row + k] == null)
                        //if (Desks[col + j][row + k] == null)
                            throw new Exception("desk is null!");
                        //Debug.WriteLine("desk is null!");
                        if (Desks[col + j, row + k].IsEmpty())
                        //if (Desks[col + j][row + k].IsEmpty())
                            continue;
                        //Debug.WriteLine("Adding to comboCount: desk at col " + (col + j) + ", row " + (row + k));
                        for (int l = 0; l < comboCount.Length; l++)
                        {
                            // Find out who (p) is sitting at [col + j, row + k].
                            int index = (int)Desks[col + j, row + k].index;
                            //int index = (int)Desks[col + j][row + k].index;
                            // How comboed are p and l?
                            comboCount[l] += _combos[index, l];
                        }
                    }
                }
                // find lowest value in list
                // exclude values where used
                int min = comboCount.Min();

                // select random unused name with lowest value
                List<int> minIndices = new List<int>();
                for (int j = 0; j < comboCount.Length; j++)
                {
                    if (comboCount[j] == min && !used[j])
                    {
                        minIndices.Add(j);
                    }
                }
                MyShuffle.MyExtensions.Shuffle(minIndices);
                int nameToPlace = minIndices[0];

                // place the student
                //Debug.WriteLine("Placing student " + nameToPlace + " at desk col " + col + " row " + row);
                Desks[col, row].SetName(names[nameToPlace], nameToPlace);
                //Desks[col][row].SetName(names[nameToPlace], nameToPlace);
                used[nameToPlace] = true;

            }
            //owner.checkPlaceEnable();
        }
    }
}
