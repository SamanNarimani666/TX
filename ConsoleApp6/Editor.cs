class Editor
{
    Buffer _buffer;
    Cursor _cursor;
    Stack<(Cursor, Buffer)> _history;


    public Editor()
    {
        var lines = File.ReadAllLines("test.txt").Where(x => x != Environment.NewLine).ToArray();
        _buffer = new Buffer(lines);
        _cursor = new Cursor();
        _history = new Stack<(Cursor, Buffer)>();
    }


    void ClearClipBoard()
    {
        _buffer.ClearClipboard();
    }

    public void Run()
    {
        while (true)
        {
            Render();
            var character = Console.ReadKey(true);
            HandleInput(character);
        }
    }
    private void HandleInput(ConsoleKeyInfo character)
    {
        if ((character.Modifiers & ConsoleModifiers.Control) != 0)
        {
            if (character.Key == ConsoleKey.RightArrow)
            {
                AddToClipboard();
            }
            else if (character.Key == ConsoleKey.Q)
            {
                Environment.Exit(0);
            }
            else if (character.Key == ConsoleKey.S)
            {
                File.WriteAllLines("test.txt", _buffer.GetLines());
                Environment.Exit(0);
            }
            else if (character.Key == ConsoleKey.Z)
            {
                ClearClipBoard();
                RestoreSnapshot();
            }
        }
        else
        {
            ClearClipBoard();
            if (character.Key == ConsoleKey.UpArrow)
            {
                MoveCursorUp();
            }
            else if (character.Key == ConsoleKey.DownArrow)
            {
                MoveCursorDown();
            }
            else if (character.Key == ConsoleKey.LeftArrow)
            {
                MoveCursorLeft();
            }
            else if (character.Key == ConsoleKey.RightArrow)
            {
                MoveCursorRight();
            }
            else if (character.Key == ConsoleKey.Backspace)
            {
                DeleteCharacter();
            }
            else if (character.Key == ConsoleKey.Enter)
            {
                InsertNewLine();
            }
            else if (IsTextChar(character))
            {
                InsertCharacter(character.KeyChar);
            }
        }
    }

    private bool IsTextChar(ConsoleKeyInfo character)
    {
        return !Char.IsControl(character.KeyChar);
    }

    private void Render()
    {
        if (_cursor.Col >= 0 && _cursor.Row >= 0)
        {
            Console.Clear();
            _buffer.Render();
            Console.SetCursorPosition(_cursor.Col, _cursor.Row);
            Console.BackgroundColor = ConsoleColor.Black;

        }
    }

    private void SaveSnapshot()
    {
        _history.Push((_cursor.Clone(), _buffer.Clone()));
    }

    private void RestoreSnapshot()
    {
        if (_history.Count > 0)
        {
            var (cursor, buffer) = _history.Pop();
            _cursor = cursor;
            _buffer = buffer;
        }
    }



    private void AddToClipboard()
    {

        _buffer.AddToClipboard(_cursor.Col, _cursor.Row);

        MoveCursorRight();

        //int row = _cursor.Row;

        //int col = _cursor.Col;

        //var lines = _buffer.GetLines().ToArray();

        //string paste = lines[row];

        //if (paste.Length > _cursor.Col)

        //{

        //    char c = paste[col];

        //    _clipboard.Append(c);

        //    MoveCursorRight();
        //}





        //var lines = _buffer.GetLines().ToArray();

        //StringBuilder p = new StringBuilder();

        //p.Append(lines[_cursor.Row].ToString());


        //if (p.Length > _cursor.Col)

        //{

        //    char temp = p[_cursor.Col];

        //    string m = $"\x1b[32m{temp}\x1b[0m";

        //    p.Remove(_cursor.Col, 1); // Remove the original character

        //    p.Insert(_cursor.Col, m); // Insert the formatted character in its place

        //    lines[_cursor.Row] = p.ToString(); // Update the line with the modified string

        //    _buffer.SetLines(lines);


        //    MoveCursorRight();

        //    _buffer.Render();
        //}

    }




    private void MoveCursorUp()
    {
        SaveSnapshot();
        _cursor = _cursor.Up(_buffer);
    }

    private void MoveCursorDown()
    {
        SaveSnapshot();
        _cursor = _cursor.Down(_buffer);
    }

    private void MoveCursorLeft()
    {
        SaveSnapshot();
        _cursor = _cursor.Left(_buffer);
    }



    private void MoveCursorRight()
    {
        SaveSnapshot();
        _cursor = _cursor.Right(_buffer, true);
    }

    private void DeleteCharacter()
    {
        if (_cursor.Col > 0)
        {
            SaveSnapshot();
            _buffer = _buffer.Delete(_cursor.Row, _cursor.Col - 1);
            _cursor = _cursor.Left(_buffer);
        }
        else if (_cursor.Col == 0)
        {
            SaveSnapshot();
            _buffer = _buffer.DeleteLine(_cursor.Row, _cursor.Col);
            _cursor = _cursor.Up(_buffer).MoveToRow(_cursor.Row - 1).MoveToCol(_buffer.GetLines().ToArray()[_cursor.Row - 1].Length);
        }
    }

    private void InsertNewLine()
    {
        SaveSnapshot();
        _buffer = _buffer.SplitLine(_cursor.Row, _cursor.Col);
        _cursor = _cursor.Down(_buffer).MoveToCol(0);
    }

    private void InsertCharacter(char character)
    {
        SaveSnapshot();
        _buffer = _buffer.Insert(character.ToString(), _cursor.Row, _cursor.Col);
        _cursor = _cursor.Right(_buffer, false);
    }
}
