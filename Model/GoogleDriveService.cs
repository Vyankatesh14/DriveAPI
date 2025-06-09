using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DriveGoogleAPi.Services
{
    public class GoogleDriveService
    {
        private readonly DriveService _driveService;

        public GoogleDriveService(IConfiguration config)
        {
            var serviceAccountJsonPath = config["GoogleDrive:ServiceAccountJsonPath"];
            if (string.IsNullOrEmpty(serviceAccountJsonPath))
                throw new ArgumentNullException(nameof(serviceAccountJsonPath), "Service account path is missing in config.");

            GoogleCredential credential;
            using (var stream = new FileStream(serviceAccountJsonPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(DriveService.Scope.DriveReadonly);
            }

            _driveService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "GoogleDriveVideoApi"
            });
        }

        public async Task<MemoryStream> DownloadFileAsync(string fileId)
        {
            var request = _driveService.Files.Get(fileId);
            var stream = new MemoryStream();
            await request.DownloadAsync(stream);
            stream.Position = 0;
            return stream;
        }

        public async Task<string> GetFileNameAsync(string fileId)
        {
            var file = await _driveService.Files.Get(fileId).ExecuteAsync();
            return file.Name;
        }
    }
}