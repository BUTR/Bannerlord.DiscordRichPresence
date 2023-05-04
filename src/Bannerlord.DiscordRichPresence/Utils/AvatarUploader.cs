using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

using TaleWorlds.Engine;

namespace Bannerlord.DiscordRichPresence.Utils
{
    public static class AvatarUploader
    {
        public static async void UploadTexture(Texture characterTexture)
        {
            const string localPath = "discord_rpc_player_temp.bin";
            // Credits to billw2012, taken from
            // https://github.com/billw2012/Bannerlord-Twitch/blob/54e117b97b39476f35adefd23a1bb9e0f06d09c2/BannerlordTwitch/BannerlordTwitch/Documentation/DocumentationGenerator.cs#L329-L334
            characterTexture.TransformRenderTargetToResource(localPath);
            characterTexture.SaveToFile(localPath);
            for (var i = 0; i < 100 && !File.Exists(localPath); i++)
                await Task.Delay(100);

            DiscordSubModule.Instance.AvatarUrl = await UploadAvatarAsync(File.ReadAllBytes(localPath));
            File.Delete(localPath);
        }
        
        private static async Task<string?> UploadAvatarAsync(byte[] bgraData)
        {
            try
            {
                var assembly = typeof(DiscordSubModule).Assembly;
                var uploadUrlAttr = assembly.GetCustomAttributes<AssemblyMetadataAttribute>().FirstOrDefault(a => a.Key == "AvatarUploadUrl");
                if (uploadUrlAttr is null)
                    return null;

                var httpWebRequest = WebRequest.CreateHttp(uploadUrlAttr.Value);
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/octet-stream";
                httpWebRequest.ContentLength = bgraData.Length;
                httpWebRequest.UserAgent = $"Bannerlord.DiscordRichPresence v{assembly.GetName().Version}";

                using var requestStream = await httpWebRequest.GetRequestStreamAsync().ConfigureAwait(false);
                await requestStream.WriteAsync(bgraData, 0, bgraData.Length).ConfigureAwait(false);

                using var response = await httpWebRequest.GetResponseAsync().ConfigureAwait(false);
                if (response is not HttpWebResponse httpWebResponse)
                    return null;

                if (httpWebResponse.StatusCode is not HttpStatusCode.OK or HttpStatusCode.Created)
                    return null;

                using var stream = response.GetResponseStream();
                if (stream is null)
                    return null;

                using var responseReader = new StreamReader(stream);
                return await responseReader.ReadLineAsync().ConfigureAwait(false);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}