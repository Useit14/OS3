using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace List
{
    class ProcessVirtualMemory
    {
        int virtualMemorySize;

        public ProcessVirtualMemory(int virtualMemorySize)
        {
            this.virtualMemorySize = virtualMemorySize;
        }

        public void SwapPage(int addr, List<Process> processes,RAM rAM)
        {
                var activeProcess = processes.Find(x => x.idProcess == addr);
                    if (rAM.GetCurrentSizeMemoryRar(processes) == virtualMemorySize)
                    {
                    var swapProcess = processes.Find(x => x.ramAddress != default);
                    var tempRam = swapProcess.ramAddress;
                    var tempVirtual = swapProcess.virtualAddress;
                    swapProcess.ramAddress = activeProcess.ramAddress;
                    swapProcess.virtualAddress = activeProcess.virtualAddress;
                    activeProcess.ramAddress = tempRam;
                    activeProcess.virtualAddress = tempVirtual;
                    } else {
                    var swapProcess = processes.Find(x => x.ramAddress == 0 && x.virtualAddress == 0);
                        activeProcess.ramAddress = swapProcess.idProcess-1;
                        activeProcess.virtualAddress = 0;
                    }
        }

        public void Init(List<Process> list)
        {
            for (int i =0;i<list.Count;i++)
            {
                if (GetCurrentSizeMemoryVirtual(list) < virtualMemorySize && list[i].ramAddress==default && list[i].virtualAddress == default)
                {
                    list[i].virtualAddress = list[i].idProcess;
                }
            }
        }

        private int GetCurrentSizeMemoryVirtual(List<Process> list)
        {
            int size = 0;
            foreach (var process in list)
            {
                if (process.virtualAddress != default)
                {
                    size++;
                }
            }
            return size;

        }


        public bool isHasAddr(List<Process> list, int addr)
        {
            bool res = false;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].idProcess == addr && list[i].virtualAddress!=default)
                {
                    res = true;
                    return res;
                }
            }
            return res;
        }

    }
}
