
---
# Trying out CQRS for Non-Dependent Address Entity
[Docs](https://www.telerik.com/blogs/applying-cqrs-pattern-aspnet-core-application)
- Having a Event store
- Command & Query

---
# Vertical Slice Architecture
- **Feature-based Structure:** Each feature is a self-contained module with its own domain logic, data access, and user interface.
- **Single Responsibility:** Each slice is responsible for one specific functionality or feature, reducing dependencies between different parts of the application.
- **Encapsulation:** Each slice encapsulates its logic, making the application more modular and easier to test.
- **CQRS (Command Query Responsibility Segregation):** Often used in conjunction with VSA to separate read and write operations, enhancing performance and scalability.

### References
> [jimmy-ref](https://www.jimmybogard.com/vertical-slice-architecture/)
> [graywoodfine-ref](https://garywoodfine.com/implementing-vertical-slice-architecture/)

--- 
### Accessor can be recursive :)
```csharp
public DateTime DateOfBirth  
{  
    get => DateOfBirth;  
    set => {
		DateOfBirth = value; // on recursive sets value to DateOfBirth
	    Age = DateTime.Today.Year - value.Year;  
	}
}
```
### Required attribute instead of nullable
```csharp
[MaxLength(30)] public required string State { get; set; }
							|
							Ensures that object is not getting initialized without passing those properties
```

Ok then what about `Annotations.Required` and keyword `required`
```cs
[Required] // does it in runtime
public required Entity entity; // does it in compile-time
```
using it both is a over kill:
- `Annotations.Required` can be used for Entity since it  can be translated to sql constraints
- `required` can be used for internal must one like DTO, also we can have `init` for setter.


### Layer by Feature 
[ref](https://www.jimmybogard.com/vertical-slice-architecture/)
- removes the complexity look why developing

### TPC (Table Per Concrete Type)
[ref](https://www.scholarhat.com/tutorial/entityframework/understanding-inheritance-in-entity-**framework)
- Here I have used TPC inheritance for the entities, which reduces lot of boiler plate from the repos

## Handling two one to one relations
```csharp
modelBuilder.Entity<Profile.Profile>()  
    .HasMany<Match.Match>(profile => profile.Matches)  
    .WithOne(match => match.ProfileOne)  
    .HasForeignKey(match => match.ProfileOneId)  
    .OnDelete(DeleteBehavior.NoAction);  
modelBuilder.Entity<Profile.Profile>()  
    .HasMany<Match.Match>(profile => profile.Matches)  
    .WithOne(match => match.ProfileTwo)  
    .HasForeignKey(match => match.ProfileTwoId)  
    .OnDelete(DeleteBehavior.NoAction);

modelBuilder.Entity<Match.Match>()  
    .HasOne<Profile.Profile>(match => match.ProfileOne)  
    .WithMany()  
    .OnDelete(DeleteBehavior.Cascade);  
modelBuilder.Entity<Match.Match>()  
    .HasOne<Profile.Profile>(match => match.ProfileTwo)  
    .WithMany()  
    .OnDelete(DeleteBehavior.Cascade);
```
with this tried to establish one to one between profile matches, but under profile it's just a single ennumerable collection
- got this a error while adding migration
```powershell
Unable to create a 'DbContext' of type ''. The exception 'Cannot create a relationship between 'Profile.Matches' and 'Match.ProfileTwo' because a relationship already exists between 'Profile.Matches' and 'Match.ProfileOne'. Navigations can only participate in a single relationship. If you want to override an existing relationship call 'Ignore' on the navigation 'Match.ProfileTwo' first in 'OnModelCreating'.' was thrown while attempting to create an instance. For the different patterns supported at design time, see https://go.microsoft.com/fwlink/?linkid=851728
```

Solution:
- For Ef to handle this kind of multiple one to many relations, just have them in separate ICollections
```csharp
modelBuilder.Entity<Match.Match>()  
    .HasOne<Profile.Profile>(match => match.SentProfile)  
    .WithMany(profile => profile.SentMatches)  
    .OnDelete(DeleteBehavior.NoAction);  
modelBuilder.Entity<Match.Match>()  
    .HasOne<Profile.Profile>(match => match.ReceivedProfile)  
    .WithMany(profile => profile.ReceivedMatches)  
    .OnDelete(DeleteBehavior.NoAction);


```

## New changes in NUnit 4
[ref](https://docs.nunit.org/articles/nunit/release-notes/breaking-changes.html):w

- All the legacy assertion will be under `ClassicAssert lib`


## Refresh token based auth & token invalidation
- After careful, Generate of two tokens have been implemented.
- with this now a user can 
	- Logout of other device
	- Can opt for temporary device sign-in ( stay signed ability )


## Handling Secrets in dotnet
[ref](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-8.0)
- As for 2FA needed lot's of secrets to be handled ( more sensitive )
- `dotnet user-secrets init` - in current project, creates secrets store for us. those secrets are store under `~/.microsoft/usersecrets/<user_secrets_id>/secrets.json`
- For that specific project new secrets key is create and added with the `App.csproj`

- Adding a secret: `dotnet user-secrets set "Movies:ServiceApiKey" "12345"`
- Getting it in app: `var movieApiKey = builder.Configuration["Movies:ServiceApiKey"];`
- Listing them: `dotnet user-secrets list`
- Removing: `dotnet user-secrets remove "Movies:ConnectionString"`
- Removes all secrets: `dotnet user-secrets clear`
