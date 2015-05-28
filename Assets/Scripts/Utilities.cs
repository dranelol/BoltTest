using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class Pair<T1, T2>
{
    public Pair(T1 item1, T2 item2)
    {
        First = item1;
        Second = item2;
    }

    public T1 First { get; set; }
    public T2 Second { get; set; }

    public override string ToString()
    {
        return "< " + First.ToString() + ", " + Second.ToString() + " >";
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Pair<T1, T2> other = (Pair<T1, T2>)obj;
        return (First.Equals(other.First) && Second.Equals(other.Second));
    }

    public override int GetHashCode()
    {
        if (First == null || Second == null)
        {
            return 0;
        }

        else
        {
            return First.GetHashCode() + 17 * Second.GetHashCode();
        }
    }
}

public static class Messages
{
    public static string[] Messages1 = 
    {
        "Hello world", 
        "I can't let you do that Dave", 
        "Would you like to play a game of chess?" 
    };

    public static string[] Messages2 = 
    {
        "Arigatou, senpai-sama-san-chan", 
        "Onrii Ei izu eikuseputoruuu, noto Bii! Yu disonoru yoru famuriiiii!!!",
        "Konichiwa, mother fucker" 
    };

    public static string[] Messages3 = 
    {
        "More of those... those [FF0000FF]things.[-]",
    };

    public static string[] Messages4 = 
    {
        "[800080FF]" + "M" + "[-]" +
        "[0000FFFF]" + "o" + "[-]" +
        "[008000FF]" + "r" + "[-]" +
        "[FFFF00FF]" + "e " + "[-]" +
        "[FFA500FF]" + "o" + "[-]" +
        "[FF0000FF]" + "f " + "[-]" +
        "those... those " +
        "[FF0000FF]" + "things." + "[-]",
        
        "Hello world", 
        "I can't let you do that Dave", 
        "Would you like to play a game of chess?" 
    };

    public static string[] Messages5 = 
    {
        "12 12345 12345 123 123456 12 12 1234 123456789 12 123 1234567890 12 12345 12345 123 123456 12 12 1234 123456789 12 123 1234567890 12 12345 12345 123 123456 12 12 1234 123456789 12 123 1234567890",
    };

    public static string[] Messages6 = 
    {
        "[FF0000FF]A[-] [FF0000FF]A[-] [FF0000FF]A[-] [FF0000FF]A[-] [FF0000FF]A[-] [FF0000FF]A[-]" 
     + " [FF0000FF]A[-] [FF0000FF]A[-] [FF0000FF]A[-] [FF0000FF]A[-] [FF0000FF]A[-] [FF0000FF]A[-]"
     + " [FF0000FF]A[-] [FF0000FF]A[-] [FF0000FF]A[-] [FF0000FF]A[-] [FF0000FF]A[-] [FF0000FF]A[-]",
    };
}

public static class Utilities 
{

    static char[] SplitChars = new char[] { ' ', '\t' };

    public static string ColorToHexString(Color color)
    {
        Color32 color32 = color;
        return color32.r.ToString("X2") + color32.g.ToString("X2") + color32.b.ToString("X2") + color32.a.ToString("X2");
    }

    public static string[] ParseText(string text, int MaxChars)
    {
        string[] words = StrExplode(text);
        int lineLength = 0;
        StringBuilder builder = new StringBuilder();

        for (int i = 0; i < words.Length; i++)
        {
            string word = words[i];

            // if adding the new word to the current line would be too long,
            // then put it on a new line

            int wordLength = StripTags(word).Length;
            //int wordLength = word.Length;

            if (lineLength + wordLength > MaxChars)
            {
                // only move down to a new line if we have text on the current line

                // avoids situation where wrapped whitespace causes emptylines in text
                if (lineLength > 0)
                {
                    builder.Append(System.Environment.NewLine);
                    lineLength = 0;
                }

                // remove leading whitespace from the word so the new line starts flush to the left.
                word = word.TrimStart();
            }

            builder.Append(word);
            lineLength += wordLength;

        }

        string[] lines = builder.ToString().Split(new string[] { System.Environment.NewLine }, System.StringSplitOptions.None);

        return lines;
    }

    static string[] StrExplode(string str)
    {
        List<string> words = new List<string>();
        int start = 0;

        while (true)
        {
            // look for the first word split, starting from 0
            int index = str.IndexOfAny(SplitChars, start);


            if (index == -1)
            {
                // no more word splits, we're done!
                words.Add(str.Substring(start));

                return words.ToArray();
            }

            // we need to split a word off
            string newWord = str.Substring(start, index - start);

            char next = str.Substring(index, 1)[0];

            if (char.IsWhiteSpace(next))
            {
                // stick dashes, etc to the previous word; whitespace doesnt matter

                words.Add(newWord);
                words.Add(next.ToString());
            }

            else
            {
                words.Add(newWord + next);
            }

            start = index + 1;
        }
    }

    static string StripTags(string text)
    {
        string newText = "";

        char[] charArray = text.ToCharArray();

        for (int i = 0; i < charArray.Length; i++)
        {
            if (charArray[i] == '[')
            {
                // parse text, find end bracket, increase counter by width of tag

                int length = 0;

                for (int j = i; j < charArray.Length; j++)
                {
                    if (charArray[j] == ']')
                    {
                        length = j - i;
                        break;
                    }
                }

                i += length;
            }

            else
            {

                newText += charArray[i];
            }

        }

        return newText;
    }
	
    
}
