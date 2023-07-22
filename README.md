# Map-Loader
this is an overly advanced map-loader for your unity game.

[![Download](https://img.shields.io/badge/Download-blue.svg)](https://github.com/JokerJosh0/Map-Loader/releases)
[![Help Discord](https://img.shields.io/badge/Download-blue.svg)](https://discord.gg/VnjNfXYN4M)

![](https://github.com/JokerJosh0/Map-Loader/assets/137851579/5916e24d-8107-4e27-a63d-dbd5f65c11b1)




# Getting Started - Maps

![](https://github.com/JokerJosh0/Map-Loader/assets/137851579/eba770cb-50ad-4eb4-bbca-79277a2435ff)

Once you add a map via the + button, you will see 4 variables - Unique Id, Enable, Disable, and Disable On Start. 

Unique ID - used for referencing in a script using:
```cs
Map.Change("uniqueId");
```
Enable - Objects you want to enable once the dunction is called upon.

Disable - Objects you want to disable once the dunction is called upon.

Disable On Start - If checked, everything in the Enable variable will be disabled upon starting the game.

# Getting Started - Loaders

![](https://github.com/JokerJosh0/Map-Loader/assets/137851579/41b7c1ad-c697-4529-a60d-8ccbda88f981)

Once you add a loader via the + button, you will see 3 variables - Loader GameObject, ID To Change, and Dynamic Handle

Loader GameObject - This is the gameObject that will be used for handling the maps

ID To Change - This is the ID of the map that you want to change  (this can be found in the Maps list)

Dynamic Handle - This is used when you want the client to pass the loader and everything in the Enable variable will enable etc, but when the client goes back out the loader then everything in the Enable variable will be disabled. when this is ticked off, it will just call the change method as normal.

# Loaders - Advanced

![](https://github.com/JokerJosh0/Map-Loader/assets/137851579/7e3bc07f-e4e4-4482-9cce-17ec7f54e23c)


If you select the maploader gameobject (the object with the maploader script) and enable gizmos, you will be able to see every loader that youve added , outlined in a box. This box with display the ID of the loader, and when you hover over it with your mouse it wil highlight dark blue, but once you click on it, the loader gameObject will be selected

idk i just thought this was cool when making it :)


