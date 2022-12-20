using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace List
{
    class ProcessVirtualMemory
    {
        List<Process> virtualProcesses;

        int virtualMemorySize;

        public ProcessVirtualMemory(int virtualMemorySize)
        {
            this.virtualMemorySize = virtualMemorySize;
            virtualProcesses = new List<Process>();
        }

        public void SwapPage(RAM rAM)
        {
            var ramProcesses = rAM.GetProcesses();
            var activeProcess = virtualProcesses.Find(x => x.currentStatus == Process.Status.Active);
            if (ramProcesses.Count == virtualMemorySize)
            {
                var swapProcess = ramProcesses.Find(x => x.currentStatus==Process.Status.Ready);
                var temp = swapProcess;
                swapProcess = activeProcess;
                activeProcess = temp;
            }
            else
            {
                var swapProcess = ramProcesses.Find(x => x.currentStatus == Process.Status.Zombie);
                ramProcesses.Remove(swapProcess);
                ramProcesses.Add(activeProcess);
                virtualProcesses.Remove(activeProcess);
            }
        }

        public void Init(List<Process> virtualProcesses)
        {
            this.virtualProcesses.Clear();
            this.virtualProcesses.AddRange(virtualProcesses);
            
        }

        private int GetCurrentSizeMemoryVirtual(List<Process> processes)
        {
            int size = 0;
            foreach (var process in processes)
            {
                if (process.virtualAddress!=-1 && process.currentStatus != Process.Status.Zombie)
                {
                    size++;
                }
            }
            return size;

        }


        public bool isHasAddr(int idProcess)
        {
            bool res = false;
            foreach (var process in virtualProcesses)
            {
                if (process.idProcess == idProcess)
                {
                    res = true;
                    return res;
                }
            }
            return res;
        }

        public void Add(Process process)
        {
            virtualProcesses.Add(process);
        }

        public void Remove(Process process)
        {
            virtualProcesses.Remove(process);
        }

        public List <Process> GetProcesses()
        {
            return virtualProcesses;
        }

    }
}
