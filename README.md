# Map-Loader
this is an overly advanced map-loader for your unity game.

[![Download](https://img.shields.io/badge/Download-blue.svg)](https://github.com/JokerJosh0/Map-Loader/releases)
[![Discord](https://img.shields.io/badge/Discord-blue.svg)](https://discord.gg/VnjNfXYN4M)

![](https://cdn.discordapp.com/attachments/1132271654333071370/1132272170534445096/DALL_E_2023-07-22_17.34.59-removebg-preview_1.png)


# Getting Started

![](https://cdn.discordapp.com/attachments/1132271654333071370/1132271715544731718/collider.PNG)

The Recognised Collider variable is used to verify that what is entering the loader is the player.
Set this as your players body collider.


# Getting Started - Maps

![]((https://cdn.discordapp.com/attachments/1132271654333071370/1132271716417146971/maps.PNG))

Once you add a map via the + button, you will see 4 variables - Unique Id, Enable, Disable, and Disable On Start. 

Unique ID - used for referencing in a script using:
```cs
Map.Change("uniqueId");
```
Enable - Objects you want to enable once the function is called upon.

Disable - Objects you want to disable once the function is called upon.

Disable On Start - If checked, everything in the Enable variable will be disabled upon starting the game.

# Getting Started - Loaders

Dont worry about using colliders, its all handled for you :) but you are limited to only having box-shaped loaders.

![](https://cdn.discordapp.com/attachments/1132271654333071370/1132271716085792838/loaders.PNG)

Once you add a loader via the + button, you will see 3 variables - Loader GameObject, ID To Change, and Dynamic Handle

Loader GameObject - This is the gameObject that will be used for handling the maps

ID To Change - This is the ID of the map that you want to change  (this can be found in the Maps list)

Dynamic Handle - This is used when you want the client to pass the loader and everything in the Enable variable will enable etc, but when the client goes back out the loader then everything in the Enable variable will be disabled. when this is ticked off, it will just call the change method as normal.
```cs
Map.Change("uniqueId", dynamicHandle: true : false);
```

# Loaders - Advanced

![](https://cdn.discordapp.com/attachments/1132271654333071370/1132271715834150912/loader.PNG)


If you select the maploader gameobject (the object with the maploader script) and enable gizmos, you will be able to see every loader that youve added , outlined in a box. This box with display the ID of the loader, and when you hover over it with your mouse it wil highlight dark blue, but once you click on it, the loader gameObject will be selected

idk i just thought this was cool when making it :)



```js
Created By JokerJosh - discord: joker.josh
```
