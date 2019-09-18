using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rubius.DevSaunaB.DataServer.Services
{
    public class SocketStateHolder
    {
        private List<string> _states;

        public SocketStateHolder()
        {
            _states = new List<string>();
        }

        public void AddState(string state)
        {
            _states.Add(state);
        }

        public List<string> GetStates()
        {
            return _states;
        }
    }
}
