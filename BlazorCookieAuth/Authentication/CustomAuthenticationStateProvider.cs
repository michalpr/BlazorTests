namespace BlazorCookieAuth.Authentication
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Text.Json;
    using System.Threading.Tasks;
    using BlazorCookieAuth.Data;
    using Microsoft.AspNetCore.Components.Authorization;
    using Microsoft.JSInterop;

    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime jsRuntime;
        private readonly IDbDataService userService;

        private UserItem cachedUser;

        public CustomAuthenticationStateProvider(IJSRuntime jsRuntime, IDbDataService userService)
        {
            this.jsRuntime = jsRuntime;
            this.userService = userService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            if (cachedUser == null)
            {
                var userAsJson = await jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "currentUser");
                if (!string.IsNullOrEmpty(userAsJson))
                {
                    cachedUser = JsonSerializer.Deserialize<UserItem>(userAsJson);

                    identity = this.SetupClaimsForUser(cachedUser);
                }
            }
            else
            {
                identity = this.SetupClaimsForUser(cachedUser);
            }

            var cachedClaimsPrincipal = new ClaimsPrincipal(identity);
            return await Task.FromResult(new AuthenticationState(cachedClaimsPrincipal));
        }

        public void ValidateLogin(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
                throw new Exception("Enter username");

            var identity = new ClaimsIdentity();
            try
            {
                var user = userService.ValidateUser(username, password);
                identity = this.SetupClaimsForUser(user);
                var serialisedData = JsonSerializer.Serialize(user);
                jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", serialisedData);
                cachedUser = user;
            }
            catch (Exception e)
            {
                throw e;
            }

            this.NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity))));
        }

        public void Logout()
        {
            cachedUser = null;
            var user = new ClaimsPrincipal(new ClaimsIdentity());
            jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", "");
            this.NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        private ClaimsIdentity SetupClaimsForUser(UserItem user)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim("Level", user.SecurityLevel.ToString()));

            var identity = new ClaimsIdentity(claims, "apiauth_type");
            return identity;
        }
    }
}