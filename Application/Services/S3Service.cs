using Amazon.S3;
using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon.S3.Transfer;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Application.Services
{
    public class S3Service : IS3Service
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public S3Service(IConfiguration config)
        {
            _bucketName = config["AWS:BucketName"];
            _s3Client = new AmazonS3Client(
                config["AWS:AccessKey"],
                config["AWS:SecretKey"],
                Amazon.RegionEndpoint.SAEast1
            );
        }

        public async Task<string> UploadImagemLivroAsync(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Arquivo inválido");

            // Validação de tipo de arquivo
            var extensoesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var extensao = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!extensoesPermitidas.Contains(extensao))
                throw new ArgumentException("Tipo de arquivo não permitido");

            // Gera um nome único para o arquivo
            var nomeArquivo = $"livros/{id}/{Guid.NewGuid()}{extensao}";

            try
            {
                using var stream = file.OpenReadStream();

                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = stream,
                    Key = nomeArquivo,
                    BucketName = _bucketName,
                    ContentType = file.ContentType,
                    //CannedACL = S3CannedACL.PublicRead // Deixa o arquivo público
                };

                var transferUtility = new TransferUtility(_s3Client);
                await transferUtility.UploadAsync(uploadRequest);

                // Retorna a URL pública do arquivo
                var url = $"https://{_bucketName}.s3.amazonaws.com/{nomeArquivo}";
                return url;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao fazer upload: {ex.Message}");
            }
        }

    }

}
