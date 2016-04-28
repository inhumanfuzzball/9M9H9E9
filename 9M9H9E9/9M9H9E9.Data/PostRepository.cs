using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace _9M9H9E9.Data
{
    public class PostRepository
    {
        private const string PARTITION_KEY = "posts";

        public List<Post> GetAll()
        {
            TableQuery<DtoPost> retrieveOperation = 
                new TableQuery<DtoPost>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "posts"));

            return PostsTable.ExecuteQuery(retrieveOperation).Select(post => new Post
            {
                Body = post.Body,
                Id = post.RowKey,
                Link = "http://reddit.com" + post.Link,
                Posted = post.Posted
            }).ToList();
        }

        public bool Exists(string postId)
        {
            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<DtoPost>(PARTITION_KEY, postId);

            // Execute the retrieve operation.
            TableResult retrievedResult = PostsTable.Execute(retrieveOperation);

            return retrievedResult.Result != null;
        }

        public void Add(Post p)
        {
            DtoPost post = new DtoPost
            {
                Body = p.Body,
                Link = p.Link,
                PartitionKey = PARTITION_KEY,
                Posted = p.Posted,
                RowKey = p.Id,
                Timestamp = DateTime.UtcNow
            };

            PostsTable.Execute(TableOperation.Insert(post));
        }

        private CloudTable _postsTable;

        private CloudTable PostsTable
        {
            get
            {
                if (_postsTable != null) return _postsTable;

                CloudStorageAccount storageAccount = 
                    CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString);
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                _postsTable = tableClient.GetTableReference(PARTITION_KEY);
                _postsTable.CreateIfNotExists();
                return _postsTable;
            }
        }
    }
}