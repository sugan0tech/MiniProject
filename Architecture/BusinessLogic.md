
## Auth
---
### Login
- checks if user verified 
- then logs in and returns token
- Multiple failed login ( > 5 )  deactivates account
### Logout
- Logs out & invalidates the existing token. (black listing)
### ResetPassword
- automatically generates new token for new creds.
### Register
- Generates new account, in order to use need to be verified. 
---
## Membership

| Basic Member $5             | Premium Member $15                    | Free Member $0           |
| --------------------------- | ------------------------------------- | ------------------------ |
| - 15 matches per month <br> | - Unlimited matches per month<br>     | - 5 matches permonth<br> |
| - No private chats          | - Upto 25 private chats<br>           | - No private chat        |
| - Up to 5 views Per month   | - Unlimited lookup on profile viewers | - No profile views       |
> Any Member ship will be automatically canceled on the due date, and temporary access will be restricted. Might be using ==CRON job== and ==flusher== on app startup.
### Free Trail
> For user they can enjoy a 7 day free trail with the insights of Premium Member ship features ( chats and unlimited viewers view )

### Membership validation
- Will be using middleware for membership validation for required apis. [ref](https://medium.com/@shubhadeepchat/net-core-middleware-explained-8c21bf646700)

---
## Match Making

- With the use of user created Preference set a cumulative match results will be provided.
- Match with Tags ( getting matches irrespective of general Preference, just using specific tags )
- Match request to a profile

### Match Progress
- Any one can express a match request to a profile
- Only if both likes each other in match, they can 
	- Do private chat if Premium member
	- For Basic Member, they can engage by contact details
- if any one of them dislikes, it will be discarded
---
## Chat
-  Premium user can do a private chat without sharing personal details first.
- Either Users ( Profile person or Profile manager) can engage in a chat.

---
## Profile Manager
- Any user can manager multiple profile for their `sons`, `daughter`, `relations` and  `friends`. Or they can lookup for themselves `myself`
---
## Profile Views
- A state of Current Profile Views is stored
- Can be used for improving Profile for better engagement
---
## Staff
- Internal Admins & Support Team.
- Complete Internal Access for support and bug fix.