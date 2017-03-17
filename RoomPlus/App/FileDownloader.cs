using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using CoreGraphics;
using Foundation;
using UIKit;

namespace com.atombooster.roomplus
{
	public static class FileDownloader
	{
		static WebClient webClient;

		public static UIImage DownloadImage(string URL, CGSize size)
		{
			if (URL.StartsWith("/", StringComparison.InvariantCultureIgnoreCase))
				URL = URL.Substring(1, URL.Length - 1);
			
			URL = SystemValue.PublicURL + URL;
			
			webClient = new WebClient();
			//An large image url
			var url = new Uri(URL);
			byte[] bytes = null;

			//webClient.DownloadProgressChanged += HandleDownloadProgressChanged;

			//Start download data using DownloadDataTaskAsync
			try
			{
				bytes = webClient.DownloadData(url);
			}
			catch (OperationCanceledException)
			{
				Console.WriteLine("Task Canceled!");
				return null;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				return null;
			}


			if (bytes != null)
				return ImageFromBytes(bytes, size.Width, size.Height);
			else
				return null;
		}

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
