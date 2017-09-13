using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PerformanceMonitor
{
    public partial class frmMain : Form
    {
        public static PerformanceCounter WebApplications = new PerformanceCounter("ASP.NET Applications", "Request Execution Time", "__Total__");
        public static PerformanceCounter WebService = new PerformanceCounter("Web Service", "Current Connections", "_Total");
        public frmMain()
        {
            InitializeComponent();
            MonitorTime.Enabled = true;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            lbRequestExecutionTime.Title.Text = "响应时间";
        }

        private void MonitorTime_Tick(object sender, EventArgs e)
        {

            lbRequestExecutionTime.Current.Text = WebApplications.NextValue().ToString();
            lbWebCurrentConnections.Current.Text = WebService.NextValue().ToString();
        }
    }
}
