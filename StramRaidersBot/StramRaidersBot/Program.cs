using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace StramRaidersBot
{
    public class Program
    {
        DiscordSocketClient discordSocketClient;
        CommandService commandService;

        /// <summary>
        /// Start Discord Bot
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // 봇의 진입점 실행
            new Program().BotMain().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Enterance Bot
        /// </summary>
        /// <returns></returns>
        public async Task BotMain()
        {
            // Initailize SR Bot
            // Set LogLevel
            discordSocketClient = new DiscordSocketClient(new DiscordSocketConfig()
            {
                LogLevel = Discord.LogSeverity.Verbose
            });
            // Initialize Command Service Client
            // Set LogLevel
            commandService = new CommandService(new CommandServiceConfig()
            {
                LogLevel = Discord.LogSeverity.Verbose
            });

            discordSocketClient.Log += OnClientLogReceived;
            commandService.Log += OnClientLogReceived;
            
            await discordSocketClient.LoginAsync(TokenType.Bot, Properties.Resources.API_Key); //봇의 토큰을 사용해 서버에 로그인
            await discordSocketClient.StartAsync();                         //봇이 이벤트를 수신하기 시작

            discordSocketClient.MessageReceived += OnClientMessage;         //봇이 메시지를 수신할 때 처리하도록 설정

            await commandService.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: null);  //봇에 명령어 모듈 등록

            await Task.Delay(-1);   //봇이 종료되지 않도록 블로킹
        }

        private async Task OnClientMessage(SocketMessage arg)
        {
            //수신한 메시지가 사용자가 보낸 게 아닐 때 취소
            var message = arg as SocketUserMessage;
            if (message == null) return;

            int pos = 0;

            //메시지 앞에 !이 달려있지 않고, 자신이 호출된게 아니거나 다른 봇이 호출했다면 취소
            if (!(message.HasCharPrefix('!', ref pos) ||
             message.HasMentionPrefix(discordSocketClient.CurrentUser, ref pos)) ||
              message.Author.IsBot)
                return;

            var context = new SocketCommandContext(discordSocketClient, message);                    //수신된 메시지에 대한 컨텍스트 생성   

            //모듈이 명령어를 처리하게 설정
            var result = await commandService.ExecuteAsync(
                context: context,
                argPos: pos,
                services: null
                );
        }

        /// <summary>
        /// 봇의 로그를 출력하는 함수
        /// </summary>
        /// <param name="msg">봇의 클라이언트에서 수신된 로그</param>
        /// <returns></returns>
        private Task OnClientLogReceived(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());  //로그 출력
            return Task.CompletedTask;
        }
    }
}
