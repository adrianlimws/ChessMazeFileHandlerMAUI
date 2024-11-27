using System;
using System.Text;
using ChessMaze.FileHandler.Filer.Interfaces;


namespace ChessMaze.FileHandler.Filer.Extensions;

public class LevelConverter : IConverter
{
    public string Expanded { get; set; }
    public string Compressed { get; set; }

    public void Compress(string uncompressedLevel)
    {
        var lines = uncompressedLevel.Split('\n');
        var sb = new StringBuilder();

        foreach (var line in lines)
        {
            int emptyCount = 0;
            foreach (char character in line)
            {
                if (character == 'E')
                {
                    emptyCount++;
                }
                else
                {
                    if (emptyCount > 0)
                    {
                        sb.Append($"E{emptyCount}");
                        emptyCount = 0;
                    }

                    sb.Append(character);
                }
            }

            if (emptyCount > 0)
            {
                sb.Append($"E{emptyCount}");
            }

            sb.Append(',');
        }

        Compressed = sb.ToString().TrimEnd(',');
    }

    public void Expand(string compressedLevel)
    {
        var lines = compressedLevel.Split(','); // split to individual lines
        var sb = new StringBuilder();

        foreach (var line in lines)
        {
            for (int ch = 0; ch < line.Length; ch++)
            {
                // if  character is 'E' followed by a digit
                if (line[ch] == 'E' && ch + 1 < line.Length && char.IsDigit(line[ch + 1]))
                {
                    // parse num of empty cells
                    int count = int.Parse(line[ch + 1].ToString());
                    // add 'E' and count num of times
                    sb.Append(new string('E', count));
                    ch++; // skip next char that has the digit since its processed
                }
                else
                {
                    sb.Append(line[ch]); // if not 'E' followed by digit, append the char
                }
            }
            sb.AppendLine();
        }

        Expanded = sb.ToString().TrimEnd();
    }
}

// References
// https://learn.microsoft.com/en-us/dotnet/api/system.text.stringbuilder?view=net-8.0
// https://codereview.stackexchange.com/questions/223450/run-length-encoding-in-c
// https://stackoverflow.com/questions/27573521/run-length-encoding-of-a-given-sorted-string
// https://learn.microsoft.com/en-us/dotnet/api/system.string.-ctor?view=net-8.0#system-string-ctor(system-char-system-int32)
