using System;
using AppKit;
using Foundation;

using YoutubeExplode;
using YoutubeExplode.Models.MediaStreams;


namespace youtubegetter
{
    public partial class ViewController : NSViewController
    {

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Do any additional setup after loading the view.
        }

        public override NSObject RepresentedObject
        {
            get
            {
                return base.RepresentedObject;
            }
            set
            {
                base.RepresentedObject = value;
                // Update the view, if already loaded.
            }
        }

        void ChangeErrorLabel(Status e){
            switch (e)
            {
                case Status.NoCheckBox: errorLabel.StringValue = "You must select a format.";
                    break;
                case Status.InvalidURL: errorLabel.StringValue = "You must enter a valid Youtube URL";
                    break;

                default:
                    break;
            }

            errorLabel.Hidden = false;
        }



        partial void streamInfoClicked(NSObject sender)
        {
            if(!HelperFunctions.validUrl(urlEntryBox)){
                ChangeErrorLabel(Status.InvalidURL);
                return;
            }

            var urlString = urlEntryBox.StringValue;
            HelperFunctions.CheckBitRate(urlString, infoLabelOne, infoLabelTwo);
        }


        partial void DownloadButtonClicked(NSObject sender)
        {
            if (audioCheckBox.State != NSCellStateValue.On &&
                videoCheckBox.State != NSCellStateValue.On)
            {
                ChangeErrorLabel(Status.NoCheckBox);
                return;
            }

            if (!HelperFunctions.validUrl(urlEntryBox))
            {
                ChangeErrorLabel(Status.InvalidURL);
                return;
            }


            var urlString = urlEntryBox.StringValue;
            var dlg = new NSSavePanel();
            dlg.Title = "Save Location";
            if (dlg.RunModal() == 1){
                var saveLocation = dlg.Url.Path;
                HelperFunctions.GetVideo(saveLocation, urlString, videoCheckBox,
                                         audioCheckBox);
            }
            else{
                return;
            }

        }
    }
}