# Sample
A sample class that is documented.

This a second paragraph.
It is indented.

This is the third paragraph.

Here is a _italic_, _italic2_ fragment.
Here is a **bold**, **bold2** fragment.
Here is a `code` fragment.
Here is a ~~strikethrough~~ fragment.

_Remarks section._
_Second line._

_Another paragraph._

```cs
public class Sample<T0, T1>
```

## Type Parameters
- `T0`: A first type parameter.
- `T1`: A second type parameter.

## Fields
### Field1
A sample field.

```cs
public int Field1
```

## Properties
### Property1
A sample property.

```cs
public int Property1 { get; set; }
```

## Methods
### Method(int, string)
A simple method.

It contains two parameters:
- `x` means `x`
- `y` means `y`

It contains three type parameters:
- `M0` is the first one
- `M1` is the second one
- `M2` is the third one

```cs
public TimeSpan Method<M0, M1, M2>(int x, string y)
```

#### Parameters
- `x`: The `x` of the method.
- `y`: The `y` of the method.

#### Type Parameters
- `M0`: The first type parameter of the method.
- `M1`: The second type parameter of the method.
- `M2`: The third type parameter of the method.

#### Returns
The `TimeSpan` instance.

### Method(short, string)
The overloaded [`Method{M0,M1,M2}(int,string)`](./Method{M0,M1,M2}(int,string).md).

```cs
public TimeSpan Method<M0, M1, M2>(short x, string y)
```
