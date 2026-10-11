# SoundWave (Working title)

## Creators

- Mikaela Kristen C. De Guzman (rgbsod)
- Trisha Mae A. Hechenagocia (rite-rash)

## Overview

SoundWave is a music-themed scripting language. Its syntax borrowed terms from music production. How ths works is that a track declares a value, stream outputs it, and an album will group related tracks into a function. The intended flow is for the input to be a number that doubles as a MIDI note number (0-127), so that arithmetic on a track behaves like transposition and for stream to output the result as a note name.

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


Exit codes: 0 [clean scan], 65 [unrecognized character, unterminated string], 70 [runtime].

## File extension

`.play` — must match the `ext` field in every `tests/lab*/manifest.json`.
## Lexical structure

### Keywords


| Keyword | Purpose | Status |
|---|---|---|
| `track` | variable declaration | implemented |
| `stream` | output statement | implemented |
| `album` | function definition | implemented (parsing not yet built) |
| `repeat` | loop | implemented (parsing not yet built) |
|



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

NOTE: Precedence and associativity not yet drafted. Will be declared and established during Lab 2.


### Literals

| Kind | Syntax | Produces |
|---|---|---|
| number | `4`, `3.14` | a `double` |
| string | `"hello"` (no escape sequences supported) | a `string` |
|

### Identifiers

- Start characters: letters (`a`-`z`, `A`-`Z`) or underscore (`_`)
- Continue characters: letters, digits, or underscore
- Case-sensitive: yes
- No length limit is enforced yet.


### Comments

- Line comments: '//', runs to the end of the line. Discarded by scanner.
- Block comments: not supported yet for lab1.
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

```
[Your complete context-free grammar, current as of the latest activity.
Unambiguous, with precedence and associativity encoded in rule structure.]
```

## Parse output format

```
[one line of real --parse output, e.g. (+ 1.0 (* 2.0 3.0))]
```

- Groupings print as: [form]
- Numbers print as: [form]

## Semantics

### Values and types

[What runtime values exist, and how they are represented in the host
language.]

### Value printing

- Numbers: [e.g. 5 rather than 5.0]
- Nil: [spelling]
- Strings: [with or without quotes]

### Truthiness

[The complete rule. Which values are false in a condition; everything else is
true.]

### Operator semantics

- Arithmetic: [accepted operand types]
- `+` on strings: [concatenation, error, or coercion]
- Mixed types: [what happens]
- Comparison: [accepted operand types]
- Equality across types: [false, or an error]
- Division by zero: [value produced, or runtime error]

### Scope and bindings

- Redeclaration in the same scope: [allowed or an error]
- Uninitialized variable holds: [value]
- Shadowing: [behavior]
- Undefined name: [static error with exit 65, or runtime error with exit 70]

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
ERROR AT LINE 1: Unrecognized character @
ERROR AT LINE 1: Unterminated string
```


| Failure | Exit code |
|---|---|
| [lexical error] | 65 |
| [syntax error] | 65 |
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
- Boolean and nil-like literals do not yet exist.
- No explicit statement terminator token yet exists.
- `album` (functions) and `repeat` (loops) are recognized as keywords but
  do not do anything yet.
- Operator precedence and associativity are not yet finalized.

## Changelog

| Activity | What changed in the language |
|---|---|
| Lab 1 | Scanner implemented |
