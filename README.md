# LynxUp AR iPhone
LynxUp is an application that gives prospective students a tour of Rhodes College. The application will use augmented reality (AR) to introduce students to the college’s many facets. Students can scan the buildings plaques to get fun facts about the history of the building. Additionally, LynxUp provides an easy to follow interface for students to go on a guided tour of different parts of the campus. LynxUp uses user profiles to provide students with a engaging and personalized experience. Students receive experience points (XP) for each building they visit and item they collect, encouraging further exploration of the campus and friendly competition. LynxUp's map provides students with an indication of which locations they have visited and which they still need to visit, hopefully teaching them their way around campus.


## Features
 - [X] GPS Guidance
 - [X] Item Collection
 - [X] Choose your own Quest
 - [X] Unlock new places on the map


## System Diagram
![System Diagram](assets/diagram.png)

## Screenshots of UI
![Screenshot 1](assets/screenshot_1.png)
![Screenshot 2](assets/screenshot_2.png)

## Technologies
Our architecture comprises four major parts: the frontend, backend, database, and maps API. The frontend is built via Unity’s AR IOS kit and interacts with the backend and the maps API using REST API. The backend consists of a Flask server programmed with Python; it will facilitate an interaction with the front end and the database. The database will hold the user's data, progress, and asset information, including previously viewed locations and collected items. The database will also provide links to the virtual items in the storage. Finally, the maps API will provide minimap functionality and coordinate your location to the quest.

## Dependencies
- Firebase - database
- Python - language for backend
- Flask - connecting backend to frontend using REST APIs
- Unity XR - iOS AR framework for designing frontend
