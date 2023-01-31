using System;
using System.Collections.Generic;
using System.Text;

namespace XBasicSeatingChart
{
    
    internal sealed class Model
    {
        private static volatile Model _instance;
        private static readonly object _instanceLock = new object();

        public static Model Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new Model();
                    }
                }

                return _instance;
            }
        }

        private Model()
        {

        }
    }
}
