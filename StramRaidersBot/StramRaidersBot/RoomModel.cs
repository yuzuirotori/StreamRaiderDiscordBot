using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace StramRaidersBot
{
    public class RoomModel : TableEntity
    {
        public RoomModel(string nickname, string boxType)
        {
            this.PartitionKey = nickname;
            this.RowKey = boxType;
        }
        public RoomModel()
        {

        }
        public int leftTime { get; set; }
    }
}
