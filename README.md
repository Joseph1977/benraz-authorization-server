# benraz-authorization-server
an implementation of a comprehensive Authorization server

## How to use


## Features
Manage:
* Users
* Claims and roles
* Applications, each its own:
  * id
  * audiance
  * claims
  * roles (includes list of claims, kind of group)
  * expiration time token
  * server to server access tokens
  * secure redirect
* Login with Microsoft, Google and Facebook
* Access tokens (JWT) Types:
  * User (include all the claims)
  * User (include only roles) - `not implemented yet`
  * User reference, revocable access token id, only id without claims `not implemented yet`
  * Server to server (revocable access token), includes custom claims: add during the creation (example: customer-id this access token create for, this can allow platform verify the request IP in that corresponding client whitelists)

Support:
1. Auth2 and OpenID flow
2. Internal SSO (manage multiple applications)
3. Sign up with verification flow (both email and code)
4. Reset password


## Structure
* \benraz-authorization-server\Authorization.Domain. Business entities and services.
* \benraz-authorization-server\Authorization.Domain.Tests. Unit tests for business services and business logic.
* \benraz-authorization-server\Authorization.EF. Data access layer implementation with Entity Framework Core. Contains:
  * database context;
  * entities configurations;
  * folder for entities data configurations (predefined lookups entries);
  * repositories;
  * database migration service (used for database migration and could be extended with default appendable lookups entries addition).
* \benraz-authorization-server\Authorization.EF.Tests. Tests for data access layer, like tests for repositories. Already contains a base class for repository tests.
* \benraz-authorization-server\Authorization.WebApi. Web API layer, based on ASP.NET Core Web API technology. Has Swagger, NLog, database context and authorization mechanism plugged in.
* \benraz-authorization-server\Authorization.WebApi.IntegrationTests. Integration tests for Web API layer. Already has:
  * Config that prepares server, HTTP client and database context;
  * StartupStub that extends Startup of the server under test and mocks an authorization mechanism to unbind the server under test from Benraz Authorization server;
  * ControllerTestsBase which could be used a base class for all controllers tests.

