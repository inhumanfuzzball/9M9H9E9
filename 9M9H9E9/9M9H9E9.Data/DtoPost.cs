using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace _9M9H9E9.Data
{
    public class DtoPost : TableEntity
    {
        public string Link { get; set; }
        public string Body { get; set; }
        public DateTime Posted { get; set; }
    }
}