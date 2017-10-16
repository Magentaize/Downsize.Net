# Downsize.Net [![Build status](https://img.shields.io/appveyor/ci/Magentaize/downsize-net.svg?style=flat-square&colorB=4BAE4F)](https://ci.appveyor.com/project/Magentaize/downsize-net) [![NuGet](https://img.shields.io/nuget/v/Downsize.Net.svg?style=flat-square)](https://www.nuget.org/packages/Downsize.Net)
Tag-safe HTML and XML text-truncation!

```
PM> Install-Package Downsize.Net
```

## Usage
```csharp
var options = new DownsizeOptions(words: 26);
```

or
```csharp
var options = new DownsizeOptions(
                  words: 2,
                  append: "... (read more)");
```

or
```csharp
var options = new DownsizeOptions(
                  characters: 7,
                  contextualTags: new List<string>() {"p", "ul", "ol", "pre", "blockquote"});
```

or
```csharp
var options = new DownsizeOptions(
                  characters: 15,
                  round: true);
```

Get the downsize result:
```csharp
var result = Downsize.Substring("<p>some markup here...</p>", options);
```
