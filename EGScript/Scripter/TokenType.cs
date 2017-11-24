using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGScript.Scripter
{
    /// <summary>
    /// The type of token (identifier, operator, bracket, etc).
    /// </summary>
    public enum TokenType
    {
        NONE = 0,

        /// <summary>
        /// '['
        /// </summary>
        LEFT_BRACKET,
        /// <summary>
        /// ']'
        /// </summary>
        RIGHT_BRACKET,
        /// <summary>
        /// '{'
        /// </summary>
        LEFT_BRACE,
        /// <summary>
        /// '}'
        /// </summary>
        RIGHT_BRACE,
        /// <summary>
        /// '('
        /// </summary>
        LEFT_PARENTHESIS,
        /// <summary>
        /// ')'
        /// </summary>
        RIGHT_PARENTHESIS,

        /// <summary>
        /// ';'
        /// </summary>
        SEMICOLON,
        /// <summary>
        /// ':'
        /// </summary>
        COLON,
        /// <summary>
        /// '::'
        /// </summary>
        DOUBLE_COLON,

        /// <summary>
        /// ','
        /// </summary>
        COMMA,

        /// <summary>
        /// 'function'
        /// </summary>
        FUNCTION,
        /// <summary>
        /// 'class'
        /// </summary>
        CLASS,
        /// <summary>
        /// 'global'
        /// </summary>
        GLOBAL,
        /// <summary>
        /// 'include'
        /// </summary>
        INCLUDE,
        /// <summary>
        /// 'if'
        /// </summary>
        IF,
        /// <summary>
        /// 'else'
        /// </summary>
        ELSE,
        /// <summary>
        /// 'while'
        /// </summary>
        WHILE,
        /// <summary>
        /// 'for'
        /// </summary>
        FOR,
        /// <summary>
        /// 'switch'
        /// </summary>
        SWITCH,
        /// <summary>
        /// 'case' (in a switch statement)
        /// </summary>
        CASE,
        /// <summary>
        /// 'default' (in a switch statement)
        /// </summary>
        DEFAULT,
        /// <summary>
        /// 'break' (out of a loop or switch statement)
        /// </summary>
        BREAK,
        /// <summary>
        /// 'continue' (in a loop)
        /// </summary>
        CONTINUE,
        /// <summary>
        /// 'return' (from a function)
        /// </summary>
        RETURN,
        /// <summary>
        /// 'new' (class instance)
        /// </summary>
        NEW,
        /// <summary>
        /// 'null'
        /// </summary>
        NULL,
        /// <summary>
        /// 'true'
        /// </summary>
        TRUE,
        /// <summary>
        /// 'false'
        /// </summary>
        FALSE,

        /// <summary>
        /// '='
        /// </summary>
        EQUALS,
        /// <summary>
        /// '&lt;='
        /// </summary>
        LESS_THAN_EQUALS,
        /// <summary>
        /// '&gt;='
        /// </summary>
        GREATER_THAN_EQUALS,
        /// <summary>
        /// '=='
        /// </summary>
        EQUALS_EQUALS,
        /// <summary>
        /// '!='
        /// </summary>
        NOT_EQUALS,
        /// <summary>
        /// '+='
        /// </summary>
        PLUS_EQUALS,
        /// <summary>
        /// '-='
        /// </summary>
        MINUS_EQUALS,
        /// <summary>
        /// '*='
        /// </summary>
        TIMES_EQUALS,
        /// <summary>
        /// '/='
        /// </summary>
        DIVIDE_EQUALS,

        /// <summary>
        /// '||'
        /// </summary>
        OR,
        /// <summary>
        /// '&&'
        /// </summary>
        AND,
        /// <summary>
        /// '!'
        /// </summary>
        NOT,
        /// <summary>
        /// '&lt;'
        /// </summary>
        LESS_THAN,
        /// <summary>
        /// '&gt;'
        /// </summary>
        GREATER_THAN,
        /// <summary>
        /// '+'
        /// </summary>
        PLUS,
        /// <summary>
        /// '-'
        /// </summary>
        MINUS,
        /// <summary>
        /// '*'
        /// </summary>
        TIMES,
        /// <summary>
        /// '/'
        /// </summary>
        DIVIDE,
        /// <summary>
        /// '^'
        /// </summary>
        EXPONENT,
        /// <summary>
        /// '%'
        /// </summary>
        MODULO,
        /// <summary>
        /// '++'
        /// </summary>
        INCREMENT,
        /// <summary>
        /// '--'
        /// </summary>
        DECREMENT,
        /// <summary>
        /// '->'
        /// </summary>
        ARROW,
        /// <summary>
        /// '#'
        /// </summary>
        COUNT,

        /// <summary>
        /// Function/method/variable/other identifier
        /// </summary>
        IDENTIFIER,
        /// <summary>
        /// Alphanumeric value inside quotation marks (")
        /// </summary>
        STRING,
        /// <summary>
        /// Numeric value
        /// </summary>
        NUMBER,


        /// <summary>
        /// EOF (End Of File)
        /// </summary>
        END_OF_FILE
    }
}
