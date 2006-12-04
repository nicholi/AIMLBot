using System;
using System.Xml;
using AIMLbot.Utils;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;

namespace ExampleCustomAIMLTags
{
    /// <summary>
    /// Ranslatestay Englishway ordsway intoway Igpay Atinlay :-)
    /// 
    /// (Translates English words into Pig Latin)
    /// </summary>
    [CustomTag]
    public class piglatin : AIMLTagHandler
    {
        public piglatin()
        {
            this.inputString = "piglatin";
        }

        protected override string ProcessChange()
        {
            if (this.templateNode.Name.ToLower() == "piglatin")
            {
                if (this.templateNode.InnerText.Length > 0)
                {
                    StringBuilder result = new StringBuilder();
                    string[] words = this.templateNode.InnerText.ToLower().Split(" ".ToCharArray());

                    foreach (string word in words)
                    {
                        char[] letters = word.ToCharArray();

                        string consonantEnd = "ay";
                        string vowelEnd = "way";
                        string[] doubleConsonants = { "ph", "th", "ch", "pn", "sh", "st", "sp" };
                        string[] punctuation = { "\"", ".", "!", ";", "?", ")" };
                        Regex vowels = new Regex("[aeiou]", RegexOptions.IgnoreCase);
                        Regex validChars = new Regex("[a-z]", RegexOptions.IgnoreCase);
                        int locationOfFirstLetter = 0;
                        bool isVowelEnding=false;
                        string firstChar = "";
                        foreach (char character in letters)
                        {
                            if (vowels.IsMatch(character.ToString()))
                            {
                                isVowelEnding = true;
                                firstChar=character.ToString();
                                break;
                            }
                            else if (validChars.IsMatch(character.ToString()))
                            {
                                isVowelEnding = false;
                                string firstCharPair = word.Substring(locationOfFirstLetter, 2);
                                foreach (string doubleCheck in doubleConsonants)
                                {
                                    if (firstCharPair == doubleCheck)
                                    {
                                        firstChar = firstCharPair;
                                    }
                                }
                                if (firstChar.Length == 0)
                                {
                                    firstChar = character.ToString();
                                }
                                break;
                            }
                            locationOfFirstLetter++;
                        }
                        // stitch together
                        if (locationOfFirstLetter > 0)
                        {
                            // start the word with any non-character chars (e.g. open brackets)
                            result.Append(word.Substring(0, locationOfFirstLetter));
                        }
                        int newStart = 0;
                        if (isVowelEnding)
                        {
                            newStart = locationOfFirstLetter;
                        }
                        else
                        {
                            newStart = locationOfFirstLetter + firstChar.Length;
                        }
                        string tail = "";
                        if(isVowelEnding)
                        {
                            tail=vowelEnd;
                        }
                        else
                        {
                            tail=consonantEnd;
                        }

                        for(int i=newStart;i<letters.Length;i++)
                        {
                            string letter = letters[i].ToString();
                            bool isCharacter = true;
                            foreach(string puntuationEnd in punctuation)
                            {
                                if(letter==puntuationEnd)
                                {
                                    tail+=letter;
                                    isCharacter=false;
                                }
                            }

                            if(isCharacter)
                            {
                                result.Append(letter);
                            }
                        }
                        if (!isVowelEnding)
                        {
                            result.Append(firstChar);
                        }
                        result.Append(tail+" ");                        
                    }
                    XmlNode dummySentence = getNode("<sentence>" + result.ToString().Trim() + "</sentence>");
                    AIMLbot.AIMLTagHandlers.sentence sentenceMaker = new AIMLbot.AIMLTagHandlers.sentence(this.bot, this.user, this.request, this.result, dummySentence);

                    return sentenceMaker.Transform();
                }
            }
            return string.Empty;
        }
    }
}
