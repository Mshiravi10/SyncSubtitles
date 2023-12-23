using Microsoft.Extensions.DependencyInjection;
using SyncSubtitles;
using SyncSubtitles.Interfaces;
using SyncSubtitles.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

class Program
{
    static ITelegramBotClient botClient;
    private readonly SyncSubtitles.Interfaces.IUpdateHandler _updateHandler;
    private readonly IHandleError _handleError;

    public Program(SyncSubtitles.Interfaces.IUpdateHandler updateHandler, IHandleError handleError)
    {
        _updateHandler = updateHandler;
        _handleError = handleError;
    }

    static void Main(string[] args)
    {

        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        var serviceProvider = serviceCollection.BuildServiceProvider();

        botClient = new TelegramBotClient("####"); // your bot token

        using var cts = new CancellationTokenSource();
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };

        var updateHandler = serviceProvider.GetService<SyncSubtitles.Interfaces.IUpdateHandler>();
        var handleError = serviceProvider.GetService<IHandleError>();



        botClient.StartReceiving(
            updateHandler.HandleUpdateAsync,
            handleError.HandleErrorAsync,
            receiverOptions,
            cancellationToken: cts.Token);

        Console.WriteLine("Bot is running...");
        Console.ReadLine();

        cts.Cancel();
    }

    static void ConfigureServices(IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var transactionServiceTypes = assembly.GetTypes()
            .Where(t => t.IsInterface && typeof(IAutoInjectable).IsAssignableFrom(t) && t != typeof(IAutoInjectable));

        foreach (var serviceType in transactionServiceTypes)
        {
            var implementation = assembly.GetTypes().FirstOrDefault(t => serviceType.IsAssignableFrom(t) && t.IsClass);
            if (implementation != null)
            {
                services.AddTransient(serviceType, implementation);
            }
        }
    }



























}


