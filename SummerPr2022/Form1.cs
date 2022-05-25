﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace SummerPr2022
{
    public partial class Form1 : Form
    {
        public int n;
        public int a = 1, b = 2;
        const double x0 = 1, y0 = 0, minX = 1, maxX = 2;
        ZedGraphControl zedGrapgControl1 = new ZedGraphControl();
        public Form1()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            zedGrapgControl1.Location = new Point(8, 30);
            zedGrapgControl1.Name = "text";
            zedGrapgControl1.Size = new Size(500, 500);
            Controls.Add(zedGrapgControl1);
            GraphPane my_Pane = zedGrapgControl1.GraphPane;
            my_Pane.Title.Text = "Результат:";
            my_Pane.XAxis.Title.Text = "X";
            my_Pane.YAxis.Title.Text = "Y";
        }
        private void GetSize()
        {
            zedGrapgControl1.Location = new Point(10, 10);
            zedGrapgControl1.Size = new Size(ClientRectangle.Width - 20, ClientRectangle.Height - 20);
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            GetSize();
        }

        private void Clear(ZedGraphControl Zed_GraphControl)
        {
            zedGrapgControl1.GraphPane.CurveList.Clear();
            zedGrapgControl1.GraphPane.GraphObjList.Clear();

            zedGrapgControl1.GraphPane.XAxis.Type = AxisType.Linear;
            zedGrapgControl1.GraphPane.XAxis.Scale.TextLabels = null;
            zedGrapgControl1.GraphPane.XAxis.MajorGrid.IsVisible = false;
            zedGrapgControl1.GraphPane.YAxis.MajorGrid.IsVisible = false;
            zedGrapgControl1.GraphPane.YAxis.MinorGrid.IsVisible = false;
            zedGrapgControl1.GraphPane.XAxis.MinorGrid.IsVisible = false;
            zedGrapgControl1.RestoreScale(zedGrapgControl1.GraphPane);

            zedGrapgControl1.AxisChange();
            zedGrapgControl1.Invalidate();
        }

        static double f1(double x, double y)//Исходное дифференциальное уравнение
        {
            //return (-(x*x*x*Math.Sin(y)) + x)/2*y;
            return 1.0/(-2*y/(x*x*x*Math.Sin(y) - x));
        }
        static double f2(double y)//Точное решение задачи Коши 
        {
            return Math.Sqrt(y / (-Math.Cos(y) + Math.PI - 1));
        }

        private void Eiler(ZedGraphControl Zed_GraphControl)//сам метод ломанных Эйлера
        {
            GraphPane my_Pane = Zed_GraphControl.GraphPane;
            PointPairList list1 = new PointPairList();
            PointPairList list2 = new PointPairList();

            n = Convert.ToInt32(textBox1.Text);
            double maxNev = 0, h, x, y, curNev;

            const int maxY = 12;

            x = 1; y = Math.PI;
            h = 1.0/n;
            bool b = true;
            for (;y < maxY;)
            {
                y += h;
                x += h * f1(x, y);

                curNev = 0;

                if ((x >= 1 && x <= 2) && b)
                {
                    curNev = Math.Abs(x- f2(y));
                    list1.Add(x, y);
                }
                    
                else if (x >= 1 && x <= 2)
                {
                    curNev = Math.Abs(x - f2(y));
                    list2.Add(x, y);
                } 
                else
                    b = false;
                if (curNev > maxNev)
                    maxNev = curNev;

            }

            LineItem d1 = my_Pane.AddCurve("Метода Эйлера", list1, Color.Blue, SymbolType.None);
            LineItem d2 = my_Pane.AddCurve("", list2, Color.Blue, SymbolType.None);
            textBox2.Text = maxNev.ToString();
            zedGrapgControl1.AxisChange();
            zedGrapgControl1.Invalidate();
        }

        private void Rez(ZedGraphControl Zed_GraphControl)//Построение графика точного решения
        {
            const int maxY = 12;

            GraphPane my_Pane = Zed_GraphControl.GraphPane;

            PointPairList list1 = new PointPairList();
            PointPairList list2 = new PointPairList();

            bool b = true;
            int i = 0;
            double x, y;

            for (double k = Math.PI; k < maxY; k+=0.01, i++)
            {
                y = k;
                x = f2(y);

                if((x >= 1 && x <= 2) && b)
                    list1.Add(x, y);
                else if (x >= 1 && x <= 2)
                    list2.Add(x, y);
                else
                    b = false;
                
                
            }
            LineItem myCircle = my_Pane.AddCurve("Точное решение", list1, Color.Red, SymbolType.None);
            LineItem myCircle2 = my_Pane.AddCurve("",list2, Color.Red, SymbolType.None);

            zedGrapgControl1.AxisChange();
            zedGrapgControl1.Invalidate();
        }
        private void GriddenOn(GraphPane my_Pane)
        {
            //my_Pane.XAxis.MajorGrid.IsVisible = true;
            //my_Pane.YAxis.MajorGrid.IsVisible = true;
            //my_Pane.YAxis.MinorGrid.IsVisible = true;
            //my_Pane.XAxis.MinorGrid.IsVisible = true;
        }
        private void button1_Click(object sender, EventArgs e)//Эйлер
        {
            GriddenOn(zedGrapgControl1.GraphPane);
            Eiler(zedGrapgControl1);
        }
        private void button3_Click_1(object sender, EventArgs e)//точное
        {
            GriddenOn(zedGrapgControl1.GraphPane);
            Rez(zedGrapgControl1);
        }
        private void button2_Click(object sender, EventArgs e)//чистка
        {
            Clear(zedGrapgControl1);
            GriddenOn(zedGrapgControl1.GraphPane);
        }
    }
}

