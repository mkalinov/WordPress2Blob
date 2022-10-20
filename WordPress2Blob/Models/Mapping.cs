using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordPress2Blob.Models
{
    public class Mapping
    {
        public string blobPath { get; set; }



        /// <summary>
        /// gets an array from Word Press FTP
        /// </summary>
        public byte[] bytes { get; set; }


        #region FTP

        public string path { get; set; }
        public string fileName { get; set; }
        public string fullFtpPath { get; set; }

        #endregion FTP
    }
}
