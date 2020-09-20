using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Net;

namespace VkParserV1._2
{
    class FindFileAndDownload
    {

        //private string[] FileStreeam(string stream)
        //{
        //    //https://pp.vk.me/c543101/v543101181/3088/dFKzspG3HLs.jpg
        //    string[] filestriamArr = stream.Split(new Char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
        //    string[] fileName = filestriamArr[filestriamArr.Length - 1].Split(new Char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
        //    return fileName;

        //}

        //передаем урл картинки
        //В ответ приходит картинка
        public Image fileFind(string fileUrl)
        {
            string[] PachUrl = FilePatch(fileUrl);
            string filePatch = AppDomain.CurrentDomain.BaseDirectory + "img\\" + PachUrl[PachUrl.Length - 3] + '\\' + PachUrl[PachUrl.Length - 2] + '\\' + PachUrl[PachUrl.Length - 1];
            string filePatchName = AppDomain.CurrentDomain.BaseDirectory + "img\\" + PachUrl[PachUrl.Length - 3] + '\\' + PachUrl[PachUrl.Length - 2] + '\\';
            //если картики не существует то путь приходит ошибка
            if (File.Exists(filePatch))
            {
                return LoadImgFile(filePatch);
            }
            //переходим к загрузке картинки
            else
            {
                return LoadImgFile(DownloadImageFile(filePatchName, fileUrl, PachUrl[PachUrl.Length - 1]));
            }
        }

        private string[] FilePatch(string fileUrl)
        {
            return fileUrl.Split(new Char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private Image LoadImgFile(string filePatch)
        {
            Bitmap image = new Bitmap(filePatch);
            return image;
        }

        private string DownloadImageFile(string filePatchName, string fileUrl, string fileName)
        {
            if (!Directory.Exists(filePatchName))
            {
                Directory.CreateDirectory(filePatchName);
            }
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(fileUrl, filePatchName+ fileName);
                }
                return filePatchName + fileName;
        }
    }
}
