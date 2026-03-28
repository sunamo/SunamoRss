### SunamoRss

Wrapper around the [Microsoft.SyndicationFeed.ReaderWriter](https://www.nuget.org/packages/Microsoft.SyndicationFeed.ReaderWriter) library for reading RSS feeds.

#### Features

- Read latest posts from RSS feed files (synchronous and asynchronous)
- Returns structured data with title, link, description and publish date

#### Usage

```csharp
// Synchronous
var posts = RssHelper.Latest5PostsFromRss("path/to/feed.xml");

// Asynchronous
var posts = await RssHelper.Latest5PostsFromRssAsync("path/to/feed.xml");
```

#### Target Frameworks

`net10.0`, `net9.0`, `net8.0`

#### Links

- [NuGet](https://www.nuget.org/profiles/sunamo)
- [GitHub](https://github.com/sunamo/PlatformIndependentNuGetPackages)
- [Developer site](https://sunamo.cz)

Request for new features / bug report: [Mail](mailto:radek.jancik@sunamo.cz) or on GitHub
