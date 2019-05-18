/*
 * 
 * Data from https://archive.ics.uci.edu/ml/datasets/Flags
 * 
 * Target: identify, does data set have sunstar (22 attribute in matrix) atribute or not 
 * 
 * Aušra Čepulionytė, IFF-6/4 gr.
 * Martynas Švykas, IFF-6/1 gr.
 * Jonas Jermakovičius, IFF-6/1 gr.
 * 
 * NOTES.txt
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using MathNet.Numerics.LinearAlgebra;
using WindowsFormsApp1;
using System.Windows.Forms;

namespace Flags
{
	class Program
	{
		static Dictionary<string, int> Country = new Dictionary<string, int>();
		static Dictionary<string, string> ColorReplacements = new Dictionary<string, string> {
			{"black", "1"},
			{"blue", "2"},
			{"brown", "3"},
			{"gold", "4"},
			{"green", "5"},
			{"orange", "6"},
			{"red", "7"},
			{"white", "8"}
		};
		static Dictionary<double, string> Attributes = new Dictionary<double, string> {
			{0, "name"},
			{1, "landmass"},
			{2, "zone"},
			{3, "area"},
			{4, "population"},
			{5, "language"},
			{6, "religion"},
			{7, "bars"},
			{8, "stripes"},
			{9, "colours"},
			{10, "red"},
			{11, "green"},
			{12, "blue"},
			{13, "gold"},
			{14, "white"},
			{15, "black"},
			{16, "orange"},
			{17, "mainhue"},
			{18, "circles"},
			{19, "crosses"},
			{20, "saltires"},
			{21, "quarters"},
			{22, "sunstars"},
			{23, "crescent"},
			{24, "triangle"},
			{25, "icon"},
			{26, "animate"},
			{27, "text"},
			{28, "topleft"},
			{29, "botright"}
		};
		/*group a area attribute values*/
		static Dictionary<double, double> Attr3AreaGroups = new Dictionary<double, double> {
			{3,10000}, /*>10000*/
			{2,1000}, /*1000-10000*/
			{1,100}, /*100-1000*/
			{0,0} /*0-100*/
		};
		/*group a population attribute values*/
		static Dictionary<double, double> Attr4PopulationGroups = new Dictionary<double, double> {
			{3,1000}, /*>1000*/
			{2,100}, /*100-1000*/
			{1,10}, /*10-100*/
			{0,0} /*0-10*/
		};

		static void Main(string[] args)
		{
			string flagData = GetDataFromWeb();
			Matrix<double> dataMatrix = ConvertToDataMatrix(flagData);
			Matrix<double> dataMatrixNormalized = DataNormalization(dataMatrix);
			Matrix<double> covarianceMatrix = CovarianceMatrix(dataMatrixNormalized);

			Dictionary<double, double> covarianceSunStarSorted = SortedSunsetCovariation(covarianceMatrix);
			Dictionary<double, double> sortedByMostReflectAttributes = SortedByMostReflectAttributes(covarianceSunStarSorted);

			Console.WriteLine("Data matrix: \n\r" + dataMatrix.ToString());
			Console.WriteLine("Data matrix normalized: \n\r" + dataMatrixNormalized.ToString());
			Console.WriteLine("Covariance matrix: \n\r" + covarianceMatrix.ToString());
			Console.WriteLine("Covariance of Sunstar sorted attribute: \n\r" + String.Join("\n\r ", covarianceSunStarSorted));
			Console.WriteLine("Covariance of Sunstar sorted by most reflected attributes: \n\r" + String.Join("\n\r ", sortedByMostReflectAttributes));

			/*sortedByMostReflectAttributes key should be used for attributes selecion*/
			/*experiments of clasificator starts:*/
			List<Tuple<double, double>> xy = new List<Tuple<double, double>>();

			double dimensionsQuantity = dataMatrixNormalized.ColumnCount-1; /* experiments for clasificator*/
			double accurancy = 0;
			while (dimensionsQuantity>1)
			{
				Matrix<double> dataMatrixReduced = GetReducedMatrix(dataMatrixNormalized, dimensionsQuantity, sortedByMostReflectAttributes);

				/*clasifikatorius, experimentai, kryzmine patikra...*/

				accurancy = 1 / dimensionsQuantity * 100; /* priskirti klasifikatoriaus tiksluma*/
				xy.Add(new Tuple<double,double>(dimensionsQuantity, accurancy));
				dimensionsQuantity--;
				Console.WriteLine("Reduced Data matrix: \n\r" +  dataMatrixReduced.ToString());
			}
			DrawChart(xy);
			Console.ReadKey();
		}

		/*Draw accurancy chart*/
		static void DrawChart(List<Tuple<double, double>> xy)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1(xy));
		}

		/*Get dimensions reduced matrix*/
		static Matrix<double> GetReducedMatrix(Matrix<double> dataMatrixNormalized, double dimensionsQuantity, Dictionary<double, double> sortedByMostReflectAttributes)
		{
			Matrix<double> dataMatrixReduced = Matrix<double>.Build.Dense(dataMatrixNormalized.RowCount,(int) dimensionsQuantity+1);
			int counter = 1;
			dataMatrixReduced.SetColumn(0, dataMatrixNormalized.Column(22));
			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write("Attributes in use: ");
			foreach (KeyValuePair<double, double> kvp in sortedByMostReflectAttributes)
			{
				Console.Write("\t" + kvp.Key+"-"+ Attributes[kvp.Key]);
				dataMatrixReduced.SetColumn(counter, dataMatrixNormalized.Column((int)kvp.Key));
				counter++;
				if (counter >= dimensionsQuantity+1)
					break;
			}
			Console.Write("\n\r");
			Console.ForegroundColor = ConsoleColor.White;
			return dataMatrixReduced;
		}

		/*Get soreted by most reflect to sunset attributes: Return dictionary <real index of attribute , sorted probabilities>*/
		static Dictionary<double, double> SortedByMostReflectAttributes(Dictionary<double, double> covarianceSunStarSorted)
		{
			Dictionary<double, double> covarianceSunStarSortedAbs = new Dictionary<double, double>(); //nuokrypis
			foreach (KeyValuePair<double, double> kvp in covarianceSunStarSorted)
			{
				covarianceSunStarSortedAbs.Add(kvp.Key, Math.Abs(covarianceSunStarSorted[kvp.Key]));
			}
			covarianceSunStarSortedAbs= covarianceSunStarSortedAbs.OrderByDescending(x => x.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
			//var sortedDict = (from x in covarianceSunStarSortedAbs orderby x.Value descending select x).ToDictionary(pair => pair.Key, pair => pair.Value);
			return covarianceSunStarSortedAbs;
		}

		/*Get soreted by most reflect to sunset attributes: Return dictionary <real index of attribute , sorted probabilities>*/
		static Dictionary<double, double> SortedSunsetCovariation(Matrix<double> covarianceMatrix)
		{
			Dictionary<double, double> covarianceSunStarSorted = new Dictionary<double, double>();
			for (int i = 0; i < covarianceMatrix.RowCount; i++)
			{
				covarianceSunStarSorted.Add(i, covarianceMatrix[i, 22]);
			}
			covarianceSunStarSorted = covarianceSunStarSorted.OrderByDescending(x => x.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
			return covarianceSunStarSorted;
		}

		/* Calculate covariance*/
		static Matrix<double> CovarianceMatrix(Matrix<double> dataMatrix)
		{
			Matrix<double> covarianceMatrix = Matrix<double>.Build.Dense(dataMatrix.ColumnCount, dataMatrix.ColumnCount); /*matrix 194x30*/
			for (int i = 0; i < dataMatrix.ColumnCount - 1; i++)
			{
				for (int ii = i + 1; ii < dataMatrix.ColumnCount; ii++)
				{
					double cov = Covariance(dataMatrix.Column(i), dataMatrix.Column(ii));
					covarianceMatrix[i, ii] = cov;
					covarianceMatrix[ii, i] = cov;
					//covarianceMatrix[i, i] = Dispersion(dataMatrix.Column(i));
					covarianceMatrix[i, i] = 0;
				}
			}
			return covarianceMatrix;
		}

		/* Function to find covariance of two vectors*/
		static double Covariance(Vector<double> arr1, Vector<double> arr2)
		{
			int arr1Length = arr1.Count;
			int arr2Length = arr2.Count;
			if (arr1Length == arr2Length)
			{
				double mean1 = arr1.Average();
				double mean2 = arr2.Average();
				double sum = 0;
				for (int i = 0; i < arr1Length; i++)
					sum = sum + (arr1[i] - mean1) * (arr2[i] - mean2);
				return sum / (arr1Length - 1);
			}
			else return 0;
		}

		/* Function to find dispersion of vector*/
		static double Dispersion(Vector<double> arr)
		{
			int arrLength = arr.Count;
			double mean = arr.Average();
			double sum = 0;
			for (int i = 0; i < arrLength; i++)
				sum = sum + Math.Pow((arr[i] - mean), 2);
			return sum / (arrLength - 1);
		}

		/*Data min-max normalization to [0,1]: v'=(v-minA)/(maxA-minA)*/
		private static Matrix<double> DataNormalization(Matrix<double> dataMatrix)
		{
			Matrix<double> dataMatrixNormalized = Matrix<double>.Build.Dense(dataMatrix.RowCount, dataMatrix.ColumnCount); /*matrix 194x30*/
			for (int i = 0; i < dataMatrix.ColumnCount; i++)
			{
				dataMatrixNormalized.SetColumn(i, Normalization(dataMatrix.Column(i)));
			}
			return dataMatrixNormalized;
		}

		static Vector<double> Normalization(Vector<double> arr)
		{
			Vector<double> arrNormalized = Vector<double>.Build.Dense(arr.Count);
			double min = arr.Min();
			double max = arr.Max();
			for (int i = 0; i < arr.Count; i++)
				arrNormalized[i] = (arr[i] - min) / (max - min);
			return arrNormalized;
		}

		/* 
		 * Get data matrix:
		 * replace country name to index;
		 * replace color to color number
		*/
		private static Matrix<double> ConvertToDataMatrix(string flagData)
		{
			int countryCounter = 0;
			List<string> arrLines = flagData.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
			Matrix<double> dataMatrix = Matrix<double>.Build.Dense(arrLines.Count, 30); /*matrix 194x30*/
			foreach (string line in arrLines)
			{
				string newLine = Regex.Replace(line, string.Join("|", ColorReplacements.Keys.Select(k => k.ToString()).ToArray()), m => ColorReplacements[m.Value]); /*replace color to number*/
				string[] arr = newLine.Split(',').ToArray();
				Country.Add(arr[0], countryCounter); /*add country to dictionary: key=country, value=number*/
				arr[0] = countryCounter.ToString(); /*replace country to number*/
				if (int.Parse(arr[22]) > 0) /*if flag have >0 sunstars, then 1 (true), if ==0, then 0 (false) */
				{
					arr[22] = "1";
				}
				else
				{
					arr[22] = "0";
				}
				foreach (KeyValuePair<double, double> kvp in Attr3AreaGroups) /*group area*/
				{
					if (double.Parse(arr[3]) > kvp.Value)
					{
						arr[3] = kvp.Key.ToString();
						break;
					}
				}
				foreach (KeyValuePair<double, double> kvp in Attr4PopulationGroups) /*group population*/
				{
					if (double.Parse(arr[4]) > kvp.Value)
					{
						arr[4] = kvp.Key.ToString();
						break;
					}
				}
				dataMatrix.SetRow(countryCounter, Array.ConvertAll(arr, double.Parse)); /*parse from string to to int array and add to matrix row*/
				countryCounter++;
			}
			return dataMatrix;
		}

		/*Get flag data from web*/
		static string GetDataFromWeb()
		{
			WebClient client = new WebClient();
			string flagData = client.DownloadString("https://archive.ics.uci.edu/ml/machine-learning-databases/flags/flag.data");
			return flagData;
		}
	}
}
