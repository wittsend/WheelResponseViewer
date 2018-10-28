using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WheelResponseViewer
{
	public partial class main : Form
	{
		public main()
		{
			InitializeComponent();
		}

		private void cmdOpenFile_Click(object sender, EventArgs e)
		{
			string fileName;

			dlgOpenFile = new System.Windows.Forms.OpenFileDialog();
			if (dlgOpenFile.ShowDialog() == DialogResult.OK)
			{
				fileName = dlgOpenFile.FileName;
				WheelDataFile wdf = new WheelDataFile();
				if (!wdf.OpenFile(fileName))
				{
					lblFileState.Text = "No file open";
					//Delete the wdf object
				}

				//Update the file status label
				lblFileState.Text = wdf.FileName;

				//Populate the graph
				wdf.graphData(ref graph, "response", "area"); 
			}
		}

		private void graph_Click(object sender, EventArgs e)
		{
			
		}
	}
}
