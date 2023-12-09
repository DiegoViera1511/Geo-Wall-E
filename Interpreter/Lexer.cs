
using System.Text.RegularExpressions;

namespace Interpreter
{
    public enum TokenType
    {
        //ATOMS 
        NUMBER , STRING , BOOLEAN , TRUE , FALSE , ID , UNDEFINED 

        //KeyWords
        , IF , THEN , ELSE , DRAW , COLOR , RESTORE , SAMPLES , POINT , POINTS , LINE , SEGMENT 
        , RAY , CIRCLE , IMPORT , ARC , MEASURE , COUNT , RANDOMS , LET , IN , SKIP ,

        //Colors
        Blue , Red , Yellow , Green , Cyan , Magenta , White , Gray , Black

        //Operations
        , PLUS , MINUS , MULTIPLICATION , DIVISION , POWER , GREATER , LESS , GREATER_EQUAL , LESS_EQUAL , NOT , NOT_EQUAL , EQUAL , EQUAL_EQUAL 
        , MODULE , AND , OR 

        //Others
        ,LEFT_PARENTHESIS , RIGHT_PARENTHESIS , LEFT_BRACE , RIGHT_BRACE , EOF , COMMA , THREE_DOTS
        
    }

    public class Token
    {

        private string stringForm ;
        public string StringForm {get => stringForm;}

        private TokenType typeOfToken ;
        public TokenType TypeOfToken {get => typeOfToken;}

        private int line ;
        public int Line{get => line;}

        public Token(string stringForm , TokenType typeOfToken , int line)
        {
            this.stringForm = stringForm ;
            this.typeOfToken = typeOfToken ;
            this.line = line ;
        }
    }

    class Lexer 
    {
        public static List<string> KeyWordsList = new()
        {"let ","in","if","then","else","true","false"
        ,"point","line","segment","ray","circle", "points" ,
        "sequence","color","restore","import","draw","arc","measure","intersect",
        "count","randoms","points","samples"};
        
        public static List<List<Token>> Tokenize(string input)
        {
            List<List<Token>> result = new();

            Regex AllTokens ;
            
            AllTokens = new(@"\d+$|\d+\.\d+$|\+|\-|\*|\^|/|%|\(|\)|{|}|(>=)|(<=)|<[=]{0}|>[=]{0}|!=|;|,|let |={1,2}|\.\.\.|_|
            draw|color|restore|samples|undefined|points|point|line|segment|ray|circle|secuence|import|arc|measure|intersect|count|
            randoms|then|function|if|else|!|\&|\||true|false|(\u0022([^\u0022\\]|\\.)*\u0022)|@|\w+|[^\(\)\+\-\*/\^%<>=!&\|,;\s]+");
                
            string[] lines = Regex.Split(input , Environment.NewLine) ;
            for(int i = 0 ; i < lines.Length ; i++)
            {
                result.Add(new List<Token>());
                List<Match> tokens = AllTokens.Matches(lines[i]).ToList() ;
                foreach(Match t in tokens)
                {
                    string token = t.Value ; 
                    int actualLine = i + 1 ;
                    
                    if(IsNumber(token))
                    {
                        result[i].Add(new Token( token , TokenType.NUMBER , actualLine ));
                    }
                    else if(IsBoolean(token))
                    {
                        result[i].Add(new Token( token , TokenType.BOOLEAN , actualLine ));
                    }
                    else if(IsString(token))
                    {
                        result[i].Add( new Token( token , TokenType.STRING , actualLine ) );
                    }
                    else if(token == "undefined")
                    {
                        result[i].Add( new Token( token , TokenType.UNDEFINED , actualLine ) );
                    }
                    else if(IsID(token))
                    {
                        result[i].Add( new Token( token , TokenType.ID , actualLine ));
                    }
                    else 
                    {
                        switch (token)
                        {
                            case "if" :
                            result[i].Add( new Token(token , TokenType.IF , actualLine ));
                            break ;
                            case "then" :
                            result[i].Add( new Token(token , TokenType.THEN , actualLine));
                            break ;
                            case "else" :
                            result[i].Add( new Token( token , TokenType.ELSE , actualLine ));
                            break ;
                            case "draw" :
                            result[i].Add( new Token(token , TokenType.DRAW , actualLine));
                            break ;
                            case "color" :
                            result[i].Add( new Token(token , TokenType.COLOR , actualLine)) ;
                            break ;
                            case "restore" : 
                            result[i].Add (new Token(token , TokenType.RESTORE , actualLine));
                            break ;
                            case "samples" :
                            result[i].Add(new Token(token , TokenType.SAMPLES , actualLine));
                            break;
                            case "point" :
                            result[i].Add(new Token(token , TokenType.POINT , actualLine));
                            break ;
                            case "points" :
                            result[i].Add(new Token(token , TokenType.POINTS , actualLine));
                            break ;
                            case "line" :
                            result[i].Add( new Token( token , TokenType.LINE , actualLine));
                            break ;
                            case "segment" :
                            result[i].Add(new Token (token , TokenType.SEGMENT , actualLine));
                            break ;
                            case "ray" :
                            result[i].Add(new Token(token , TokenType.RAY , actualLine));
                            break ;
                            case "circle" :
                            result[i].Add(new Token(token , TokenType.CIRCLE , actualLine));
                            break ;
                            case "import" :
                            result[i].Add(new Token(token , TokenType.IMPORT , actualLine));
                            break;
                            case "arc" :
                            result[i].Add(new Token(token , TokenType.ARC , actualLine));
                            break ;
                            case "measure" :
                            result[i].Add(new Token(token , TokenType.MEASURE , actualLine ));
                            break ;
                            case "let " :
                            result[i].Add(new Token(token , TokenType.LET , actualLine));
                            break ;
                            case "in" :
                            result[i].Add(new Token(token , TokenType.IN , actualLine));
                            break ;
                            case "_" :
                            result[i].Add(new Token(token , TokenType.SKIP , actualLine));
                            break;
                            case "+" :
                            result[i].Add(new Token(token , TokenType.PLUS , actualLine));
                            break ;
                            case "-" :
                            result[i].Add(new Token(token , TokenType.MINUS , actualLine));
                            break ;
                            case "*" :
                            result[i].Add(new Token(token , TokenType.MULTIPLICATION , actualLine));
                            break ;
                            case "/" :
                            result[i].Add(new Token(token , TokenType.DIVISION , actualLine));
                            break;
                            case "^" :
                            result[i].Add(new Token(token , TokenType.POWER , actualLine));
                            break ;
                            case ">" :
                            result[i].Add(new Token(token , TokenType.GREATER , actualLine));
                            break ;
                            case "<" :
                            result[i].Add(new Token(token , TokenType.LESS , actualLine));
                            break;
                            case ">=" :
                            result[i].Add(new Token(token , TokenType.GREATER_EQUAL , actualLine));
                            break;
                            case "<=" :
                            result[i].Add(new Token(token , TokenType.LESS_EQUAL , actualLine));
                            break;
                            case "!" :
                            result[i].Add(new Token(token , TokenType.NOT , actualLine));
                            break ;
                            case "!=" :
                            result[i].Add(new Token(token , TokenType.NOT_EQUAL , actualLine));
                            break;
                            case "=" :
                            result[i].Add(new Token(token , TokenType.EQUAL , actualLine));
                            break ;
                            case "==" :
                            result[i].Add(new Token(token , TokenType.EQUAL_EQUAL , actualLine));
                            break ;
                            case "%" :
                            result[i].Add(new Token(token , TokenType.MODULE , actualLine));
                            break ;
                            case "&" :
                            result[i].Add(new Token(token , TokenType.AND , actualLine));
                            break;
                            case "|" :
                            result[i].Add(new Token(token , TokenType.OR , actualLine));
                            break ;
                            case "(" :
                            result[i].Add(new Token (token , TokenType.LEFT_PARENTHESIS , actualLine));
                            break ;
                            case ")" :
                            result[i].Add(new Token(token , TokenType.RIGHT_PARENTHESIS , actualLine));
                            break ;
                            case "{" :
                            result[i].Add(new Token( token , TokenType.LEFT_BRACE , actualLine));
                            break ;
                            case "}" :
                            result[i].Add(new Token(token , TokenType.RIGHT_BRACE , actualLine));
                            break ;
                            case ";" :
                            result[i].Add(new Token(token , TokenType.EOF , actualLine));
                            break ;
                            case "," :
                            result[i].Add(new Token(token , TokenType.COMMA , actualLine));
                            break ;
                            case "..." :
                            result[i].Add(new Token(token , TokenType.THREE_DOTS , actualLine));
                            break ;
                            default :
                            throw new LexicalErrors(token , actualLine);
                        }
                    }
                }
            }
            return result ;
        }

        public static bool IsNumber(string Token)
        {
            return Regex.IsMatch(Token , @"^\d+$|^\d+\.\d+$") ? true : false ;
        }
        public static bool IsString(string Token)
        {
            return Regex.IsMatch(Token , @"(\u0022([^\u0022\\]|\\.)*\u0022)") ? true : false ;
        }
        public static bool IsBoolean(string Token)
        {
           return Regex.IsMatch(Token , @"^true$|^false$") ?  true : false ;
        }
        public static bool IsID(string Token)
        {
            return Regex.IsMatch( Token , @"^[a-zA-Z]+\w*$") && !KeyWordsList.Contains(Token) ? true : false ;
        }

    }
}