using FirstFrame.Caching;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _02.CachingTester
{
    public partial class frmMain : Form
    {
        RedisCache _RedisCache = RedisCache.GetInstance();
        public frmMain()
        {
            InitializeComponent();
        }

        private void btSet_Click(object sender, EventArgs e)
        {
            _RedisCache.Set("test", this.ToString(), new TimeSpan(0, 0, 60));
        }

        private void btGet_Click(object sender, EventArgs e)
        {
            string GetResult = _RedisCache.Get<string>("test");
            MessageBox.Show(GetResult.ToString());
        }
    }
}
