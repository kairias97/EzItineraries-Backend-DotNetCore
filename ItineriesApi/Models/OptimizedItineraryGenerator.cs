using ItinerariesApi.Models.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesApi.Models
{
    public class OptimizedItineraryGenerator : IItineraryGenerator
    {
        ApplicationDbContext context;

        public OptimizedItineraryGenerator(ApplicationDbContext ctx)
        {
            context = ctx;
        }
        public ItineraryProposal GenerateProposal(ItineraryRequest request)
        {
            //First the lookup of the matrix
            var connectionsMatrix = context.TouristAttractionConnections
                .Where(tac => tac.CityId == request.CityId)
                .OrderBy(tac => tac.OriginId)
                .ThenBy(tac => tac.DestinationId)
                .GroupBy(con => con.OriginId)
                .Select(originConnectionGroup => originConnectionGroup.Select(group => group)
                .ToArray()).ToArray();
            //Distance matrix in the format for the algorithm
            var distanceMatrix = connectionsMatrix.Select(array => array.Select(ta => ta.Distance).ToArray()).ToArray();
            //There will go the logic of the algorithm

            //To mockup the order of the attractions in the form of an array with the Ids of the attractions
            var ids = connectionsMatrix.Select(row => row.Select(r => r.OriginId).First()).ToList();
            if ( ids.Count > 0)
            {
                ids.Add(ids[0]);
            }

            //Retrieval of db items in the order they are gonna be visited
            TouristVisit[] attractionVisits = new TouristVisit[ids.Count];
            var routeAttractions = context.TouristAttractions
                .Include(ta => ta.Category)
                .Include(ta => ta.City)
                .ThenInclude(city => city.Country)
                .Where(ta => ids.Contains(ta.Id))
                .ToList();
            Parallel.For(0, ids.Count, index =>
            {
                attractionVisits[index] = new TouristVisit
                {
                    Order = index + 1,
                    TouristAttraction = routeAttractions.Where(ra => ra.Id == ids[index]).First()
                };
            });
            ItineraryProposal proposal = new ItineraryProposal();

            proposal.TouristVisits = attractionVisits;
            proposal.CalculatedKmDistance = 154.32;
            proposal.Score = 20.34;
            return proposal;
        }
    }
}
