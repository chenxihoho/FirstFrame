using FirstFrame.DapperEx;
using FirstFrame.Task;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskTester
{
    public partial class frmMain : Form
    {
        public static DbBase dbMall = new DbBase(new DBTools(), (int)DbIndex.DB_MALL);
        TaskSchedule gTaskSchedule = TaskSchedule.GetInstance();
        public frmMain()
        {
            InitializeComponent();
            gTaskSchedule.SetDataBase(dbMall);
        }

        private void btLoadTask_Click(object sender, EventArgs e)
        {
            TaskSchedule.GetInstance().StartSchedule();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            lbStatus.Text = string.Format("未执行任务总数：{0}", gTaskSchedule.GetUnProcessedTaskCount());
        }

        private void btExecuteTask_Click(object sender, EventArgs e)
        {

        }
    }
}
