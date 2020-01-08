using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonInterfaces {
    public interface IPuppet {
        void GlobalStatus();
        void Crash();
        void Freeze();
        void Unfreeze();
        void InjectDelay(string pid);
        string LocalState(int roundID);
        string GetPid();
    }
}
