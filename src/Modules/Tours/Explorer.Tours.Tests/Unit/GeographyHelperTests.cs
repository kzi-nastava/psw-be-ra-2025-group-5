using Explorer.Tours.Core.Domain.Shared;
using Shouldly;

namespace Explorer.Tours.Tests.Unit;

public class GeographyHelperTests
{
    [Fact]
    public void CalculateDistance_ReturnsZero_ForSameCoordinates()
    {
        var latitude = 44.7866;
        var longitude = 20.4489;
        
        var distance = GeographyHelper.CalculateDistance(latitude, longitude, latitude, longitude);
        
        distance.ShouldBe(0, tolerance: 0.0001);
    }

    [Fact]
    public void CalculateDistance_ReturnsCorrectDistance_ForKnownLocations()
    {
        var belgradeLat = 44.7866;
        var belgradeLon = 20.4489;
        var noviSadLat = 45.2671;
        var noviSadLon = 19.8335;
        
        var distance = GeographyHelper.CalculateDistance(belgradeLat, belgradeLon, noviSadLat, noviSadLon);
        
        distance.ShouldBeInRange(70, 90);
    }

    [Fact]
    public void CalculateDistance_ReturnsPositiveValue_ForDifferentCoordinates()
    {
        var lat1 = 44.7866;
        var lon1 = 20.4489;
        var lat2 = 45.2671;
        var lon2 = 19.8335;
        
        var distance = GeographyHelper.CalculateDistance(lat1, lon1, lat2, lon2);
        
        distance.ShouldBeGreaterThan(0);
    }

    [Fact]
    public void CalculateDistance_IsSymmetric()
    {
        var lat1 = 44.7866;
        var lon1 = 20.4489;
        var lat2 = 45.2671;
        var lon2 = 19.8335;
        
        var distance1 = GeographyHelper.CalculateDistance(lat1, lon1, lat2, lon2);
        var distance2 = GeographyHelper.CalculateDistance(lat2, lon2, lat1, lon1);
        
        distance1.ShouldBe(distance2, tolerance: 0.0001);
    }
}

