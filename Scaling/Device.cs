using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Scaling
{
    /// <summary>
    /// 每种设备代表不同的X1,X2,Y1,Y2策略
    /// </summary>
    public class Device
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double X1 { get; set; }
        public double X2 { get; set; }
        public double Y1 { get; set; }
        public double Y2 { get; set; }

    }

    public static class Devices
    {
        private static readonly string Fp = System.Windows.Forms.Application.StartupPath + "\\Device.json";
        private static readonly List<Device> DefaultDevices = new List<Device>
        {
            new Device
            {
                Name = "WAGO 750-452 750-453 750-552 750-553",
                Description = "0~20mA输入输出",
                X1 = 0,
                X2 = 32767,
                Y1 = 0,
                Y2 = 20
            },
            new Device
            {
                Name = "WAGO 750-454 750-554",
                Description = "4~20mA输入输出",
                X1 = 0,
                X2 = 32767,
                Y1 = 4,
                Y2 = 20
            },
            new Device
            {
                Name = "WAGO 750-456 750-457 750-556 750-557",
                Description = "-10~10V输入输出",
                X1 = 0,
                X2 = 32767,
                Y1 = -10,
                Y2 = 10
            },
            new Device
            {
                Name = "Beckhoff ES3124 ES4124",
                Description = "4~20mA输入输出",
                X1 = 0,
                X2 = 32767,
                Y1 = 4,
                Y2 = 20
            }
        };

        public static List<Device> ReadDevices()
        {
            if (!File.Exists(Fp))
            {
                //文件不存在时写入默认配置
                WriteDevices(DefaultDevices);
                return DefaultDevices;
            }    
            return JsonConvert.DeserializeObject<List<Device>>(File.ReadAllText(Fp));
        }

        public static void WriteDevices(List<Device> devices)
        {
            if (devices == null || devices.Count == 0)
                devices = DefaultDevices;
            File.WriteAllText(Fp, JsonConvert.SerializeObject(devices));
        }
    }
}
