﻿/*
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
		//static Dictionary<string, int> ColorReplacements = new Dictionary<string, int>
		//{
		//	{"black",1},
		//	{"blue",2},
		//	{"brown",3},
		//	{"gold",4},
		//	{"green",5},
		//	{"orange",6},
		//	{"red",7},
		//	{"white",8}
		};
		static List<string> colorNo = new List<string>();
		static void Main(string[] args)
		{
			string flagData = GetDataFromWeb();
			Matrix<double> dataMatrix = ConvertToDataMatrix(flagData);
			Console.ReadKey();
		}

		/*Function to find mean*/
		static float Mean(float[] arr, int n)
		{
			float sum = 0;
			for (int i = 0; i < n; i++)
				sum = sum + arr[i];
			return sum / n;
		}

		/* Function to find covariance cov(X,Y)=1/n*SUMn((Xi-X')*(Yi-Y'))*/
		static float Covariance(float[] arr1, float[] arr2)
		{
			int arr1Length = arr1.Length;
			int arr2Length = arr2.Length;
			if (arr1Length == arr2Length)
			{
				float sum = 0;
				for (int i = 0; i < arr1Length; i++)
					sum = sum + (arr1[i] - Mean(arr1, arr1Length)) * (arr2[i] - Mean(arr2, arr1Length));
				return sum / (arr1Length - 1);
			}
			else return 0;
			// Console.WriteLine(Covariance(arr1, arr2, m));
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
			Matrix<double> dataMatrix = Matrix<double>.Build.Dense(arrLines.Count, 30);
			foreach (string line in arrLines)
			{
				string newLine = Regex.Replace(line, string.Join("|", ColorReplacements.Keys.Select(k => k.ToString()).ToArray()), m => ColorReplacements[m.Value]); /*replace color to number*/
				string[] arr = newLine.Split(',').ToArray();
				Country.Add(arr[0], countryCounter); /*add country to dictionary: key=country, value=number*/
				arr[0] = countryCounter.ToString(); /*replace country to number*/
				dataMatrix.SetRow(countryCounter, Array.ConvertAll(arr, double.Parse)); /*parse from string to to int array and add to matrix row*/
				countryCounter++;
			}
			Console.WriteLine(dataMatrix.ToString());
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
