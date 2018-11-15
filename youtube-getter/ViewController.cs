using System;
using AppKit;
using Foundation;

using YoutubeExplode;
using YoutubeExplode.Models.MediaStreams;


namespace youtubegetter
{
    public partial class ViewController : NSViewController
    {
        enum Errors{
            NoCheckBox,
            InvalidURL

        }

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

        void ChangeErrorLabel(Errors e){


            switch (e)
            {
                case Errors.NoCheckBox: errorLabel.StringValue = "You must select a format.";
                    break;
                case Errors.InvalidURL: errorLabel.StringValue = "You must enter a valid Youtube URL";
                    break;

                default:
                    break;
            }

            errorLabel.Hidden = false;
        }

        partial void DownloadButtonClicked(NSObject sender)
        {
            if (audioCheckBox.State != NSCellStateValue.On &&
                videoCheckBox.State != NSCellStateValue.On)
            {
                ChangeErrorLabel(Errors.NoCheckBox);
                return;
            }


            string urlString = urlEntryBox.StringValue;

            if (string.IsNullOrEmpty(urlString)){
                ChangeErrorLabel(Errors.InvalidURL);
                return;
            }
            try
            {
                var client = new YoutubeClient();
                var id = YoutubeClient.ParseVideoId(urlString);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                ChangeErrorLabel(Errors.InvalidURL);
                return;
            }


            var dlg = new NSSavePanel();
            dlg.Title = "Save Location";


            if (dlg.RunModal() == 1){
                var saveLocation = dlg.Url.Path;
                GetVideo(saveLocation, urlString);

            }
            else{
                return;
            }

        }
        


        //Wishlist: Implement progress bar.
        private async void GetVideo(string fileLocation,
                                    string address)
        {
            var client = new YoutubeClient();
            var id = YoutubeClient.ParseVideoId(address);

            var streamInfoSet = await client.GetVideoMediaStreamInfosAsync(id);

            if (videoCheckBox.State == NSCellStateValue.On)
            {
                var streamInfo = streamInfoSet.Muxed.WithHighestVideoQuality();
                var ext = streamInfo.Container.GetFileExtension();
                await client.DownloadMediaStreamAsync(streamInfo, fileLocation + "." + ext);
            }

            if (audioCheckBox.State == NSCellStateValue.On)
            {
                //instead of running a native converter or something we'll take a lower
                //bitrate to save on development time.
                for (int i = 0; i < streamInfoSet.Audio.Count; i++)
                {
                    if( streamInfoSet.Audio[i].AudioEncoding == AudioEncoding.Aac || 
                       streamInfoSet.Audio[i].AudioEncoding == AudioEncoding.Mp3 ){
                        var streamInfo = streamInfoSet.Audio[i];
                        var ext = streamInfo.Container.GetFileExtension();
                        if (ext == "m4a")
                            ext = "mp4"; //because iTunes won't play dash m4a, but it will play mp4.
                        await client.DownloadMediaStreamAsync(streamInfo, fileLocation + "." + ext);
                        break;
                    }
                }
            }

        }
    }
}