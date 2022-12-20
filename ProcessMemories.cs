using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace List
{
    class ProcessMemories
    {
        public int idProcess;
        public Process.Status currentStatus;

        public ProcessMemories(int idProcess, Process.Status currentStatus)
        {
            this.idProcess = idProcess;
            this.currentStatus = currentStatus;
        }
    }
}
