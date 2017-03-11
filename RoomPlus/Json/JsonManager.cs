using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace com.atombooster.roomplus
{
	public class JsonManager
	{
		private static JsonManager _jsonManger;

		private static readonly object _singletonLock = new object();
		private System.Threading.Thread _executeTaskThread;
		private System.Threading.AutoResetEvent _autoCondition = new System.Threading.AutoResetEvent(false);

		#region Data 
		private MainImages[] _MainPageImages { get; set; }
		private WeatherData[] _WeatherDate { get; set; }
		#endregion

		private string url = string.Empty; //"deviceStartup";
		private RequestName requestName { get; set; }

		internal static JsonManager Instance
		{
			get
			{
				lock (JsonManager._singletonLock)
				{
					if (JsonManager._jsonManger == null)
					{
						JsonManager._jsonManger = new JsonManager();
					}

					return JsonManager._jsonManger;
				}
			}
			set
			{
				JsonManager._jsonManger = value;
			}
		}

		/// <summary>
		/// Gets the main images.
		/// </summary>
		/// <returns>The main images.</returns>
		public MainImages[] GetMainImages()
		{
			this.requestName = RequestName.RequestMainImages;
			url = SystemValue.PublicURL + "api/MainPageImages";

			this.ExcuteRequest(null);
			return this._MainPageImages;
		}

		/// <summary>
		/// Excutes the actual request.
		/// </summary>
		/// <param name="param">Parameter.</param>
        async private void ExcuteRequest(object param)
		{
			try
			{
				string strRequest = PrepareRequest(param);

				object obj = await this.SendRequest(strRequest);

				if (obj != null)
				{
					switch (requestName)
					{
						case RequestName.RequestMainImages:
							this._MainPageImages = (MainImages[])obj;
							break;

						case RequestName.RequestWeather:
							this._WeatherDate = (WeatherData[])obj;
							break;
						case RequestName.RequestServiceGroup:
							//this._ServiceGroup = (ServiceGroup[])obj;
							break;
						case RequestName.RequestServices:
							//this._Services = (Services[])obj;
							break;
						case RequestName.RequestServiceDetail:
							//this._ServiceDetail = (ServiceDetail[])obj;
							break;
						case RequestName.SendServiceRequest:
							//this._TransResponse = (TransactionResponse)obj;
							break;
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}

		/// <summary>
		/// Sends the request to the remote server
		/// </summary>
		/// <returns>The request.</returns>
		/// <param name="RequestJson">Request json.</param>
		async private System.Threading.Tasks.Task<object> SendRequest(string RequestJson)
		{
			object obj = null;
			try
			{
				//if (string.IsNullOrEmpty(RequestJson))
				//    return null;

				HttpClient httpClient = new HttpClient();
				//httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				//if (!string.IsNullOrEmpty(SessionId) && this.requestName != RequestName.KeepAlive)
				httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
													   "Basic", SystemValue.ClientId + ":" + SystemValue.SecrectKey); //testuser:Pass1word


				httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				HttpContent content = new StringContent(RequestJson);
				content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

				//LogHandler.AuditLog(UserManager.Email, requestName + " @ " + RequestJson);
				//LogHandler.AuditLog(UserManager.Email, "URL: " + url);

				////////
				var resp = httpClient.PostAsync(new Uri(url, UriKind.Absolute), content);
				string response = await resp.Result.Content.ReadAsStringAsync().ConfigureAwait(false);

				///////
				//JsonConvert json_serializer = new JavaScriptSerializer();
				//LogHandler.AuditLog(UserManager.Email, "Response: " + response);

				switch (requestName)
				{
					case RequestName.RequestMainImages:
						obj = JsonConvert.DeserializeObject<MainImages[]>(response);//  json_serializer.Deserialize<DeviceStartupResponse>(response);
						break;
					case RequestName.RequestWeather:
						//obj = JsonConvert.DeserializeObject<WeatherData[]>(response);//  json_serializer.Deserialize<DeviceStartupResponse>(response);
						break;
					case RequestName.RequestServiceGroup:
						//obj = JsonConvert.DeserializeObject<ServiceGroup[]>(response);
						break;
					case RequestName.RequestServices:
						//obj = JsonConvert.DeserializeObject<Services[]>(response);
						break;
					case RequestName.RequestServiceDetail:
						//obj = JsonConvert.DeserializeObject<ServiceDetail[]>(response);
						break;
					case RequestName.SendServiceRequest:
						//obj = JsonConvert.DeserializeObject<TransactionResponse>(response);
						break;
				}
			}
			catch (Exception ex)
			{

				Console.WriteLine(ex.ToString());
			}

			return obj;
		}

		private string PrepareRequest(object param)
		{
			string strRequestJson = string.Empty;

			try
			{
				switch (requestName)
				{
					case RequestName.RequestMainImages:
						//Do nothing                       
						strRequestJson = string.Empty;
						break;

					case RequestName.RequestWeather:
						strRequestJson = string.Empty;
						break;
					case RequestName.RequestServiceGroup:
						//strRequestJson = string.Empty;
						//GeneralParameter para = (GeneralParameter)param;
						//strRequestJson = JsonConvert.SerializeObject(para);
						break;
					case RequestName.RequestServices:
						//strRequestJson = string.Empty;
						//GeneralParameter para1 = (GeneralParameter)param;
						//strRequestJson = JsonConvert.SerializeObject(para1);
						break;
					case RequestName.RequestServiceDetail:
						//strRequestJson = string.Empty;
						//GeneralParameter para2 = (GeneralParameter)param;
						//strRequestJson = JsonConvert.SerializeObject(para2);
						break;
					case RequestName.SendServiceRequest:
						//TransactionRequest request = (TransactionRequest)param;
						//strRequestJson = JsonConvert.SerializeObject(request);
						break;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}

			return strRequestJson;
		}
	}
}
