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
            this.sizeMemoryRar =sizeMemoryRar;
        }

        public void CreateRAMArray(List<Process> list, ProcessVirtualMemory processVirtualMemory)
        {
            for(int i=0;i<list.Count;i++)
            {
                if (GetCurrentSizeMemoryRar(list) < sizeMemoryRar && list[i].ramAddress == default && list[i].virtualAddress == default)
                {
                    list[i].ramAddress = list[i].idProcess;
                }
                else if(GetCurrentSizeMemoryRar(list) >= sizeMemoryRar)
                {
                    processVirtualMemory.Init(list);
                    break;
                }
            }
        }

        public int GetCurrentSizeMemoryRar(List<Process> list)
        {
            int size = 0;
            foreach(var process in list)
            {
                if (process.ramAddress != default)
                {
                    size++;
                }
            }
            return size;

        }

        public bool isHasAddr(List<Process> list,int addr)
        {
            bool res = false;
            foreach (var process in list)
            {
                if (process.ramAddress != default && process.idProcess==addr)
                {
                    res = true;
                    return res;
                }
            }
            return res;
        }

        public void SetDefault(List<Process> list, int addr)
        {
            foreach (var process in list)
            {
                if (process.idProcess == addr)
                {
                    process.ramAddress = default;

                }
            }
        }
    }
}
