using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;
using Microsoft.Azure.WebJobs;
using _9M9H9E9.Data;

namespace _9M9H9E9.RssScraper
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    public class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            var host = new JobHost();
            host.Call(typeof(Program).GetMethod("CheckForPosts"));
        }
        
        [NoAutomaticTrigger]
        public static void CheckForPosts()
        {
            PostRepository postRepo = new PostRepository();

            foreach (Post p in GetPosts())
            {
                // If we have this item break as we are upto date
                if (postRepo.Exists(p.Id)) break;
                
                // Add the new item
                postRepo.Add(p);
            }
        }

        public static List<Post> GetPosts()
        {
            List<Post> posts = new List<Post>();
            XmlReader reader = XmlReader.Create("https://www.reddit.com/user/_9MOTHER9HORSE9EYES9/comments/.rss");
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            reader.Close();

            if (feed == null) return posts;

            posts.AddRange(
                from item in feed.Items
                let textContent = item.Content as TextSyndicationContent
                where textContent != null
                select new Post
                {
                    Body = textContent.Text,
                    Id = item.Id,
                    Posted = item.LastUpdatedTime.UtcDateTime,
                    Link = item.Links[0].Uri.ToString(),
                });

            return posts;
        }
    }
}
