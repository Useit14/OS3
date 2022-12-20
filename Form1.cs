using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace List
{
    public partial class Form1 : Form
    {
        public delegate void SomeDelegat();

        static ProcessManager processManager;
        static ProcessManager PManagerTmp;
        static int count;
        static int countTmp;

        Obj obj;

        static RAM ramMemory = new RAM();
        static ProcessVirtualMemory virtualMemory = new ProcessVirtualMemory();

        public Form1()
        {
            InitializeComponent();
            processManager = new ProcessManager();
            PManagerTmp = new ProcessManager();

            ramMemory.box = CreateTextBox(true,ramMemory.memorySize);
            virtualMemory.box = CreateTextBox(false,virtualMemory.memorySize);

            count = 0;


        }

        TextBox[] CreateTextBox(bool isRam,int n)
        {
            TextBox[] box = new TextBox[n];
            if (isRam)
            {
                for (int i = 0; i < groupRAM.Controls.Count; i++)
                {
                    var control = groupRAM.Controls[i];
                    box[i] = (TextBox)control;
                }
            }
            else
            {
                for (int i = 0; i < groupVirtualMemory.Controls.Count; i++)
                {
                    var control = groupVirtualMemory.Controls[i];
                    box[i] = (TextBox)control;
                }
            }
            
            return box;
        }

        class Obj
        {
           public Label _labelMainTimer;
           public ListBox _listBox1;
           public Chart _chart1;
           public Chart _chart2;
            public GroupBox _groupRAM;
            public GroupBox _groupVirtualMemory;


            public Obj(Label labelMainTimer,ListBox listBox1,Chart chart1, Chart chart2, GroupBox groupRAM, GroupBox groupVirtualMemory)
            {
                this._labelMainTimer = labelMainTimer;
                this._listBox1 = listBox1;
                this._chart1 = chart1;
                this._chart2 = chart2;
                this._groupRAM = groupRAM;
                this._groupVirtualMemory = groupVirtualMemory;
            }

            public Label getLabel()
            {
                return _labelMainTimer;
            }
        }

        public static void ToList(ListBox listBox1, Chart chart1, Chart chart2)
        {
            List<Process> processes = processManager.GetList();
            listBox1.Items.Clear();
            foreach(var process in processes)
            {
                listBox1.Items.Add($"Идентификатор: {process.idProcess}; Ram: {process.ramAddress}; Virtual: {process.virtualAddress}; Имя: { process.name}; Время использования: {process.timeUsed}; Ресурсное время: {process.timeResource}; Базовый приоритет: {process.basePriority}; Текущий приоритет: {process.currentPriority} ; Статус: {process.currentStatus} ") ;
            }
            processManager.Draw(chart1.Series["Series1"],0);
            processManager.Draw(chart2.Series["Series1"],1);
        }

        private void buttonCreateNewProcess_Click(object sender, EventArgs e)
        {
            try
            {
                List<Process> processesManager = processManager.GetList();
                List<Process> processesManagerTmp = PManagerTmp.GetList();


                if (processesManager.Count < ramMemory.memorySize + virtualMemory.memorySize)               // Проверяем есть ли вообще место в памяти
                {
                    Process pr = new Process(processManager.getLastId() + 1, TBNameProcess.Text, int.Parse(TBTime.Text), int.Parse(TBBasePriority.Text));


                    if (ramMemory.CountProcess < ramMemory.memorySize)                                     // Добавляем процесс в память
                    {
                        processManager.processAdd(pr);
                        ramMemory.Add(pr);
                        ramMemory.Update();
                    }
                    else
                    {
                        PManagerTmp.processAdd(pr);
                        virtualMemory.Add(pr);
                        virtualMemory.Update();
                    }

                    ToList(listBox1, chart1, chart2);
                    TBNameProcess.Text = "";
                    TBBasePriority.Text = "";
                    TBTime.Text = "";
                }
                else
                {
                    MessageBox.Show("Memory is full.\n Wait for some process to finish and try again", "Memory overflow error");
                }
            }
            catch 
            {
                MessageBox.Show("Создайте процессы");
            };
        }

        private void buttonDeleteNewProcess_Click(object sender, EventArgs e)
        {
            int idProcess = listBox1.SelectedIndex;
            if (idProcess != -1)
            {
                processManager.processRemove(idProcess);
                processManager.verifyForTerminated();
                ToList(listBox1, chart1, chart2);
                TBNameProcess.Text = "";
                TBBasePriority.Text = "";
                TBTime.Text = "";
                ramMemory.Update();
                virtualMemory.Update();
            }
            
        }
        private void buttonGo_Click(object sender, EventArgs e)
        {
            obj = new Obj(labelMainTimer, listBox1, chart1,chart2,groupRAM,groupVirtualMemory);
            Process activeProcess = Scheduler.getNextActive(processManager.GetList());
            TBNameProcess.Text = activeProcess.name;
            TBCurrentPriority.Text = activeProcess.currentPriority.ToString();
            TBTime.Text = activeProcess.timeUsed.ToString();
            timer1.Start();
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            ramMemory.Update();
            virtualMemory.Update();
        }

        private void btn_RandomProcess_Click(object sender, EventArgs e)
        {
            try
            {
                List<Process> processes = processManager.GetList();
                
                if (processes.Count < ramMemory.memorySize + virtualMemory.memorySize)               // Проверяем есть ли вообще место в памяти
                {
                    Random random = new Random();
                    Process process = new Process(processManager.getLastId() + 1, TBNameProcess.Text + random.Next(), random.Next(3, 5), random.Next(3, 5));


                    if (ramMemory.CountProcess < ramMemory.memorySize)                                     // Добавляем процесс в память
                    {
                        processManager.processAdd(process);
                        ramMemory.Add(process);
                        ramMemory.Update();
                    }
                    else
                    {
                        PManagerTmp.Add(process);
                        virtualMemory.Add(process);
                        virtualMemory.Update();
                    }
                    ToList(listBox1, chart1, chart2);
                    TBNameProcess.Text = "";
                    TBCurrentPriority.Text = "";
                    TBTime.Text = "";
                }
                else
                {
                    MessageBox.Show("Memory is full.\n Wait for some process to finish and try again", "Memory overflow error");
                }
            }
            catch
            {
                MessageBox.Show("Создайте процессы");
            };
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Label labelMainTimer = ((List.Form1.Obj)obj)._labelMainTimer;
            ListBox listBox1 = ((List.Form1.Obj)obj)._listBox1;
            Chart chart1 = ((List.Form1.Obj)obj)._chart1;
            Chart chart2 = ((List.Form1.Obj)obj)._chart2;
            GroupBox groupRAM = ((List.Form1.Obj)obj)._groupRAM;
            GroupBox groupVirtualMemory = ((List.Form1.Obj)obj)._groupVirtualMemory;

            processManager.nextTime(labelMainTimer, groupRAM, groupVirtualMemory);
            PManagerTmp.verifyForTerminated();


            List<Process> processesManager = processManager.GetList();
            List<Process> processesTmp = PManagerTmp.GetList();

            if (ramMemory.CountProcess < ramMemory.memorySize && virtualMemory.CountProcess != 0)
            {
                Process pr = new Process(processesTmp.Max().idProcess, processesTmp.Max().name, processesTmp.Max().timeResource);
                pr.Copy(processesTmp.Max());
                processesTmp.Max().currentStatus = Process.Status.Zombie;
                processesTmp.Remove(processesTmp.Max());
                processesManager.Add(pr);
                ramMemory.Add(pr);
            }
            ramMemory.Update();
            virtualMemory.Update();
            ToList(listBox1, chart1, chart2);

            if (processesManager.Count != count || processesTmp.Count != countTmp)
            {
                count = processesManager.Count;
                countTmp = processesTmp.Count;
            }
        }
    }
}