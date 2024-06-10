using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odeon.InvoiceApp
{
    public class KeyValuePair
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public KeyValuePair(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
