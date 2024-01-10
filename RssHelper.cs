namespace SunamoRss;

public class RssHelper
{
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
                    // Read Item
                    case SyndicationElementType.Item:
                        var item = feedReader.ReadItem()
                        .Result; //AsyncHelper.ci.GetResult(feedReader.ReadItem());
                        result.Add(new Tuple<string, string, string, DateTimeOffset>(item.Title,
                        item.Links.First().Uri.ToString(), item.Description, item.Published));
                        break;

                        #region MyRegion

                        //// Read category
                        //case SyndicationElementType.Category:
                        //    ISyndicationCategory category = await feedReader.ReadCategory();
                        //    break;
                        //// Read Image
                        //case SyndicationElementType.Image:
                        //    ISyndicationImage image = await feedReader.ReadImage();
                        //    break;
                        //// Read link
                        //case SyndicationElementType.Link:
                        //    ISyndicationLink link = await feedReader.ReadLink();
                        //    break;
                        //// Read Person
                        //case SyndicationElementType.Person:
                        //    ISyndicationPerson person = await feedReader.ReadPerson();
                        //    break;
                        //// Read content
                        //default:
                        //    ISyndicationContent content = await feedReader.ReadContent();
                        //    break;

                        #endregion
                }

                if (result.Count == 5) break;
            }
        }

        return result;
    }

    private static async Task<List<Tuple<string, string, DateTimeOffset>>> Latest5PostsFromRssAsync(string filePath)
    {
        List<Tuple<string, string, DateTimeOffset>> result = new();

        using (var xmlReader = XmlReader.Create(filePath, new XmlReaderSettings { Async = true }))
        {
            RssFeedReader feedReader = new(xmlReader);

            while (await feedReader.Read())
            {
                switch (feedReader.ElementType)
                {
                    // Read category
                    case SyndicationElementType.Category:
                        await feedReader.ReadCategory();
                        break;

                    // Read Image
                    case SyndicationElementType.Image:
                        await feedReader.ReadImage();
                        break;

                    // Read Item
                    case SyndicationElementType.Item:
                        var item = await feedReader.ReadItem();

                        result.Add(new Tuple<string, string, DateTimeOffset>(item.Title,
                        item.Links.First().Uri.ToString(), item.Published));

                        break;

                    // Read link
                    case SyndicationElementType.Link:
                        await feedReader.ReadLink();
                        break;

                    // Read Person
                    case SyndicationElementType.Person:
                        await feedReader.ReadPerson();
                        break;

                    // Read content
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
