using Knights.Core.Common;
using Knights.Fluid.Datums;
using System;
using System.Collections.Generic;
using System.IO;

namespace BetterAfrica.PayAt
{
    public class PayAtWorker
    {
        public string[] CreateLines(DateTime dataDateTime, IEnumerable<Account> accounts)
        {
            // should be fetched from the database
            var seqNo = "000001";
            var date = dataDateTime.ToString("yyyy_MM_dd_HH_mm_ss");

            // generate file
            var fileType = "A"; // ACCOUNT
            var prefix = "11690";
            var fileName = $"{fileType}_{prefix}_{seqNo}_{date}.csv";

            var lines = new List<string>();
            var header = "H,FULL,1.0";
            lines.Add(header);

            var count = 0;
            foreach (var account in accounts)
            {
                var number = account.Number;
                if (number < 0 || number > 1000000) continue;

                var title = account.NameTitle?.Replace(",", " ");

                var firstName = account.NameFirst?.Replace(",", " ");
                if (firstName.IsNullOrWhiteSpace()) continue;

                var lastName = account.NameLast?.Replace(",", " ");
                if (lastName.IsNullOrWhiteSpace()) continue;

                var id = new SouthAfricanIdentityNumber
                {
                    Number = account.IdentityNumber
                };
                //if (!id.IsValid) continue;
                var identity = id.Number;

                var contact = account.ContactNumber?.Replace(",", " ");
                if (contact.IsNullOrWhiteSpace()) continue;

                if (account.Amount < 0 || account.Amount > 10000) continue;
                var amountCents = (int)(account.Amount * 100);
                string references = null;
                if (account.ExtraNumbers != null)
                    foreach (var extra in account.ExtraNumbers)
                    {
                        if (references != null) references += ";";
                        var reference = extra.Replace(",", " ").Replace(";", " ");
                        references += extra;
                    }

                var active = account.IsActive ? "1" : "0";

                var line = $"D,{number},{title},{firstName},{lastName},{identity},{contact},{amountCents},{active}";

                lines.Add(line);
                count++;
            }

            lines.Add($"T,{count}");

            return lines.ToArray();
        }
    }

    public class Account
    {
        public int Number { get; set; }
        public string NameTitle { get; set; }
        public string NameFirst { get; set; }
        public string NameLast { get; set; }
        public string IdentityNumber { get; set; }
        public string ContactNumber { get; set; }
        public decimal Amount { get; set; }
        public IEnumerable<string> ExtraNumbers { get; set; }
        public bool IsActive { get; set; }
    }
}