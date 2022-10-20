using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;
using WordPress2Blob.Models;

namespace WordPress2Blob.Helpers
{
    public class FtpHelper
    {

        public static void FTPDownload(Mapping card, string ftpUserID, string ftpPassword)
        {

            FileStream outputStream;

            //Now the FTP web request completed and ready for response.
            FtpWebRequest ftpWebRequest = GetDownloadFTPWebRequest(card.path, card.fileName, card.fullFtpPath, ftpUserID, ftpPassword, out outputStream);

            //Getting response from the request.
            FtpWebResponse response = (FtpWebResponse)ftpWebRequest.GetResponse();

            //TO DO: combine the method with blob uplod
            PassFileStreamDataToStream(response, outputStream);

        }

        //TO DO: combine the method with blob uplod
        /// <summary>
        /// It takes the stream as bytes from response and writes the bytes to the file stream as File.
        /// </summary>
        public static void PassFileStreamDataToStream(FtpWebResponse response, FileStream outputStream)
        {
            try
            {
                Stream ftpStream = response.GetResponseStream();
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                //reads the stream of response based on request.
                readCount = ftpStream.Read(buffer, 0, bufferSize);

                //Writing Stream to the Path specified in the File stream.
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                outputStream.Close();
                response.Close();
            }

            catch (Exception ex)
            {
                //TO DO: to log
                Console.WriteLine(ex.Message.ToString());
            }
        }



        public static FtpWebRequest GetDownloadFTPWebRequest( string path, string fileName, string fullFtpPath,string ftpUserID, string ftpPassword, out FileStream fs)

        {

            //Creates file stream based on the passed path.
            fs = new FileStream(path + "\\" + fileName, FileMode.Create);

            //Creating FTP request.
            FtpWebRequest request = CreateFTPRequest(fullFtpPath, fileName);

            //Setting FTP Credentials.
            request.Credentials = CreateNetworkCredentials(ftpUserID, ftpPassword);
            request.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.CacheIfAvailable);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.KeepAlive = false;
            request.UseBinary = true;
            request.UsePassive = true;
            request.ContentLength = fs.Length;

            return request;

        }

        /// <summary>
        ///Creates the FTP web request
        /// </summary>
        static FtpWebRequest CreateFTPRequest(string fullFtpPath, string fileName)
        {
            return (FtpWebRequest)WebRequest.Create(new Uri(fullFtpPath + "/" + fileName));
        }

        /// <summary>
        /// NetworkCredential just to set the User and password of ftp path.
        /// </summary>
        public static NetworkCredential CreateNetworkCredentials(string ftpUserID, string ftpPassword)
        {
            return new NetworkCredential(ftpUserID, ftpPassword);
        }

    }

}