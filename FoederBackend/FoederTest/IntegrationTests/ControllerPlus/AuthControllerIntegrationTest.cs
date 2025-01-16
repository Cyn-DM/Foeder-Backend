using System.Net;
using System.Security.Cryptography;
using System.Text;
using FoederAPI.Controllers;
using FoederBusiness.Helpers;
using FoederBusiness.Services;
using FoederBusiness.Tools;
using FoederDAL;
using FoederDAL.Repository;
using FoederDomain.DomainModels;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Text.Json;
using FoederBusiness.Dtos;

namespace FoederTest.IntegrationTests.ControllerPlus;

[TestFixture]
public class AuthControllerIntegrationTest
{
    
    [Test]
    public async Task LoginSuccessfull()
    {
        await using var application = new ApiWebApplicationFactory();
        using var client = application.CreateClient();
        
        var request = new 
        {
            CredentialResponse = "valid_id_token",
        };
        
        var requestContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        
        
        var response = await client.PostAsync("api/Auth/Login", requestContent);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var responseContent = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<LoginTokenResult>(responseContent);
        
        Assert.IsNotNull(responseObject);
        Assert.IsNotEmpty(responseObject.AccessToken);
        Assert.IsNotEmpty(responseObject.RefreshToken);


    }
    
    [Test]
    public async Task AssertLoginFailed()
    {

        await using var application = new ApiWebApplicationFactory();
        using var client = application.CreateClient();
        
        var request = new 
        {
            CredentialResponse = "invalid_id_token",
        };
        
        var requestContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        
        
        var response = await client.PostAsync("api/Auth/Login", requestContent);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
        var responseContent = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<LoginTokenResult>(responseContent);
        
        Assert.IsNotNull(responseObject);
        Assert.IsNull(responseObject.AccessToken);
        Assert.IsNull(responseObject.RefreshToken);

    }
    
}