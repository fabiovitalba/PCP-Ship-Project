# Fabio Vitalba - Ship Navigation Project ('Boil Voyage')
This is the final project for the course "Physical Computing Project 24/25" for Fabio Vitalba.

## Boil Voyage
The Project is divided into two repositories. This repository is the 3D world created using Unity.
The 3D world allows players to control a ship through a set of islands and ports.
Find the other repository here: [Arduino Water Boiler Controller](https://github.com/fabiovitalba/PCP-Water-Boiler-Controller).

## 1. Description
In this Unity 3D project, you will find a navigateable ship that can sail accross the ocean between some islands. The ship has the ability to accelerate or decelerate as well as to steer/rudder, based on input. Whenever the ship is near to a port, that port is stored as a checkpoint in case the ship goes on land.

Each major island hosts a small village together with a port. A couple of shipwrecks can be found while navigating.

During navigation a rudder icon on the bottom left corner of the screen helps the player understand in which direction the ship is rotating to. This helps navigation while using the Water Boiler. Behind the rudder icon the users find a red line, which indicates the current input rotation - for example from the Water Boiler or the Keyboard. This helps understanding the ships behaviour while turning.
At the bottom right corner a set of knots appear to indicate the current ship speed, which is sometimes hard to graps while on the open sea. This also helps the navigation.

## 2. Visuals
### 2.1. Video
[Video Demo](https://youtube.com/...)

### 2.2. Screenshots
Here are a few screenshots of some key elements of the 3D world.

#### Starting position
![Starting position](/Screenshots/in-game-start.png)

#### Port checkpoint
![Port checkpoint](/Screenshots/in-game-port.png)

#### Island village
![Island village](/Screenshots/in-game-village.png)

#### Shipwreck
![Shipwreck](/Screenshots/in-game-shipwreck.png)

## 3. Installation
This Project requires some external assets from the Unity Asset Store. By opening this project through the Unity Hub, it will automatically download any required assets and build it in order to be run.

### 3.1. Fine tuning of Arduino Controller
When using the **Arduino Water Boiler Controller** you might have to fine tune some settings inside the 3D world. Here are the most important settings to look at.

Locate the `ControllerConnector` GameObject and find the Serial Port Listener Script attached to it:
![Water Boiler Controller Connection](/Screenshots/water-boiler-controller-connection.png)
- Port Name: This must be the Port that is used by the Arduino Water Boiler Controller that is attached through USB.
  - Finding the Port Name on MacOs/Linux:
  Go to the Terminal and execute the command: `ls /dev/{tty,cu}.*` in order to list all the connected devices. Since the water boiler is connected through USB it must be one of the devices containing "usbserial". Copy the full name and paste it into the `Port Name` property of the script.


Locate the `PlayerShip` GameObject and find the Water Boiler Script attached to it:
![Water Boiler Controller Settings](/Screenshots/water-boiler-controller-settings.png)
- Water Boiler Connected: This will turn active once the game detects the Water Boiler Controller to be connected.
- Cutoff Rotation Value: This must be in line with the Value that you define in the Arduino Script. It defines after which value the rotation angle is no longer increased.
- Switch Value Threshold: Defines the minimum value for the switch (Water boiler button) to count as activated.
- Min Light Value: Defines the minimum value that is received by Arduino from the light-dependent resistor.
- Max Light Value: Defines the maximum value that is received by Arduino from the light-dependent resistor.


## 4. Usage
The project was only tested inside Unity 3D, so you will have to open the repository from there in order to access and run the project.

### 4.1. Controls
#### 4.1.1. Keybinds (Keyboard)
- `W`, `A`, `S`, `D` in order to move the ship.
- `Q`, `E` allow the user to tilt the camera to the left or the right.
- `SPACE`/` ` in order to activate the searchlights attached to the ship.

#### 4.1.2. Keybinds (Arduino Water Boiler)
- Rotate the Boiler to the left or the right in order to steer the ship.
- Lift or drop the Boiler's lid in order to accelerate more or less.
- Press the button on the Boiler in order to activate the searchlights attached to the ship.

## 5. Support
For issues feel free to contact me on fvitalba@unibz.it, or by opening an Issue on the Repository.

### 5.1. Common Issues
- **I connected the Controller and selected the correct port, but the controller is not connected.**
  - You might have another program open that is listening on that specific port. One example for this could be Arduino IDE which automatically listens on the port of your controller. An easy solution is to close all other programs except Unity.

## 6. Contributing
Currently this project is not open for contributions at it is in very early stages of development.

## 7. Authors and acknowledgment
- Main author: Fabio Vitalba
- Various Free Asset Packs from the Unity Asset Store:
  - AllSky Free - 10 Sky / Skybox Set
  - AQUAS Lite - Built-In Render Pipeline
  - FREE Fantasy Terrain Textures
  - RPG Poly Pack - Lite
  - Stylized Pirate Ship
  - URP Stylized Water Shader - Proto Series
- Ardity Connector for Arduino Serial Port connection
- Background Music from pixabay.com:
  - Ribhava Gravawal - Ghostly Groove Dark Ambience
  - Kaydream321 - Ocean Waves White Noise
- Rudder UI icon: Tobia Vitalba

## 8. License
MIT License
Copyright (c) 2025 Fabio Vitalba

## 9. Project status
This Project is currently being worked on.
