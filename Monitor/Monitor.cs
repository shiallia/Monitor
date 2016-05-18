using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using System.Diagnostics;
//using System.Management;
using Microsoft.VisualBasic.Devices;
using System.Net.NetworkInformation;


//这里面添加了一些注释
namespace DeviceManager
{
    class Monitor : IDisposable
    {
        public delegate void CallbackEventHandler(float a, ulong b, string c, string d);

        public event CallbackEventHandler callback;

        public ulong GetPhysicalMemSize()
        {
            return cinf.TotalPhysicalMemory;
        }

        PerformanceCounter cpu;
        ComputerInfo cinf;

        public Monitor()
        {
            cpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            cinf = new ComputerInfo();
            
        }

        public void Run()
        {
            while (true)
            {
                //cpu占用
                var percentage = cpu.NextValue();
                //内存占用
                var mem = cinf.AvailablePhysicalMemory;

                //当前ip
                string localIP = string.Empty;
                NetworkInterface[] interfaces = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
                int len = interfaces.Length;
                for (int i = 0; i < len; i++)  
                {
                    NetworkInterface ni = interfaces[i];
                    if (ni.Name == "WLAN")
                    {
                        IPInterfaceProperties property = ni.GetIPProperties();
                        foreach (UnicastIPAddressInformation ip in property.UnicastAddresses)
                        {
                            if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            {
                                localIP = ip.Address.ToString();                                
                            }
                        }
                    }
                }

                //client进程内存占用
                Process[] myPro = Process.GetProcessesByName("client");
                string yzdMem = string.Empty;
                if(myPro.Length != 0)
                {
                    yzdMem = (myPro[0].PrivateMemorySize64 / 1024 / 1024).ToString();
                }
                
                callback(percentage, mem , localIP , yzdMem);

                System.Threading.Thread.Sleep(1000);
            }
        }

        public void Dispose()
        {
            cpu.Dispose();
        }
    }
}
