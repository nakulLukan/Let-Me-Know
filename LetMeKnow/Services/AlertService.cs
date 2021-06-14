using System.IO;
using System.Reflection;

namespace LetMeKnow.Services
{
    public struct AlertService
    {
        public void Alert()
        {
            string alertResourceName = "LetMeKnow.Assets.slot_avail_alert.wav";

            var assembly = typeof(App).GetTypeInfo().Assembly;
            var audioStream = assembly.GetManifestResourceStream(alertResourceName);

            var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            player.Load(audioStream);
            player.Seek(0);
            player.Play();

            player.PlaybackEnded += (x, y) =>
            {
                audioStream.Dispose();
            };
        }
    }
}
