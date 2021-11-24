# Next Server Available
This is an Azure Function using C# in .NET 5.0 to find next available lowest numbered server. 

__Problem__

You have a stack of servers that maybe unavailable. The servers are numbered. We want to post a list of all servers with issues and find the next lowest number server to return.

## Example

__Input__

```json
[
    {"number":5},
    {"number":2},
    {"number":1}
]
```

__Output__

```json
{
    "number": 3
}
```
