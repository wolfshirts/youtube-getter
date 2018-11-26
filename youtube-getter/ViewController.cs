using System;
using AppKit;
using Foundation;

using YoutubeExplode;
using YoutubeExplode.Models.MediaStreams;
using System.Diagnostics;


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
            try
            {
                Process ffmpeg = new Process();
                ffmpeg.StartInfo.UseShellExecute = true;
                ffmpeg.StartInfo.FileName = "ffmpeg";
                ffmpeg.StartInfo.CreateNoWindow = true;
                ffmpeg.Start();
            }
            catch
            {
                var text = "You are missing a dependency which this application " +
                    "relies on. Transcoding is done with ffmpeg. Please install " +
                    "ffmpeg and try again.";
                var alert = new NSAlert()
                {
                    AlertStyle = NSAlertStyle.Critical,
                    InformativeText = text,
                    MessageText = "Missing Dependency",
                };
                alert.RunModal();
                NSApplication.SharedApplication.Terminate(this);
            }
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
            if (!HelperFunctions.validUrl(urlEntryBox))
            {
                ChangeErrorLabel(Status.InvalidURL);
                return;
            }

            if (audioCheck.State != NSCellStateValue.On &&
                videoCheckBox.State != NSCellStateValue.On)
            {
                ChangeErrorLabel(Status.NoCheckBox);
                return;
            }


            var urlString = urlEntryBox.StringValue;
            var dlg = new NSSavePanel();
            dlg.Title = "Save Location";
            if (dlg.RunModal() == 1){
                var saveLocation = dlg.Url.Path;
                HelperFunctions.GetVideo(saveLocation, urlString, videoCheckBox,
                                         audioCheck);
            }
            else{
                return;
            }

        }
    }
}