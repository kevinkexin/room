using Foundation;
using System;
using UIKit;

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

			try
			{
				
				var activityIndicatorView = new Xamarin.iOS.DGActivityIndicatorViewBinding.DGActivityIndicatorView(Xamarin.iOS.DGActivityIndicatorViewBinding.DGActivityIndicatorAnimationType.BallScale, UIColor.White);
				this.View.AddSubview(activityIndicatorView);

				activityIndicatorView.StartAnimating();

				//Object initillizer
				this._jManager = JsonManager.Instance;

				//0. Load the System Vaalues
				SystemValue.LoadDefaultValue();

				///
				this.ChangeBackgroundColor();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}

		#region Logic for the Main Page Left Scroll View 

		/// <summary>
		/// Loads the scroll image from database.
		/// </summary>
		private void LoadScrollImage()
		{
			var images = this._jManager.GetMainImages();

			/*a
			for (int i = 0; i < images.Length; i++)
			{
				var imageView = new UIImageView(View.Frame);
				imageView.Image = UIImage.FromBundle("Kemah");
			}

			/*
			int i;

			for (i = 1; i < 5; i++)
			{
				UILabel label = new UILabel();
				RectangleF rectangle = new RectangleF();
				rectangle.X = (float)(this.swMainAdv.Frame.Width * i) + 50;
				rectangle.Y = 0;
				rectangle.Size = (SizeF)this.swMainAdv.Frame.Size;
				label.Frame = rectangle;
				label.Text = i.ToString();

				this.swMainAdv.AddSubview(label);
			}
			*/
			// set pages and content siz
			//this.pcMainAdv.Pages = i;
			//this.swMainAdv.ContentSize = new SizeF((float)swMainAdv.Frame.Width * i, (float)swMainAdv.Frame.Height);

			this.swMainAdv.Scrolled += SwMainAdv_Scrolled;
		}

		/// <summary>
		/// Handle the main adv scrolled event
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		void SwMainAdv_Scrolled(object sender, EventArgs e)
		{
			this.pcMainAdv.CurrentPage = (int)System.Math.Floor(this.swMainAdv.ContentOffset.X / this.swMainAdv.Frame.Size.Width);
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