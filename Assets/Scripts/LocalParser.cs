using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LSB
{
    public class LocalParser
    { 

        private static List<string> GetAnimationListCodes()
        {
            List<string> AnimationListCodes = new List<string>();
            TextAsset codes = (TextAsset)Resources.Load("Codes");
            char[] delimiters = new char[] { '\r', '\n' };
            foreach (string s in codes.text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries))
            {
                AnimationListCodes.Add(s);
            }
            return AnimationListCodes;
        } 

        public static ExpressionList ParseExpressionList(string input)
        {
            List<string> animationListCodes = GetAnimationListCodes();
            ExpressionList output = new ExpressionList();
            output.tokens = new List<Expression>();
            List<string> list = SplitInput(RemoveDiacritics(input.ToLower()), animationListCodes);
            foreach (string word in list)
            { 
                Expression expression = new Expression();
                expression.word = RemoveDiacritics(word.ToLower());
                expression.code = new List<string>();
                foreach (string animation in animationListCodes)
                { 
                    char[] delimiters = new char[] { '#' };
                    string wordToMatch = RemoveDiacritics(animation.Split(delimiters)[0].ToLower());
                     
                    if (expression.word==wordToMatch)
                    {
                        string code = "#"+animation.Split(delimiters)[1]; 
                        expression.code.Add(code);
                        break;
                    }
                }
                output.tokens.Add(expression);
            }
            return output;
        }  

        private static List<string> SplitInput(string input, List<string> animationListCodes)
        {
            List<string> list = new List<string>(); 
            if (!IsPhrase(input, animationListCodes))
            { 
                return input.Split(' ').ToList();
            }
            else
            {
                list.Add(input);
            }
            return list;
        }

        private static bool IsPhrase(string input, List<string> animationListCodes)
        {
            List<string> phrases = new List<string>(); 
            foreach (string word in animationListCodes)
            {
                if(word.Contains(' '))
                {
                    char[] delimiters = new char[] { '#' };
                    phrases.Add(RemoveDiacritics(word.Split(delimiters)[0].ToLower())); 
                }
            }
            return phrases.Contains(input);
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
                    expression.code.Add(getLetterAnimationCode(letter));
                }
            }
            return expression;
        }

        private static string getLetterAnimationCode(char letter)
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
