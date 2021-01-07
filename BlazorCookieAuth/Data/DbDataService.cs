namespace BlazorCookieAuth.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class DbDataService : IDbDataService
    {
        private readonly List<DataItem> DataItemList = new List<DataItem>();

        private static readonly string[] CodeList = new[]
        {
            "0H5 2233", "6A8 6532", "4E3 5784", "5L5 8871", "3C2 3298", "2S1 4285", "2T8 5493", "3H4 5381", "5A1 6985", "0A3 5214"
        };

        public Task<List<DataItem>> GetDataItemList()
        {
            var rng = new Random();
            this.DataItemList.Clear();
            this.DataItemList.AddRange(Enumerable.Range(1, 5).Select(index => new DataItem
            {
                Id = index,
                Date = DateTime.Now.AddDays(index),
                ValueA = rng.Next(0, 60),
                Code = CodeList[rng.Next(CodeList.Length)]
            }).ToArray());

            return Task.FromResult(this.DataItemList);
        }

        public Task<string[]> GetCodeList()
        {
            return Task.FromResult(CodeList);
        }

        public Task<DataItem> GetDataItemById(int id)
        {
            return Task.FromResult((DataItem)this.DataItemList.Where(note => note.Id == id).First().Clone());
        }

        public Task UpdateDataItem(DataItem dataItem)
        {
            try
            {
                this.DataItemList[this.DataItemList.FindIndex(item => item.Id == dataItem.Id)] = dataItem;
            }
            catch (Exception e)
            {
                return Task.FromException(e);
            }

            return Task.CompletedTask;
        }

        public UserItem ValidateUser(string username, string password)
        {
            return new UserItem { UserName = username, SecurityLevel = 1 };
        }
    }
}
