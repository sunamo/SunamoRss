namespace SunamoRss;

/// <summary>
/// Provides helper methods for reading and parsing RSS feeds.
/// </summary>
public class RssHelper
{
    /// <summary>
    /// Reads up to 5 latest posts from an RSS feed file synchronously.
    /// Returns a list of tuples containing title, link URL, description and publish date.
    /// </summary>
    /// <param name="filePath">Absolute path to the RSS feed XML file.</param>
    /// <returns>List of tuples with title, link, description and publish date of the latest posts.</returns>
    public static List<Tuple<string, string, string, DateTimeOffset>> Latest5PostsFromRss(string filePath)
    {
        List<Tuple<string, string, string, DateTimeOffset>> result = new();

        if (File.Exists(filePath))
        {
            using var xmlReader = XmlReader.Create(filePath, new XmlReaderSettings());
            RssFeedReader feedReader = new(xmlReader);

            while (feedReader.Read().Result)
            {
                switch (feedReader.ElementType)
                {
                    case SyndicationElementType.Item:
                        var syndicationItem = feedReader.ReadItem().Result;
                        result.Add(new Tuple<string, string, string, DateTimeOffset>(syndicationItem.Title,
                            syndicationItem.Links.First().Uri.ToString(), syndicationItem.Description, syndicationItem.Published));
                        break;
                }

                if (result.Count == 5) break;
            }
        }

        return result;
    }

    /// <summary>
    /// Reads up to 5 latest posts from an RSS feed file asynchronously.
    /// Returns a list of tuples containing title, link URL and publish date.
    /// </summary>
    /// <param name="filePath">Absolute path to the RSS feed XML file.</param>
    /// <returns>List of tuples with title, link and publish date of the latest posts.</returns>
    public static async Task<List<Tuple<string, string, DateTimeOffset>>> Latest5PostsFromRssAsync(string filePath)
    {
        List<Tuple<string, string, DateTimeOffset>> result = new();

        using (var xmlReader = XmlReader.Create(filePath, new XmlReaderSettings { Async = true }))
        {
            RssFeedReader feedReader = new(xmlReader);

            while (await feedReader.Read())
            {
                switch (feedReader.ElementType)
                {
                    case SyndicationElementType.Category:
                        await feedReader.ReadCategory();
                        break;

                    case SyndicationElementType.Image:
                        await feedReader.ReadImage();
                        break;

                    case SyndicationElementType.Item:
                        var syndicationItem = await feedReader.ReadItem();

                        result.Add(new Tuple<string, string, DateTimeOffset>(syndicationItem.Title,
                            syndicationItem.Links.First().Uri.ToString(), syndicationItem.Published));

                        break;

                    case SyndicationElementType.Link:
                        await feedReader.ReadLink();
                        break;

                    case SyndicationElementType.Person:
                        await feedReader.ReadPerson();
                        break;

                    default:
                        await feedReader.ReadContent();
                        break;
                }

                if (result.Count == 5) break;
            }
        }

        return result;
    }
}
