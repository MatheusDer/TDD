using FluentAssertions;
using Newtonsoft.Json;
using RoomBookingApp.Core.Enums;
using RoomBookingApp.Core.Models;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using Xunit;

namespace RoomBookingApp.Api.IntegrationTests;

public class RoomBookingControllerTests : IClassFixture<IntegrationTest<Program>>
{
    private readonly HttpClient _client;
    private readonly IntegrationTest<Program> _factory;
    private readonly RoomBookingRequest _request = new()
    {
        Date = DateTime.Now.AddDays(1),
        FullName = "Test",
        Email = "test@email.com"
    };

    public RoomBookingControllerTests(IntegrationTest<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task BookRoom_WithInvalidModel_ReturnsBadRequest()
    {
        var response = await _client.PostAsync("/roombooking", new StringContent
        (
            JsonConvert.SerializeObject(new RoomBookingRequest()),
            Encoding.UTF8,
            "application/json")
        );

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task BookRoom_WithValidModel_ReturnsCreated()
    {
        var response = await _client.PostAsync("/roombooking", new StringContent
        (
            JsonConvert.SerializeObject(_request),
            Encoding.UTF8,
            "application/json")
        );

        var roomBookingResult = await response.Content.ReadFromJsonAsync<RoomBookingResult>();

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        roomBookingResult.Should().NotBeNull();
        roomBookingResult.RoomBookingId.Should().NotBeNull();
        roomBookingResult.Email.Should().Be(_request.Email);
        roomBookingResult.FullName.Should().Be(_request.FullName);
        roomBookingResult.Date.Should().Be(_request.Date);
        roomBookingResult.Flag.Should().Be(BookingFlag.Success);
    }

    [Fact]
    public async Task BookRoom_WithNoRoomsAvailable_ReturnsNotFound()
    {
        await _client.PostAsync("/roombooking", new StringContent
        (
            JsonConvert.SerializeObject(_request),
            Encoding.UTF8,
            "application/json")
        );

        var secondBookingResponse = await _client.PostAsync("/roombooking", new StringContent
        (
            JsonConvert.SerializeObject(_request),
            Encoding.UTF8,
            "application/json")
        );

        var roomBookingResult = secondBookingResponse.Content.ReadAsStringAsync();

        secondBookingResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

        roomBookingResult.Should().NotBeNull();
        roomBookingResult.Result.Should().BeEquivalentTo("No Rooms Available");
    }
}
