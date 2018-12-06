using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterAfrica.PayAt
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var accounts = new List<Account>();
            accounts.Add(new Account
            {
                Amount = 30.01m,
                ContactNumber = "0813702097",
                ExtraNumbers = new[] { "0813702097", "0613909635" },
                IdentityNumber = "6907315115089",
                IsActive = true,
                NameFirst = "Justin",
                NameLast = "Minnaar",
                NameTitle = "Mr",
                Number = 1,
            });
            accounts.Add(new Account
            {
                Amount = 30.02m,
                ContactNumber = "0871950828",
                ExtraNumbers = new[] { "0827207858", "0635485614" },
                IdentityNumber = "9503145173088",
                IsActive = true,
                NameFirst = "Luke",
                NameLast = "Griqua",
                NameTitle = "Mr",
                Number = 2,
            });
            accounts.Add(new Account
            {
                Amount = 30.03m,
                ContactNumber = "0728698549",
                ExtraNumbers = null,
                IdentityNumber = "7111120007081",
                IsActive = false,
                NameFirst = "Karen",
                NameLast = "Campbell",
                NameTitle = "Ms",
                Number = 3,
            });

            var worker = new PayAtWorker();
            var fileId = 1;
            var upload = false;
            worker.CreateLines(fileId, new DateTime(2018, 12, 6), accounts, upload);
        }
    }
}