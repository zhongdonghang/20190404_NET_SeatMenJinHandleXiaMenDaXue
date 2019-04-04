using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CardLogSync2
{
    public partial class SyncForm : Form
    {
        public SyncForm()
        {
            InitializeComponent();
        }
        string intervalTime = ConfigurationManager.AppSettings["GetDataInterval"];
        string syncName = ConfigurationManager.AppSettings["SyncName"];
        System.Timers.Timer t;
        Code.AccessSync accessSync;
        //private void button1_Click(object sender, EventArgs e)
        //{

        //}

        void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            t.Stop();
            try
            {
                rtb_Message.Text = "";
                accessSync.Sync();
            }
            catch (Exception ex)
            {
                SeatManage.SeatManageComm.WriteLog.Write(ex.Message);
            }
            finally
            {
                t.Start();
            }
        }

        private void SyncForm_Load(object sender, EventArgs e)
        {
            lb_name.Text = syncName;
            btn_End.Enabled = false;
            accessSync = new Code.AccessSync();
            accessSync.SyncProgress += accessSync_SyncProgress;
        }

        void accessSync_SyncProgress(string message)
        {
            this.Invoke(new Action(() =>
            {
                //SeatManage.SeatManageComm.WriteLog.Write(message);
                rtb_Message.Text += message + "\n";

            }));
        }

        private void btn_End_Click(object sender, EventArgs e)
        {
            t.Stop();
            t.Elapsed -= t_Elapsed;
            btn_End.Enabled = false;
            btn_Start.Enabled = true;
        }

        private void btn_Start_Click(object sender, EventArgs e)
        {
            btn_Start.Enabled = false;
            btn_End.Enabled = true;
            t = new System.Timers.Timer(int.Parse(intervalTime) * 1000);
            t.Elapsed += new System.Timers.ElapsedEventHandler(t_Elapsed);
            t.Enabled = true;
            t.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void btn_End_Click_1(object sender, EventArgs e)
        {
            t.Stop();
            t.Elapsed -= t_Elapsed;
            btn_End.Enabled = false;
            btn_Start.Enabled = true;
        }

        private void SyncForm_Load_1(object sender, EventArgs e)
        {
            lb_name.Text = syncName;
            btn_End.Enabled = false;
            accessSync = new Code.AccessSync();
            accessSync.SyncProgress += accessSync_SyncProgress;
        }
    }
}
