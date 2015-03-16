using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BoS_Launcher.Tools.Helper
{
    public static class ChatFormat
    {
        private static readonly Regex UrlRegex = new Regex(@"(?#Protocol)(?:(?:ht|f)tp(?:s?)\:\/\/|~/|/)?(?#Username:Password)(?:\w+:\w+@)?(?#Subdomains)(?:(?:[-\w]+\.)+(?#TopLevel Domains)(?:com|org|net|gov|mil|biz|info|mobi|name|aero|jobs|museum|travel|[a-z]{2}))(?#Port)(?::[\d]{1,5})?(?#Directories)(?:(?:(?:/(?:[-\w~!$+|.,=]|%[a-f\d]{2})+)+|/)+|\?|#)?(?#Query)(?:(?:\?(?:[-\w~!$+|.,*:]|%[a-f\d{2}])+=(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)(?:&amp;(?:[-\w~!$+|.,*:]|%[a-f\d{2}])+=(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)*)*(?#Anchor)(?:#(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)?");

        /// <summary>
        /// Checks if a string is a hyperlink
        /// </summary>
        /// <param name="word">word to check</param>
        /// <returns>Bool if the wor is a hyperlink</returns>
        public static bool IsHyperlink(string word)
        {
            // First check to make sure the word has at least one of the characters we need to make a hyperlink
            if (word.IndexOfAny(@":.\/".ToCharArray()) != -1)
            {
                if (Uri.IsWellFormedUriString(word, UriKind.Absolute))
                {
                    // The string is an Absolute URI
                    return true;
                }
                else if (UrlRegex.IsMatch(word))
                {
                    try
                    {
                        Uri uri = new Uri(word, UriKind.RelativeOrAbsolute);

                        if (!uri.IsAbsoluteUri)
                        {
                            // rebuild it it with http to turn it into an Absolute URI
                            uri = new Uri(@"http://" + word, UriKind.Absolute);
                        }

                        if (uri.IsAbsoluteUri)
                        {
                            return true;
                        }
                    }
                    catch
                    {
                        return false;
                    }

                }
                else
                {
                    try
                    {
                        Uri wordUri = new Uri(word);

                        // Check to see if URL is a network path
                        if (wordUri.IsUnc || wordUri.IsFile)
                        {
                            return true;
                        }
                    }
                    catch
                    {
                        return false;
                    }

                }
            }

            return false;
        }
    }
}
