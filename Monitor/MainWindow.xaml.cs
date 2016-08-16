using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Management;
//using System.Net;
//using System.Net.NetworkInformation;
//using System.Text;
using System.Windows;
using System.Windows.Input;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;

namespace MyMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filepath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retval, int size, string filePath);

        private string strFilePath = @"d:/clent/config/devicetag.ini";//获取INI文件路径

        public static void CopyDir(string srcPath, string aimPath)
        {
            try
            {
                // 检查目标目录是否以目录分割字符结束如果不是则添加之
                if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
                    aimPath += Path.DirectorySeparatorChar;
                // 判断目标目录是否存在如果不存在则新建之
                if (!Directory.Exists(aimPath))
                    Directory.CreateDirectory(aimPath);
                // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
                // 如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法
                // string[] fileList = Directory.GetFiles(srcPath);
                string[] fileList = Directory.GetFileSystemEntries(srcPath);
                // 遍历所有的文件和目录
                foreach (string file in fileList)
                {
                    // 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                    if (Directory.Exists(file))
                        CopyDir(file, aimPath + Path.GetFileName(file));
                    // 否则直接Copy文件
                    else
                        File.Copy(file, aimPath + Path.GetFileName(file), true);
                }
            }
            catch
            {
                MessageBox.Show("还原失败", "系统还原", MessageBoxButton.OKCancel);
            }
        }

        public static void Kill(string name)
        {
            Process[] process;//创建一个PROCESS类数组
            process = Process.GetProcesses();//获取当前任务管理器所有运行中程序
            foreach (Process proces in process)//遍历
            {
                if (proces.ProcessName == name)
                {
                    proces.Kill();
                }
            }
        }

        public MainWindow()
        {
            //FocusManager.FocusedElement = "{Binding ElementName=[文本框的名字]}"
            
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
            
            
        }




       

        decimal ConvertBytes(ulong b, int iteration)
        {
            long iter = 1;
            for (int i = 0; i < iteration; i++)
                iter *= 1024;
            return Math.Round((Convert.ToDecimal(b)) / Convert.ToDecimal(iter), 2, MidpointRounding.AwayFromZero);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SettingWindow settingWindow = new SettingWindow();
            settingWindow.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("本操作将还原业务程序，并重新启动设备", "系统还原", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                Kill("cmd");
                Kill("client");
                Kill("YSTenLoader");
                System.Threading.Thread.Sleep(1000);
                CopyDir(@"c:/bak", "d:/");
                System.Diagnostics.Process.Start("shutdown", @"/r /t 0");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("本操作将提交对c盘的更改，并重新启动设备", "ewf提交", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                System.Diagnostics.Process.Start(@"c:/shell/commit.bat");
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("本操作将会将程序版本号归0，并重新启动设备", "系统还原", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                WritePrivateProfileString("Device", "Pkg_VersionNum", "0", strFilePath);
                WritePrivateProfileString("Device", "Pkg_Version", "0", strFilePath);
                System.Diagnostics.Process.Start("shutdown", @"/r /t 0");
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Border_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            Process.Start("cmd");
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            RegistryHelper rh2 = new RegistryHelper();
            rh2.SetRegistryData(Registry.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", "DisableTaskMgr", "");
            Thread.Sleep(500);
            Process.Start("taskmgr");
            Thread.Sleep(1000);
            rh2.SetRegistryData(Registry.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", "DisableTaskMgr", "1");
        }
    }
}
