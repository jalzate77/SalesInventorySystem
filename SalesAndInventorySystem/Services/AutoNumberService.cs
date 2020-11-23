using SalesAndInventorySystem.Common;
using SalesAndInventorySystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace SalesAndInventorySystem.Services
{
    [Export(typeof(AutoNumberService))]
    public class AutoNumberService
    {
        private Settings settings;

        public string GetNextItemSequence(InventoryDbContext context)
        {
            settings = context.Settings.First();

            string output = "";

            if (settings.AutoNumberItemReset)
            {
                output = GetNext(settings.AutoNumberItem);
                settings.AutoNumberItemReset = false;
            }
            else
            {
                output = GetNext(settings.AutoNumberItem, context.Items.Select(i => i.ItemId).Last());
            }

            return output;
        }

        public string GetNextSourcingSequence(InventoryDbContext context)
        {
            settings = context.Settings.First();

            string output = "";

            if (settings.AutoNumberSourcingReset || context.SourcingTransactions.Count() == 0)
            {
                output = GetNext(settings.AutoNumberSourcing);
                settings.AutoNumberSourcingReset = false;
            }
            else
            {
                var max = context.SourcingTransactions.OrderByDescending(i => i.DateCreated).FirstOrDefault();

                output = GetNext(settings.AutoNumberSourcing, max.TransactionId);
            }

            return output;
        }

        public string GetNextSalesSequence(InventoryDbContext context)
        {
            settings = context.Settings.First();

            string output = "";

            if (settings.AutoNumberSaleReset)
            {
                output = GetNext(settings.AutoNumberSale);
                settings.AutoNumberSaleReset = false;
            }
            else
            {
                output = GetNext(settings.AutoNumberSale, context.SaleTransactions.Select(i => i.TransactionId).Last());
            }

            return output;
        }

        private string GetNext(string format)
        {
            string output = "";

            var tokens = format.Split('-', StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < tokens.Length; i++)
            {
                string token = tokens[i];

                if (token.StartsWith('{') && token.EndsWith('}'))
                {
                    string[] realToken = token.Replace("{", "").Replace("}", "").Split(":");

                    var value = "";

                    if (realToken[0] == "date")
                    {
                        if (realToken.Length > 1)
                            value = DateTime.Now.ToString(realToken[1]);
                        else
                            value = DateTime.Now.ToShortDateString();
                    }
                    else if (realToken[0] == "auto")
                    {
                        if (realToken.Length > 1)
                            value = 1.ToString(realToken[1].Replace("#", "0"));
                        else
                            value = 1.ToString();
                    }

                    output = output.AppendDelimited(value, "-");
                }
                else
                {
                    output = output.AppendDelimited(token, "-");
                }
            }

            return output;
        }

        private string GetNext(string format, string previousId)
        {
            string output = "";

            var prevSeqtokens = previousId.Split('-', StringSplitOptions.RemoveEmptyEntries);

            var tokens = format.Split('-', StringSplitOptions.RemoveEmptyEntries);

            if (IsMatch(format, previousId))
            {
                for (int i = 0; i < tokens.Length; i++)
                {
                    string token = tokens[i];

                    if (token.StartsWith('{') && token.EndsWith('}'))
                    {
                        string[] prevSeqRealToken = prevSeqtokens[i].Replace("{", "").Replace("}", "").Split(":");

                        string[] realToken = token.Replace("{", "").Replace("}", "").Split(":");

                        var value = "";

                        if (realToken[0] == "date")
                        {
                            if (realToken.Length > 1)
                                value = DateTime.Now.ToString(realToken[1]);
                            else
                                value = DateTime.Now.ToShortDateString();

                        }
                        else if (realToken[0] == "auto")
                        {
                            if (realToken.Length > 1)
                                value = (int.Parse(prevSeqRealToken[0]) + 1).ToString(realToken[1].Replace("#", "0"));
                            else
                                value = (int.Parse(prevSeqRealToken[0]) + 1).ToString();
                        }

                        output = output.AppendDelimited(value, "-");
                    }
                    else
                    {
                        output = output.AppendDelimited(token, "-");
                    }
                }
            }
            else
            {
                output = GetNext(format);
            }


            return output;
        }

        private bool IsMatch(string format, string previousId)
        {
            var prevSeqtokens = previousId.Split('-', StringSplitOptions.RemoveEmptyEntries);

            var tokens = format.Split('-', StringSplitOptions.RemoveEmptyEntries);

            if (prevSeqtokens.Length != tokens.Length)
                return false;

            for (int i = 0; i < tokens.Length; i++)
            {
                string token = tokens[i];

                if (token.StartsWith('{') && token.EndsWith('}'))
                {
                    string[] prevSeqRealToken = prevSeqtokens[i].Replace("{", "").Replace("}", "").Split(":");

                    string[] realToken = token.Replace("{", "").Replace("}", "").Split(":");

                    if (realToken[0] == "date")
                    {
                        if (!DateTime.TryParse(prevSeqRealToken[0], out _))
                        {
                            return false;
                        }

                    }
                    else if (realToken[0] == "auto")
                    {
                        if (!int.TryParse(prevSeqRealToken[0], out _))
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    if (token != prevSeqtokens[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
