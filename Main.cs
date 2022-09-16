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
namespace EbDaemon
{
    public partial class Main : Form
    {
        private Timer timer;
        private int Open_Ok = 0;
        private int Open_Failt = 0;
        public Main()
        {
            InitializeComponent();
            timer = new Timer();
            timer.Interval = Configs.Instance.TimeSpan*1000; //30秒，1分钟
            timer.Tick += timer_Tick;
            timer.Start();
            txtTimeSpan.Text = Configs.Instance.TimeSpan.ToString();
            XS.Core.Log.InfoLog.Info("服务启动..."); 

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            var process = Configs.Instance.getProcess;
            foreach (var item in process)
            {
                CheckApp(item);
            }
        }

        private void CheckApp(KeyValuePair<string, string> pair)
        {
            string pName = pair.Key;
            string appPath = pair.Value;
            foreach (Process p in Process.GetProcesses())
            {
                if (p.ProcessName.ToUpper().Contains(pName.ToUpper()))
                {
                    return;
                }
            }

            // App没有打开
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = appPath;
            startInfo.WindowStyle = ProcessWindowStyle.Minimized;//最小化
            try
            {
                //System.Diagnostics.Process.Start(appPath);
                Process.Start(startInfo);
                Open_Ok++;
                lbCountOk.Text = Open_Ok.ToString();
                XS.Core.Log.InfoLog.DebugFormat("{0}启动完成！", pName);
            }
            catch (Exception e)
            {
                Open_Failt++;
                lbCountFailt.Text = Open_Failt.ToString();
                XS.Core.Log.ErrorLog.ErrorFormat("打开应用失败：{0}", e.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string timeSpan = txtTimeSpan.Text.Trim();
            if (!string.IsNullOrEmpty(timeSpan))
            {
                Configs.Instance.TimeSpan = int.Parse(timeSpan);
                Configs.Instance.Save();
            }
            else
            {
                MessageBox.Show("请输入正确的时间秒数!");
            }
        }

        private void btnShowProcess_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Configs.Instance.ProcessList);
        }
    }
}
