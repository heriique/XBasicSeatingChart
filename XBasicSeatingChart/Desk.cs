using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace XBasicSeatingChart
{
    internal class Desk : INotifyPropertyChanged
    {
        //private bool _active = true; //can it be used?
        public bool Active {
            get { return DeskName != null; }
            set
            {
                if (value == false)
                {
                    if (DeskName != null)
                        DeskName = null;
                } 
                else
                {
                    if (DeskName == null)
                    {
                        DeskName = string.Empty;
                        //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Active)));
                    } 
                    else
                    {
                        throw new InvalidOperationException("Trying to enable a desk that is already enabled."); //consider removing when all works well, or change to warning
                    }
                }
            }
        }
        //int column, row;
        private string _deskName;
        public string DeskName
        {
            get { return _deskName; }
            set
            {
                if (value != _deskName)
                {
                    if (value == null)
                    {
                        index = null;
                    }
                    if (_deskName == null || value == null)
                    {
                        _deskName = value;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Active)));
                    }
                    else
                    {
                        _deskName = value;
                    }
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DeskName)));
                }
            }
        }
        public int? index; // of the name sitting there, in Names
        

        public event PropertyChangedEventHandler PropertyChanged;

        public Desk(string name, int? index)
        {
            DeskName = name;
            this.index = index;

        }

        public void SetName(string name, int? index)
        {
            DeskName = name;
            this.index = index;
            //SetActive();
        }

        /// <summary>
        /// Returns whether the desk is (empty or inactive).
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(_deskName);
        }

        /*public bool IsActive()
        {
            return active;
        }*/

        /*public void SetActive()
        {
            active = true;
        }*/

        /*public void SetInactive()
        {
            active = false;
            DeskName = null;
            index = null;
        }*/

        /*public void SwapActive()
        {
            if (active)
            {
                SetInactive();
            }
            else
            {
                SetActive();
            }
        }*/
    }
}
