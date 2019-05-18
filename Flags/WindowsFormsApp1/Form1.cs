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
		public Form1(List<Tuple<double, double>> XY)
		{
			InitializeComponent();
			ClearForm();
			PreparareForm(0, 30, 0, 100);
			Fx = chart1.Series.Add("Tikslumas");
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
				//Fx.Points[index].AxisLabel = "#VAL \n #PERCENT";
				index++;
			}
			Fx.BorderWidth = 3;
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
