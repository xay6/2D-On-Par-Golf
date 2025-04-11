# Brandon Mason Unit Testing Plan
### Fetching a range of users from the leaderboard
- Code to be tested: API call
- Makes use of a request body including a course ID, and upper and lower limits for a range of users to fetch.
- Output:
	- A report of what tests passed, failed, or were skipped.
	- A stack trace if there were any errors.
	- A table showing what percentage of code per file was tested(Only if using "npm run test:coverage")
### Registering a new user and then logging in
- Code to be tested: API call
- Executes two POST requests that each use a request body including a username and password.
- Output:
	- A report of what tests passed, failed, or were skipped.
	- A stack trace if there were any errors.
	- A table showing what percentage of code per file was tested(Only if using "npm run test:coverage")
### Establishing a connection to a  MongoDB instance
- Code to be tested: Connection establishment and error logging.
- Makes use of a mocked MongoDB instance and the contents of a .env file relating to the database
- Output:
	- A stack trace if there were any errors.
	- A success or failure message.