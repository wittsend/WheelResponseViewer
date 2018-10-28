using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace WheelResponseViewer
{
	//Data structure to store individual wheel data points.
	struct WheelDataPoint
	{
		public int force;
		public int startX;
		public int endX;
		public int deltaX;
		public double deltaXDeg;
	}

	struct NormDataPoint
	{
		public double input;
		public double output;
	}

	class WheelDataFile
	{


		//Global Class Data
		private string _FileName;
		private StreamReader _WDFile;
		private ArrayList _RawWheelData = new ArrayList();
		private ArrayList _NormalisedData = new ArrayList();
		private bool _FileLoaded = false;

		//Properties
		public string FileName                 //Read-Only File Name Property for the object
		{
			get
			{
				return _FileName;
			}
		}

		public string FileLoaded                 //Read-Only File Name Property for the object
		{
			get
			{
				return _FileLoaded;
			}
		}

		public ArrayList DataPoints                 //Read-Only Property for the object
		{
			get
			{
				return _NormalisedData;
			}
		}

		//Constructor
		public WheelDataFile()
		{

		}

		public bool OpenFile(string fileName)
		{
			string lineBuffer;
			string[] dataFields;
			int fieldNumber = 0;
			int checkSum = 0;

			this._FileName = fileName;

			//Code to open the file and load in data
			//Open the file
			_WDFile = File.OpenText(fileName);

			//Look at the data in the first line to ensure this is a valid file
			lineBuffer = _WDFile.ReadLine();
			string[] fileHeadings = lineBuffer.Split(',');

			foreach (string heading in fileHeadings)
			{
				string trimmed = heading.Trim();			//Remove whitespace
				switch (trimmed)
				{
					case "force":
						checkSum += 0x01;
						break;

					case "startX":
						checkSum += 0x02;
						break;

					case "endX":
						checkSum += 0x04;
						break;

					case "deltaX":
						checkSum += 0x08;
						break;

					case "deltaXDeg":
						checkSum += 0x10;
						break;
				}
			}

			//If the correct headings are not seen in the first row
			if (checkSum != 0x1F)
			{
				//Close the file
				_WDFile.Close();
				//Report an error
				MessageBox.Show("File Error", "Incorrect file type. Please open a log2 test file", MessageBoxButtons.OK, MessageBoxIcon.Error);
				//Exit the method
				return false;
			}

			WheelDataPoint convertedData;
			convertedData.force = 0;
			convertedData.startX = 0;
			convertedData.endX = 0;
			convertedData.deltaX = 0;
			convertedData.deltaXDeg = 0.0;

			//Now read the rest of the data into the object
			while(!_WDFile.EndOfStream)
			{
				lineBuffer = _WDFile.ReadLine();
				dataFields = lineBuffer.Split(',');
				fieldNumber = 0;
				foreach(string field in dataFields)
				{
					switch(fieldNumber)
					{
						//force value
						case 0:
							int.TryParse(field, out convertedData.force);
							break;

						//startX value
						case 1:
							int.TryParse(field, out convertedData.startX);
							break;

						//endX value
						case 2:
							int.TryParse(field, out convertedData.endX);
							break;

						//deltaX value
						case 3:
							int.TryParse(field, out convertedData.deltaX);
							break;

						//deltaXDeg value
						case 4:
							double.TryParse(field, out convertedData.deltaXDeg);
							break;
					}
					fieldNumber++;
				}

				_RawWheelData.Add(convertedData);
			}

			//Close the file
			_WDFile.Close();

			this.processData();

			_FileLoaded = true;

			return true;
		}

		private bool processData()
		{
			int maxForce = 0;
			int minForce = 0;
			int numOfPoints = 0;
			int maxDelta = 0;
			bool minValueFound = false;

			//Normalise the data and determine the deadzone
			foreach(WheelDataPoint point in _RawWheelData)
			{
				numOfPoints++;
				if (point.force > maxForce) maxForce = point.force;
				if (point.force > maxDelta) maxForce = point.deltaX;
				if (point.deltaX != 0) minValueFound = true;
				if (!minValueFound) minForce = point.force;
			}

			NormDataPoint normPoint;
			normPoint.input = 0.0;
			normPoint.output = 0.0;

			foreach(WheelDataPoint point in _RawWheelData)
			{
				normPoint.input = (double)point.force / (double)maxForce;
				normPoint.output = (double)point.deltaX / (double)maxDelta;
				_NormalisedData.Add(normPoint);
			}

			return true;
		}

		public bool graphData(System.Windows.Forms.DataVisualization.Charting.Chart chart, string series, string chartArea)
		{



			return true;
		}

	}
}
