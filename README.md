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
"Developed shot mechanics, including UI for power and angle selection, physics-based shot force calculations, and hole detection. Also worked on a game design document for golf club mechanics."

- Jira Task: SCRUM-11 Design and implement a UI for shot power and angle selection
    - Description: Created a user interface for players to select shot power and angle.
    - [Jira Link](https://cs3398-romulans-spring.atlassian.net/jira/software/projects/SCRUM/boards/1?selectedIssue=SCRUM-11)


- Jira Task: SCRUM-67 Add a hole to the board for the ball to go in to.
    - Description: Created a scoring system that maintains the player's total score across different levels while keeping track of individual level scores.
    - [Bitbucket Branch and Commit(s)](https://bitbucket.org/cs3398-romulans-s25/on-par/branch/SCRUM-67-add-a-hole-to-the-board-for-the)
    - [Pull Request](https://bitbucket.org/cs3398-romulans-s25/on-par/pull-requests/3)
    - [Jira Link](https://cs3398-romulans-spring.atlassian.net/jira/software/projects/SCRUM/boards/1?selectedIssue=SCRUM-67)


- Jira Task: SCRUM-12 Create a script to capture player input (mouse or keyboard) to adjust power and angle
    - Description: Developed input controls to modify shot power and angle.
    - [Jira Link](https://cs3398-romulans-spring.atlassian.net/jira/software/projects/SCRUM/boards/1?selectedIssue=SCRUM-12)
    - [Bitbucket Branch and Commit(s)](https://bitbucket.org/cs3398-romulans-s25/on-par/branch/SCRUM-12-rebase-for-issue-key)

- Jira Task: SCRUM-14 Test different force values to ensure smooth and predictable ball movement.
    - Description: Adjusted and tested force values for smooth ball motion.
    - [Jira Link](https://cs3398-romulans-spring.atlassian.net/jira/software/projects/SCRUM/boards/1?selectedIssue=SCRUM-14)


- Jira Task: SCRUM-13 Apply physics-based shot force calculations using Unity’s Rigidbody2D (With visual display for aim and shot power).
    - Description: Integrated physics-based calculations for shot mechanics, including a visual display for aim and shot power.
    - [Bitbucket Branch and Commit(s)](https://bitbucket.org/cs3398-romulans-s25/on-par/branch/SCRUM-13-rebase-for-issue-key)
    - [Pull Request](https://bitbucket.org/cs3398-romulans-s25/on-par/pull-requests/3)
    - [Jira Link](https://cs3398-romulans-spring.atlassian.net/jira/software/projects/SCRUM/boards/1?selectedIssue=SCRUM-13)


- Jira Task: SCRUM-25 Create a game design document outlining the mechanics for each golf club type (Driver, Iron, and Putter).
    - Description: Documented golf club mechanics for the game, detailing differences between club types.
    - [Jira Link](https://cs3398-romulans-spring.atlassian.net/jira/software/projects/SCRUM/boards/1?selectedIssue=SCRUM-25)
    - [Bitbucket Branch and Commit(s)](https://bitbucket.org/cs3398-romulans-s25/on-par/branch/SCRUM-25-alternate-method-using-scriptableobjects)


Natalie Morales: 
"Implemented core gameplay mechanics, including stroke counting, score tracking, and golf club switching."

- Jira Task: SCRUM-49 Implement a user interface to display the current number of strokes per round
    - Description: Developed a stroke counter that tracks the number of strokes a player takes in a level.
    - [Jira Link](https://cs3398-romulans-spring.atlassian.net/jira/software/projects/SCRUM/boards/1?selectedIssue=SCRUM-49)
    - [Bitbucket Branch and Commit(s)](https://bitbucket.org/cs3398-romulans-s25/on-par/branch/SCRUM-49-implement-a-user-interface-to-d)


- Jira Task: SCRUM-48 Develop the logic to calculate the score for each hole and the cumulative total score.
    - Description: Created a scoring system that maintains the player's total score across different levels while keeping track of individual level scores.
    - [Bitbucket Branch and Commit(s)](https://bitbucket.org/cs3398-romulans-s25/on-par/branch/SCRUM-48-score-tracking-feature)
    - [Pull Request](https://bitbucket.org/cs3398-romulans-s25/on-par/pull-requests/13)
    - [Jira Link](https://cs3398-romulans-spring.atlassian.net/jira/software/projects/SCRUM/boards/1?selectedIssue=SCRUM-48)


 - Jira Task: SCRUM-27 implement a mechanism that allows you to switch between different clubs (driver, iron, putter)
    - Description: Allowed players to switch between different golf clubs (Driver, Iron, Putter), each affecting the shot differently.
    - [Bitbucket Branch and Commit(s)](https://bitbucket.org/cs3398-romulans-s25/on-par/branch/SCRUM-27-implement-a-mechanism-that-allo)



## Features
- Mouse controls - Will launch the ball into the air.
- Gravity - Will pull the ball back to the ground after being launched.
- Custom golf courses
