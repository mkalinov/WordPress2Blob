using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordPress2Blob.Models
{
    public class ErrorHolder
    {

        public int badTitle { get; set; }
        public int notFolder { get; set; }
        public int errorsCounter { get; set; }
        public int updateIssuesCounter { get; set; }
        public int supposelyDeletedCounter { get; set; }
        public int badTitleRetrieveIssuesCounter { get; set; }
        public int filesWithNoParentsIdCounter { get; set; }
        public int blobFilesUploadedCounter { get; set; }
        public int blobFilesUploadIssuesCounter { get; set; }
        public List<string> blobUploadIssues { get; set; }
        public List<string> filesWithNoParentId { get; set; }
        public List<string> badTitles { get; set; }
        public List<string> notFolders { get; set; }
        public List<string> errors { get; set; }
        public List<string> updateIssues { get; set; }
    }
}
