using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//初始化函数待修改
namespace SAC
{
    public enum Type { U, I, R }
    public class Element
    {
        private string name;  //名字
        private Type type;    //类型，电压源，电流源，电阻
        private double voltage; //电压  通常未知的我设置为-1
        private double current; //电流
        private double resist;  //可能存在的电阻
        private List<Element> neighbors = null; //其下位元件
        private bool start = false; //是否为“开始”元件,只能是元件关系矩阵中编号为0的元件！
        //下方为成员属性
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        public double Voltage
        {
            get
            {
                return voltage;
            }
            set
            {
                voltage = value;
            }
        }
        public double Current
        {
            get
            {
                return current;
            }
            set
            {
                current = value;
            }
        }
        public double Resist//不是所有元件都会有电阻，因此其访问有条件，未想到更好的方法
        {
            get
            {
                if (this.type.Equals(Type.R))
                {
                    return resist;
                }
                else
                {
                    throw new Exception("这个东西没有电阻！");
                }
            }
            set
            {
                if (this.type.Equals(Type.R))
                {
                     resist=value;
                }
                else
                {
                    throw new Exception("这个东西没有电阻！" );
                }
            }

        }
        public List<Element> Neighbors
        {
            get
            {
                if (neighbors == null)
                    neighbors = new List<Element>();
                return neighbors;
            }
            set
            {
                neighbors = value;
            }
        }
        public bool Start
        {
            get { return start; }
            set { start = value; }
        }
        public Type Type
        {
            get { return type; }
            set { type = value; }
        }
        public Element(bool st,Type el,string name,double u,double i,List<Element> nb= null)
        {
            this.neighbors = nb;
            this.start = st;
            this.type = el;
            this.current = i;
            this.voltage = u;
            this.name = name;
        }
        public Element(Type el,string name,double u=-1, double i=-1) : this(false, el,name,u, i) { }    
    }

}
