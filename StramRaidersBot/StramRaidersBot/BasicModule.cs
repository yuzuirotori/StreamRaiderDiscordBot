using Discord;
using Discord.Commands;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace StramRaidersBot
{
    // 모듈 클래스의 접근제어자가 public이면서 ModuleBase를 상속해야 모듈이 인식된다.
    public class BasicModule : ModuleBase<SocketCommandContext>
    {
        public CloudTable TableInit(string tableName)
        {
            // 1. Get StorageAccount by StorageKey
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Properties.Resources.Storage_Key);
            // 2. Init Table Client
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            // 3. Crete Tabel If Not Exist
            CloudTable table = tableClient.GetTableReference(tableName);
            table.CreateIfNotExistsAsync();

            return table;
        }

        /// <summary>
        /// 사용법 안내 함수
        /// </summary>
        /// <returns></returns>
        [Command("help")]
        [Alias("Help", "HELP", "사용법", "도움", "도움말", "명령어", "Command")]
        public async Task HelpCommnad(params string[] stringArray)
        {
            await AddHelpCommnad(stringArray);
            string helpString = null;
            helpString = "!시간(Time)으로 남은 추가된 시간표를 확인 가능합니다.\n삭제의 경우 !D 스트리머이름 으로 강제 삭제가 가능합니다.";
            await Context.Channel.SendMessageAsync(helpString);
        }

        [Command("ADD")]
        [Alias("Add", "add", "추가")]
        public async Task AddHelpCommnad(params string[] stringArray)
        {
            string helpString = null;
            helpString = "단타방 추가 방법\n!S(s) => Super Boss방 추가\n!B(b) => Boss방 추가\n!G(g) => Royalty Gold방 추가\n!T(t) => Royalty Token방 추가\n!K(k) => Royalty Skin방 추가\n각 명령어 뒤에 스트리머 / 남은시간(분 숫자만)";
            await Context.Channel.SendMessageAsync(helpString);
        }

        /// <summary>
        /// 슈퍼보스방 추가 함수
        /// </summary>
        /// <param name="stringArray"></param>
        /// <returns></returns>
        [Command("S")]
        [Alias("s", "ㄴ")]
        public async Task AddSBCommnad(params string[] stringArray)
        {
            try
            {
                await InsertNewRoomInfo(stringArray, "RoomInfo", "s");
            }
            catch (StorageException sex)
            {
                Console.WriteLine(sex.Message);
                await Context.Channel.SendMessageAsync(sex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Context.Channel.SendMessageAsync("잘못된 형식의 추가입니다.\nWrong Type To Add\n추가형식 : 방이름 / 남은시간(숫자만) \nAdd Type : Room Name / Left Time(Only Integer minute)");
            }
        }

        /// <summary>
        /// 보스방 추가 함수
        /// </summary>
        /// <param name="stringArray"></param>
        /// <returns></returns>
        [Command("B")]
        [Alias("b", "ㅠ")]
        public async Task AddBCommnad(params string[] stringArray)
        {
            try
            {
                await InsertNewRoomInfo(stringArray, "RoomInfo", "b");
            }
            catch (StorageException sex)
            {
                Console.WriteLine(sex.Message);
                await Context.Channel.SendMessageAsync(sex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Context.Channel.SendMessageAsync("잘못된 형식의 추가입니다.\nWrong Type To Add\n추가형식 : 방이름 / 남은시간(숫자만) \nAdd Type : Room Name / Left Time(Only Integer minute)");
            }
        }

        /// <summary>
        /// 로얄골드방 추가 함수
        /// </summary>
        /// <param name="stringArray"></param>
        /// <returns></returns>
        [Command("G")]
        [Alias("g", "ㅎ")]
        public async Task AddGCommnad(params string[] stringArray)
        {
            try
            {
                await InsertNewRoomInfo(stringArray, "RoomInfo", "g");
            }
            catch (StorageException sex)
            {
                Console.WriteLine(sex.Message);
                await Context.Channel.SendMessageAsync(sex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Context.Channel.SendMessageAsync("잘못된 형식의 추가입니다.\nWrong Type To Add\n추가형식 : 방이름 / 남은시간(숫자만) \nAdd Type : Room Name / Left Time(Only Integer minute)");
            }
        }

        /// <summary>
        /// 로얄토큰방 추가 함수
        /// </summary>
        /// <param name="stringArray"></param>
        /// <returns></returns>
        [Command("T")]
        [Alias("t", "ㅅ")]
        public async Task AddTCommnad(params string[] stringArray)
        {
            try
            {
                await InsertNewRoomInfo(stringArray, "RoomInfo", "t");
            }
            catch (StorageException sex)
            {
                Console.WriteLine(sex.Message);
                await Context.Channel.SendMessageAsync(sex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Context.Channel.SendMessageAsync("잘못된 형식의 추가입니다.\nWrong Type To Add\n추가형식 : 방이름 / 남은시간(숫자만) \nAdd Type : Room Name / Left Time(Only Integer minute)");
            }
        }

        /// <summary>
        /// 로얄스킨방 추가 함수
        /// </summary>
        /// <param name="stringArray"></param>
        /// <returns></returns>
        [Command("K")]
        [Alias("k", "ㅏ")]
        public async Task AddSCommnad(params string[] stringArray)
        {
            try
            {
                await InsertNewRoomInfo(stringArray, "RoomInfo", "k");
            }
            catch (StorageException sex)
            {
                Console.WriteLine(sex.Message);
                await Context.Channel.SendMessageAsync(sex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Context.Channel.SendMessageAsync("잘못된 형식의 추가입니다.\nWrong Type To Add\n추가형식 : 방이름 / 남은시간(숫자만) \nAdd Type : Room Name / Left Time(Only Integer minute)");
            }
        }

        public async Task InsertNewRoomInfo(string[] stringArray, string tableName, string boxType)
        {
            string name;
            int leftTime;
            if (stringArray.Length > 1)
            {
                name = stringArray[0];
                leftTime = Int32.Parse(stringArray[2].Trim());
            }
            else
            {
                string[] sbStringArray = stringArray[0].Split("/");
                name = sbStringArray[0];
                leftTime = Int32.Parse(sbStringArray[1].Trim());
            }
            // Init Table Storage Connection
            CloudTable table = TableInit(tableName);

            TableQuery<RoomModel> query = new TableQuery<RoomModel>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, name.ToString()));

            TableQuerySegment<RoomModel> tableQueryResult = table.ExecuteQuerySegmentedAsync(query, null).Result;
            List<RoomModel> result = new List<RoomModel>();
            result.AddRange(tableQueryResult.Results);

            if (result.Count >= 1)
            {
                await Context.Channel.SendMessageAsync("해당 스트리머의 정보가 존재 합니다.");
            }
            else
            {
                RoomModel roomModel = new RoomModel(name, boxType)
                {
                    leftTime = leftTime
                };
                TableOperation insert = TableOperation.Insert((TableEntity)roomModel);
                await table.ExecuteAsync(insert);
                await ScheduleCommand(stringArray);
            }
        }

        /// <summary>
        /// 추가된 단타방 리스트 출력
        /// </summary>
        /// <param name="stringArray"></param>
        /// <returns></returns>
        [Command("Schedule")]
        [Alias("Time", "time", "시간", "단타")]
        public async Task ScheduleCommand(params string[] stringArray)
        {
            string roomInfo = "";

            // RoomInfo의 모든 엔티티 가져오기
            CloudTable table = TableInit("RoomInfo");

            TableContinuationToken token = null;
            var entities = new List<RoomModel>();
            var queryResult = table.ExecuteQuerySegmentedAsync(new TableQuery<RoomModel>(), token);
            entities.AddRange(queryResult.Result);

            //Embed 메시지를 생성할 빌더 인스턴스 생성 
            EmbedBuilder eb = new EmbedBuilder();

            eb.Color = Color.Blue;
            eb.Title = "단타방 시간표";

            //슈퍼 보스 일정 생성
            foreach (var item in entities.FindAll(x=>x.RowKey.Equals("s")))
            {
                if (item.Timestamp.AddMinutes(item.leftTime) > DateTime.UtcNow)
                {
                    roomInfo += item.PartitionKey + " / " + item.Timestamp.AddHours(9).AddMinutes(item.leftTime).ToString("HH:mm")+"\n";
                }
            }
            eb.AddField("Super Boss", roomInfo == "" ? "일정이 없습니다." : roomInfo);

            //보스 일정 생성
            roomInfo = "";
            foreach (var item in entities.FindAll(x => x.RowKey.Equals("b")))
            {
                if (item.Timestamp.AddMinutes(item.leftTime) > DateTime.UtcNow)
                {
                    roomInfo += item.PartitionKey + " / " + item.Timestamp.AddHours(9).AddMinutes(item.leftTime).ToString("HH:mm") + "\n";
                }
            }
            eb.AddField("Boss", roomInfo == "" ? "일정이 없습니다." : roomInfo);

            //골드방 일정 생성
            roomInfo = "";
            foreach (var item in entities.FindAll(x => x.RowKey.Equals("g")))
            {
                if (item.Timestamp.AddMinutes(item.leftTime) > DateTime.UtcNow)
                {
                    roomInfo += item.PartitionKey + " / " + item.Timestamp.AddHours(9).AddMinutes(item.leftTime).ToString("HH:mm") + "\n";
                }
            }
            eb.AddField("Royalty Gold", roomInfo == "" ? "일정이 없습니다." : roomInfo);

            //토큰방 일정 생성
            roomInfo = "";
            foreach (var item in entities.FindAll(x => x.RowKey.Equals("t")))
            {
                if (item.Timestamp.AddMinutes(item.leftTime) > DateTime.UtcNow)
                {
                    roomInfo += item.PartitionKey + " / " + item.Timestamp.AddHours(9).AddMinutes(item.leftTime).ToString("HH:mm") + "\n";
                }
            }
            eb.AddField("Royalty Token", roomInfo == "" ? "일정이 없습니다." : roomInfo);

            //스킨방 일정 생성
            roomInfo = "";
            foreach (var item in entities.FindAll(x => x.RowKey.Equals("k")))
            {
                if (item.Timestamp.AddMinutes(item.leftTime) > DateTime.UtcNow)
                {
                    roomInfo += item.PartitionKey + " / " + item.Timestamp.AddHours(9).AddMinutes(item.leftTime).ToString("HH:mm") + "\n";
                }
            }
            eb.AddField("Royalty Skin", roomInfo == "" ? "일정이 없습니다." : roomInfo);

            await Context.Channel.SendMessageAsync("", false, eb.Build());

            await FindPassedRoom(entities, stringArray);
        }

        public async Task FindPassedRoom(List<RoomModel> entities, params string[] stringArray)
        {
            List<RoomModel> removeEntities = new List<RoomModel>();
            foreach (var item in entities)
            {
                if (item.Timestamp.AddMinutes(item.leftTime) <= DateTime.UtcNow)
                {
                    removeEntities.Add(item);
                }
            }
            await DeleteCommand(removeEntities);
        }

        [Command("Delete")]
        [Alias("D", "d")]
        public async Task DeleteCommand(params string[] stringArray)
        {
            // Init Table Storage Connection
            CloudTable table = TableInit("RoomInfo");

            string name;
            if (stringArray.Length > 1)
            {
                name = stringArray[0];
            }
            else
            {
                string[] sbStringArray = stringArray[0].Split("/");
                name = sbStringArray[0];
            }

            TableQuery<RoomModel> query = new TableQuery<RoomModel>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, name.ToString()));

            TableQuerySegment<RoomModel> tableQueryResult = table.ExecuteQuerySegmentedAsync(query, null).Result;
            List<RoomModel> entities = new List<RoomModel>();
            entities.AddRange(tableQueryResult.Results);

            try
            {
                foreach (var item in entities)
                {
                    TableOperation delete = TableOperation.Delete((TableEntity)item);
                    await table.ExecuteAsync(delete);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
        }

        public async Task DeleteCommand(List<RoomModel> entities)
        {
            // Init Table Storage Connection
            CloudTable table = TableInit("RoomInfo");

            try
            {
                foreach (var item in entities)
                {
                    TableOperation delete = TableOperation.Delete((TableEntity)item);
                    await table.ExecuteAsync(delete);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
