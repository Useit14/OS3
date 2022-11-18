using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace List
{
    class RAM
    {
        int sizeMemoryRar;

        public RAM(int sizeMemoryRar)
        {
            this.sizeMemoryRar = sizeMemoryRar;
        }

        public void CreateRAMArray(List<Process> list)
        {
            foreach(var process in list)
            {
                if(GetCurrentSizeMemoryRar() < sizeMemoryRar && process.currentStatus == Process.Status.Active)
                {
                    process.ramAddress = process.name;
                } else {
                    ProcessVirtualMemory processVirtualMemory = new ProcessVirtualMemory();
                    processVirtualMemory.Init(sizeMemoryRar);
                    processVirtualMemory.SwapPage(process);

                }
            }
        }

        private int GetCurrentSizeMemoryRar()
        {
            int currentSizeMemoryRar = 0;
            ProcessManager processManager = new ProcessManager();
            List<Process> processes = processManager.GetList();
            foreach(var process in processes)
            {
                if (process.ramAddress != null)
                {
                    currentSizeMemoryRar+=process.sizeMemory;
                }
            }
            return currentSizeMemoryRar;

        }

    }
}
