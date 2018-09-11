using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesAdminWebApp.Models.DAL
{
    public class EFTouristAttractionConnectionRepository : ITouristAttractionConnectionRepository
    {
        private ApplicationDbContext context;
        private IDistanceCalculator distanceCalculator;
        public EFTouristAttractionConnectionRepository(ApplicationDbContext ctx,
            IDistanceCalculator calculator)
        {
            context = ctx;
            distanceCalculator = calculator;

        }
        public void RecalculateConnections(int touristAttractionId)
        {
            var targetAttraction = context.TouristAttractions
                .Where(ta => ta.Id == touristAttractionId)
                .Select(ta => new
                {
                    AttractionId = ta.Id,
                    ta.Geoposition.Latitude,
                    ta.Geoposition.Longitude,
                    ta.CityId,
                    ta.Active
                }).FirstOrDefault();
            if (targetAttraction == null)
            {
                return;
            }
            //If it is active the matrix has to be recalculated
            if (targetAttraction.Active)
            {
                //lookup for the attractions that dont have a connection attached to the target attractio
                var attractionsForConnections = context.TouristAttractions
                    .Where(ta => ta.Id != targetAttraction.AttractionId && ta.CityId == targetAttraction.CityId && ta.Active && !ta.OriginPositionDistances.Any(opd => opd.DestinationId == targetAttraction.AttractionId))
                    .Select(ta => new
                    {
                        AttractionId = ta.Id,
                        ta.Geoposition.Latitude,
                        ta.Geoposition.Longitude,
                    }).ToList();
                //Now there goes the distance calculation for each one
                //Calculating distances from targetAttraction as origin and the rest as destinations
                var targetAsDestinationConnections = attractionsForConnections
                    .Select(a => new TouristAttractionConnection
                    {
                        CityId = targetAttraction.CityId,
                        OriginId = a.AttractionId,
                        DestinationId = targetAttraction.AttractionId,
                        Distance = distanceCalculator.CalculateKmDistance(a.Latitude, a.Longitude, targetAttraction.Latitude, targetAttraction.Longitude)
                    });
                var targetAsOriginConnections = targetAsDestinationConnections
                    .Select(c => new TouristAttractionConnection
                    {
                        CityId = targetAttraction.CityId,
                        OriginId = c.DestinationId,//In the last dataset query the destination was the target attraction
                        DestinationId = c.OriginId,
                        Distance = c.Distance
                    });
                context.TouristAttractionConnections.AddRange(targetAsDestinationConnections);
                context.TouristAttractionConnections.AddRange(targetAsOriginConnections);
            } else
            {
                //Here is the processing for the removal
                var connectionsToRemove = context.TouristAttractionConnections
                    .Where(tac => tac.OriginId == targetAttraction.AttractionId || tac.DestinationId == targetAttraction.AttractionId)
                    .ToList();
                context.TouristAttractionConnections.RemoveRange(connectionsToRemove);
            }
            context.SaveChanges();
        }
    }
}
