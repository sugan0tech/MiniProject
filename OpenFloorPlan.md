
# Service Func
---
1. User -> can manager multiple profiles ( as with relation )
2. MemberShip of (Basic & Premium). Premium -> chat (without sharing creds) & profile view
3. Preference -> cumulative set of preference and segrigates results
4. A match is generated and shared between two, if both accepted can share profiles.
5. Staff -> Background staffs Admin and Employees
6. Premium -> ( 25 profile chats per month )
7. CRON & startup based premium validation
8. Free Train for basic user ( 7 days as premium user )
## Entities
---
- User 
```mermaid

classDiagram
    class User {
        -UserId: int
        -Email: string
        -FirstName: string
        -LastName: string
        -PhoneNumber: string
        -AddressId: int
        -Address: Address
        -IsVerified: bool
        -Password: byte[]
        -HashKey: byte[]
        -loginAttempts: int = 0
        -Messages: IEnumerable<Message>
    }

```


- Profile
```mermaid
classDiagram
    class Profile {
        -ProfileId: int [Key]
        -DateOfBirth: DateTime
        -CreatedAt: DateTime = DateTime.Now
        -Education: string
        -AnnualIncome: int
        -Occupation: string
        -MaritalStatus: string
        -MotherTongue: string
        -Religion: string
        -Ethinicity: string
        -Bio: string
        -ProfilePicture: byte[]
        -Habits: string
        -Gender: string
        -Weight: int
        -Height: int
        -MembershipId: int [FK]
        -ManagedById: int [FK]
        -UserId: int
        -ManagedByRelation: string
        -ProfileViews: IEnumerable<ProfileView>
        -Matches: IEnumerable<Match>
        -PreferenceId: int [FK]
        -Membership: Membership
        -ManagedBy: User
        -User: User
        -Preference: Preference
    }

```
- MemberShip
```mermaid
classDiagram
    class Membership {
        -MembershipId: int
        -Type: string [MaxLength: 20]
        -Description: string [MaxLength: 100]
        -EndsAt: DateTime
        -IsTrail: bool
    }
```
- ProfileView
```mermaid
classDiagram
    class ProfileView {
        -Id: int [Key]
        -ViewerId: int
        -ViewedProfileAt: int
        -ViewedAt: DateTime
    }
```
- Preference
```mermaid
classDiagram
    class Preference {
        -PreferenceId: int [Key]
        -MotherTongue: string
        -Religion: string
        -Education: string
        -Occupation: string
        -HeightRange: Tuple<int, int>
        -AgeRange: Tuple<int, int>
        -PreferenceForId: int [ForeignKey]
        -CreatedAt: DateTime = DateTime.Now
        -UpdatedAt: DateTime
    }

```
- Match
```mermaid
classDiagram
    class Match {
        -MatchId: int [Key]
        -ProfileOneId: int [ForeignKey]
        -ProfileOneLike: bool?
        -ProfileTwoId: int [ForeignKey]
        -ProfileTwoLike: bool?
        -Level: int [Range: 1-7]
        -FoundAt: DateTime
    }

    class Profile {
    }

    Match "1" -- "1" Profile: ProfileOne
    Match "1" -- "1" Profile: ProfileTwo

```
- Message
```mermaid
classDiagram
    class Message {
        -MessageId: int [Key]
        -SenderId: int [ForeignKey]
        -ReceiverId: int [ForeignKey]
        -SentAt: DateTime
        -Seen: bool
    }

    class User {
    }

    Message "1" -- "1" User: Sender
    Message "1" -- "1" User: Receiver

```
- Staff
```mermaid
classDiagram
    class Staff {
        -StaffId: int [Key]
        -Email: string [Required, RegularExpression]
        -FirstName: string [Required]
        -LastName: string [Required]
        -PhoneNumber: string [StringLength: 10]
        -AddressId: int [ForeignKey]
        -Address: Address
        -IsVerified: bool
        -Password: byte[]
        -HashKey: byte[]
        -Role: string
        -loginAttempts: int = 0
    }

```



