namespace EGScript.Scripter
{

    public class Token
    {
        public TokenType Type { get; }
        public double Number { get; }
        public string Text { get; }

        public Token()
        {
            Type = TokenType.NONE;
            Text = "";
            Number = 0;
        }

        public Token(TokenType type)
        {
            Type = type;
            Text = "";
            Number = 0;
        }

        public Token(TokenType type, string text)
        {
            Type = type;
            Text = text;
            Number = 0;
        }

        public Token(TokenType type, double number)
        {
            Type = type;
            Text = "";
            Number = number;
        }

        public static string TypeToString(TokenType type)
        {
            switch(type)
            {
                case TokenType.LEFT_BRACE: return "{";
                case TokenType.RIGHT_BRACE: return "}";
                case TokenType.LEFT_BRACKET: return "[";
                case TokenType.RIGHT_BRACKET: return "]";
                case TokenType.LEFT_PARENTHESIS: return "(";
                case TokenType.RIGHT_PARENTHESIS: return ")";

                case TokenType.COLON: return ":";
                case TokenType.SEMICOLON: return ";";
                case TokenType.COMMA: return ",";

                case TokenType.FUNCTION: return "function";
                case TokenType.CLASS: return "class";
                case TokenType.INCLUDE: return "include";
                case TokenType.IF: return "if";
                case TokenType.ELSE: return "else";
                case TokenType.WHILE: return "while";
                case TokenType.FOR: return "for";
                case TokenType.SWITCH: return "switch";
                case TokenType.CASE: return "case";
                case TokenType.DEFAULT: return "default";
                case TokenType.BREAK: return "break";
                case TokenType.CONTINUE: return "continue";
                case TokenType.RETURN: return "return";
                case TokenType.NEW: return "new";
                case TokenType.NULL: return "null";
                case TokenType.TRUE: return "true";
                case TokenType.FALSE: return "false";

                case TokenType.EQUALS: return "=";
                case TokenType.EQUALS_EQUALS: return "==";
                case TokenType.NOT_EQUALS: return "!=";
                case TokenType.PLUS_EQUALS: return "+=";
                case TokenType.MINUS_EQUALS: return "-=";
                case TokenType.TIMES_EQUALS: return "*=";
                case TokenType.DIVIDE_EQUALS: return "/=";
                case TokenType.LESS_THAN_EQUALS: return "<=";
                case TokenType.GREATER_THAN_EQUALS: return ">=";

                case TokenType.OR: return "||";
                case TokenType.AND: return "&&";
                case TokenType.LESS_THAN: return "<";
                case TokenType.GREATER_THAN: return ">";
                case TokenType.PLUS: return "+";
                case TokenType.MINUS: return "-";
                case TokenType.TIMES: return "*";
                case TokenType.DIVIDE: return "/";
                case TokenType.EXPONENT: return "^";
                case TokenType.MODULO: return "%";
                case TokenType.NOT: return "!";
                case TokenType.INCREMENT: return "++";
                case TokenType.DECREMENT: return "--";
                case TokenType.ARROW: return "->";

                case TokenType.IDENTIFIER: return "IDENTIFIER";
                case TokenType.STRING: return "STRING";
                case TokenType.NUMBER: return "NUMBER";

                default:
                    return "UNIDENTIFIED";
            }
        }
    }
}
