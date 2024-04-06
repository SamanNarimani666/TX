class Buffer
{
    string[] _lines;
    List<Clipboard> _clipboard = new List<Clipboard>();

    //test

    internal void AddToClipboard(int col, int row)
    {
        if (!_clipboard.Any(c => c.Row == row))
        {
            _clipboard.Add(new Clipboard
            {
                Row = row,
                StartCol = col,
                EndCol = col
            });
        }
        else
        {
            var current = _clipboard.FirstOrDefault(c => c.Row == row);

            current.EndCol = col;

            _clipboard.Remove(current);
            _clipboard.Add(current);
        }
    }

    public void SetLines(string[] lines)
    {
        _lines = lines;
    }


    public void ClearClipboard()
    {
        _clipboard.Clear();
    }

    //end test

    public Buffer(IEnumerable<string> lines)
    {
        _lines = lines.ToArray();
    }

    public IEnumerable<string> GetLines()
    {
        var result = _lines;
        return result;
    }

    public void Render()
    {
        for (int i = 0; i < _lines.Length; i++)
        {
            if (_clipboard != null && _clipboard.Count > 0)
            {
                bool isHighlighted = false; // Flag to track if the line is in clipboard
                foreach (var item in _clipboard)
                {
                    if (item.Row == i)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        isHighlighted = true; // Set the flag if the line is in clipboard
                        break; // No need to continue searching the clipboard once found
                    }
                }

                // Check if the line should be highlighted
                if (isHighlighted)
                {
                    Console.WriteLine(_lines[i]);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(_lines[i]);
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(_lines[i]);
            }
        }



        //foreach (var line in _lines)
        //{
        //    Console.WriteLine(line);
        //}



    }

    public int LineCount()
    {
        return _lines.Length;
    }

    public int LineLength(int row)
    {
        return _lines[row].Length;
    }

    internal Buffer Insert(string character, int row, int col)
    {
        var linesDeepCopy = (string[])_lines.Clone();
        linesDeepCopy[row] = linesDeepCopy[row].Insert(col, character);
        return new Buffer(linesDeepCopy);
    }

    internal Buffer Delete(int row, int col)
    {
        var linesDeepCopy = (string[])_lines.Clone();
        linesDeepCopy[row] = linesDeepCopy[row].Remove(col, 1);
        return new Buffer(linesDeepCopy);
    }

    internal Buffer SplitLine(int row, int col)
    {
        var linesDeepCopy = new List<string>(_lines);
        var line = linesDeepCopy[row];
        var newLines = new[] { line.Substring(0, col), line.Substring(col) };
        linesDeepCopy[row] = newLines[0];
        linesDeepCopy.Insert(row + 1, newLines[1]);
        return new Buffer(linesDeepCopy);
    }

    internal Buffer DeleteLine(int row, int col)
    {

        var linesDeepCopy = (string[])_lines.Clone();
        try
        {
            if (linesDeepCopy[row] == "")
            {
                if (row >= 0 && row < linesDeepCopy.Length)
                {
                    for (int i = row; i < linesDeepCopy.Length - 1; i++)
                    {
                        linesDeepCopy[i] = linesDeepCopy[i + 1];
                    }

                    Array.Resize(ref linesDeepCopy, linesDeepCopy.Length - 1);
                }
            }
            else if (linesDeepCopy[row] != string.Empty)
            {
                if (row >= 0 && row < linesDeepCopy.Length && row - 1 < linesDeepCopy.Length)
                {
                    linesDeepCopy[row - 1] = linesDeepCopy[row - 1] + linesDeepCopy[row];


                    for (int i = row; i < linesDeepCopy.Length - 1; i++)
                    {
                        linesDeepCopy[i] = linesDeepCopy[i + 1];
                    }

                    Array.Resize(ref linesDeepCopy, linesDeepCopy.Length - 1);
                }
            }
        }
        catch { }
        return new Buffer(linesDeepCopy);
    }




    internal Buffer Clone()
    {
        return new Buffer(_lines);
    }

}
