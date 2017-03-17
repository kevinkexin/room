using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using RoomPlusWebAPI.App_Code;
using WebApiBasicAuth.Filters;

namespace RoomPlusWebAPI.Controllers
{
    public class MainImagesController : ApiController
    {
		[IdentityBasicAuthentication]
		[System.Web.Http.Authorize]

		[System.Web.Http.HttpGet]
		[System.Web.Http.HttpPost]
		public IEnumerable<MainImages> GetAllMainImages()
		{
			MainImages[] mainpageImages = null;

			try
			{
				using (DataAccess da = new DataAccess())
				{
					DataTable dtData = da.GetMainImages();
					mainpageImages = new MainImages[dtData.Rows.Count];
					for (int i = 0; i < dtData.Rows.Count; i++)
					{

						mainpageImages[i] = new MainImages(Convert.ToInt32(dtData.Rows[i]["SID"]),
															   dtData.Rows[i]["PicName"].ToString(),
															   dtData.Rows[i]["PicURL"].ToString(),
															   Convert.ToInt32(dtData.Rows[i]["ServiceId"].ToString()),
															   Convert.ToInt32(dtData.Rows[i]["DetailId"].ToString()),
															   dtData.Rows[i]["Remark"].ToString(),
															   Convert.ToDateTime(dtData.Rows[i]["LastModifyDate"]));
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}

			return mainpageImages;
		}

		[System.Web.Http.HttpGet]
		[System.Web.Http.HttpPost]
		public IHttpActionResult GetMainImage(int id)
		{
			//var obj = mainpageImages.FirstOrDefault((p) => p.SID == id);
			MainImages MainImage = null;
			try
			{
				using (DataAccess da = new DataAccess())
				{
					DataTable dtData = da.GetMainImages(id);

					if (dtData.Rows.Count > 0)
						MainImage = new MainImages(Convert.ToInt32(dtData.Rows[0]["SID"]),
																dtData.Rows[0]["PicName"].ToString(),
																dtData.Rows[0]["PicURL"].ToString(),
																Convert.ToInt32(dtData.Rows[0]["ServiceId"].ToString()),
																Convert.ToInt32(dtData.Rows[0]["DetailId"].ToString()),
																dtData.Rows[0]["Remark"].ToString(),
																Convert.ToDateTime(dtData.Rows[0]["LastModifyDate"]));

				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}

			if (MainImage == null)
				return NotFound();
			else
				return Ok(MainImage);
			//return MainImage;
		}
    }
}
