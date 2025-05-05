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


## ✅ Prerequisites
- macOS system  
- Unity Hub + Unity Editor (with iOS Build Support)  
- Xcode (latest version)  
- Apple Developer Account (for physical device testing)  
- AR-capable iOS device (iPhone 6s or later)  
- Git installed on your machine  
- Python 3.9 installed  
- Flask (latest)  

---

## 🔧 Step-by-Step Guide

### 1. Clone the GitHub Repository
```
git clone git@github.com:Rhodes-CS-comp486/lynxup-ar-iphone.git  
cd lynxup-ar-iphone/
```
---

### 2. Open the Project in Unity
- Launch Unity Hub  
- Click “Open” and select the cloned project folder  
- Let Unity load dependencies and resolve packages  
- If prompted, install the matching Unity Editor version (check ProjectSettings/ProjectVersion.txt)

---

### 3. Install Required Packages (if needed)
- Open Window > Package Manager  
- Ensure required AR packages are installed:  
  - AR Foundation  
  - ARKit XR Plugin  
- Install XR Plugin Management:  
  - Go to Edit > Project Settings > XR Plugin Management  
  - Enable iOS and select ARKit

---

### 4. Set Up iOS Build Settings
- Go to File > Build Settings  
- Select iOS and click Switch Platform  
- Open Player Settings:  
  - Set your Company Name and Product Name  
  - Set a valid Bundle Identifier (e.g., com.yourcompany.arapp)  
  - Under Other Settings:  
    - Scripting Backend: IL2CPP  
    - Architecture: ARM64  
    - Target minimum iOS version: Usually 13.0+  
    - Enable Camera Usage Description  
  - Enable ARKit under XR Plugin Management

---

### 5. Build Xcode Project
- Back in Build Settings, click Build  
- Choose a folder to export the Xcode project  
- Unity generates an Xcode workspace/project

---

### 6. Open and Configure in Xcode
- Open the generated .xcworkspace in Xcode  
- Plug in your iOS device  
- In the Signing & Capabilities tab:  
  - Select your Apple Team  
  - Ensure the Provisioning Profile and Bundle ID match  
  - Enable Camera, Location and Motion permissions if not already set

---

### 7. Build and Run on iOS Device
- Select your connected device as the target  
- Click the Run (▶) button  
- Trust the developer on your iPhone when prompted  
- The AR app will launch and use the device camera and GPS

---

### 8. Run the Flask Server
- Open the terminal and run the backend:  
  cd backend/  
  python3.9 -m venv .venv  
  source .venv/bin/activate  
  pip install -r requirements.txt  
  flask --app router.py run  

This installs firebase-tools and flask and runs the local server.
