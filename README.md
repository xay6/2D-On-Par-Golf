# On Par
#### This project is maintained by:
- Ariana Zapata
- Brandon Mason
- Jalen Stewart
- Natalie Morales
- Xavier Ortiz

> On Par is a two dimensional golf game for casual gamers who want a fun and relaxing game that they can play anytime, anywhere. Through simple controls and clear cut goals, our game will provide players with an experience that will challenge their precision and strategy skills. Through unlockable cosmetic items, the game will give the player a sense of achievement under little to no stress.

## Table of Contents
  - [General Information](#general-information)
  - [Technologies Used](#technologies-used)
  - [Sprint 1 Contributions](#sprint-1-contributions)
  - [Next Steps](#next-steps)
  - [Burnup Chart](#burnup chart for sprint 1)
  - [Features](#features)
  


## General Information
![On Par](./img/OnPar.png)


## Technologies Used
- [Unity Engine](https://unity.com/products/unity-engine)
- C#
- VSCode
- AI/ChatGPT


## Sprint 1 Contributions
Brandon Mason: 
Developed shot mechanics, including UI for power and angle selection, physics-based shot force calculations, and hole detection. Also worked on a game design document for golf club mechanics.

- SCRUM-11 Design and implement a UI for shot power and angle selection
    - Description: Created a user interface for players to select shot power and angle.
    - [Jira Link](https://cs3398-romulans-spring.atlassian.net/jira/software/projects/SCRUM/boards/1?selectedIssue=SCRUM-11)


- SCRUM-67 Add a hole to the board for the ball to go in to.
    - Description: Created a scoring system that maintains the player's total score across different levels while keeping track of individual level scores.
    - [Bitbucket Branch and Commit(s)](https://bitbucket.org/cs3398-romulans-s25/on-par/branch/SCRUM-67-add-a-hole-to-the-board-for-the)
    - [Pull Request](https://bitbucket.org/cs3398-romulans-s25/on-par/pull-requests/3)
    - [Jira Link](https://cs3398-romulans-spring.atlassian.net/jira/software/projects/SCRUM/boards/1?selectedIssue=SCRUM-67)

- SCRUM-12 Create a script to capture player input (mouse or keyboard) to adjust power and angle
    - Description: Developed input controls to modify shot power and angle.
    - [Bitbucket Branch and Commit(s)](https://bitbucket.org/cs3398-romulans-s25/on-par/branch/SCRUM-12-rebase-for-issue-key)
    - [Jira Link](https://cs3398-romulans-spring.atlassian.net/jira/software/projects/SCRUM/boards/1?selectedIssue=SCRUM-12)

- SCRUM-14 Test different force values to ensure smooth and predictable ball movement.
    - Description: Adjusted and tested force values for smooth ball motion.
    - [Jira Link](https://cs3398-romulans-spring.atlassian.net/jira/software/projects/SCRUM/boards/1?selectedIssue=SCRUM-14)


- SCRUM-13 Apply physics-based shot force calculations using Unity's Rigidbody2D (With visual display for aim and shot power).
    - Description: Integrated physics-based calculations for shot mechanics, including a visual display for aim and shot power.
    - [Bitbucket Branch and Commit(s)](https://bitbucket.org/cs3398-romulans-s25/on-par/branch/SCRUM-13-rebase-for-issue-key)
    - [Pull Request](https://bitbucket.org/cs3398-romulans-s25/on-par/pull-requests/3)
    - [Jira Link](https://cs3398-romulans-spring.atlassian.net/jira/software/projects/SCRUM/boards/1?selectedIssue=SCRUM-13)


- SCRUM-25 Create a game design document outlining the mechanics for each golf club type (Driver, Iron, and Putter).
    - Description: Documented golf club mechanics for the game, detailing differences between club types.
    - [Bitbucket Branch and Commit(s)](https://bitbucket.org/cs3398-romulans-s25/on-par/branch/SCRUM-25-alternate-method-using-scriptableobjects)
    - [Jira Link](https://cs3398-romulans-spring.atlassian.net/jira/software/projects/SCRUM/boards/1?selectedIssue=SCRUM-25)


Ariana Zapata:
Developed UI components, including the leaderboard panel, main menu UI, login UI, and guest functionality integration

- SCRUM-19 Design and implement a UI panel for the leaderboard.
    - Description: Created a leaderboard UI panel to display player rankings.
    - [Bitbucket Branch and Commit(s)](https://bitbucket.org/cs3398-romulans-s25/on-par/branch/SCRUM-19)
    - [Jira Link](https://cs3398-romulans-spring.atlassian.net/jira/software/projects/SCRUM/boards/1?selectedIssue=SCRUM-19)


- SCRUM-65 Integrate "Guest" Functionality
    - Description: Implemented guest login functionality to allow players to access the game without an account.
    - [Bitbucket Branch and Commit(s)](https://bitbucket.org/cs3398-romulans-s25/on-par/branch/SCRUM-65)
    - [Pull Request](https://bitbucket.org/cs3398-romulans-s25/on-par/pull-requests/14)
    - [Jira Link](https://cs3398-romulans-spring.atlassian.net/jira/software/projects/SCRUM/boards/1?selectedIssue=SCRUM-65)


- SCRUM-62 Design & Implement Main Menu UI
    - Description: Designed and implemented the main menu interface for easy game navigation.
    - [Bitbucket Branch and Commit(s)](https://bitbucket.org/cs3398-romulans-s25/on-par/branch/SCRUM-62)
    - [pull request](https://bitbucket.org/cs3398-romulans-s25/on-par/pull-requests/11)
    - [Jira Link](https://cs3398-romulans-spring.atlassian.net/jira/software/projects/SCRUM/boards/1?selectedIssue=SCRUM-62)

- SCRUM-63 Develop Login UI.
    - Description: Created a login UI that allows players to sign in and access their game progress.
    - [Bitbucket Branch and Commit(s)](https://bitbucket.org/cs3398-romulans-s25/on-par/branch/SCRUM-63)
    - [Jira Link](https://cs3398-romulans-spring.atlassian.net/jira/software/projects/SCRUM/boards/1?selectedIssue=SCRUM-62)




Natalie Morales: 
Implemented core gameplay mechanics, including stroke counting, score tracking, and golf club switching.

- SCRUM-49 Implement a user interface to display the current number of strokes per round
    - Description: Developed a stroke counter that tracks the number of strokes a player takes in a level.
    - [Bitbucket Branch and Commit(s)](https://bitbucket.org/cs3398-romulans-s25/on-par/branch/SCRUM-49-implement-a-user-interface-to-d)
    - [Jira Link](https://cs3398-romulans-spring.atlassian.net/jira/software/projects/SCRUM/boards/1?selectedIssue=SCRUM-49)


- SCRUM-48 Develop the logic to calculate the score for each hole and the cumulative total score.
    - Description: Created a scoring system that maintains the player's total score across different levels while keeping track of individual level scores.
    - [Bitbucket Branch and Commit(s)](https://bitbucket.org/cs3398-romulans-s25/on-par/branch/SCRUM-48-score-tracking-feature)
    - [Pull Request](https://bitbucket.org/cs3398-romulans-s25/on-par/pull-requests/13)
    - [Jira Link](https://cs3398-romulans-spring.atlassian.net/jira/software/projects/SCRUM/boards/1?selectedIssue=SCRUM-48)
    

 - SCRUM-27 implement a mechanism that allows you to switch between different clubs (driver, iron, putter)
    - Description: Allowed players to switch between different golf clubs (Driver, Iron, Putter), each affecting the shot differently.
    - [Bitbucket Branch and Commit(s)](https://bitbucket.org/cs3398-romulans-s25/on-par/branch/SCRUM-27-implement-a-mechanism-that-allo)
    - [Jira Link](https://cs3398-romulans-spring.atlassian.net/jira/software/projects/SCRUM/boards/1?selectedIssue=SCRUM-27)


Xavier Ortiz
Developed core gameplay levels, camera mechanics, and the Hole-in-One challenge logic and UI.

- SCRUM-60 Learning Unity
    - Description: Studied Unity development practices to better contribute to the project.
    - [Bitbucket Branch and Commit(s)](https://bitbucket.org/cs3398-romulans-s25/on-par/branch/SCRUM-60-learning-unity)
    - [Jira Link](https://cs3398-romulans-spring.atlassian.net/jira/software/projects/SCRUM/boards/1?selectedIssue=SCRUM-60)

- SCRUM-68 Making the camera following the golf ball/player
    - Description: Implemented a smooth camera-follow system to track the golf ball's movement.
    - [Bitbucket Branch and Commit(s)](https://bitbucket.org/cs3398-romulans-s25/on-par/branch/SCRUM-68-making-the-camera-following)
    - [Jira Link](https://cs3398-romulans-spring.atlassian.net/jira/software/projects/SCRUM/boards/1?selectedIssue=SCRUM-68)

 - SCRUM-34 Create 18 playable levels/courses for the game.
    - Description:  Designed and implemented 18 unique golf courses with different terrains and obstacles.
    - [Bitbucket Branch and Commit(s)](https://bitbucket.org/cs3398-romulans-s25/on-par/branches/?search=SCRUM-34)
    - [Jira Link](https://cs3398-romulans-spring.atlassian.net/jira/software/projects/SCRUM/boards/1?selectedIssue=SCRUM-34)

- SCRUM-70 Adding the hole script to all the levels
    - Description: Integrated hole detection mechanics into all game levels.
    - [Bitbucket Branch and Commit(s)](https://bitbucket.org/cs3398-romulans-s25/on-par/branch/SCRUM-70-adding-the-hole-script)
    - [Jira Link](https://cs3398-romulans-spring.atlassian.net/jira/software/projects/SCRUM/boards/1?selectedIssue=SCRUM-70)
    
- SCRUM-55 Design the Hole-in-One Challenge UI & Level Indicators
    - Description:  Created a UI display for the Hole-in-One challenge, including level indicators.
    - [Bitbucket Branch and Commit(s)](https://bitbucket.org/cs3398-romulans-s25/on-par/branch/SCRUM-55-design-the-hole-in-one-challenge)
    - [Jira Link](https://cs3398-romulans-spring.atlassian.net/jira/software/projects/SCRUM/boards/1?selectedIssue=SCRUM-55)

- SCRUM-56 Implement Hole-in-One Challenge Logic
    - Description: Developed the logic for the Hole-in-One challenge, ensuring correct scoring and mechanics.
    - [Bitbucket Branch and Commit(s)](https://bitbucket.org/cs3398-romulans-s25/on-par/branch/SCRUM-56-implement-hole-in-one-challenge)
    - [Jira Link](https://cs3398-romulans-spring.atlassian.net/jira/software/projects/SCRUM/boards/1?selectedIssue=SCRUM-56)

    

Jalen Stewart:
Developed obstacle and wind mechanics, including collision detection, wind trajectory effects, and UI elements for tracking wind direction.

 - SCRUM-29 Design the behavior and visual representation of obstacles (e.g., trees, water hazards, sand traps) and wind mechanics (e.g., direction, strength, visual indicators).
    - Description: Designed trees, water hazards, sand traps, and wind effects, including direction, strength, and visual indicators.
    - [Bitbucket Branch and Commit(s)](https://bitbucket.org/cs3398-romulans-s25/on-par/branch/SCRUM-29-design-the-behavior-and-visual-)
    - [Jira Link](https://cs3398-romulans-spring.atlassian.net/jira/software/projects/SCRUM/boards/1?selectedIssue=SCRUM-29)


- SCRUM-30 Implement obstacle collision detection and physics interactions in Unity.
    - Description: Developed physics-based interactions and collision detection for obstacles.
    - [Bitbucket Branch and Commit(s)](https://bitbucket.org/cs3398-romulans-s25/on-par/branch/SCRUM-30-implement-obstacle-collision-de)
    - [Jira Link](https://cs3398-romulans-spring.atlassian.net/jira/software/projects/SCRUM/boards/1?selectedIssue=SCRUM-30)

 - SCRUM-31 implement a mechanism that allows you to switch between different clubs (driver, iron, putter)
    - Description: Implemented wind effects that dynamically affect ball movement.fect ball movement.
    - [Bitbucket Branch and Commit(s)](https://bitbucket.org/cs3398-romulans-s25/on-par/branch/SCRUM-31-add-wind-mechanics-to-influence)
    - [Jira Link](https://cs3398-romulans-spring.atlassian.net/jira/software/projects/SCRUM/boards/1?selectedIssue=SCRUM-31)

- SCRUM-69 Create wind UI to track the wind and direction.
    - Description: Developed an on-screen UI to visually display wind direction and intensity.
    - [Bitbucket Branch and Commit(s)](https://bitbucket.org/cs3398-romulans-s25/on-par/branch/SCRUM-69-create-wind-ui-to-track-the-win)
    - [Jira Link](https://cs3398-romulans-spring.atlassian.net/jira/software/projects/SCRUM/boards/1?selectedIssue=SCRUM-69)
    

 - SCRUM-33 Add obstacles and wind to existing or new game courses.
    - Description: Integrated obstacles and wind mechanics into different golf course designs.
    - [Bitbucket Branch and Commit(s)](https://bitbucket.org/cs3398-romulans-s25/on-par/branch/SCRUM-33-adjust-sand-logic)
    - [Jira Link](https://cs3398-romulans-spring.atlassian.net/jira/software/projects/SCRUM/boards/1?selectedIssue=SCRUM-33)


## Next Steps for Sprint 2
  - Brandon Mason
  - Ariana Zapata
  - Natalie Morales
      - A feature that implements Lava pits that instantly reset the ball.
      - implement a neater and more in depth UI for features such as clubs, wind text, scores. 
      - Sync in-game lighting with the player's real-world time for a dynamic day-night cycle.
      
  - Xavier Ortiz: 
      - Continue working on my hole-in-challege and UI look
      - Create a reward track for completing the challeges
      - Create a couple of custom golf balls to choose from
  - Jaylen Stewart

## Burnup Chart for Sprint 1
![Burnup Chart](./img/burnupChart.png)

## Features
- Mouse controls - Will launch the ball into the air.
- 18 Custom golf courses
- Stroke Counter â€“ Tracks the number of strokes per level.
- Total Score Tracking and Maintains the player's score across different levels.
- Golf Club Selection â€“ Switch between Driver, Iron, and Putter, each affecting the shot differently.
- Wind Mechanics â€“ Wind dynamically influences ball trajectory based on direction and strength.
- Obstacles and Hazards â€“ Trees, water hazards, sand traps, and other environmental challenges.
- Main Menu & Navigation â€“ Easy access to game modes and settings.
- Leaderboard UI â€“ Tracks top scores for competitive play.
- Realistic Shot Force System â€“ Uses Unityâ€™s Rigidbody2D for accurate physics-based golf shots.
- Collision Detection â€“ Ensures the ball interacts correctly with terrain, walls, and obstacles.
- Camera Follow System â€“ Automatically tracks the ball after each shot.
