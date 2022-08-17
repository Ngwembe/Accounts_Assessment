﻿public class Transaction
{
    //  Primary unique key, incremental, generated by DB
    public long UniqueId { get; set; }

    //  Transaction reference id provided by caller of the transaction service
    public Guid ReferenceId { get; set; }

    public long AccountNr { get; set; }

    public decimal Amount { get; set; }

    //  Additional data provided by the transaction service
    public string Payload { get; }
}