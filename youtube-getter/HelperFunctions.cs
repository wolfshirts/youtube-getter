using System;
using YoutubeExplode;
using YoutubeExplode.Models.MediaStreams;
using AppKit;
namespace youtubegetter
{
    public static class HelperFunctions
    {
       public static async void CheckBitRate(string address, NSTextField infoLabelOne, 
                                        NSTextField infoLabelTwo)
        {
            var client = new YoutubeClient();
            var id = YoutubeClient.ParseVideoId(address);
            var video = await client.GetVideoAsync(id);
            var streamInfoSet = await client.GetVideoMediaStreamInfosAsync(id);
            var audioStream = streamInfoSet.Audio.WithHighestBitrate();
            var encoding = audioStream.AudioEncoding.ToString();
            var bitRate = audioStream.Bitrate.ToString();
            var videoInfo = video.Author + "-" + video.Title;
            infoLabelOne.StringValue = videoInfo;
            infoLabelTwo.StringValue = encoding + ":" + bitRate;
            infoLabelOne.Hidden = false;
            infoLabelTwo.Hidden = false;
        }

        public static async void GetVideo(string fileLocation,
                                          string address, NSButton videoCheckBox,
                                         NSButton audioCheckBox)
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
                var audioStream = streamInfoSet.Audio.WithHighestBitrate();
                var ext = audioStream.Container.GetFileExtension();
                var tempName = Guid.NewGuid().ToString("n").Substring(0, 8) + "." + ext;
                await client.DownloadMediaStreamAsync(audioStream, "/tmp/");

            }

        }

        public static bool validUrl(NSTextField urlEntryBox)
        {
            string urlString = urlEntryBox.StringValue;
            if (string.IsNullOrEmpty(urlString))
            {
                return false;

            }

            try
            {
                var client = new YoutubeClient();
                var id = YoutubeClient.ParseVideoId(urlString);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            return true;
        }


    }
}
