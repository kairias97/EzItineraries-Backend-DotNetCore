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
                .Select(originConnectionGroup => originConnectionGroup.Select(a => new {
                    a.OriginId,
                    a.DestinationId,
                    OriginRating = a.Origin.Rating,
                    OriginCategoryId = a.Origin.CategoryId,
                    a.Distance
                }).ToArray()).ToArray();
            //Distance matrix in the format for the algorithm
            double[][] distanceMatrix = connectionsMatrix.Select(array => array.Select(ta => ta.Distance).ToArray()).ToArray();

            //preference weights setup in the format for the algorithm.
            double[] weights = request.Preferences.OrderBy(p => p.CategoryId).Select(p => p.Weight).ToArray();
           

            int initialPosition = 0;
            //Scored attractions setup
            List<ScoredAttraction> scoredAttractions =  connectionsMatrix.Select(attractionsArray => attractionsArray.First())
                .Select(attraction => new ScoredAttraction
                {
                    ExternalId = attraction.OriginId,
                    Position = initialPosition++,
                    WasVisited = false,
                    ArtScore = ScoredAttraction.CalculateArtScore((BelongingScoreCategory)attraction.OriginCategoryId, 
                        Convert.ToDouble(attraction.OriginRating)),
                    HistoryScore = ScoredAttraction.CalculateHistoryScore((BelongingScoreCategory)attraction.OriginCategoryId,
                        Convert.ToDouble(attraction.OriginRating)),
                    ReligionScore = ScoredAttraction.CalculateReligionScore((BelongingScoreCategory)attraction.OriginCategoryId,
                        Convert.ToDouble(attraction.OriginRating)),
                    NatureScore = ScoredAttraction.CalculateNatureScore((BelongingScoreCategory)attraction.OriginCategoryId,
                        Convert.ToDouble(attraction.OriginRating)),
                    ExternalCategoryId = attraction.OriginCategoryId
                }).ToList();
            int originAttractionPosition = -1;
            //Selection of a random attraction prioritizing the highest one rated
            foreach (var preference in request.Preferences.OrderByDescending(p => p.Weight))
            {
                if (scoredAttractions.Any(sa => sa.ExternalCategoryId == preference.CategoryId))
                {
                    int[] attractionsPositionsByPreference = scoredAttractions.Where(sa => sa.ExternalCategoryId == preference.CategoryId)
                        .Select(sa => sa.Position).ToArray();
                    int randomIndex = new Random().Next(0, attractionsPositionsByPreference.Length);
                    originAttractionPosition = attractionsPositionsByPreference[randomIndex];
                    break;
                }

            }
            //It means there is no attractions at all since none matched with the categories or the weights are invalid
            decimal totalWeight = (decimal)0.00;
            foreach (var w in weights)
            {
                totalWeight += Convert.ToDecimal(w);
            }
            if(originAttractionPosition == -1 || weights.Length != 4 || totalWeight != (decimal)1.00)
            {
                return new ItineraryProposal
                {
                    Score = 0.00,
                    CalculatedKmDistance = 0.00,
                    TouristVisits = new List<TouristVisit>()
                };
            }
            

            /*
            if (request.MaxDistance < 1)
            {
                return new ItineraryProposal
                {
                    Score = 0.00,
                    CalculatedKmDistance = 0.00,
                    TouristVisits = new List<TouristVisit>()
                };
            }*/

            CustomVNSItinerary itineraryImplementation = new CustomVNSItinerary(weights, distanceMatrix, scoredAttractions, originAttractionPosition);
            
            OptimizedItinerary calculatedItinerary = itineraryImplementation.CreateSingleDayItinerary(request.MaxDistance, 10);

            int[] itineraryOrderedVisitsIds = calculatedItinerary.Select(sa => sa.ExternalId).ToArray(); 
            
            //Retrieval of db items in the order they are gonna be visited
            TouristVisit[] attractionVisits = new TouristVisit[itineraryOrderedVisitsIds.Length];
            var routeAttractions = context.TouristAttractions
                .Include(ta => ta.Category)
                .Include(ta => ta.City)
                .ThenInclude(city => city.Country)
                .Where(ta => itineraryOrderedVisitsIds.Contains(ta.Id))
                .ToList();

            Parallel.For(0, itineraryOrderedVisitsIds.Length, index =>
            {
                attractionVisits[index] = new TouristVisit
                {
                    Order = index + 1,
                    TouristAttraction = routeAttractions.Where(ra => ra.Id == itineraryOrderedVisitsIds[index]).First()
                };
            });
            
            ItineraryProposal proposalResult = new ItineraryProposal();

            proposalResult.TouristVisits = attractionVisits;
            proposalResult.CalculatedKmDistance = calculatedItinerary.Distance;
            proposalResult.Score = calculatedItinerary.Score;
            return proposalResult;
        }
    }
}
