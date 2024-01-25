using Blog.News.Consumer;
using Blog.News.Consumer.Settings;
using MassTransit;
using Blog.News.Consumer.Handlers;
using Blog.Services.Email;
using Blog.Domain.Interfaces.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<Worker>();
        var applicationSettings = new ApplicationSettings();
        hostContext.Configuration.GetSection(nameof(ApplicationSettings)).Bind(applicationSettings);

        services.AddSingleton(applicationSettings);
        services.AddScoped<IEmailService, EmailService>();

        services.AddMassTransit(config =>
        {
            config.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(applicationSettings.MassTransitSettings.Server, "/", host =>
                {
                    host.Username(applicationSettings.MassTransitSettings.User);
                    host.Password(applicationSettings.MassTransitSettings.Password);
                });
                cfg.ReceiveEndpoint(applicationSettings.MassTransitSettings.NewsQueue, e =>
                {
                    e.Consumer<PublishNewsMessageHandler>(context);
                });

                cfg.ConfigureEndpoints(context);
            });

            config.AddConsumer<PublishNewsMessageHandler>();
        });
    })
    .Build();

host.Run();
