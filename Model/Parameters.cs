﻿namespace Model;

public static class Parameters
{
    public static string[] Keywords => new string[]
    {
        ".net%20core",
        "asp.net",
        ".net",
        "net",
        "C%20sharp",
        "C%23"
    };

    public static string[] KeywordsFilters => new string[]
    {
        "net",
        "c#"
    };
    public static string RabbitMqExchangeName => "rabbit_mq_exchange";
}
