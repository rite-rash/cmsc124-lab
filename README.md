# SoundWave (Working title)

## Creators

- Mikaela Kristen C. De Guzman (rgbsod)
- Trisha Mae A. Hechenagocia (rite-rash)

## Overview

SoundWave is a music-themed scripting language. Its syntax borrowed terms from music production. How ths works is that a track declares a value, stream outputs it, and an album will group related tracks into a function. The intended flow is for the input to be a number that doubles as a MIDI note number (0-127), so that arithmetic on a track behaves like transposition and for stream to output the result as a note name.

For the sake of familiarity, this documentation will use **C# keywords** rather than the language's keywords.

## Host language and build

- Host language: C# (.NET 8)
- Version metadata: `cmsc124-lab.csproj` (`TargetFramework net8.0`)
- Build: `./build.sh`
- Requires the .NET 8 SDK, which `./build.sh` restores and builds
  from a clean checkout with no other setup.

## Running it
| Command | What it does |
|---|---|
| `./run <file>` | [Executes a program. Available from Lab 4.] |
| `./run --tokenize <file>` | [Prints the token stream.] |
| `./run --parse <file>` | [Prints the parsed tree.] |
| `./run --eval <file>` | [Evaluates each expression and prints its value.] |
| `./run` | [Starts the REPL.] |

Exit code 0 means the program ran with no errors. See Errors and Diagnostics section for other exit codes.

## File extension

`.play` — must match the `ext` field in every `tests/lab*/manifest.json`.
## Lexical structure

### Keywords

| Keyword | Purpose | Status |
|---|---|---|
| `track` | variable declaration | (parsing not yet implemented) |
| `stream` | output statement | (parsing not yet implemented)  |
| `album` | function definition | (parsing not yet implemented)  |
| `repeat` | loop | (parsing not yet implemented) |
| `live` | boolean true | implemented |
| `mute` | boolean false | implemented |
| `rest` | nil | implemented |
| `chord` | logical and | implemented |
| `solo` | logical or | implemented |


### Operators

| Operator | Meaning |
|---|---|
| `+` | addition |
| `-` | subtraction |
| `*` | multiplication |
| `/` | division |
| `=` | assignment |
| `==` | equality |
| `!` | logical not |
| `!=` | not-equal |
| `<` `<=` `>` `>=` | comparison |



### Literals

| Kind | Syntax | Produces |
|---|---|---|
| number | `4`, `3.14` | a `double` |
| string | `"hello"` (no escape sequences supported) | a `string` |
| boolean | `live`, `mute`, |a `bool` |
|nil | `rest`| a `null`|

### Identifiers

- Start characters: letters (`a`-`z`, `A`-`Z`) or underscore (`_`)
- Continue characters: letters, digits, or underscore
- Case-sensitive: yes
- No length limit is enforced yet.

### Comments
- Line comments: '//', runs to the end of the line. Discarded by scanner.
- Block comments: not supported.
- Nesting: no block comments to nest
- [Harness note: comment_prefix in tests/lab*/manifest.json is set to the
  token above.]

## Whitespace and termination

- Whitespace significant: No. Spaces, tabs, and carriage returns are
  discarded after being consumed.
- Statement terminator: Semicolon, but not implemented yet.
- Block delimiters: scanned but not yet used recognized as a dellimeter.
- Grouping delimiters: scanned but not yet used for grouping.

## Token output format

```
Token(type=LYRIC, lexeme="Alice", literal=Alice, line=1)
```

`type` is the token category (keyword, operator, literal, or `LABEL` for an
identifier), `lexeme` is the raw text as it appeared in the source,
`literal` is the converted runtime value for number/string tokens and
`null` for everything else, and `line` is the 1-indexed source line, used
for error messages. Frozen as of Lab 1; changes are recorded in the
changelog.

## Grammar
tree → declaration* ;
declaration → varDecl| statement;
varDecl     → "track" IDENTIFIER ( "=" expression )? ";" ;
statement   → exprStmt | printStmt | block ;
printStmt   → "stream" expression ";" ;
block       → "{" declaration* "}" ;
exprStmt    → expression ";" ;

expression → equality
equality → comparison ( ( "!=" | "==" ) comparison )*
comparison → term ( ( ">" | ">=" | "<" | "<=" ) term )*
term → factor ( ( "-" | "+" ) factor )*
factor → unary ( ( "/" | "*" ) unary )*
unary → ( "!" | "-" ) unary | primary
primary → NUMBER | STRING | "live" | "mute" | "rest" | "(" expression ")"

## Parse output format
``` (+ 2.0 (* 3.0 4.0)) ```

- Groupings print as: `group( ...) `
- Numbers print as: ```1.0, 3.0```






## Semantics

- **Paradigm:** SoundWave is an imperative language that executes sequential statements and supports side-effect output operations
- **Mutability:** Bindings are `mutable`. Variables which are created using `track` are updated using `=`
- **Binding time:** runtime
- **Storage-lifetime:** implicit heap-dynamic
- **Output:** `stream`statement




### Values and types

A runtime value is one of four kinds, stored in the
host language as a single `object`:


 
SOUNDWAVE value | Host representation |
|---|---|
| number | `double` |
| string | `string` |
| boolean (`live`, `mute`) | `bool` |
| nil (`rest`) | `null` |


### Value printing

- **Numbers** always print with a decimal point: `2 + 3` prints `5.0`, and
  `7 / 2` prints `3.5`. 
- **Nil** prints as `rest`. (confirm)
- **Booleans** print as `live` and `mute`. (confirm)
- **Strings** print without quotes: `"hi" + " there"` prints `hi there`.
 

### Truthiness

`mute` and `rest` are the only values that are falsey, following the rule Lox uses. Every other value is
truthy, including `0` and an empty string. 

### Operator semantics

- Arithmetic: track with track, string with string (concat), number with string (concat).
- `+` on strings: concatenation if both operands are strings. coercion with priority to strings if performed on a string and a number.
- Mixed types: Runtime error with exit code 70 for operators other than `+`.
- Comparison: Between numbers only, unless checking for equality.
- Equality across types: Allows comparing between numbers and strings.
- Division by zero: Runtime error with exit code 70.

### Scope and bindings

- Redeclaration in the same scope: Error with exit code 65
- Uninitialized variable holds: `rest`
- Shadowing: allowed in nested scopes
- Undefined name: Runtime error with exit code 70

### Control flow and functions

- Logical operators return: [booleans, or the operand]
- Dangling else binds to: [which if]
- Closure capture of a loop variable: [per iteration, or shared]
- Function with no return statement produces: [value]
- Arity mismatch: [message and exit code]

## Native functions

| Name | Arguments | Returns | Notes |
|---|---|---|---|
| [name] | [count and types] | [type] | [caveats] |


## Errors and diagnostics

Message format:

```
[line 1] Error at 'live': Expect expression.
```


| Failure | Exit code |
|---|---|
|[usage command error]| 64 |
| [lexical/syntax error] | 65 |
| [no input error] | 66 |
| [runtime error] | 70 |


## Testing conventions


| Folder | Activity | Mode | Flag |
|---|---|---|---|
| tests/lab1 | Scanner | sidecar | `--tokenize` |
| tests/lab2 | Parser | sidecar | `--parse` |
| tests/lab3 | Evaluator | inline | `--eval` |
| tests/lab4 | Context | inline | none |
| tests/lab5 | Functions | inline | none |

```
[specific tests]...
```

```
└── 📁tests
    └── 📁lab0
        ├── hello.expected
        ├── hello.play
        ├── manifest.json
    └── 📁lab1
        └── 📁errors
            ├── unrecognized.exit
            ├── unrecognized.expected
            ├── unrecognized.play
            ├── unterminated.exit
            ├── unterminated.expected
            ├── unterminated.play
        └── 📁strings
            ├── basics.expected
            ├── basics.play
            ├── escapes.expected
            ├── escapes.play
            ├── multiline.expected
            ├── multiline.play
        ├── comments.expected
        ├── comments.play
        ├── keywords.expected
        ├── keywords.play
        ├── manifest.json
        ├── numbers.expected
        ├── numbers.play
        ├── operators.expected
        ├── operators.play
    └── 📁lab2
        └── 📁errors
            ├── bad-token.exit
            ├── bad-token.expected
            ├── bad-token.play
            ├── missing-operand.exit
            ├── missing-operand.expected
            ├── missing-operand.play
            ├── unclosed-paren.exit
            ├── unclosed-paren.expected
            └── unclosed-paren.play
        ├── grouping.exit
        ├── grouping.expected
        ├── grouping.play
        ├── left-assoc.exit
        ├── left-assoc.expected
        ├── left-assoc.play
        ├── literals.exit
        ├── literals.expected
        ├── literals.play
        ├── manifest.json
        ├── mixed.exit
        ├── mixed.expected
        ├── mixed.play
        ├── precedence.exit
        ├── precedence.expected
        ├── precedence.play
        ├── unary.exit
        ├── unary.expected
        ├── unary.play

```

Run locally with:

```bash
curl -sSL https://raw.githubusercontent.com/WhiteLicorice/cmsc-124-harness/v1.1/run_tests.py -o run_tests.py
./build.sh
python3 run_tests.py tests/lab1
```

## Sample code

```
track greeting = "hello"
stream greeting
```

Output:

```
Token(type=TRACK, lexeme=track, literal=null, line=1)
Token(type=LABEL, lexeme=greeting, literal=null, line=1)
Token(type=EQUAL, lexeme==, literal=null, line=1)
Token(type=LYRIC, lexeme="hello", literal=hello, line=1)
Token(type=STREAM, lexeme=stream, literal=null, line=2)
Token(type=LABEL, lexeme=greeting, literal=null, line=2)
Token(type=EOF, lexeme=, literal=null, line=2)
```

## Design rationale

We chose music production as our theme because we found it interesting. To adhere to the theme, we move away from generic keywords like `var` and `print`. `track`and instead replaced the variable declaration with `track` and `stream` for outputy, then `album` for function definition followed from that directly. What took our time was deciding on how to handle our function definitions. Right now, we are inspired on using MIDI for the flow of this program. Our plan for numeric values is to have them double as MIDI note numbers, so that addition and subtraction
on a track function as transposition, with `stream` responsible for converting the stored number into a note name before printing.

For the scanner, we limited string handling to simple double-quoted literals without escape sequences, and comments to single-line only. Both
were for the purpose of simplification and to focus more on the logic.

## Known limitations

- Escape sequences inside strings are not supported.
- `album` (functions) and `repeat` (loops) are recognized as keywords but
  do not do anything yet.
- Multi-line comments are not supported.
  
## Changelog

| Activity | What changed in the language |
|---|---|
| Lab 1 | Scanner implemented |
| Lab 2 | Parser implemented, Added live/mute/rest as boolean and nil literals: 
