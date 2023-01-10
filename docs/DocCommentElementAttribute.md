# DocCommentElementAttribute
An XML-documentation attribute (e.g. `name` in `param`, etc.).

```cs
public record DocCommentElementAttribute(string Name, string Value)
```

## Properties
### Name
The name of the attribute (e.g. `name`, `cref`, etc.)

```cs
public string Name { get; }
```

### Value
The value of the attribute (e.g. the actual name in `name` attribute).

```cs
public string Value { get; }
```
