/*
 * 
 * Data from https://archive.ics.uci.edu/ml/datasets/Flags
 * 
 * Aušra Čepulionytė, IFF-6/4 gr.
 * Martynas Švykas, IFF-6/1 gr.
 * Jonas Jermakovičius, IFF-6/1 gr.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Flags
{


	class Program
	{
		static void Main(string[] args)
		{
			string flagData = GetDataFromWeb();
			List<object> obj = GetDataMatrix(flagData);
		}

		/*Get data matrix*/
		private static List<object> GetDataMatrix(string flagData)
		{
			List<string> arr = flagData.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
			List<object> obj = new List<object>();
			foreach (var item in arr)
			{
				obj.Add(item.Split(','));
			}
			return obj;
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
