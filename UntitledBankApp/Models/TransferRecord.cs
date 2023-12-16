using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UntitledBankApp.Models
{
    public class TransferRecord
    {
        public DateTime Timestamp { get; set; }
        public int SourceAccountNumber { get; set; }
        public int TargetAccountNumber { get; set; }
        public decimal Amount { get; set; }
        public bool TransferSuccessful { get; set; }
    }
}
