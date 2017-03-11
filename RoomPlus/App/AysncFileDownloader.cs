using System;
using System.IO;
using System.Net;
using UIKit;

namespace RoomPlus
{
	public class AysncFileDownloader
	{
		WebClient webClient;

		/// <summary>
		/// Downloads the file (images) async.
		/// </summary>
		/// <param name="URL">URL.</param>
		/// <param name="FileName">File name.</param>
		async void DownloadAsync(string URL, string FileName)
		{
			webClient = new WebClient();
			//An large image url
			var url = new Uri(URL);
			byte[] bytes = null;

			webClient.DownloadProgressChanged += HandleDownloadProgressChanged;

			//Start download data using DownloadDataTaskAsync
			try
			{
				bytes = await webClient.DownloadDataTaskAsync(url);
			}
			catch (OperationCanceledException)
			{
				Console.WriteLine("Task Canceled!");
				return;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				return;
			}


			//Download Completed
			string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			string localFilename = FileName;
			string localPath = Path.Combine(documentsPath, localFilename);

			//Save the image using writeAsync
			FileStream fs = new FileStream(localPath, FileMode.OpenOrCreate);
			await fs.WriteAsync(bytes, 0, bytes.Length);

			Console.WriteLine("localPath:" + localPath);

			//everthing thing is done
		}

		void HandleDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			//e.ProgressPercentage / 100.0f;
		}
	}
}
