namespace WheelResponseViewer
{
	partial class main
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

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
		private void InitializeComponent()
		{
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
			System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
			this.graph = new System.Windows.Forms.DataVisualization.Charting.Chart();
			this.dlgOpenFile = new System.Windows.Forms.OpenFileDialog();
			this.cmdOpenFile = new System.Windows.Forms.Button();
			this.lblFileState = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.graph)).BeginInit();
			this.SuspendLayout();
			// 
			// graph
			// 
			chartArea1.Name = "area";
			this.graph.ChartAreas.Add(chartArea1);
			legend1.Enabled = false;
			legend1.Name = "legend";
			this.graph.Legends.Add(legend1);
			this.graph.Location = new System.Drawing.Point(34, 12);
			this.graph.Name = "graph";
			series1.ChartArea = "area";
			series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
			series1.Legend = "legend";
			series1.Name = "response";
			this.graph.Series.Add(series1);
			this.graph.Size = new System.Drawing.Size(556, 316);
			this.graph.TabIndex = 0;
			this.graph.Text = "Response Plot";
			title1.Name = "title";
			title1.Text = "Wheel Output vs Input";
			this.graph.Titles.Add(title1);
			this.graph.Click += new System.EventHandler(this.graph_Click);
			// 
			// dlgOpenFile
			// 
			this.dlgOpenFile.DefaultExt = "csv";
			this.dlgOpenFile.FileName = "dlgOpenFile";
			// 
			// cmdOpenFile
			// 
			this.cmdOpenFile.Location = new System.Drawing.Point(34, 347);
			this.cmdOpenFile.Name = "cmdOpenFile";
			this.cmdOpenFile.Size = new System.Drawing.Size(86, 55);
			this.cmdOpenFile.TabIndex = 1;
			this.cmdOpenFile.Text = "Open WheelCheck file";
			this.cmdOpenFile.UseVisualStyleBackColor = true;
			this.cmdOpenFile.Click += new System.EventHandler(this.cmdOpenFile_Click);
			// 
			// lblFileState
			// 
			this.lblFileState.AutoSize = true;
			this.lblFileState.Location = new System.Drawing.Point(31, 331);
			this.lblFileState.Name = "lblFileState";
			this.lblFileState.Size = new System.Drawing.Size(64, 13);
			this.lblFileState.TabIndex = 2;
			this.lblFileState.Text = "No file open";
			// 
			// main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(633, 418);
			this.Controls.Add(this.lblFileState);
			this.Controls.Add(this.cmdOpenFile);
			this.Controls.Add(this.graph);
			this.Name = "main";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.graph)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataVisualization.Charting.Chart graph;
		private System.Windows.Forms.OpenFileDialog dlgOpenFile;
		private System.Windows.Forms.Button cmdOpenFile;
		private System.Windows.Forms.Label lblFileState;
	}
}

