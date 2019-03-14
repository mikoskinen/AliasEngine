# Alias Engine

Alias Engine is a C# and .NET Standard based engine, which allows you to create aliases for your commands. It's inspired by the alias support offered by mIrc: https://www.mirc.com/help/html/index.html?aliases.html.

[![NuGet](https://img.shields.io/nuget/v/AliasEngine.svg)](https://www.nuget.org/packages/AliasEngine/)

## Example:

Full command: /join

Alias: /j

```
AliasConverter.AddAlias("/j /join");
...
AliasConverter.Convert("/j") = "/join";
```

## Defining an alias

In simplest form, Alias Engine can be used to shorten commands:

```
AliasConverter.AddAlias("/j /join");
AliasConverter.AddAlias("/x multiple return words");
```

Example:

```
AliasConverter.Convert("/j") = "/join"
```

## Aliases with Parameters

Alias Engine supports aliases with parameters. You can define one or many parameters for each alias:

```
AliasConverter.AddAlias("/j /join {0}");
AliasConverter.AddAlias("/j1 /join {0} {1}");
```

Example:

```
AliasConverter.Convert("/j1 hello there") = "/join hello there"
```

## Aliases with multiword parameters

Alias Engine supports cases where an alias contains a multiword parameter:

```
AliasConverter.AddAlias("/t /topic {0}-");
```

Example:

```
AliasConverter.Convert("/t hello there") = "/topic hello there"
```

## Aliases with Multiple Commands

AliasConver.Convert always returns an array of commands. This is because one alias can contain multiple commands:

```
AliasConverter.AddAlias("/j2 /join {0} | /join {1}");
```

Example:

```
AliasConverter.Convert("/j2 hello there") =  ["/join hello", "/join there"]
```
