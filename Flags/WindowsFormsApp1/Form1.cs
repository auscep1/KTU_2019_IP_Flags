using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp1
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}
		Series Fx;
		public Form1(List<Tuple<double, double>> XY, int iterations)
		{
			InitializeComponent();
			ClearForm();
			PreparareForm(0, 30, 0, 100);
			Fx = chart1.Series.Add("Tikslumas");
			chart1.Titles.Add(string.Format("Kryžminė patikra, kai iteracijų {0}", iterations)).Font = new System.Drawing.Font("Arial", 16, FontStyle.Bold);
			Fx.ChartType = SeriesChartType.Line;
			Fx.IsValueShownAsLabel = true;
			int index = 0;
			foreach (Tuple<double, double> xy in XY)
			{
				Fx.Points.AddXY(xy.Item1, xy.Item2);
				Fx.Points[index].MarkerStyle = MarkerStyle.Circle;
				Fx.Points[index].MarkerSize = 8;
				Fx.Points[index].MarkerColor = Color.Red;
				Fx.Points[index].Label = "#VALY{#.##}" + " % ";
				index++;
			}
			Fx.BorderWidth = 3;
		}

		Series[] Fxarr;
		public Form1(double[,] XY, List<double> xAxis)
		{
			xAxis.Sort();
			Fxarr = new Series[XY.GetLength(1)];
			InitializeComponent();
			ClearForm();
			PreparareForm(0, 30, 0, 100);
			Fxarr[XY.GetLength(1) - 1] = chart1.Series.Add("Kryžminės patikros tikslumas");
			Fxarr[XY.GetLength(1) - 1].ChartType = SeriesChartType.Line;
			//Fxarr[XY.GetLength(1) - 1].IsValueShownAsLabel = true;
			for (int i = 0; i < XY.GetLength(1) - 1; i++)
			{
				Fxarr[i] = chart1.Series.Add("Iteracija " + (i + 1));
				Fxarr[i].ChartType = SeriesChartType.Line;
				//	Fxarr[i].IsValueShownAsLabel = true;
			}
			int index = 0;
			chart1.Titles.Add("Klasifikatoriaus tikslumas, kai iteracijų " + (XY.GetLength(1) - 1)).Font = new System.Drawing.Font("Arial", 16, FontStyle.Bold);
			for (int c = 0; c < XY.GetLength(0); c++)
			{
				for (int r = 0; r < XY.GetLength(1); r++)
				{
					Fxarr[r].Points.AddXY(xAxis[c], XY[c, r]);
					//Fxarr[r].Points[index].MarkerStyle = MarkerStyle.Circle;
					//Fxarr[r].Points[index].MarkerSize = 8;
					//Fxarr[r].Points[index].MarkerColor = Color.Red;
					//if (r == XY.GetLength(1) - 1)
					//	Fxarr[r].Points[index].Label = "#VALY{#.##}" + " % ";
				}
				index++;
			}
			for (int i = 0; i < XY.GetLength(1); i++)
			{
				if (i == XY.GetLength(1) - 1)
				{
					Fxarr[i].Color = Color.Red;
					Fxarr[i].BorderWidth = 4;
				}
				else Fxarr[i].BorderWidth = 2;

			}
		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}

		public void ClearForm()
		{
			chart1.Series.Clear();
		}
	}
}
