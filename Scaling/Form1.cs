using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Scaling
{
    public partial class Form1 : Form
    {
        private List<Device> _devices;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _devices = new List<Device>(Devices.ReadDevices());
            foreach (var device in _devices)
            {
                comboBox1.Items.Add(device.Name);
            }
            CalGainOffset();
            toolTip1.SetToolTip(comboBox1, "请选择一个设备!");
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //选中设备时替换值
            var device = _devices[comboBox1.SelectedIndex];
            numericUpDownX1.Value = (decimal)device.X1;
            numericUpDownX2.Value = (decimal)device.X2;
            numericUpDownA1.Value = (decimal)device.Y1;
            numericUpDownA2.Value = (decimal)device.Y2;
            toolTip1.ToolTipTitle = device.Name;
            toolTip1.SetToolTip(comboBox1, device.Description);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                numericUpDownA1.ReadOnly = false;
                numericUpDownA2.ReadOnly = false;
                numericUpDownA3.ReadOnly = false;
                numericUpDownA4.ReadOnly = false;
            }
            else
            {
                numericUpDownA1.ReadOnly = true;
                numericUpDownA2.ReadOnly = true;
                numericUpDownA3.ReadOnly = true;
                numericUpDownA4.ReadOnly = true;
            }

            CalGainOffset();
        }

        private void numericUpDownX1_ValueChanged(object sender, EventArgs e)
        {
            CalGainOffset();
        }

        private void CalGainOffset()
        {
            decimal gain, offset;
            try
            {
                if (checkBox1.Checked)
                {
                    //先求出X和A1的关系，再带入A1和Y的关系
                    var gain1 = (numericUpDownA2.Value - numericUpDownA1.Value) /
                                (numericUpDownX2.Value - numericUpDownX1.Value);
                    var offset1 = numericUpDownA2.Value - gain1 * numericUpDownX2.Value;
                    var gain2 = (numericUpDownY2.Value - numericUpDownY1.Value) /
                           (numericUpDownA4.Value - numericUpDownA3.Value);
                    var offset2 = numericUpDownY2.Value - gain2 * numericUpDownA4.Value;
                    gain = gain1 * gain2;
                    offset = gain2 * offset1 + offset2;
                }
                else
                {
                    gain = (numericUpDownY2.Value - numericUpDownY1.Value) /
                           (numericUpDownX2.Value - numericUpDownX1.Value);
                    offset = numericUpDownY2.Value - gain * numericUpDownX2.Value;
                }
            }
            catch
            {
                gain = 0;
                offset = 0;
            }
            
            textBoxGain.Text = gain.ToString();
            textBoxOffset.Text = offset.ToString();
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {
            
        }
    }
}
