using System.IO;

namespace MongoDB.Interfaces
{
    public interface IGridFsCollection
    {
        void UploadFile(Stream file, string fileName);
        byte[] DownloadFile(string fileName);
        string DownloadImageURL(string fileName);
    }
}