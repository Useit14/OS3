using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace List
{
    internal class Process
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
    }
}
