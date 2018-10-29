using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
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
		private ArrayList _CorrectedData = new ArrayList();
		private bool _FileLoaded = false;

		private int _maxForce = 0;
		private int _minForce = 0;
		private int _maxDelta = 0;
		private const int _rFactorFFBRange = 11500;

		//Properties
		public string FileName                 //Read-Only File Name Property for the object
		{
			get
			{
				return _FileName;
			}
		}

		public bool FileLoaded                 //Read-Only File Name Property for the object
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

		public int NumberOfPoints                 //Read-Only Property for the object
		{
			get
			{
				return _RawWheelData.Count;
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
			//The maximum output value for the game we are creating a correction profile for
			int maxOutputValue = 1000;
			//The sample in the normalised data where the deadzone ends.
			int startSample = 0;

			//Find max values of force and deltaX for normalisation
			foreach(WheelDataPoint point in _RawWheelData)
			{
				if (point.force > _maxForce) _maxForce = point.force;
				if (point.force > _maxDelta) _maxDelta = point.deltaX;
			}

			//Scan from right to left until the first 0 value is seen. This marks the start of
			//the deadzone.
			for(int i = _RawWheelData.Count - 1; i >= 0; i--)
			{
				if(((WheelDataPoint)_RawWheelData[i]).deltaX <= 0)
				{
					_minForce = ((WheelDataPoint)_RawWheelData[i]).force;
					startSample = i;
					break;
				}
			}

			//Normalise the raw data ready for creating the correction data
			NormDataPoint normPoint;
			normPoint.input = 0.0;
			normPoint.output = 0.0;

			foreach(WheelDataPoint point in _RawWheelData)
			{
				normPoint.input = (double)point.force / (double)_maxForce;
				normPoint.output = (double)point.deltaX / (double)_maxDelta;
				_NormalisedData.Add(normPoint);
			}

			double outStart, outEnd, inStart, inEnd;
			int steps;


			//Now lets create a new set with the correction data
			//Interpolate the normalised data to the max value used by the software
			for (int i = startSample; i < _NormalisedData.Count - 1; i++)
			{
				
				outStart = ((NormDataPoint)_NormalisedData[i]).input * maxOutputValue;
				outEnd = ((NormDataPoint)_NormalisedData[i + 1]).input * maxOutputValue;
				inStart = ((NormDataPoint)_NormalisedData[i]).output * maxOutputValue;
				inEnd = ((NormDataPoint)_NormalisedData[i + 1]).output * maxOutputValue;

				//Work out the number of steps in the current interval:
				steps = (int)(inEnd - inStart);

				//Interpolate the values for the current interval and store them
				for(int j = 0; j < steps; j++)
				{
					//Recycling normPoint
					normPoint.input = j + (int)inStart;
					normPoint.output = (int)(((outEnd - outStart) / steps) * j + outStart);
					_CorrectedData.Add(normPoint);
				}


			}


			return true;
		}

		public bool graphData(ref Chart chart, string series, string chartArea)
		{
			
			if (_FileLoaded)
			{
				//Alter the chart area so that the axis ranges are 0-100%
				//chart.ChartAreas[chartArea].AxisX.Maximum = 100;
				//chart.ChartAreas[chartArea].AxisX.Minimum = 0;
				//chart.ChartAreas[chartArea].AxisY.Maximum = 100;
				//chart.ChartAreas[chartArea].AxisY.Minimum = 0;
				
				//Set the chart area axis titles
				chart.ChartAreas[chartArea].AxisX.Title = "Input force %";
				chart.ChartAreas[chartArea].AxisY.Title = "Output force %";
				
				//Load the data into the chart
				chart.Series[series].ChartArea = chartArea;
				chart.Series[series].ChartType = SeriesChartType.Line;
				
				foreach(NormDataPoint normPoint in _NormalisedData)
				{
					chart.Series[series].Points.AddXY(normPoint.input * 100, normPoint.output * 100);
				}

				return true;
			}

			return false;
		}

	}
}
