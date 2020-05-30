using System;
using System.Collections.Generic;

namespace LSB
{
    [Serializable]
    public class ExpressionList
    {
        public List<Expression> tokens;

        public string getexpressions()
        {
            string res="";
            foreach (var item in tokens)
            {
                res += "WORD: " + item.getWord() + " CODE: " + item.getList()+"\n";
            }
            return res;
        }
    }
}
