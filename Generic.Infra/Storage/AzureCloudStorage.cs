using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generic.Infra.Storage
{
    public class AzureCloudStorage
    {
        private static CloudBlobClient cloudBlobStorage;
        private static CloudBlobDirectory userDirectory;
        public static String BLOB_CONTAINER = "images";

        public static string PathImage
        {
            get
            {
                string url = String.Empty;
                url = ConfigurationManager.AppSettings["BaseUrl"].ToString();

                if (url.EndsWith(@"/"))
                    url += "azure/images";
                else
                    url += "/azure/images";

                return url;
            }
        }


        static AzureCloudStorage()
        {
            Setup();
        }

        private static void Setup()
        {
            if (ConfigurationManager.AppSettings["Microsoft.Storage.ConnectionString"] == null)
                throw new ConfigurationErrorsException("Configuração do Storage não encontrada");

            string configurationSettings = ConfigurationManager.AppSettings["Microsoft.Storage.ConnectionString"].ToString();

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(configurationSettings);

            cloudBlobStorage = cloudStorageAccount.CreateCloudBlobClient();

            //Verificando se o container de Foto de Profile do Usuário está criado
            CloudBlobContainer container = cloudBlobStorage.GetContainerReference("images");

            if (container.CreateIfNotExists())
            {
                container.SetPermissions(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });

            }

            //Criando as referencias para os diretorios
            userDirectory = container.GetDirectoryReference("user");

        }

        public static void SaveToStorage(AzureCloudImageDirectoryStorageEnum type, byte[] buffer, string fileName, string subDirectory)
        {
            CloudBlockBlob blob;

            //if (type == AzureCloudImageDirectoryStorageEnum.user)
            blob = userDirectory.GetDirectoryReference(subDirectory).GetBlockBlobReference(fileName);
           

            using (Stream stream = new MemoryStream(buffer))
            {
                blob.UploadFromStream(stream);
            }

        }

        public static byte[] GetFromStorage(AzureCloudImageDirectoryStorageEnum type, string fileName, string subDirectory)
        {
            CloudBlockBlob blob;
            byte[] file;

            //if (type == AzureCloudImageDirectoryStorageEnum.user)
                blob = userDirectory.GetDirectoryReference(subDirectory).GetBlockBlobReference(fileName);
         

            using (var memoryStream = new MemoryStream())
            {
                try
                {
                    blob.DownloadToStream(memoryStream);
                    file = memoryStream.ToArray();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return file;
        }
    }

    public enum AzureCloudImageDirectoryStorageEnum
    {
        user = 1
    }

}

