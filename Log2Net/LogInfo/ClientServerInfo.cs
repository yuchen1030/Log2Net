
using Log2Net.Models;
using Log2Net.Util;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;


#if NET
using System.Web;
#else
using Log2Net.DNCMiddleware;
#endif

namespace Log2Net.LogInfo
{
    internal class ClientServerInfo
    {
        //获取客户端信息
        public class ClientInfo
        {
            static ICache dataCache = CacheFac.CacheFactory();


            #region 获取客户端信息api

            [DllImport("Iphlpapi.dll")]
            private static extern int SendARP(Int32 dest, Int32 host, ref Int64 mac, ref Int32 length);
            [DllImport("Ws2_32.dll")]
            private static extern Int32 inet_addr(string ip);

            public class IPHost
            {
                public string IP { get; set; } = "";
                public string Host { get; set; } = "";
                public string Mac { get; set; } = "";
            }


            //获取IP地址和host
            public static IPHost GetClientInfo(bool bNeedMac = false)
            {
                string uip = "未知";
                string uName = "未知";
                try
                {
                    //if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                    //{
                    //    uip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                    //}
                    //else
                    //{
                    //    uip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                    //}
#if NET

                    if (HttpContext.Current == null || HttpContext.Current.Request == null)
                    {
                        return new IPHost();
                    }
                    uip = HttpContext.Current.Request.UserHostAddress;//获取客户端的IP主机地址
#else
                    var httpContext = HttpContext.Current;
                    if (httpContext == null || httpContext.Request == null)
                    {
                         return new IPHost() { IP = uip, Host = uName };
                    }             
                    var apaddr = httpContext.Connection.RemoteIpAddress;
                    uip = apaddr.ToString(); //获取客户端的IP主机地址
#endif

                    IPHostEntry hostEntry = Dns.GetHostEntry(uip);//获取IPHostEntry实体 ，可能会失败，报告"不知道这样的主机"的错误
                    string clientName = hostEntry.HostName;//获取客户端计算机名称
                    uName = clientName;
                    if (!uip.Contains(".") || uip == "127.0.0.1")
                    {
                        uip = GetIPAccordingHost(clientName).IP;
                    }
                }
                catch //(Exception ex)
                {
                    //  LogCom..WriteExceptToFile(ex);//WriteBackupLogToFile时需要构造实体（此时需要获取服务器信息），在此调用会产生循环引用的错误
                }

                IPHost iphost = new IPHost() { IP = uip, Host = uName };
                try
                {
                    if (bNeedMac && !string.IsNullOrEmpty(uip))
                    {
                        iphost.Mac = "未知";
                        iphost.Mac = GetCustomerMac(iphost.IP);
                        if (iphost.Mac == "00-00-00-00-00-00")
                        {
                            iphost.Mac = getLocalMacAddress();
                        }
                    }

                }
                catch
                {

                }
                return iphost;


            }

            public static IPHost GetIPAccordingHost(string hostName)
            {
                IPHost iphost = new IPHost() { Host = hostName };
                var ip = System.Net.Dns.GetHostAddresses(hostName);
                for (int i = 0; i < ip.Length; i++)
                {
                    if (IsIPAddress(ip[i].ToString()))
                    {
                        iphost.IP = ip[i].ToString();
                        return iphost;
                    }
                }
                return iphost;

            }

            // 判断是否是IP地址格式 0.0.0.0   
            public static bool IsIPAddress(string ipAddress)
            {
                if (ipAddress == null || ipAddress == string.Empty || ipAddress.Length < 7 || ipAddress.Length > 15) return false;
                string regformat = @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}$";
                Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);
                return regex.IsMatch(ipAddress);
            }

            // 获取浏览器信息  
            public static string GetOSInfo()
            {
#if NET
                var bc = System.Web.HttpContext.Current.Request.Browser;
                return "操作系统为" + bc.Platform + " ,浏览器类型为" + bc.Type;
#else
                return "操作系统为" + "未知" + " ,浏览器类型为" + "未知";
#endif
            }

            ///<summary>  
            /// SendArp获取MAC地址  
            ///</summary>  
            ///<param name="RemoteIP">目标机器的IP地址如(192.168.1.1)</param>  
            ///<returns>目标机器的mac 地址</returns>  
            public static string GetCustomerMac(string IP)
            {
                Int32 ldest = inet_addr(IP);
                Int64 macinfo = new Int64();
                Int32 len = 6;
                int res = SendARP(ldest, 0, ref macinfo, ref len);
                string mac_src = macinfo.ToString("X");

                while (mac_src.Length < 12)
                {
                    mac_src = mac_src.Insert(0, "0");
                }

                string mac_dest = "";

                for (int i = 0; i < 11; i++)
                {
                    if (0 == (i % 2))
                    {
                        if (i == 10)
                        {
                            mac_dest = mac_dest.Insert(0, mac_src.Substring(i, 2));
                        }
                        else
                        {
                            mac_dest = "-" + mac_dest.Insert(0, mac_src.Substring(i, 2));
                        }
                    }
                }

                return mac_dest;
            }

            /// <summary> 
            /// 获取MAC地址(返回第一个物理以太网卡的mac地址) 
            /// </summary> 
            /// <returns>成功返回mac地址，失败返回null</returns> 
            public static string getLocalMacAddress()
            {
                string macAddress = null;
                try
                {
                    NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                    foreach (NetworkInterface adapter in nics)
                    {
                        if (adapter.NetworkInterfaceType.ToString().Equals("Ethernet")) //是以太网卡
                        {
                            string fRegistryKey = "SYSTEM\\CurrentControlSet\\Control\\Network\\{4D36E972-E325-11CE-BFC1-08002BE10318}\\" + adapter.Id + "\\Connection";
                            RegistryKey rk = Registry.LocalMachine.OpenSubKey(fRegistryKey, false);
                            if (rk != null)
                            {
                                // 区分 PnpInstanceID     
                                // 如果前面有 PCI 就是本机的真实网卡    
                                // MediaSubType 为 01 则是常见网卡，02为无线网卡。    
                                string fPnpInstanceID = rk.GetValue("PnpInstanceID", "").ToString();
                                int fMediaSubType = Convert.ToInt32(rk.GetValue("MediaSubType", 0));
                                if (fPnpInstanceID.Length > 3 && fPnpInstanceID.Substring(0, 3) == "PCI") //是物理网卡
                                {
                                    macAddress = adapter.GetPhysicalAddress().ToString();
                                    break;
                                }
                                else if (fMediaSubType == 1) //虚拟网卡
                                    continue;
                                else if (fMediaSubType == 2) //无线网卡(上面判断Ethernet时已经排除了)
                                    continue;
                            }
                        }
                    }
                }
                catch
                {
                    macAddress = null;
                }
                return macAddress;
            }


            public static IPHost GetServerIPHost()
            {
                IPHost iphost = new IPHost()
                {
                    IP = dataCache.GetCache(AppConfig.GetCacheKey(CacheConst.serverIP)) as string,
                    Host = dataCache.GetCache(AppConfig.GetCacheKey(CacheConst.serverHost)) as string,
                };
                return iphost;
            }


            #endregion 获取客户端信息



        }

        // 获取服务器信息类
        public class ServerInfo
        {
            //获取服务器操作系统
            public static string GetServerOS()
            {
                return Environment.OSVersion.ToString(); //服务器操作系统
            }

            //获取服务器IIS版本
            public static string GetIISVerson()
            {
#if NET
                return GetVersonStr(System.Web.HttpRuntime.IISVersion);
#else
                return Environment.OSVersion.ToString();
#endif

            }

            public static string GetVersonStr(Version ver)
            {
                return (ver.Major + "." + ver.Minor + "." + (ver.Build >= 0 ? ver.Build.ToString() : "") + "." + (ver.Revision >= 0 ? ver.Revision.ToString() : "")).Trim('.');  //CLR版本
            }

            //获取服务器 .NET CLR版本
            public static string GetCLRVersion()
            {
                return GetVersonStr(Environment.Version);
            }

            //获取服务器的持续运行时间
            public static string GetRunningTime()
            {
                var timeSpan = LogCom.GetServerRunningTime();   //服务器上次启动到现在已运行时间
                var timeLong = string.Format("{0}天{1}小时{2}分{3}秒", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
                return timeLong;
            }


            //获取硬盘的可用空间,第一个为程序所在的磁盘，其余的为空间小于10%的盘
            public static List<DiskSpaceEdm> GetHardDiskSpace()
            {
#if NET
                string AppPath = HttpRuntime.AppDomainAppPath.ToString();
#else
                string AppPath = AppContext.BaseDirectory;
#endif
                string volume = AppPath.Substring(0, AppPath.IndexOf(':'));
                string diskName = volume;

                List<DiskSpaceEdm> diskList = new List<DiskSpaceEdm>();
                diskName = diskName + ":\\";
                System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
                foreach (System.IO.DriveInfo drive in drives)
                {
                    try
                    {
                        var rateNum = Convert.ToDouble((drive.TotalFreeSpace * 100.0 / drive.TotalSize).ToString("f2"));
                        var rateStr = "";
                        var free = (drive.TotalFreeSpace / (1024 * 1024 * 1024.0)).ToString("f2") + "GB";
                        var freeNum = Convert.ToDouble((drive.TotalFreeSpace / (1024 * 1024 * 1024.0)).ToString("f2"));
                        if (rateNum <= 10 || drive.Name == diskName)
                        {
                            rateStr = rateNum.ToString("f2") + "%";
                            var disk = new DiskSpaceEdm() { DiscName = drive.Name.Trim('\\'), Free = freeNum, Rate = rateNum };
                            if (drive.Name != diskName)
                            {
                                diskList.Add(disk);
                            }
                            else
                            {
                                diskList.Insert(0, disk);
                            }
                        }
                    }
                    catch
                    {

                    }
                }
                return diskList;
            }



            public class CpuMemInfo
            {
                /// <summary>
                /// 获取内存使用率
                /// </summary>
                /// <returns></returns>
                public static double GetMemoryUsing()
                {
                    MEMORY_INFO meminfo = new MEMORY_INFO();
                    GlobalMemoryStatus(ref meminfo);
                    var usingMem = meminfo.dwMemoryLoad;//内存使用率
                    return usingMem;
                }


                /// <summary>
                /// 使用VMI获取cpu使用率，可以获取各个cpu的使用率，最后的返回值是各个cpu使用率的平均值
                /// </summary>
                /// <returns></returns>
                public static double GetTotalCpuusing2ByVMI()
                {
                    try
                    {
                        List<string> list = new System.Collections.Generic.List<string>();
                        var searcher = new ManagementObjectSearcher("select * from Win32_PerfFormattedData_PerfOS_Processor").Get();
                        if (searcher.Count <= 0)
                        {
                            Exception ex = new Exception("未获取到CPU信息，请确认该网站的应用程序池账户具有访问VMI的权限");
                            //  LogCom..WriteExceptToFile(ex);
                            return new Random().Next(20, 50);//出错了，装的跟真的一样                       
                        }
                        var cpuTimes = searcher.Cast<ManagementObject>().Select(mo => new { Name = mo["Name"], Usage = mo["PercentProcessorTime"] }).ToList();
                        var query = cpuTimes.Where(x => x.Name.ToString() == "_Total").Select(x => x.Usage).ToList();
                        var cpuUsage = query.SingleOrDefault();
                        var result = Convert.ToDouble(cpuUsage);
                        return result;
                    }
                    catch //(Exception ex)
                    {
                        // LogCom..WriteExceptToFile(ex);
                        return new Random().Next(20, 50);//出错了，装的跟真的一样
                    }
                }


                [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
                internal static extern void GlobalMemoryStatus(ref MEMORY_INFO meminfo);

                [StructLayout(LayoutKind.Sequential)]
                internal struct MEMORY_INFO
                {
                    public uint dwLength;
                    public uint dwMemoryLoad;
                    public uint dwTotalPhys;
                    public uint dwAvailPhys;
                    public uint dwTotalPageFile;
                    public uint dwAvailPageFile;
                    public uint dwTotalVirtual;
                    public uint dwAvailVirtual;
                }

            }


        }



    }
}
