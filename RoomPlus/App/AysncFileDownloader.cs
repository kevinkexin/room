using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using CoreGraphics;
using Foundation;
using UIKit;

namespace com.atombooster.roomplus
{
	public static class AysncFileDownloader
	{
		static WebClient webClient;

		public delegate void ImageIsReadyHandler(UIImage image);
		public static ImageIsReadyHandler ImageIsReady;


		#region Async Functions
		/// <summary>
		/// Downloads the file (images) async.
		/// </summary>
		/// <param name="URL">URL.</param>
		/// <param name="FileName">File name.</param>
		async static void DownloadAsync(string URL, string FileName)
		{
			webClient = new WebClient();
			//An large image url
			var url = new Uri(URL);
			byte[] bytes = null;

			//webClient.DownloadProgressChanged += HandleDownloadProgressChanged;

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

		/// <summary>
		/// Downloads the file (images) async.
		/// </summary>
		/// <param name="URL">URL.</param>
		/// <param name="FileName">File name.</param>
		public async static void DownloadImageAsync(string URL, CGSize size)
		{
			webClient = new WebClient();
			//An large image url
			var url = new Uri(URL);
			byte[] bytes = null;

			webClient.DownloadFileCompleted += HandleDownloadPFileCompleted(bytes, size);

			//Start download data using DownloadDataTaskAsync
			try
			{
				bytes = await webClient.DownloadDataTaskAsync(url);
			}
			catch (OperationCanceledException)
			{
				Console.WriteLine("Task Canceled!");
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
		}

		/*
		void HandleDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			//e.ProgressPercentage / 100.0f;
		}
		*/

		static AsyncCompletedEventHandler HandleDownloadPFileCompleted(byte[] Imagebyte, CGSize size)
		{
			Action<object, AsyncCompletedEventArgs> action = (sender, e) =>
			{
				//var _filename = filename;

				if (e.Error != null)
				{
					Console.WriteLine(e.Error.ToString());
					//throw e.Error;
				}
				//if (!_downloadFileVersion.Any())
				//{
				//	 complited = true;
				//}
				//DownloadFile();

				//Need to convert the bytes to UIImage for later display
				if (Imagebyte != null)
				{
					if (ImageIsReady != null)
					{
						UIImage image = ImageFromBytes(Imagebyte, size.Width, size.Height);
						ImageIsReady(image);
					}
				}
			};

			return new AsyncCompletedEventHandler(action);
		}

		#endregion

		static private UIImage ImageFromBytes(byte[] bytes, nfloat width, nfloat height)
		{
			try
			{
				NSData data = NSData.FromArray(bytes);
				UIImage image = UIImage.LoadFromData(data);
				CGSize scaleSize = new CGSize(width, height);
				UIGraphics.BeginImageContextWithOptions(scaleSize, false, 0);
				image.Draw(new CGRect(0, 0, scaleSize.Width, scaleSize.Height));
				UIImage resizedImage = UIGraphics.GetImageFromCurrentImageContext();
				UIGraphics.EndImageContext();
				return resizedImage;
			}
			catch (Exception)
			{
				return null;
			}
		}
	}
}
