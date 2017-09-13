using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FirstFrame.Security;
using FirstFrame.PacketProtocol;

namespace PassportTester
{
    public partial class Tester : Form
    {
        byte[] CryptPassword;
        //ValidCode ValidCode = new ValidCode(4, ValidCode.CodeType.Numbers);
        public Tester()
        {
            InitializeComponent();
            //picValidCode.Image = Bitmap.FromStream(ValidCode.CreateCheckCodeImage());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CryptPassword = Passport.GetCryptPassword(tbPassword.Text);
            tbDbPassword.Text = Passport.GetHex(CryptPassword);

            int saltLength = 10;
            byte[] saltValue = new byte[saltLength];
            int saltOffset = CryptPassword.Length - saltLength;
            for (int i = 0; i < saltLength; i++)
            {
                saltValue[i] = CryptPassword[saltOffset + i];
            }
            tbSalt.Text = Passport.GetHex(saltValue);
        }

        private void btPasswordTest_Click(object sender, EventArgs e)
        {
            if (CryptPassword == null) return;

            int saltLength = 10;
            byte[] saltValue = new byte[saltLength];
            int saltOffset = CryptPassword.Length - saltLength;
            for (int i = 0; i < saltLength; i++)
            {
                saltValue[i] = CryptPassword[saltOffset + i];
            }
            tbDbSalt.Text = Passport.GetHex(saltValue);


            if (Passport.ComparePassword(Passport.HexStringToByte(tbDbPassword.Text), Passport.HashPassword(tbCheckPassword.Text)))
            //if (Passport.ComparePassword(CryptPassword, Passport.HashPassword(tbCheckPassword.Text)))
            {
                lbTestResult.Text = "密码正确";
            }
            else
            {
                lbTestResult.Text = "密码错误";
            }

            byte[] _CryptPassword = Passport.GetCryptPassword(tbPassword.Text);
            tbUserCryptPassword.Text = Passport.GetHex(_CryptPassword);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            edToken.Text = Passport.GenToken(edUserID.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            byte[] _CryptPassword = Passport.HexStringToByte(tbDbPassword.Text);
        }

        private void btPraseToken_Click(object sender, EventArgs e)
        {
            if (edToken.Text == string.Empty) return;
            edLength.Text = Passport.GetData(edToken.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //if (!edValidCode.Text.Equals(ValidCode.CheckCode))
            //{
            //    MessageBox.Show(" 请输入正确的验证码！", "系统提示");
            //    edValidCode.Focus();
            //    return;
            //}
            //MessageBox.Show(" 正确！", "系统提示");
        }

        private void btGetUniqueID_Click(object sender, EventArgs e)
        {
            //SnowFlake.Twepoch = long.Parse(tbTwepoch.Text);
            tbUniqueID.Text = SnowFlake.Instance().GetSerialID().ToString();
        }

        private void btReadSerialID_Click(object sender, EventArgs e)
        {
            tbTwepoch.Text = SnowFlake.Instance().GetSerialString(long.Parse(tbUniqueID.Text));
        }

        private void btBatchGetUniqueID_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                lbSerialID.Items.Add(SnowFlake.Instance().GetSerialID().ToString());
            }
        }
    }
}
