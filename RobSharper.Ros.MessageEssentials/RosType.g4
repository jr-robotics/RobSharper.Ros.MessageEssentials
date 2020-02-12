grammar RosType;

/* ------------------------------------------------------------------ */   
/* PARSER RULES                                                       */
/* ------------------------------------------------------------------ */

type_input
    : type EOF
    | array_type EOF
    ;

type
    : built_in_type
    | ros_type
    | ros_package_type
    ;

built_in_type
    : INT8
    | UINT8
    | INT16
    | UINT16
    | INT32
    | UINT32
    | INT64
    | UINT64
    | BYTE
    | CHAR
    | FLOAT32
    | FLOAT64
    | TIME
    | DURATION
    | STRING
    | BOOL
    ;

ros_type
    : IDENTIFIER
    ;

ros_package_type
    : IDENTIFIER SLASH IDENTIFIER
    ;

array_type
    : variable_array_type
    | fixed_array_type
    ;

variable_array_type
    : type OPEN_BRACKET CLOSE_BRACKET
    ;

fixed_array_type
    : type OPEN_BRACKET INTEGER_LITERAL CLOSE_BRACKET
    ;


/* ------------------------------------------------------------------ */   
/* LEXER RULES                                                        */
/* ------------------------------------------------------------------ */
BOOL:                       'bool';
INT8:                       'int8';
UINT8:                      'uint8';
BYTE:                       'byte';
CHAR:                       'char';
INT16:                      'int16';
UINT16:                     'uint16';
INT32:                      'int32';
UINT32:                     'uint32';
INT64:                      'int64';
UINT64:                     'uint64';
FLOAT32:                    'float32';
FLOAT64:                    'float64';
STRING:                     'string';
TIME:                       'time';
DURATION:                   'duration';

SLASH:                      '/';
OPEN_BRACKET:               OpenBracket;
CLOSE_BRACKET:              CloseBracket;

INTEGER_LITERAL:            [0-9]+;

IDENTIFIER:                 (Lowercase | Uppercase) (Lowercase | Uppercase | Digit | '_')*; 

fragment OpenBracket:       '[';
fragment CloseBracket:      ']';

fragment Lowercase:         [a-z];
fragment Uppercase:         [A-Z];
fragment Digit:             [0-9];