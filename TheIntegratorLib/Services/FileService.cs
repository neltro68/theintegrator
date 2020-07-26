using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TheIntegratorLib.Utilities;

namespace TheIntegratorLib.Services
{
    public interface IFileService
    {
        void UseCache(IDataCache userSalesCache);
        Task ProcessAsync(List<Stream> files, string path, IUserSalesService userSalesService);
    }
    public class FileService : IFileService
    {
        private IDataCache _userSalesCache;
        public FileService()
        {
        }

        public async Task ProcessAsync(List<Stream> files, string path, IUserSalesService userSalesService)
        {
            var target = Path.Combine("path", "temp");
            if (!Directory.Exists(target))
                Directory.CreateDirectory(target);

            userSalesService.UseCache(_userSalesCache);
            foreach (Stream file in files)
            {
                using (StreamReader reader = new StreamReader(file))
                {
                    bool isHeader = true;
                    string line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        if (isHeader)
                        {
                            isHeader = false;
                            userSalesService.SetHeader(line);
                        }
                        else
                        {
                            userSalesService.Record(line);
                        }
                    }
                }
            }
        }

        public void UseCache(IDataCache userSalesCache)
        {
            _userSalesCache = userSalesCache;
        }
    }
}
