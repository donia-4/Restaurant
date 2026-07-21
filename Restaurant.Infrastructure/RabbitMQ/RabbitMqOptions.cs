using Restaurant.Application.Common.Messages;

namespace Restaurant.Infrastructure.RabbitMQ;

public sealed class RabbitMqOptions
{
    public const string SectionName = "RabbitMQ";

    public string HostName { get; init; } = default!;

    public int Port { get; init; } = 5672;

    public string UserName { get; init; } = default!;

    public string Password { get; init; } = default!;

    public string VirtualHost { get; init; } = "/";

    public string ExchangeName { get; init; } = ExchangeNames.RestaurantEvents;
}