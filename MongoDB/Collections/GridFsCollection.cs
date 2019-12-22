using MongoDB.Driver.GridFS;
using MongoDB.Interfaces;
using MongoDB.UnitOfWorks;
using System;
using System.IO;

namespace MongoDB.Collections
{
    public class GridFsCollection : BaseCollection, IGridFsCollection
    {
        #region Properties
        private GridFSBucket GridFS { get; set; }
        #endregion

        #region Constructors
        public GridFsCollection(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.GridFS = new GridFSBucket(unitOfWork.DBContext);
        }
        #endregion

        #region Functions
        public void UploadFile(Stream file, string fileName)
        {
            GridFS.UploadFromStream(fileName, file);
        }

        public byte[] DownloadFile(string fileName)
        {
            var file = GridFS.DownloadAsBytesByName(fileName);

            return file;
        }

        public string DownloadImageURL(string fileName)
        {
            var file = DownloadFile(fileName);
            var url = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(file));

            return url;
        }
        #endregion
    }
}