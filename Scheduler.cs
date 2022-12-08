using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace List
{
    internal class Scheduler
    {
       static Process activeProcess = null;

        public Scheduler(List<Process> processes)
        {
            getActive(processes);
        }

        public static Process getNextActive(List<Process> processes)
        {
            getActive(processes);
                foreach (var item in processes)
                {
                    if (item.currentStatus!=Process.Status.Zombie)
                    {
                        if (item == activeProcess)
                        {
                            item.currentPriority = item.basePriority;
                        }
                        else
                        {
                            item.currentPriority++;
                        }
                    }
                    
                }

            return activeProcess;
        }

        public static void getActive(List<Process> processes)
        {
            int max = -100000000;
            foreach (var item in processes)

            {
                if (item.currentPriority > max && item.currentStatus!=Process.Status.Zombie)
                {
                    max = item.currentPriority;
                }
            }
            activeProcess = processes.Find(x => x.currentPriority == max);
            if (activeProcess !=null)
            {
                activeProcess.currentStatus = Process.Status.Active;
            }
            foreach (var item in processes)

            {
                if (item!=activeProcess)
                {
                    item.currentStatus = Process.Status.Ready;
                }
            }
            foreach (var item in processes)

            {
                if (item.timeUsed >= item.timeResource)
                {
                    item.currentStatus = Process.Status.Zombie;
                }
            }
        }

    }
}
