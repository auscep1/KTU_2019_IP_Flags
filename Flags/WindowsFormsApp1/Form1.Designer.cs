using System.Drawing;

namespace WindowsFormsApp1
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		public System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		public void InitializeComponent()
		{
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
			System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
			this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
			((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
			this.SuspendLayout();
			// 
			// chart1
			// 
			chartArea1.Name = "ChartArea1";
			this.chart1.ChartAreas.Add(chartArea1);
			legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
			legend1.Name = "Legend1";
			this.chart1.Legends.Add(legend1);
			this.chart1.Location = new System.Drawing.Point(13, 13);
			this.chart1.Name = "chart1";
			series1.ChartArea = "ChartArea1";
			series1.Legend = "Legend1";
			series1.Name = "Series1";
			this.chart1.Series.Add(series1);
			this.chart1.Size = new System.Drawing.Size(1070, 464);
			this.chart1.TabIndex = 0;
			this.chart1.Text = "chart1";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1095, 489);
			this.Controls.Add(this.chart1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		public System.Windows.Forms.DataVisualization.Charting.Chart chart1;
		public void PreparareForm(float xmin, float xmax, float ymin, float ymax)
		{

			float x_grids = 10;
			//double xmin = 0; double xmax = 2 * Math.PI;
			chart1.ChartAreas[0].AxisX.MajorGrid.Interval = (xmax - xmin) / x_grids;
			chart1.ChartAreas[0].AxisX.LabelStyle.Interval = (xmax - xmin) / x_grids;
			chart1.ChartAreas[0].AxisX.MajorTickMark.Interval = (xmax - xmin) / x_grids;
			chart1.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Courier New", 8, FontStyle.Bold);

			float y_grids = 10;
			//double ymin = -1; double ymax = 1;
			chart1.ChartAreas[0].AxisY.MajorGrid.Interval = (ymax - ymin) / y_grids;
			chart1.ChartAreas[0].AxisY.LabelStyle.Interval = (ymax - ymin) / y_grids;
			chart1.ChartAreas[0].AxisY.MajorTickMark.Interval = (ymax - ymin) / y_grids;
			chart1.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Courier New", 8, FontStyle.Bold);

			chart1.ChartAreas[0].AxisX.Minimum = xmin;
			chart1.ChartAreas[0].AxisX.Maximum = xmax;
			chart1.ChartAreas[0].AxisY.Minimum = ymin;
			chart1.ChartAreas[0].AxisY.Maximum = ymax;

			chart1.Legends[0].Font = new Font("Times New Roman", 12, FontStyle.Bold);
			chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
			chart1.ChartAreas[0].CursorX.Interval = 0.01;
			chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
			chart1.ChartAreas[0].CursorY.Interval = 0.01;

		}
	}
}

