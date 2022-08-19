using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts_Assessment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountTransactionController: ControllerBase
    {
        private readonly IAccountTransactionService _accountTransactionService;

        public AccountTransactionController(IAccountTransactionService accountTransactionService)
        {
            _accountTransactionService = accountTransactionService;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            await _accountTransactionService.DepositWithdrawal(Guid.NewGuid(), 100001, 1000);

            return "Get request served!";
        }
        
        [HttpGet("{id}")]
        public string Get(int id) => "Get specific request served!";

    }
}
