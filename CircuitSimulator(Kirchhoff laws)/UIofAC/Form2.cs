using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SAC;

namespace UIofAC
{
    public partial class Form2 : Form
    {
        static int n=0;
        public Form2()
        {
            InitializeComponent();
            radioButton2.Select();
            textBox1.Text = n.ToString();
            textBox1.SelectAll();
            n++;
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 2)
            {
                ArtificialCircuit.typ = SAC.Type.R;
                textBox4.Enabled = true;
            }
            else if(comboBox1.SelectedIndex == 0)
            {
                ArtificialCircuit.typ = SAC.Type.U;
                textBox4.Enabled = false;
            }
            else
            {
                ArtificialCircuit.typ = SAC.Type.I;
                textBox4.Enabled = false;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("名称不能为空！","错误");
            }
            else if (comboBox1.SelectedIndex==-1)
            {
                MessageBox.Show("类型不能为空！", "错误");
            }
            else
            {
                //将输入的值传回窗体1,缺省值设为-1
                ArtificialCircuit.name = textBox1.Text;
                if (radioButton2.Checked)
                {
                    ArtificialCircuit.start = false;
                }
                else
                {
                    ArtificialCircuit.start = true;
                }
                if (textBox2.Text != "")
                {
                    ArtificialCircuit.u = Convert.ToDouble(textBox2.Text);
                }
                else
                {
                    ArtificialCircuit.u = -1;
                }
                if (textBox3.Text != "")
                {
                    ArtificialCircuit.i = Convert.ToDouble(textBox3.Text);
                }
                else
                {
                    ArtificialCircuit.i = -1;
                }
                if (textBox4.Text != "" && textBox4.Enabled==true)
                {
                    ArtificialCircuit.r = Convert.ToDouble(textBox4.Text);
                }
                else if(textBox4.Enabled == true)
                {
                    ArtificialCircuit.r = -1;
                }
                Close();
            }

        }
        //判断是否输入的是数字，不是的不让输入
        private void TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)8 && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void TextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)8 && e.KeyChar != '.')
            {
                e.Handled = true;
            }

        }

        private void TextBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)8 && e.KeyChar != '.')
            {
                e.Handled = true;
            }

        }
    }
}
