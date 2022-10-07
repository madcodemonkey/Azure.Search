using Azure;
using Azure.Search.Documents.Models;
using Search.Model;

namespace CreateAndPopulateApp;

public abstract class QueryHotelIndexMenuItemBase
{
    // Write search results to console
    protected async Task WriteDocumentsAsync(SearchResults<Hotel> searchResults)
    {
        // Show the facets returned from teh search if there are any.  You have to request
        // the fields you want facets on in your search options.  Furthermore, they ONLY include 
        // the values from the records that match the query.  In other words, if your document contained a category field
        // and document A had a category = 'Hilly' and document B had a category of 'Flat' and your search text would ONLY
        // return document A then your category facet would only have 'Hilly' because it will be used as a search filter if chosen.
        if (searchResults.Facets != null && searchResults.Facets.Count > 0)
        {
            Console.WriteLine("Facets");
            foreach (var facet in searchResults.Facets)
            {
                Console.WriteLine($"Facet: '{facet.Key}' and values for those facets:");
                foreach (FacetResult result in facet.Value)
                {
                    Console.WriteLine($"---{result.Value}");
                }
            }
        }

        bool somethingFound = false;
        AsyncPageable<SearchResult<Hotel>> resultList = searchResults.GetResultsAsync();
        
        await foreach (SearchResult<Hotel> result in resultList)
        {
            somethingFound = true;
            
            Console.WriteLine($"Score: {result.Score} - {result.Document}");
        }

        if (somethingFound == false)
        {
            Console.WriteLine("Nothing found.");
        }

        Console.WriteLine();
    }

    protected void WriteDocumentsAsync(AutocompleteResults autoResults)
    {
        foreach (AutocompleteItem result in autoResults.Results)
        {
            Console.WriteLine(result.Text);
        }

        Console.WriteLine();
    }
}