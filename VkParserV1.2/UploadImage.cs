using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.Drawing;

namespace VkParserV1._2
{
    class UploadImage
    {
        public void param(XmlNode xml)
        {
            photosUploadPhotoToURL(xml.SelectSingleNode("upload_url").InnerText, "C:\\Users\\albert\\Desktop\\VkParserV1.2\\VkParserV1.2\\VkParserV1.2\\bin\\Debug\\img\\v621916973\\90b4\\BWrJOKDG-sU.jpg");
        }

        private JObject photosUploadPhotoToURL(string URL, string file_path)    //загрузка фото на сервер
        {
            WebClient myWebClient = new WebClient();
            byte[] responseArray = myWebClient.UploadFile(URL, file_path);
            var json = JObject.Parse(System.Text.Encoding.ASCII.GetString(responseArray));

            return json;
        }

        private byte[] GetUploadFile(string filePatch)
        {
            byte[] fileData = File.ReadAllBytes(filePatch);
            return fileData;
                //Convert.ToBase64String(fileData);
        }

        private string GetUploadUrl(XmlNode xml)
        {
            string url = xml.SelectSingleNode("response").SelectSingleNode("upload_url").InnerText;
            return url;
        }

        public void UploadFuncion(XmlNode xml, string filePatch)
        {
            //streamresp(GetUploadUrl(xml), filePatch);
            uu(GetUploadUrl(xml), filePatch);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GetUploadUrl(xml));
            request.Method = "POST";
            request.Credentials = CredentialCache.DefaultCredentials;
            //request.TransferEncoding = "Base64";

            UTF8Encoding encoding = new UTF8Encoding();

            string postData = "photo=" + GetUploadFile(filePatch);

            request.ContentType = "multipart/form-data";
            request.ContentLength = Encoding.UTF8.GetByteCount(postData);

            using (var newStream = request.GetRequestStream())
            {

                byte[] postByte = Encoding.UTF8.GetBytes(postData);
                newStream.Write(postByte, 0, postByte.Length);
                newStream.Close();
            }
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            //HttpWebResponse rr = request.GetResponse();
            //StreamReader red = new StreamReader(rr);

            //return (HttpWebResponse)request.GetResponse();
        }
        private void streamresp(string url, string filePatch)
        {
            // Create a request using a URL that can receive a post. 
            WebRequest request = WebRequest.Create(url);
            // Set the Method property of the request to POST.
            request.Method = "POST";
            // Create POST data and convert it to a byte array.
            string postData = "";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            // Set the ContentType property of the WebRequest.
            request.ContentType = "multipart/form-data";
            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            Console.WriteLine(responseFromServer);
            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();
        }
        private void uu(string URL, string file_path)
        {
            WebClient myWebClient = new WebClient();
            byte[] responseArray = myWebClient.UploadFile(URL, file_path);
            var json = System.Text.Encoding.ASCII.GetString(responseArray);
    }
    }
}
