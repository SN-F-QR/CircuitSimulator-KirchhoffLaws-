using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using SAC;

namespace UIofAC
{
    public partial class ArtificialCircuit : Form
    {
        static int[,] mat;
        public static SAC.Type typ;
        public static string name;
        public static double u;
        public static double i;
        public static double r;
        public static bool start;
        private Element[] group;
        private Circuit cc;
        public ArtificialCircuit()
        {
            InitializeComponent();
        }
        private void 打开电路ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog path = new OpenFileDialog();
            path.ShowDialog();
            string spath = path.FileName;
            FileStream fs = new FileStream(spath, FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            string line = sr.ReadLine();
            string[] stringArray;
            char[] charArray = new char[] { ',' };//用于分割的标记
            stringArray = line.Split(charArray);//分割line
            int num = stringArray.Length;
            mat = new int[num, num];
            int n = 0;
            int m = 0;
            while (line != null)
            {
                for (n = 0; n < num; n++)
                {
                    mat[m, n] = Convert.ToInt32(stringArray[n]);
                }
                line = sr.ReadLine();
                if (line != null)
                    stringArray = line.Split(charArray);
                m++;
            }
            sr.Close();
            fs.Close();
            MessageBox.Show("打开成功", "Tip");
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            group = new Element[mat.GetLength(0)];
            for(int i = 0; i < mat.GetLength(0); i++)//通过循环录入元件值
            {
                Form2 f2 = new Form2();
                f2.ShowDialog();//先执行小窗口
                group[i] = new Element(start,typ, name, u, i);
                if(typ==SAC.Type.R && r != -1)
                {
                    group[i].Resist = r;
                }
            }
            cc= new Circuit(group, mat);
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            textBox1.Text += "The KVL of the circuit:\r\n";
            cc.LoopForForm(0, textBox1);               
        }
        private void Button3_Click(object sender, EventArgs e)
        {
            cc.KCLForForm(textBox1);
        }
        //保存文本框的内容
        private void 保存ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string txtname = "result at " + DateTime.Now.ToString() + ".txt";
            txtname = txtname.Replace(":", "-"); //替换非法字符
            txtname = txtname.Replace("/", "-");
            if (textBox1.Text != "")
            {
                FileStream fs = new FileStream(txtname, FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(textBox1.Text);
                sw.Close();
                fs.Close();
                MessageBox.Show("保存成功,文件储存在 ...\\Artificial Circuit\\UIofAC\\bin\\Debug", "Tip");
            }
            else
            {
                MessageBox.Show("还未生成相关方程");
            }
        }

        private void 新建电路ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label1.Text = "请在此输入元件关系表：";
            label1.Visible = true;
            button4.Visible = true;
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This is a task of the Circuit Analysis lesson.\r\n" +
                "Its main function is to show the Kirchhoff laws of the circuit(Pure Resistance!) you input." +
                " The program is naive and easy.^ .^\r\n" +
                "by SN-F-QR","About this program");
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            FileStream fs = new FileStream("NewCircuit.txt", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            string txt=textBox1.Text.Substring(0, textBox1.Text.Length);
            sw.Write(txt);
            sw.Close();
            fs.Close();
            textBox1.Text = "";
            MessageBox.Show("新建成功！", "Tip");
            button4.Visible = false;
            label1.Visible = false;
        }

        private void 帮助HelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("使用步骤：\r\n" +
                                          "1.打开或新建元件关系表\r\n" +
                                          "2.录入元件信息\r\n" +
                                          "3.KVL/KCL后进行保存\r\n" +
                                          "请严格按照README.md中的操作和规范进行以保证没有错误发生！\r\n"+
                                          "Please read README.md to get more information!","Help");
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
