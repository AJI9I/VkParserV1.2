using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace VkParserV1._2
{
    static class SataticClassContent
    {
        #region Получить 10 постов по критериям
        public static string[,] PostInfo = new string[10, 6];
        public static Image[] ImagePost;
        public static int i;
        private static void parametrUpdate(int groupId)
        {
            using (ContextGroupWallTextPost db = new ContextGroupWallTextPost())
            {
                var tt = db.GroupWallTextPosts
                    .Select(p => new { Text = p.text, LickeCount = p.count_likes, RepostCount = p.count_repost, PostedTable = p.postedTable, postId = p.post_id, groupId = p.from_id, ID = p.Id })
                    .Where(p => p.PostedTable == false && p.groupId == groupId)
                    //.Intersect(db.GroupWallTextPosts.Where(p => p.from_id == groupId))
                    .OrderByDescending(p => p.LickeCount)
                    .Take(10);
                foreach (var p in tt)
                {
                    PostInfo[i, 0] = Convert.ToString(p.LickeCount);
                    PostInfo[i, 1] = Convert.ToString(p.RepostCount);
                    PostInfo[i, 2] = p.Text;
                    PostInfo[i, 3] = Convert.ToString(p.postId);
                    PostInfo[i, 4] = Convert.ToString(p.groupId);
                    PostInfo[i, 5] = Convert.ToString(p.ID);
                    i++;
                }
                i = 0;
            }
        }

        public static string[] PostsFuncion(int groupId)
        {
            if (PostInfo[0, 0] == null)
            {
                parametrUpdate(groupId);
            }
            if (i == 10)
            {
                i = 0;
                parametrUpdate(groupId);
            }

            string[] PostPost = new string[5];
            PostPost[0] = PostInfo[i, 0];
            PostPost[1] = PostInfo[i, 1];
            PostPost[2] = PostInfo[i, 2];
            PostPost[3] = PostInfo[i, 3];
            PostPost[4] = PostInfo[i, 4];
            return PostPost;
        }
        #endregion
    }
}
