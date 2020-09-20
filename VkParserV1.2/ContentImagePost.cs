using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace VkParserV1._2
{
    static class ContentImagePost
    {
        
        #region подготовка изображений принадлежащих к посту

        public static bool ImageFunction(int postId, int groupId)
        {
            Bitmap[] ImagePost;
            int b = 0;
            FindFileAndDownload findfileanddownload = new FindFileAndDownload();
            using (ContextGroupWallPhoto db = new ContextGroupWallPhoto())
            {
                try
                {
                    var tt = db.GroupWallPhotos
                        .Select(p => new { id = p.Id, photo = p.photo, PhotoName = p.PhotoName, postId = p.post_id, groupId = p.GroupId })
                        .Where(p => p.postId == postId && p.groupId == groupId);
                    if (tt.Count() != 0)
                    {
                        //Создаем масссив размером полученных изображений
                        int CountImg = tt.Count();
                        ImagePost = new Bitmap[CountImg];

                        foreach (var p in tt)
                        {
                            ImagePost[b] = (Bitmap)findfileanddownload.fileFind(p.photo);
                            
                            b++;
                        }
                        StaticClass.ImagePost = ImagePost;
                        return true;
                    }
                    
                    return false;
                }
                catch { return false; }
            }

        }
        #endregion
    }
}
