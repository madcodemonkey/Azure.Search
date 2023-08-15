﻿using CustomSqlServerIndexer.Models;
using CustomSqlServerIndexer.Repositories;

namespace CustomSqlServerIndexer.Services;

public class GremlinDataService : IGremlinDataService
{
    private readonly IGremlinDataRepository _dataRepository;

    public GremlinDataService(IGremlinDataRepository dataRepository)
    {
        _dataRepository = dataRepository;
    }

    public async Task CreateAllAsync()
    {
        var rand = new Random(DateTime.Now.Millisecond);

        await _dataRepository.DeleteAllAsync();

        // Add vertexes 
        await _dataRepository.CreatePersonAsync("thomas", "Thomas", "Paine", rand.Next(29, 56), false);
        await _dataRepository.CreatePersonAsync("mary", "Mary", "Andersen", rand.Next(29, 56), false);
        await _dataRepository.CreatePersonAsync("ben", "Ben", "Miller", rand.Next(29, 56), false);
        await _dataRepository.CreatePersonAsync("robin", "Robin", "Wakefield", rand.Next(29, 56), false);

        // Add edges
        await _dataRepository.CreateKnowsRelationshipAsync("thomas", "mary");
        await _dataRepository.CreateKnowsRelationshipAsync("thomas", "ben");
        await _dataRepository.CreateKnowsRelationshipAsync("ben", "robin");
        await _dataRepository.CreateKnowsRelationshipAsync("mary", "ben");
        

        /*
  

            { "UpdateVertex",   "g.V('thomas').property('age', 44)" },
            { "CountVertices",  "g.V().count()" },
            { "Filter Range",   "g.V().hasLabel('person').has('age', gt(40))" },
            { "Project",        "g.V().hasLabel('person').values('firstName')" },
            { "Sort",           "g.V().hasLabel('person').order().by('firstName', decr)" },
            { "Traverse",       "g.V('thomas').out('knows').hasLabel('person')" },
            { "Traverse 2x",    "g.V('thomas').out('knows').hasLabel('person').out('knows').hasLabel('person')" },
            { "Loop",           "g.V('thomas').repeat(out()).until(has('id', 'robin')).path()" },
        //    { "DropEdge",       "g.V('thomas').outE('knows').where(inV().has('id', 'mary')).drop()" },
        //    { "CountEdges",     "g.E().count()" },
        //    { "DropVertex",     "g.V('thomas').drop()" },

         */

    }

    public async Task DeleteAllAsync()
    {
        await _dataRepository.DeleteAllAsync();
    }

    public async Task<List<Person>> GetPeopleAsync()
    {
        return await _dataRepository.GetPeopleAsync();
    }
}