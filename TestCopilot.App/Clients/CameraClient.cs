using Azure;
using Azure.AI.Vision.Face;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;

namespace TestCopilot.App.Clients
{
    public interface ICameraClient
    {
        Task<IReadOnlyList<FaceDetectionResult>?> AnalyseFacesAsync(byte[] bytes);
    }

    public class CameraClient : ICameraClient
    {
        private readonly string _faceApiUrl;
        private readonly string _faceApiKey;
        private readonly FaceClient _faceClient;

        public CameraClient(IConfiguration configuration)
        {
            _faceApiUrl = configuration["Vision:Endpoint"] ?? "";
            _faceApiKey = configuration["Vision:Key"] ?? "";
            _faceClient = new FaceClient(new Uri(_faceApiUrl), new AzureKeyCredential(_faceApiKey));
        }

        public async Task<IReadOnlyList<FaceDetectionResult>?> AnalyseFacesAsync(byte[] bytes)
        {
            try
            {
                using var httpClient = new HttpClient()
                {
                    BaseAddress = new Uri(new Uri(_faceApiUrl), "/face/v1.0")
                };
                bytes = File.ReadAllBytes(Path.GetFullPath("C:\\Users\\vincent.marchal\\Downloads\\face-detection-demo0-61ea2b10.png"));
                var content = new ByteArrayContent(bytes);
                content.Headers.Add("Ocp-Apim-Subscription-Key", _faceApiKey);
                var response = await httpClient.PostAsync("detect?returnFaceId=true&returnFaceLandmarks=false&recognitionModel=recognition_04&returnRecognitionModel=false&detectionModel=detection_03&faceIdTimeToLive=86400", content);

                if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return await response.Content.ReadFromJsonAsync<IReadOnlyList<FaceDetectionResult>?>();
                }
                else
                {
                    throw new Exception(response?.ReasonPhrase);
                }
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public async Task<IReadOnlyList<FaceDetectionResult>> AnalyseFacesAsyncV1_1(byte[] bytes)
        {
            try
            {
                var response = await _faceClient.DetectAsync(imageContent: new(bytes),
                        FaceDetectionModel.Detection01,
                        FaceRecognitionModel.Recognition04,
                        returnFaceId: false,
                        new List<FaceAttributeType>(){
                            FaceAttributeType.Age,
                            FaceAttributeType.FacialHair,
                            FaceAttributeType.Accessories,
                            FaceAttributeType.Exposure,
                            FaceAttributeType.Hair,
                            FaceAttributeType.Glasses,
                            FaceAttributeType.Smile
                        },
                        returnFaceLandmarks: false
                    );
                return response.Value;
            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}
