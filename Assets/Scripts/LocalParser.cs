using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LSB
{
    public class LocalParser
    {
        public static ExpressionList ParseExpressionList(string input)
        {
            ExpressionList output = new ExpressionList();
            output.tokens = new List<Expression>();
            List<string> list = input.Split(' ').ToList();
            foreach (string word in list)
            {
                Expression expression = new Expression();
                expression.word = RemoveDiacritics(word.ToLower());
                expression.code = new List<string>();
                foreach (char letter in expression.word)
                {
                    expression.code.Add(getAnimationCode(letter));
                }
                output.tokens.Add(expression);
            }
            return output;
        }

        public static Expression ParseExpression(string input)
        {
            Expression expression = new Expression();
            expression.word = RemoveDiacritics(input.ToLower());
            expression.code = new List<string>();
            foreach (char letter in expression.word)
            {
                if(letter != ' ')
                {
                    expression.code.Add(getAnimationCode(letter));
                }
            }
            return expression;
        }

        private static string getAnimationCode(char letter)
        {
            switch (letter)
            {
                case 'a': return "#00101";
                case 'b': return "#00102";
                case 'c': return "#00103";
                //case 'ch': return "#00104";
                case 'd': return "#00105";
                case 'e': return "#00106";
                case 'f': return "#00107";
                case 'g': return "#00108";
                case 'h': return "#00109";
                case 'i': return "#00110";
                case 'j': return "#00111";
                case 'k': return "#00112";
                case 'l': return "#00113";
                //case 'll': return "#00114";
                case 'm': return "#00115";
                case 'n': return "#00116";
                case 'ñ': return "#00117";
                case 'o': return "#00118";
                case 'p': return "#00119";
                case 'q': return "#00120";
                case 'r': return "#00121";
                //case 'rr': return "#00122";
                case 's': return "#00123";
                case 't': return "#00124";
                case 'u': return "#00125";
                case 'v': return "#00126";
                case 'w': return "#00127";
                case 'x': return "#00128";
                case 'y': return "#00129";
                case 'z': return "#00130";
                case '0': return "#32101";
                case '1': return "#32102";
                case '2': return "#32103";
                case '3': return "#32104";
                case '4': return "#32105";
                case '5': return "#32106";
                case '6': return "#32107";
                case '7': return "#32108";
                case '8': return "#32109";
                case '9': return "#32110";
            }
            return "#00000";
        }

        private static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();
            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }
            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
