using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SAC
{
    /// <summary>
    /// 总的来说是根据图结构进行改进得出的电路类
    /// </summary>
    public class Circuit
    {
        private const int Infinity = Int16.MaxValue;
        private IList<Element> nodes;
        private int[,] mat;
        //初始化函数，运用邻接表
        public Circuit(IList<Element> nodes,int[,]mat)
        {
            this.mat = mat;
            this.nodes = nodes;
            int i, j;
            int nOfNodes = mat.GetLength(0);
            for (i = 0; i < nOfNodes; i++)
            {
                for (j = 0; j < nOfNodes; j++)
                {
                    if (mat[i, j] != 0 && mat[i, j] != Infinity)
                    {
                        nodes[i].Neighbors.Add(nodes[j]);
                    }
                }
            }
        }
        public int Count
        {
            get
            {
                return nodes.Count;
            }
        }
        public double this[int i,Type ty]
        {
            get
            {
                if (i >= 0 && i < Count)
                {
                    if (ty.Equals(Type.I))
                        return nodes[i].Current;
                    else
                        return nodes[i].Voltage;
                }
                else
                {
                    throw new IndexOutOfRangeException("Out of Range" + this.GetType());
                }
            }
            set
            {
                if (i >= 0 && i < Count)
                {
                    if (ty.Equals(Type.I))
                        nodes[i].Current=value;
                    else
                        nodes[i].Voltage=value;
                }
                else
                {
                    throw new IndexOutOfRangeException("Out of Range" + this.GetType());
                }
            }
        }
        public void AddNode(Type ty,string name,double u,double i)
        {
            nodes.Add(new Element(ty, name,u, i));
        }
        public void AddNode(Element node)
        {
            nodes.Add(node);
        }
        public void AddEdge(Element from,Element to)
        {
            from.Neighbors.Add(to);
        }
        public int IndexOf(Element k)//返回索引
        {
            int j = 0;
            while(j< Count && !k.Name.Equals(nodes[j].Name))
            {
                j++;               
            }
            if (j >= 0 && j < Count)
            {
                return j;
            }
            else
            {
                return -1;
            }
        }
        /// <summary>
        /// 下方为核心代码KCL，KVL
        /// </summary>
        //用来储存KVL路径的list
        private List<string> all=new List<string>();
        //现阶段只能将编号为0（m=0）的元件设置为开始元件（本意是任意都可以设为开始元件的，但后来发现由于用的
        //是单向表，故不能实现）
        //正如KVL进行的原理，以一个为起点进行回转，一圈就是一个方程，转完所有圈就必定可给出所有电压关系
        public void Loop(int m)
        {
            int j;
            int i;
            if (nodes[m].Start == true)
            {
                all.Add("U"+nodes[m].Name+"=");
            }
            else
            {
                all.Add("U" + nodes[m].Name + "+");
            }
            for (j = 0; j < nodes[m].Neighbors.Count; j++)//核心，遍历每个元件的下一个连接件，
                                                                                        //重复调用可遍历所有路径
            {
                i = IndexOf(nodes[m].Neighbors[j]);//返回下一个元件的序号
                if (nodes[i].Start == false)//如果下一个不是开头元件
                {
                    Loop(i);
                }
                else
                {
                    foreach(string unit in all)//下一个回到开头了，说明完成了一次KVL
                    {
                        Console.Write(unit);
                    }
                    Console.Write('\u0008');//用于清除最后一个加号（多余的）
                    Console.Write(" ");
                    Console.WriteLine();
                }
            }
            all.RemoveAt(all.Count - 1);//该元件的所有相关KVL已记录，‘删除’该元件
        }
        public void KCL()
        {
            Console.WriteLine("The KCL of the circuit:");
            var friend =FindSameN();
            for(int k=0;k<friend.Count-1;k++)
            {
                if (friend[k] != -1 && friend[k+1]!=-1) //判断为分隔符前几个
                {
                    Console.Write("I" + nodes[friend[k]].Name + "+");   //注意代号是friend[k]                
                }
                else if (friend[k] != -1) //判断为分隔符前一个
                {
                    Console.Write("I" + nodes[friend[k]].Name + "=");
                    foreach(Element outE in nodes[friend[k]].Neighbors)
                    {
                        if (outE.Equals(nodes[friend[k]].Neighbors.Last()) != true)
                            Console.Write("I" + outE.Name + "+");
                        else
                            Console.Write("I" + outE.Name);
                    }
                    Console.WriteLine();
                }
            }
        }
        //KCL关键在判断各个元件的下位元件是否相同，相同就是同一个节点上的元件，属于同一个方程
        public List<int> FindSameN()
        {
            List<int> friend = new List<int>();//用于记录输入电流
            int nOfNodes = mat.GetLength(0);//获取行数
            List<int> ban = new List<int>(nOfNodes);
            int i, j;            
            for (i = 0; i < nOfNodes; i++)
            {
                if (ban.IndexOf(i) != -1)//如果i在封禁名单里，则跳过，含重复的邻居的会被封禁
                {
                    continue;
                }
                for (j = i; j < nOfNodes; j++)
                {
                    if (nodes[i].Neighbors.SequenceEqual(nodes[j].Neighbors)) //判断邻居（就是输出电流）是否相同
                    {
                        friend.Add(j);
                        ban.Add(j);
                    }
                }
                friend.Add(-1);// 分隔符
            }
            return friend;
        }
        public void VCR()
        {
            Console.WriteLine("The VCR of the circuit:");
            foreach(Element r in nodes)
            {
                if (r.Type == SAC.Type.R)
                {
                    Console.WriteLine("U" + r.Name + "/I" + r.Name + "=R" + r.Name);
                }
            }
        }
        //下面是重复的，为winform设计
        public void LoopForForm(int m,TextBox txb)
        {
            int j;
            int i;
            if (nodes[m].Start == true)
            {
                all.Add("U" + nodes[m].Name + "=");
            }
            else
            {
                all.Add("U" + nodes[m].Name + "+");
            }
            for (j = 0; j < nodes[m].Neighbors.Count; j++)//核心，遍历每个元件的下一个连接件，
                                                          //重复调用可遍历所有路径
            {
                i = IndexOf(nodes[m].Neighbors[j]);//返回下一个元件的序号
                if (nodes[i].Start == false)//如果下一个不是开头元件
                {
                    LoopForForm(i,txb);///递归，改函数的时候记得一起改！！！
                }
                else
                {
                    foreach (string unit in all)//下一个回到开头了，说明完成了一次KVL
                    {
                        txb.Text += unit;
                    }
                    var delete = txb.Text.Substring(0, txb.Text.Length - 1);
                    txb.Text = delete;
                    txb.AppendText("\r\n");
                }
            }
            all.RemoveAt(all.Count - 1);//该元件的所有相关KVL已记录，‘删除’该元件
        }
        public void KCLForForm(TextBox txb)
        {
            txb.AppendText("The KCL of the circuit:\r\n");
            var friend = FindSameN();
            for (int k = 0; k < friend.Count - 1; k++)
            {
                if (friend[k] != -1 && friend[k + 1] != -1) //判断为分隔符前几个
                {
                    txb.AppendText("I" + nodes[friend[k]].Name + "+");   //注意代号是friend[k]                
                }
                else if (friend[k] != -1) //判断为分隔符前一个
                {
                    txb.AppendText("I" + nodes[friend[k]].Name + "=");
                    foreach (Element outE in nodes[friend[k]].Neighbors)
                    {
                        if (outE.Equals(nodes[friend[k]].Neighbors.Last()) != true)
                            txb.AppendText("I" + outE.Name + "+");
                        else
                            txb.AppendText("I" + outE.Name);
                    }
                    txb.AppendText("\r\n");
                }
            }
        }

    }
}
