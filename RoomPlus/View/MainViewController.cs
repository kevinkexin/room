using Foundation;
using System;
using UIKit;
using CoreGraphics;
using System.Drawing;

namespace com.atombooster.roomplus
{
	
    public partial class MainViewController : UIViewController
    {
		private JsonManager _jManager;
        public MainViewController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
		}

		public override void ViewDidLoad()
		{ 
			base.ViewDidLoad();
			Xamarin.iOS.DGActivityIndicatorViewBinding.DGActivityIndicatorView activityIndicatorView = null;
			try
			{
				#region Show a Loading Image
				activityIndicatorView = new Xamarin.iOS.DGActivityIndicatorViewBinding.DGActivityIndicatorView(Xamarin.iOS.DGActivityIndicatorViewBinding.DGActivityIndicatorAnimationType.BallGridBeat, UIColor.White);
				activityIndicatorView.Frame = new CGRect((this.View.Frame.Width - activityIndicatorView.Size) / 2, (this.View.Frame.Height - activityIndicatorView.Size) / 2, activityIndicatorView.Size, activityIndicatorView.Size);
				this.View.AddSubview(activityIndicatorView);
				activityIndicatorView.StartAnimating();
				#endregion

				//Object initillizer
				this._jManager = JsonManager.Instance;

				//0. Load the System Vaalues
				SystemValue.LoadDefaultValue();

				//Change Background Image
				this.ChangeBackgroundColor();

				//Load Image from service

				this.LoadScrollImage();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
			finally
			{
				if (activityIndicatorView != null)
					activityIndicatorView.StopAnimating();
			}
		}

		#region Logic for the Main Page Left Scroll View 

		int _TotalPage = 0;
        /// <summary>
		/// Loads the scroll image from database.
		/// </summary>
		private void LoadScrollImage()
		{
			var images = this._jManager.GetMainImages();

			if (images == null || images.Length == 0)
				return;
			
			for (int i = 0; i < images.Length; i++)
			{ 
				UIImageView img = new UIImageView(FileDownloader.DownloadImage(images[i].PicURL, this.swMainAdv.Frame.Size));
				RectangleF rectangle = new RectangleF();
				rectangle.X = (float)(this.swMainAdv.Frame.Width * i);// + 50;
				rectangle.Y = 0;
				rectangle.Size = (SizeF)this.swMainAdv.Frame.Size;
				img.Frame = rectangle;

				this.swMainAdv.AddSubview(img);
			}

			/*a
			for (int i = 0; i < images.Length; i++)
			{
				var imageView = new UIImageView(View.Frame);
				imageView.Image = UIImage.FromBundle("Kemah");
			}

			/*
			  
			int i;

			for (i = 0; i < 5; i++)
			{
				UILabel label = new UILabel();
				RectangleF rectangle = new RectangleF();
				rectangle.X = (float)(this.swMainAdv.Frame.Width * i) + 50;
				rectangle.Y = 0;
				rectangle.Size = (SizeF)this.swMainAdv.Frame.Size;
				label.Frame = rectangle;
				label.TextColor = UIColor.White;
				label.Text = i.ToString();

				this.swMainAdv.AddSubview(label);
			}

			_TotalPage = i;
			*/
			_TotalPage = images.Length;
			this.pcMainAdv.Pages = _TotalPage;
			this.swMainAdv.ContentSize = new SizeF((float)swMainAdv.Frame.Width * _TotalPage, (float)swMainAdv.Frame.Height);


			// set pages and content siz
			//this.pcMainAdv.Pages = images.Length;
			//this.swMainAdv.ContentSize = new SizeF((float)swMainAdv.Frame.Width * images.Length, (float)swMainAdv.Frame.Height);

			this.swMainAdv.Scrolled += SwMainAdv_Scrolled;
			//this.swMainAdv.DecelerationEnded += HandleDecelerationEnded;
			//swMainAdv.BringSubviewToFront(pcMainAdv);

		}

		/// <summary>
		/// Handle the main adv scrolled event
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		void SwMainAdv_Scrolled(object sender, EventArgs e)
		{
			int intIndex = (int)System.Math.Floor(this.swMainAdv.ContentOffset.X / this.swMainAdv.Frame.Size.Width);

			//int intNext = (int)System.Math.Ceiling(this.swMainAdv.ContentOffset.X / this.swMainAdv.Frame.Size.Width);

			if (intIndex < 0 && this.swMainAdv.Decelerating)
			{
				//this.swMainAdv.ContentOffset = new CGPoint(swMainAdv.Frame.Width * _TotalPage - 1, swMainAdv.Frame.Height);
				swMainAdv.ScrollRectToVisible(new RectangleF((float)(this.swMainAdv.ContentSize.Width - this.swMainAdv.Frame.Width), 0, (float)this.swMainAdv.Frame.Width, (float)this.swMainAdv.Frame.Height), false);
				intIndex = _TotalPage - 1;
			}
			else if (this.swMainAdv.ContentOffset.X > (this.swMainAdv.ContentSize.Width - this.swMainAdv.Frame.Width) && this.swMainAdv.Decelerating)
			{
				swMainAdv.ScrollRectToVisible(new RectangleF(0, 0, (float)this.swMainAdv.Frame.Width, (float)this.swMainAdv.Frame.Height), false);
				intIndex = 0;
			}
		
			this.pcMainAdv.CurrentPage = intIndex;
		}

		void HandleDecelerationEnded(object sender, EventArgs e)
		{
			/*
			if (swMainAdv.ContentOffset.X == 0)
			{
				swMainAdv.ScrollRectToVisible(new RectangleF((float)(this.swMainAdv.ContentSize.Width - this.swMainAdv.Frame.Width), 0, (float)this.swMainAdv.Frame.Width, (float)this.swMainAdv.Frame.Height), false);
			}
			else if (swMainAdv.ContentOffset.X == this.swMainAdv.ContentSize.Width - this.swMainAdv.Frame.Width)
			{
				swMainAdv.ScrollRectToVisible(new RectangleF((float)this.swMainAdv.Frame.Width, 0, (float)this.swMainAdv.Frame.Width, (float)this.swMainAdv.Frame.Height), false);
			}
			*/
		}


		#endregion

		/// <summary>
		/// Changes the color of the background.
		/// it should be configurable later so we can change the backcolor programly
		/// </summary>
		private void ChangeBackgroundColor()
		{
			this.View.BackgroundColor = UIColor.FromPatternImage(UIImage.FromBundle("Background"));
			//this.View.BackgroundColor = UIColor.Black;
		}
    }
}