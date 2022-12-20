using System;
using System.Collections.Generic;
using System.Timers;
using static List.Process;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Timer = System.Timers.Timer;

namespace List
{
    internal class ProcessManager
    {
        List<Process> list = new List<Process>();
        Process activeProcess;
        int currentTicks;

        public ProcessManager()
        {
            currentTicks = 0;
            
        }

        public void Draw(Series series,int flag)
        {
            series.Points.Clear();
            foreach(var item in list)
            {
                int valueX=item.idProcess;
                int valueY;
                if (flag == 0)
                {
                    valueY = item.timeUsed;
                }
                else
                {
                    valueY = item.currentPriority;
                }
                series.Points.AddXY(valueX,valueY);
            }
        }


        public void Prepend(Process elementValue)
        {

            list.Insert(0, elementValue);
        }
        public void Append(Process elementValue)
        {
            list.Insert(list.Count, elementValue);
        }
        public void Add(Process process)
        {
            list.Add(process);
        }
        public void RemoveByIndex(int index)
        {
            int currentIndex = 0;
            foreach (var item in list)
            {
                if (currentIndex == index)
                {
                   list.Remove(item);
                    return;
                }
                currentIndex++;
            }
        }
        public void RemoveByKey(Process value)
        {
            list.Remove(value);
        }
        public Process FindByIndex(int index)
        {

            var result = list.Find(x => x.idProcess == index);
            return result;

        }
        public Process FindByKey(string name)
        {
            return list.Find(x => x.name == name);
        }
        public void Sort()
        {
            list.Sort();
        }

        public List<Process> GetList()
        {
            return list;
        }


        public int getLastId()
        {
            int current = 0;
            int lastId=0;
            if (list.Count == 1)
            {
                list[0].idProcess = 1;
                lastId = 1;
            }
            else
            {
                foreach (var item in list)
                {
                    if (current == list.Count - 1)
                    {
                        lastId = item.idProcess;
                    }
                    current++;
                }
            }
            
            return lastId;
        }

        public void processAdd(Process process)
        {
            Add(process);
        }

        public void processRemove(int idProcess)
        {
            RemoveByIndex(idProcess);
        }
        public bool isDispose()
        {
            int count=0;
            foreach (var process in list)
            {
                if (process.currentStatus == Status.Ready)
                {
                    count++;
                }
            }
            return count == list.Count;
        }
        public void verifyForTerminated()
        {
            list.RemoveAll(x => x.currentStatus == Status.Zombie);
        }

        public void verifyForReady()
        {
            foreach (var process in list)
            {
                if (process.timeUsed >= process.timeResource)
                {
                    process.currentStatus = Status.Zombie;
                }

            }

        }

        public void nextTime(Label label, GroupBox groupRAM, GroupBox groupVirtualMemory)
        {
           verifyForTerminated();
            if (list.Count != 0)
            {
                activeProcess = Scheduler.getNextActive(list);
                activeProcess.Go();
            }
            currentTicks++;
            label.Text = currentTicks.ToString();
        }
    }

}
