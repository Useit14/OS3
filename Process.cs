using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace List
{
    internal class Process: IComparable
    {
        public int idProcess;
        public string name;
        public enum Status
        {
            Ready,
            Active,
            Waiting,
            Zombie
        }

        public int timeUsed=0;
        public int timeResource=0;
        public int basePriority;
        public int currentPriority;
        public Status currentStatus;
        public int ramAddress;
        public int virtualAddress;


        public Process(int idProcess, string name, int timeResorce , int basePriority=0 )
        {
            this.idProcess = idProcess;
            this.basePriority = basePriority;
            this.currentPriority = basePriority;
            this.name = name;
            this.currentStatus = Status.Ready;
            this.timeResource = timeResorce;
            this.ramAddress = -1;
            this.virtualAddress = -1;
        }

        public void Go () {
            timeUsed++; 
        }

        public int CompareTo(object obj)
        {
            return this.currentPriority.CompareTo((obj as Process).currentPriority);
        }

        public Process Copy(Process proc)
        {
            Process process = new Process(proc.idProcess,proc.name,proc.timeResource);
            process.currentStatus = proc.currentStatus;
            process.timeUsed = proc.timeUsed;
            process.currentPriority = proc.currentPriority;
            process.basePriority = proc.basePriority;

            return process;
        }
    }
}
