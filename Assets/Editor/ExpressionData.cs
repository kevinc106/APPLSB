using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreationStates
{
    class ExpressionData
    {
        public string Expression { get; set; }
        public string[] Synonymous { get; set; }
        public string[] Code { get; set; }
        public string Type { get; set; }
        public bool Ignore { get; set; }
        internal void updateExpression(string categoryCode, string set)
        {
            for (int i = 0; i < Code.Length; i++)
            {
                if (!Code[i].Contains("#"))
                {
                    Code[i] = $"{categoryCode}{set}{Code[i]}";
                }
            } 
        }
    }
}
