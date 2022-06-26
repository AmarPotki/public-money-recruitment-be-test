using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Application.Commands;
using VacationRental.Application.ViewModels;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class PostRentalTests
    {
        private readonly HttpClient _client;

        public PostRentalTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostRental_ThenAGetReturnsTheCreatedRental()
        {
            var request = new RentalBindingModel
            {
                Units = 25,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", request))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getResponse = await _client.GetAsync($"/api/v1/rentals/{postResult.Id}"))
            {
                Assert.True(getResponse.IsSuccessStatusCode);

                var getResult = await getResponse.Content.ReadAsAsync<RentalViewModel>();
                Assert.Equal(request.Units, getResult.Units);
            }
        }
    }

    [Collection("Integration")]
    public class PutRentalTests
    {
        private readonly HttpClient _client;

        public PutRentalTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenBookingOverlap_GetBadRequest()
        {
            var request = new RentalBindingModel
            {
                Units = 25,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", request))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBookingRequest = new BookingBindingModel
            {
                RentalId = postResult.Id,
                Nights = 3,
                Start = new DateTime(2023, 01, 01)
            };

            ResourceIdViewModel postBookingResult;
            using (var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", postBookingRequest))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
                postBookingResult = await postBookingResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBookingRequest2 = new BookingBindingModel
            {
                RentalId = postResult.Id,
                Nights = 3,
                Start = new DateTime(2023, 01, 01)
            };

            ResourceIdViewModel postBookingResult2;
            using (var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", postBookingRequest2))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
                postBookingResult2 = await postBookingResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var updateRental = new UpdateRentalCommand
            {
                RentalId = postResult.Id,
                Units = 1,
                PreparationTimeInDays = 1

            };
            using (var postBookingResponse = await _client.PutAsJsonAsync($"/api/v1/rentals/{postResult.Id}", updateRental))
            {
                Assert.False(postBookingResponse.IsSuccessStatusCode);
                
            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPutRental_GetNoContentStatusCode()
        {
            var request = new RentalBindingModel
            {
                Units = 5,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", request))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBookingRequest = new BookingBindingModel
            {
                RentalId = postResult.Id,
                Nights = 3,
                Start = new DateTime(2023, 01, 01)
            };

            using (var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", postBookingRequest))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
            }

            var postBookingRequest2 = new BookingBindingModel
            {
                RentalId = postResult.Id,
                Nights = 3,
                Start = new DateTime(2023, 01, 01)
            };

            using (var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", postBookingRequest2))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
            }

            var updateRental = new UpdateRentalCommand
            {
                RentalId = postResult.Id,
                Units = 6,
                PreparationTimeInDays = 1

            };
            using (var postBookingResponse = await _client.PutAsJsonAsync($"/api/v1/rentals/{postResult.Id}", updateRental))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
                Assert.Equal(HttpStatusCode.NoContent,postBookingResponse.StatusCode);
            }
        }
    }
}
