using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAC;

namespace artificial_circuit
{
    class Demo
    {
        static void Main(string[] args)
        {
            //测试1通过，初始化函数已更改！
            //int[,] adjmat = { { 0,1,1,0,0}, { 0,0,0,1,1}, { 0,0,0,1,1}, { 1,0,0,0,0 },{ 1, 0, 0, 0, 0 } };
            //Element[] group = new Element[5];
            //group[0] = new Element(true, SAC.Type.U, 5, -1); group[0].Name = "U0";
            //group[1] = new Element(SAC.Type.R, -1, -1); group[1].Name = "U1";
            //group[2] = new Element(SAC.Type.R, -1, -1); group[2].Name = "U2";
            //group[3] = new Element(SAC.Type.R, -1, -1); group[3].Name = "U3";
            //group[4] = new Element(SAC.Type.R, -1, -1); group[4].Name = "U4";
            //Circuit test = new Circuit(group, adjmat);
            //Console.WriteLine("The KVL of the circuit:");
            //test.Loop(0);
            //test.KCL();

            //测试2通过
            int[,] adjmat = { { 0, 1, 1, 1, 0, 0 }, { 0, 0, 0, 0, 1, 0 }, { 0, 0, 0, 0, 1, 0 }, { 0, 0, 0, 0, 0, 1 }, { 0, 0, 0, 0, 0, 1 }, { 1, 0, 0, 0, 0, 0 } };
            Element[] group = new Element[6];
            group[0] = new Element(SAC.Type.U,"0"); group[0].Start = true;
            group[1] = new Element(SAC.Type.R, "1"); 
            group[2] = new Element(SAC.Type.R, "2"); 
            group[3] = new Element(SAC.Type.R, "3"); 
            group[4] = new Element(SAC.Type.R, "4");
            group[5] = new Element(SAC.Type.R, "5");
            Circuit test = new Circuit(group, adjmat);
            Console.WriteLine("The KVL of the circuit:");
            test.Loop(0);
            test.KCL();
            test.VCR();
        }
    }
}
