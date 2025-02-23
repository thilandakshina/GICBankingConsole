using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GICBankingSystem.Console.Models
{
    public class TransactionRequest
    {
        public TransactionData Transaction { get; set; }
    }

    public class TransactionData
    {
        public DateTime CreatedDate { get; set; }
        public string AccountNo { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
    }
}
