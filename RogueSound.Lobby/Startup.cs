using Cosmy.DependencyInjection;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using RogueSound.Lobby.Actions;
using System;
using System.Collections.Generic;
using System.Text;

[assembly: FunctionsStartup(typeof(RogueSound.Lobby.Startup))]
namespace RogueSound.Lobby
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var services = builder.Services;

            services.AddCosmyClient(new Cosmy.Config.CosmyConfiguration
            {
                Database = "RogueSound",
                Endpoint = Environment.GetEnvironmentVariable("CosmosEndpoint"),
                Key = Environment.GetEnvironmentVariable("CosmosKey")
            });

            services.AddScoped(typeof(GetRoomsAction));
            services.AddScoped(typeof(DeleteRoomAction));
            services.AddScoped(typeof(AddRoomAction));
        }
    }
}
