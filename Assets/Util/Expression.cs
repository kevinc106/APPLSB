using System;
using System.Collections.Generic;

namespace LSB
{
    [Serializable]
    public class Expression
    {
        public string word;
        public List<string> code;

        public string getWord()
        {
            return word;
        }

        public string getList()
        {
            string res="";
            foreach (string c in code)
            {
                res += c;
            }
            return res;
        }
         
    }

    
}
