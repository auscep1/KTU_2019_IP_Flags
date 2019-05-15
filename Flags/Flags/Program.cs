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
 * NOTES.txt !!!
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra; /*zr. NOTES.txt*/
using MathNet.Numerics.LinearAlgebra.Double;

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
		static void Main(string[] args)
		{
			string flagData = GetDataFromWeb();
			Matrix<double> dataMatrix = ConvertToDataMatrix(flagData); 
			Matrix<double> dataMatrixWithoutSunstar = dataMatrix.RemoveColumn(22); /*without sunstar attribute*/

			Matrix<double> covarianceMatrix = CovarianceMatrix(dataMatrixWithoutSunstar);

			Console.WriteLine(dataMatrix.ToString());
			Console.WriteLine(covarianceMatrix.ToString());

			Console.ReadKey();
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
					covarianceMatrix[i, i] = Dispersion(dataMatrix.Column(i));
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
				sum = sum + Math.Pow((arr[i] - mean),2);
			return sum / (arrLength - 1);
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
