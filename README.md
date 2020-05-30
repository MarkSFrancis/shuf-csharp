# shuf (C#)
A C# implementation of the UNIX coreutils app [shuf](https://www.mankier.com/1/shuf)

This application will attempt to get a small random sample of records from a large data set. By default, it uses stdin and stdout, but also supports files (see [Usage](#Usage)). 

Each record has an even probability to appear in the output, regardless of the input length. The output will be in a random order.

Each record should be separated by a newline (`\n` on a UNIX based OS, and `\r\n` on Windows). A trailing blank line is treated as a valid record, and so may appear as a record in the output.

# Usage
  shuf [options]

|Option||
|-|-|
|--n|The total number of random items to get. If the input is not long enough, all input values will be output in a random order|
|--input|The input file to read from (if not using stdin)|
|--output|The output file to write to (if not using stdout)|
|--quiet|Whether the run in quiet mode (disabling writing to stdout). Only applies if the output is to a file|

## Example:
To get 4 random values from a file in the same folder called `in.txt`

### From a terminal
```sh
dotnet run --input ./in.txt -n 4
```

### Input.txt
```
item1
item2
item3
item4
item5
item6
item7
item8
item9
item10
```

### Output (in terminal)
```
item7
item3
item9
item5
```
