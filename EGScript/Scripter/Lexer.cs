using System.Collections.Generic;
using System.Text;

namespace EGScript.Scripter
{
    public class Lexer
    {
        private char[] _buffer = new char[2] { ' ', ' ' };
        private string _code { get; }
        public int Character { get; private set; }
        public int _internalCharacter { get; private set; }
        public int Line { get; private set; }
        private bool _isAtEndOfString;
        private SortedDictionary<string, TokenType> _keywords = new SortedDictionary<string, TokenType>();

        public Lexer(string code)
        {
            _code = code;
            Consume();
            Consume();

            _keywords.Add("function", TokenType.FUNCTION);
            _keywords.Add("global", TokenType.GLOBAL);
            _keywords.Add("class", TokenType.CLASS);
            _keywords.Add("if", TokenType.IF);
            _keywords.Add("else", TokenType.ELSE);
            _keywords.Add("while", TokenType.WHILE);
            _keywords.Add("for", TokenType.FOR);
            _keywords.Add("switch", TokenType.SWITCH);
            _keywords.Add("case", TokenType.CASE);
            _keywords.Add("default", TokenType.DEFAULT);
            _keywords.Add("break", TokenType.BREAK);
            _keywords.Add("continue", TokenType.CONTINUE);
            _keywords.Add("return", TokenType.RETURN);
            _keywords.Add("new", TokenType.NEW);
            _keywords.Add("null", TokenType.NULL);
            _keywords.Add("true", TokenType.TRUE);
            _keywords.Add("false", TokenType.FALSE);
            _keywords.Add("include", TokenType.INCLUDE);
        }

        public Token NextToken()
        {
            Token token = new Token();
            while (token.Type == TokenType.NONE)
            {
                switch (_buffer[0])
                {
                    case '(': token = new Token(TokenType.LEFT_PARENTHESIS); break;
                    case ')': token = new Token(TokenType.RIGHT_PARENTHESIS); break;
                    case '[': token = new Token(TokenType.LEFT_BRACKET); break;
                    case ']': token = new Token(TokenType.RIGHT_BRACKET); break;
                    case '{': token = new Token(TokenType.LEFT_BRACE); break;
                    case '}': token = new Token(TokenType.RIGHT_BRACE); break;
                    case ':':
                        {
                            switch(_buffer[1])
                            {
                                case ':':
                                    {
                                        token = new Token(TokenType.DOUBLE_COLON);
                                        Consume();
                                    }
                                    break;
                                default: token = new Token(TokenType.COLON); break;
                            }
                        }
                        break;
                    case ';': token = new Token(TokenType.SEMICOLON); break;
                    case ',': token = new Token(TokenType.COMMA); break;
                    case '=':
                    {
                        switch (_buffer[1])
                        {
                            case '=':
                                {
                                    token = new Token(TokenType.EQUALS_EQUALS);
                                    Consume();
                                }
                                break;
                            default: token = new Token(TokenType.EQUALS); break;
                        }
                    }
                    break;
                    case '#': token = new Token(TokenType.COUNT); break;
                    case '+':
                    {
                        switch (_buffer[1])
                        {
                            case '=':
                                {
                                    token = new Token(TokenType.PLUS_EQUALS);
                                    Consume();
                                }
                                break;
                            case '+':
                                {
                                    token = new Token(TokenType.INCREMENT);
                                    Consume();
                                }
                                break;

                            default: token = new Token(TokenType.PLUS); break;
                        }
                    }
                    break;
                    case '-':
                    {
                        switch (_buffer[1])
                        {
                            case '=':
                                {
                                    token = new Token(TokenType.MINUS_EQUALS);
                                    Consume();
                                }
                                break;
                            case '-':
                                {
                                    token = new Token(TokenType.DECREMENT);
                                    Consume();
                                }
                                break;
                            case '>':
                                {
                                    token = new Token(TokenType.ARROW);
                                    Consume();
                                }
                                break;
                            default: token = new Token(TokenType.MINUS); break;
                        }
                    }
                    break;
                    case '*':
                    {
                        switch (_buffer[1])
                        {
                            case '=':
                                {
                                    token = new Token(TokenType.TIMES_EQUALS);
                                    Consume();
                                }
                                break;
                            default: token = new Token(TokenType.TIMES); break;
                        }
                    }
                    break;
                    case '/':
                    {
                        switch (_buffer[1])
                        {
                            case '=':
                                {
                                    token = new Token(TokenType.DIVIDE_EQUALS);
                                    Consume();
                                }
                                break;
                            case '/':
                                {
                                    // This is a comment, so consume the remainder of the line
                                    while (_buffer[0] != '\n')
                                    {
                                        Consume();
                                    }
                                }
                                break;
                            default: token = new Token(TokenType.DIVIDE); break;
                        }
                    }
                    break;
                    case '^': token = new Token(TokenType.EXPONENT); break;
                    case '%': token = new Token(TokenType.MODULO); break;
                    case '|':
                    {
                        switch (_buffer[1])
                        {
                            case '|':
                                {
                                    token = new Token(TokenType.OR);
                                    Consume();
                                }
                                break;

                            default:
                                throw new LexerException($"Unexpected character '{_buffer[1]}', expected '|'.", Line, Character);
                        }
                    }
                        break;
                    case '&':
                    {
                        switch (_buffer[1])
                        {
                            case '&':
                                {
                                    token = new Token(TokenType.AND);
                                    Consume();
                                }
                                break;

                            default:
                                throw new LexerException($"Unexpected character '{_buffer[1]}', expected '&'.", Line, Character);
                        }
                    }
                    break;
                    case '!':
                    {
                        switch (_buffer[1])
                        {
                            case '=':
                                {
                                    token = new Token(TokenType.NOT_EQUALS);
                                    Consume();
                                }
                                break;
                            default: token = new Token(TokenType.NOT); break;
                        }
                    }
                    break;
                    case '>':
                    {
                        switch (_buffer[1])
                        {
                            case '=':
                                {
                                    token = new Token(TokenType.GREATER_THAN_EQUALS);
                                    Consume();
                                }
                                break;
                            default: token = new Token(TokenType.GREATER_THAN); break;
                        }
                    }
                    break;
                    case '<':
                    {
                        switch (_buffer[1])
                        {
                            case '=':
                                {
                                    token = new Token(TokenType.LESS_THAN_EQUALS);
                                    Consume();
                                }
                                break;
                            default: token = new Token(TokenType.LESS_THAN); break;
                        }
                    }
                        break;
                    case '"':
                    {
                        Consume();
                        token = new Token(TokenType.STRING, GetString());
                    }
                    break;
                    case ' ': Consume(); break;
                    case '\t': Consume(); break;
                    case '\r': Consume(); break;
                    case '\n':
                    {
                        if (_isAtEndOfString)
                        {
                            token = new Token(TokenType.END_OF_FILE);
                        }
                        else
                        {
                            Consume();
                            Line++;
                            Character = 0;
                        }
                    }
                    break;
                    default:
                    {
                        if (IsDigit(_buffer[0]))
                        {
                            token = new Token(TokenType.NUMBER, GetNumber());
                        }
                        else if (IsAlpha(_buffer[0]))
                        {
                            var identifier = GetIdentifier();
                            if (_keywords.TryGetValue(identifier, out TokenType type))
                            {
                                token = new Token(type, identifier);
                            }
                            else
                            {
                                token = new Token(TokenType.IDENTIFIER, identifier);
                            }
                        }
                        else
                        {
                            throw new LexerException($"Unexpected character '{_buffer[0]}'.", Line, Character);
                        }
                        return token;
                    }
                }
            }
            Consume();
            return token;
        }

        private void Consume()
        {
            _buffer[0] = _buffer[1];

            if (_internalCharacter >= _code.Length)
            {
                _buffer[1] = '\n';
                _isAtEndOfString = true;
            }
            else
            {
                _buffer[1] = _code[_internalCharacter];
                _internalCharacter++;
                Character++;
            }
        }

        private string GetIdentifier()
        {
            var buffer = new StringBuilder();

            while(IsAlpha(_buffer[0]) || IsDigit(_buffer[0]))
            {
                buffer.Append(_buffer[0]);
                Consume();
            }

            return buffer.ToString();
        }

        private string GetString()
        {
            var buffer = new StringBuilder();
            int line = Line;
            int character = Character;

            while(_buffer[0] != '"')
            {
                buffer.Append(_buffer[0]);
                Consume();
                if (_isAtEndOfString)
                    throw new LexerException($"String was not terminated, expected '\"'.", line, character);
            }

            return buffer.ToString();
        }

        private double GetNumber()
        {
            var buffer = new StringBuilder();

            while(IsDigit(_buffer[0]) || IsNumericModifier(_buffer[0], -1))
            {
                buffer.Append(_buffer[0]);
                Consume();
            }

            if (!double.TryParse(buffer.ToString(), out double ret))
                throw new LexerException($"{buffer} is not a valid double value.", Line, Character);

            return ret;
        }

        private bool IsDigit(char character)
        {
            return ((character >= '0') && (character <= '9'));
        }

        private bool IsNumericModifier(char character, int index)
        {
            return (character == '.' || (character == '-' && index == 0));
        }

        private bool IsAlpha(char character)
        {
            return (((character >= 'a') && (character <= 'z')) || ((character >= 'A') && (character <= 'Z')) || character == '_');
        }

    }
}
