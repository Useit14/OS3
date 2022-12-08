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
        static RAM rAM;
        static ProcessVirtualMemory virtualMemory;
        Process activeProcess;
         int currentTicks;

        public ProcessManager()
        {
            currentTicks = 0;
            
        }

        public void CreateRAMArray()
        {
            rAM = new RAM(16);
            virtualMemory = new ProcessVirtualMemory(16);
            rAM.CreateRAMArray(list, virtualMemory);
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
            foreach (var process in list)
            {
                if(process.currentStatus == Status.Zombie)
                {
                    process.ramAddress = 0;
                    process.virtualAddress = 0;
                }
            }
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
            verifyForReady();
            verifyForTerminated();
           activeProcess = Scheduler.getNextActive(list);
           if (activeProcess == null)
                {
                    for (int i = 0; i < groupRAM.Controls.Count; i++)
                    {
                        groupRAM.Controls[i].BackColor = System.Drawing.Color.White;
                        groupVirtualMemory.Controls[i].BackColor = System.Drawing.Color.White;
                    }
                    return;
                }

            if (rAM.isHasAddr(list,activeProcess.idProcess))
            {
                activeProcess.Go();
            } else if(virtualMemory.isHasAddr(list,activeProcess.idProcess))
            {
                virtualMemory.SwapPage(activeProcess.idProcess,list,rAM);
                activeProcess.Go();
            }

            for(int i = 0; i < groupRAM.Controls.Count; i++)
            {
                groupRAM.Controls[i].BackColor = System.Drawing.Color.White;
                groupVirtualMemory.Controls[i].BackColor = System.Drawing.Color.White;
            }

            for(int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                if (item.ramAddress != default)
                {
                    
                        var control = groupRAM.Controls[(item.ramAddress-1)%16];

                        if (control.BackColor == System.Drawing.Color.White)
                        {
                            if (item.currentStatus == Status.Active)
                            {
                                control.BackColor = System.Drawing.Color.Green;
                            }
                            else
                            {
                                control.BackColor = System.Drawing.Color.LightBlue;
                            }
                        }
                            
                } else if (item.virtualAddress != default) {
                 
                        var control = groupVirtualMemory.Controls[(item.virtualAddress - 1) % 16];
                        if (control.BackColor == System.Drawing.Color.White)
                        {
                            control.BackColor = System.Drawing.Color.LightBlue;
                        }
                }
            }

            currentTicks++;
            label.Text = currentTicks.ToString();
        }
    }

}
