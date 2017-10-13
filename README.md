# Downsize [![Build Status](https://travis-ci.org/Magentaize/Downsize.Net.svg?branch=master)](https://travis-ci.org/Magentaize/Downsize.Net)

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

```csharp
Downsize.Substring("<p>some markup here...</p>", options);
```