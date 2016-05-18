using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

//测试内容
namespace MyMonitor
{
    /// <summary>
    /// SettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingWindow : Window
    {

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filepath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retval, int size, string filePath);

        private string strFilePath = @"d:/client/config/devicetag.ini";//获取INI文件路径
        

        private string ContentValue(string Section, string key)
        {

            StringBuilder temp = new StringBuilder(1024);
            GetPrivateProfileString(Section, key, "", temp, 1024, strFilePath);
            return temp.ToString();
        }

        public SettingWindow()
        {
            InitializeComponent();

            if (File.Exists(strFilePath))//读取时先要判读INI文件是否存在
            {

                
                textBoxIp.Text = ContentValue("NET", "Ip");
                textBoxMask.Text = ContentValue("NET", "NetMask");
                textBoxGateway.Text = ContentValue("NET", "GateWay");
                textBoxDns.Text = ContentValue("NET", "DNS");
                textBoxServer.Text = ContentValue("Device", "ServerAddr");
                



            }
            else {

                MessageBox.Show("INI文件不存在");
                this.Close();

            }

           


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            string ip = textBoxIp.Text;
            string mask = textBoxMask.Text;
            string gateway = textBoxGateway.Text;
            string dns = textBoxDns.Text;
            string server = textBoxServer.Text;

            WritePrivateProfileString("NET", "Ip", ip, strFilePath);
            WritePrivateProfileString("NET", "NetMask", mask, strFilePath);
            WritePrivateProfileString("NET", "GateWay", gateway, strFilePath);
            WritePrivateProfileString("NET", "DNS", dns, strFilePath);
            WritePrivateProfileString("Device", "ServerAddr", server, strFilePath);


            this.Close();
            System.Diagnostics.Process.Start("shutdown", @"/r /t 0");
        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();  
            }
        }
    }
}
