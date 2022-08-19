using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Accounts_Assessment
{
    internal class Program
    {
        static void Main(string[] args)
        {
            WebHost.CreateDefaultBuilder()
                .UseKestrel()
                .ConfigureServices(services =>
                {
                    services.AddScoped<IAccountStore, AccountStore>();
                    services.AddScoped<ITransactionStore, TransactionStore>();
                    services.AddScoped<IAccountTransactionService, AccountTransactionService>();
                    services.AddMvc();
                })
                .Configure(config =>
                {
                    config.UseMvc();
                })
                .Build()
                .Run();
        }
    }
}
