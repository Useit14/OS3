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
        List<Process> ramProcesses;
        List<Process> virtualProcesses;

        int sizeMemoryRar;
        public RAM(int sizeMemoryRar)
        {
            this.sizeMemoryRar =sizeMemoryRar;
            ramProcesses = new List<Process>();
            virtualProcesses = new List<Process>();


        }

        public void CreateRAMArray(List<Process> list, ProcessVirtualMemory processVirtualMemory)
        {
            ramProcesses.Clear();
            virtualProcesses.Clear();
            bool isRam = true;
            for(int i=0;i<list.Count;i++)
            {
                if (ramProcesses.Count == sizeMemoryRar)
                {
                    isRam = false;
                }
                if (isRam == true)
                {
                    ramProcesses.Add(list[i]);
                } else
                {
                    virtualProcesses.Add(list[i]);
                }
            }
            processVirtualMemory.Init(virtualProcesses);
        }

        public int GetCurrentSizeMemoryRar(List<Process> processes)
        {
            int size = 0;
            foreach(var process in processes)
            {
                if (process.ramAddress!=-1 && process.currentStatus!=Process.Status.Zombie)
                {
                    size++;
                }
            }
            return size;
        }

        public bool isHasAddr(int idProcess)
        {
            bool res = false;
            foreach (var process in ramProcesses)
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
            ramProcesses.Add(process);
        }

        public void Remove(Process process)
        {
            ramProcesses.Remove(process);
        }

        public List<Process> GetProcesses()
        {
            return ramProcesses;
        }

    }
}
