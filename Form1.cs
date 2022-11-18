using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Timer = System.Threading.Timer;

namespace List
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            

        }

        public delegate void SomeDelegat();
        static ProcessManager processManager = new ProcessManager();
        static TimerCallback tm = new TimerCallback(nextTime);
        static Timer timer;
        Obj obj;
        static int currentTicks=0;
        Random random = new Random();



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
                listBox1.Items.Add($"Идентификатор: {process.idProcess}; Имя: { process.name}; Время использования: {process.timeUsed}; Ресурсное время: {process.timeResource}; Базовый приоритет: {process.basePriority}; Текущий приоритет: {process.currentPriority} ; Статус: {process.currentStatus}, Память: {process.sizeMemory}") ;
            }
            processManager.Draw(chart1.Series["Series1"],0);
            processManager.Draw(chart2.Series["Series1"],1);
        }

        private void buttonCreateNewProcess_Click(object sender, EventArgs e)
        {
            try
            {
                processManager.processAdd(new Process(processManager.getLastId() + 1, TBNameProcess.Text, int.Parse(TBTime.Text), int.Parse(textBoxMemory.Text), int.Parse(comboBasePriority.SelectedItem.ToString())));
                ToList(listBox1, chart1,chart2);
                TBNameProcess.Text = "";
                comboBasePriority.SelectedItem = null;
                TBTime.Text = "";
            }
            catch 
            {
                MessageBox.Show("Создайте процессы");
            };
        }

        private void buttonDeleteNewProcess_Click(object sender, EventArgs e)
        {
            int idProcess = listBox1.SelectedIndex;
            processManager.processRemove(idProcess);
            ToList(listBox1, chart1, chart2);
            TBNameProcess.Text = "";
            comboBasePriority.SelectedItem=null;
            TBTime.Text = "";
        }
        private void buttonGo_Click(object sender, EventArgs e)
        {
            obj = new Obj(labelMainTimer, listBox1, chart1,chart2,groupRAM,groupVirtualMemory);
            timer = new Timer(tm, obj, 0, 1000);
            Process activeProcess = Scheduler.getNextActive(processManager.GetList());
            TBNameProcess.Text = activeProcess.name;
            comboBasePriority.SelectedText = activeProcess.currentPriority.ToString();
            TBTime.Text = activeProcess.timeUsed.ToString();
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            timer.Dispose();
        }

        private static void nextTime(object obj)
        {
            Label labelMainTimer = ((List.Form1.Obj)obj)._labelMainTimer;
            ListBox listBox1 = ((List.Form1.Obj)obj)._listBox1;
            Chart chart1 = ((List.Form1.Obj)obj)._chart1;
            Chart chart2 = ((List.Form1.Obj)obj)._chart2;
            GroupBox groupRAM = ((List.Form1.Obj)obj)._groupRAM;
            GroupBox groupVirtualMemory = ((List.Form1.Obj)obj)._groupVirtualMemory;
            //if (processManager.isDispose())
            //{
            //    Form1.timer.Dispose();
            //}
            processManager.nextTime(labelMainTimer,groupRAM,groupVirtualMemory);
            ToList(listBox1, chart1,chart2);
            
        }

    }
}