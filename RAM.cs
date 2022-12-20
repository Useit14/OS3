using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace List
{
    class RAM
    {
        public int memorySize { get; private set; }
        public int CountProcess = 0;
        Process[] page;
        public TextBox[] box;

        public RAM()
        {
            memorySize = 16;
            page = new Process[memorySize];
            box = new TextBox[memorySize];
        }

        public void Add(Process process)
        {
            for (int i = 0; i < memorySize; i++)
            {
                if (page[i] == null)
                {
                    page[i] = process;
                    CountProcess++;
                    break;
                }
            }
        }

        public void Update()
        {
            for (int i = 0; i < memorySize; i++)
            {
                if (page[i] != null)
                {
                    if (page[i].currentStatus == Process.Status.Ready)
                    {
                        box[i].BackColor = Color.LightBlue;
                    }
                    else if (page[i].currentStatus == Process.Status.Active)
                    {
                        box[i].BackColor = Color.Green;
                    }
                    else if (page[i].currentStatus == Process.Status.Waiting)
                    {
                        box[i].BackColor = Color.Yellow;
                    }
                    else if (page[i].currentStatus == Process.Status.Zombie)
                    {
                        page[i] = null;
                        box[i].BackColor = Color.White;
                        CountProcess--;
                    }
                }
            }
        }
    }
}
