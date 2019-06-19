# SpellSlinger
An augmented reality game made using Unity and Google ARCore for the Android platform.

When the game loads, move your phone around to scan the room.

Click on a grid that the app scans to place the hut and start the game.

Throw cards on the table next to the hut and bring the phone close to cast a spell.

# Documentation

Two ways to view the documentation:

Go to this link: https://shivivats.github.io/SpellSlinger/

OR

Clone the whole project, open the index.html file in the "docs" folder.

# End User Guide

## Requirements to run the game:

The player needs to have an ARCore compatible phone. The list of all compatible phones is given by Google [here](https://developers.google.com/ar/discover/supported-devices). Please check if your phone is in the list and meets the optional requirements (if any) before proceeding.

## About the Game:

Spell Slinger is an Augmented Reality game played on one single plane.

The player taps on a plane to place a hut at that position and the game starts.

The player needs to be careful not to let the plane out of the phone camera's sight once the game has started.

Once the game starts, enemies start spawning and heading towards the hut in waves. The player needs to defend the hut using spells.

The spells are spawned using physical markers provided with the app.

The markers need to placed on the physical plane where the player wants to spawn the spell. The player then needs to scan them using the phone's camera. The markers need to be covering atleast 25% of the camera frame for them to be recognized.

The player has 3 spells at their disposal right now: a fire tornado that goes from enemy to enemy, a spikes spell that spawns and stays in place and a shield that spawns around the house.

The aim is to earn a high score on the app, which is determined by how far the player get in terms of waves.

## How to use the app:

Open the app, go through the start screen and the tutorial.

After reading the tutorial, start the game and enjoy!




# Screenshots

#### Tutorial Screen
![Tutorial Screen](https://github.com/shivivats/SpellSlinger/raw/master/Screenshots/screenshot1.png)

#### Game with planes and UI
![Game with planes and UI](https://github.com/shivivats/SpellSlinger/raw/master/Screenshots/screenshot2.png)

#### Game in action with shield spell on hut
![Game in action with shield spell on hut](https://github.com/shivivats/SpellSlinger/raw/master/Screenshots/screenshot3.png)
