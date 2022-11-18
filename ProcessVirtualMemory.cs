using System;
using System.Collections.Generic;

namespace List
{
    class ProcessVirtualMemory
    {
        int virtualMemorySize;

        public void SwapPage(Process process)
        {
            ProcessManager processManager = new ProcessManager();
            List<Process> processes= processManager.GetList();
            Process currentProccess = processes.Find(x => x.name == process.name);
            if(GetCurrentSizeMemoryVirtual() < virtualMemorySize && currentProccess != null){
                process.ramAddress = null;
                process.virtualAddress = process.name;
            }
            else
            {
                throw new OverflowException();
            }
        }

        public void Init(int sizeVirtualMemory)
        {
            ProcessManager processManager = new ProcessManager();
            List<Process> processes = processManager.GetList();
            this.virtualMemorySize = sizeVirtualMemory;
            foreach (var process in processes)
            {
                if (GetCurrentSizeMemoryVirtual() < virtualMemorySize && process.currentStatus == Process.Status.Waiting || process.currentStatus == Process.Status.Ready)
                {
                    process.ramAddress = null;
                    process.virtualAddress = process.name;
                }
            }
        }

        private int GetCurrentSizeMemoryVirtual()
        {
            int currentSizeMemoryVirtual = 0;
            ProcessManager processManager = new ProcessManager();
            List<Process> processes = processManager.GetList();
            foreach (var process in processes)
            {
                if (process.virtualAddress != null)
                {
                    currentSizeMemoryVirtual+=process.sizeMemory;
                }
            }
            return currentSizeMemoryVirtual;

        }
    }
}
