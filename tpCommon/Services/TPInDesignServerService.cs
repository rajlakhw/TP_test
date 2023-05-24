using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TPInDesignServerService
    {
        private readonly InDesignServer.Application myApp;

        public TPInDesignServerService(InDesignServer.Application _myApp)
        {
            this.myApp = _myApp;
        }

        public bool RunScript(string ScriptPath)
        {

        }
    }
}
